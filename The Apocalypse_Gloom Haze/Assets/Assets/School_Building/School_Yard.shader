Shader "Custom/ProceduralConcrete_TLOU_URP"
{
    Properties
    {
        _ConcreteColor ("Concrete Base", Color) = (0.40, 0.41, 0.39, 1)
        _DarkConcrete ("Dark Concrete", Color) = (0.22, 0.23, 0.22, 1)
        _MossColor ("Moss / Algae", Color) = (0.16, 0.24, 0.16, 1)

        _WorldScale ("World Scale", Float) = 0.12

        _GravelStrength ("Gravel", Range(0,1)) = 0.35
        _DirtStrength ("Dirt", Range(0,1)) = 0.6
        _MossStrength ("Moss", Range(0,1)) = 0.75
        _CrackStrength ("Cracks", Range(0,1)) = 0.55
        _MoistureStrength ("Moisture", Range(0,1)) = 0.6
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
            };

            float4 _ConcreteColor;
            float4 _DarkConcrete;
            float4 _MossColor;

            float _WorldScale;
            float _GravelStrength;
            float _DirtStrength;
            float _MossStrength;
            float _CrackStrength;
            float _MoistureStrength;

            // ---------- Noise ----------
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
                float2 u = f * f * (3.0 - 2.0 * f);
                return lerp(a, b, u.x) +
                       (c - a) * u.y * (1.0 - u.x) +
                       (d - b) * u.x * u.y;
            }

            float fbm(float2 p)
            {
                float v = 0.0;
                float a = 0.5;
                for (int i = 0; i < 5; i++)
                {
                    v += a * noise(p);
                    p *= 2.0;
                    a *= 0.5;
                }
                return v;
            }

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float2 wp = IN.worldPos.xz * _WorldScale;

                // Base concrete variation
                float baseVar = fbm(wp * 1.2);

                // Directional fine cracks
                float crack = smoothstep(0.52, 0.56, fbm(wp * 6.5));
                crack *= _CrackStrength;

                // Dirt & dust
                float dirt = fbm(wp * 2.8) * _DirtStrength;

                // Gravel / sand
                float gravel = noise(wp * 9.0) * _GravelStrength;

                // Moisture zones (dark + moss)
                float moisture = smoothstep(0.45, 0.75, fbm(wp * 1.4)) * _MoistureStrength;

                // Moss prefers moisture & cracks
                float mossMask = smoothstep(0.4, 0.8, moisture + crack) * _MossStrength;

                // Dark edge (near walls / vertical surfaces)
                float edgeDark = saturate(1.0 - abs(IN.normalWS.y));
                edgeDark *= 0.25;

                float3 color = lerp(_ConcreteColor.rgb, _DarkConcrete.rgb, baseVar);
                color -= dirt * 0.35;
                color += gravel * 0.1;
                color -= crack * 0.3;
                color -= moisture * 0.25;
                color -= edgeDark;

                // Moss layer
                color = lerp(color, _MossColor.rgb, mossMask);

                // Global desaturation & darkening (TLOU tone)
                color = lerp(color, dot(color, float3(0.333,0.333,0.333)), 0.15);
                color *= 0.82;

                return half4(color, 1);
            }
            ENDHLSL
        }
    }
}
