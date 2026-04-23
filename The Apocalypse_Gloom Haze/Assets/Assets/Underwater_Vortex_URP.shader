Shader "Custom/DeepSea_Vortex_Depression_URP"
{
    Properties
    {
        _WaterColor ("Water Tint", Color) = (0.02,0.06,0.08,1)
        _DepthPull ("Downward Pull", Range(0,1)) = 0.45
        _Radius ("Vortex Radius", Range(0.2,3)) = 1.2
        _SpinSpeed ("Angular Drift", Range(0,0.5)) = 0.08
        _NoiseScale ("Distortion Scale", Range(1,10)) = 4
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

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

            float4 _WaterColor;
            float _DepthPull;
            float _Radius;
            float _SpinSpeed;
            float _NoiseScale;

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
                float2 u = f*f*(3-2*f);
                return lerp(a,b,u.x) + (c-a)*u.y*(1-u.x) + (d-b)*u.x*u.y;
            }

            Varyings vert (Attributes v)
            {
                Varyings o;

                float2 center = float2(0.5, 0.5);
                float2 dir = v.uv - center;
                float dist = length(dir);

                float influence = saturate(1 - dist / _Radius);
                influence = pow(influence, 2.8);

                // xoáy nghiêng vào tâm
                float angle = atan2(dir.y, dir.x);
                angle += _Time.y * _SpinSpeed * influence;

                float n = noise(v.uv * _NoiseScale + _Time.y * 0.05);
                float swirlOffset = (n - 0.5) * 0.15 * influence;

                // LÕM XUỐNG – đây là điểm mấu chốt
                float depression = -influence * influence * _DepthPull;

                v.positionOS.y += depression + swirlOffset;

                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float dist = length(i.uv - center);

                float mask = saturate(1 - dist / _Radius);
                mask = pow(mask, 2.2);

                float alpha = mask * 0.85;

                return half4(_WaterColor.rgb, alpha);
            }
            ENDHLSL
        }
    }
}
