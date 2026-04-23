Shader "Custom/ProceduralSchoolGlass_URP"
{
    Properties
    {
        _GlassColor ("Glass Tint Color", Color) = (0.75, 0.78, 0.8, 1)
        _Opacity ("Glass Opacity", Range(0.05, 0.5)) = 0.18
        _WorldScale ("World Dirt Scale", Float) = 0.6
        _DirtStrength ("Dust / Dirt Strength", Range(0,1)) = 0.35
        _Roughness ("Surface Roughness", Range(0,1)) = 0.25
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

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

            float4 _GlassColor;
            float _Opacity;
            float _WorldScale;
            float _DirtStrength;
            float _Roughness;

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
                float dirt = noise(wp * 3.0);

                // Dirt mask
                float dirtMask = saturate(dirt * _DirtStrength);

                // Base glass color
                float3 color = _GlassColor.rgb;

                // Darken slightly by dirt
                color *= lerp(1.0, 0.85, dirtMask);

                // Simple lighting
                float3 lightDir = normalize(_MainLightPosition.xyz);
                float ndl = saturate(dot(IN.normalWS, lightDir)) * 0.6 + 0.4;
                color *= ndl;

                float alpha = _Opacity + dirtMask * 0.15;

                return half4(color, alpha);
            }
            ENDHLSL
        }
    }
}
