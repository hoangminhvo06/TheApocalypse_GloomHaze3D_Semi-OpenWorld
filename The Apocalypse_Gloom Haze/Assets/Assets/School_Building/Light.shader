Shader "Custom/PanelLight_Procedural_URP"
{
    Properties
    {
        _PanelColor ("Panel Base (Off White)", Color) = (0.92, 0.93, 0.90, 1)
        _FrameColor ("Frame Color", Color) = (0.65, 0.66, 0.64, 1)
        _DirtColor  ("Dirt Tint", Color) = (0.75, 0.76, 0.72, 1)

        _EmissionStrength ("Emission Strength", Range(0,8)) = 3.0
        _PanelSoftness ("Light Falloff", Range(0.1,5)) = 2.0
        _FrameWidth ("Frame Width", Range(0.01,0.15)) = 0.05
        _DirtAmount ("Surface Dirt", Range(0,1)) = 0.35
        _NoiseScale ("Noise Scale", Float) = 35.0
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
        }

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _PanelColor;
            float4 _FrameColor;
            float4 _DirtColor;

            float _EmissionStrength;
            float _PanelSoftness;
            float _FrameWidth;
            float _DirtAmount;
            float _NoiseScale;

            // ---------- Noise ----------
            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453123);
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

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                // Panel mask (rectangular)
                float2 dist = abs(uv - 0.5);
                float panelMask = step(max(dist.x, dist.y), 0.5);

                // Frame
                float frameMask = step(0.5 - _FrameWidth, max(dist.x, dist.y));
                frameMask *= panelMask;

                // Soft light falloff from center
                float centerDist = length(uv - 0.5);
                float lightFalloff = exp(-centerDist * _PanelSoftness);

                // Frosted noise
                float n = noise(uv * _NoiseScale);
                float frosted = lerp(0.85, 1.0, n);

                // Dirt & aging
                float dirt = noise(uv * (_NoiseScale * 0.4));
                dirt = saturate(dirt * _DirtAmount);

                // Base panel color
                float3 panelCol = _PanelColor.rgb;
                panelCol *= lightFalloff;
                panelCol *= frosted;
                panelCol = lerp(panelCol, _DirtColor.rgb, dirt);

                // Frame color
                float3 frameCol = _FrameColor.rgb;

                // Combine
                float3 color = panelCol;
                color = lerp(color, frameCol, frameMask);

                // Emission (only panel, not frame)
                float3 emission = panelCol * _EmissionStrength * (1.0 - frameMask);

                return half4(color + emission, 1);
            }
            ENDHLSL
        }
    }
}
