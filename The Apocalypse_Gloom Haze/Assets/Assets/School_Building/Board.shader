Shader "Custom/School_Chalkboard_Procedural"
{
    Properties
    {
        _BoardColor ("Board Base Color", Color) = (0.12, 0.22, 0.16, 1)
        _BoardDark ("Board Dark Tone", Color) = (0.07, 0.13, 0.10, 1)
        _ChalkColor ("Chalk Color", Color) = (0.92, 0.92, 0.90, 1)

        _BoardNoise ("Board Noise Strength", Range(0,1)) = 0.35
        _ChalkStrength ("Chalk Intensity", Range(0,1)) = 0.9
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

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _BoardColor;
            float4 _BoardDark;
            float4 _ChalkColor;
            float _BoardNoise;
            float _ChalkStrength;

            // ---------------- NOISE ----------------
            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1,311.7))) * 43758.5453);
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

            // ----------- LINE DRAW (SDF) -----------
            float DrawLine(float2 uv, float2 a, float2 b, float w)
            {
                float2 pa = uv - a;
                float2 ba = b - a;
                float h = saturate(dot(pa, ba) / dot(ba, ba));
                return smoothstep(w, 0.0, length(pa - ba * h));
            }

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.positionOS.xz * 0.5 + 0.5;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;

                // Board base
                float boardVar = noise(uv * 6.0);
                float3 boardCol = lerp(_BoardColor.rgb, _BoardDark.rgb, boardVar * _BoardNoise);

                // Chalk drawings
                float chalk = 0;

                // 2 + 3 = 5
                chalk += DrawLine(uv, float2(0.15,0.7), float2(0.20,0.7), 0.008);
                chalk += DrawLine(uv, float2(0.25,0.72), float2(0.25,0.78), 0.008);
                chalk += DrawLine(uv, float2(0.25,0.75), float2(0.30,0.75), 0.008);

                // x² + 4 = 0
                chalk += DrawLine(uv, float2(0.45,0.75), float2(0.48,0.8), 0.007);
                chalk += DrawLine(uv, float2(0.48,0.75), float2(0.45,0.8), 0.007);

                // Power ^
                chalk += DrawLine(uv, float2(0.5,0.82), float2(0.52,0.85), 0.006);

                // Geometry cube
                chalk += DrawLine(uv, float2(0.65,0.3), float2(0.78,0.3), 0.007);
                chalk += DrawLine(uv, float2(0.78,0.3), float2(0.78,0.45), 0.007);
                chalk += DrawLine(uv, float2(0.78,0.45), float2(0.65,0.45), 0.007);
                chalk += DrawLine(uv, float2(0.65,0.45), float2(0.65,0.3), 0.007);

                chalk = saturate(chalk * _ChalkStrength);

                float3 finalCol = boardCol;
                finalCol = lerp(finalCol, _ChalkColor.rgb, chalk);

                return half4(finalCol, 1);
            }
            ENDHLSL
        }
    }
}
