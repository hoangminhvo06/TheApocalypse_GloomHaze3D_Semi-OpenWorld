Shader "Custom/ProceduralConcreteWall_WhiteSmoke_URP"
{
    Properties
    {
        _BaseColor ("Concrete Base Color", Color) = (0.78, 0.78, 0.75, 1)
        _DarkTint ("Aged Dark Tint", Color) = (0.55, 0.56, 0.55, 1)

        _WorldScale ("World Scale", Float) = 0.18
        _Dust ("Dust / Chalk", Range(0,1)) = 0.35
        _Cracks ("Hairline Cracks", Range(0,1)) = 0.45
        _Aging ("Surface Aging", Range(0,1)) = 0.4

        _Roughness ("Surface Roughness", Range(0.2,1)) = 0.85
        _AOIntensity ("AO Intensity", Range(0,1)) = 0.25
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }
        LOD 300

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 worldPos   : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
            };

            float4 _BaseColor;
            float4 _DarkTint;
            float _WorldScale;
            float _Dust;
            float _Cracks;
            float _Aging;
            float _Roughness;
            float _AOIntensity;

            // Simple noise
            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                float a = hash(i);
                float b = hash(i + float2(1,0));
                float c = hash(i + float2(0,1));
                float d = hash(i + float2(1,1));
                float2 u = f * f * (3 - 2 * f);
                return lerp(a, b, u.x) +
                       (c - a) * u.y * (1 - u.x) +
                       (d - b) * u.x * u.y;
            }

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float2 wp = IN.worldPos.xz * _WorldScale;

                float surface = noise(wp * 2.0);
                float crack = smoothstep(0.55, 0.58, noise(wp * 8.0));

                // Base concrete color (luôn sáng)
                float3 baseCol = lerp(_BaseColor.rgb, _DarkTint.rgb, surface * _Aging);

                // Dust / chalk layer
                baseCol = lerp(baseCol, float3(0.88,0.88,0.85), _Dust * surface);

                // Crack darkening (rất nhẹ)
                baseCol *= lerp(1.0, 0.85, crack * _Cracks);

                // Fake AO – KHÔNG nhân thẳng
                float ao = lerp(1.0, 0.8, surface * _AOIntensity);

                // Lighting
                float3 lightDir = normalize(_MainLightPosition.xyz);
                float ndl = saturate(dot(IN.normalWS, lightDir)) * 0.8 + 0.2;

                float3 color = baseCol * ndl * ao;

                return half4(color, 1);
            }
            ENDHLSL
        }
    }
}
