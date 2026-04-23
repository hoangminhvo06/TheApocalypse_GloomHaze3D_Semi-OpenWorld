Shader "Custom/WaterFog_SimpleFlow_URP"
{
    Properties
    {
        _FogColor ("Fog Color", Color) = (0.65,0.65,0.65,1)
        _FogStrength ("Fog Strength", Range(0,1)) = 0.25
        _NoiseScale ("Noise Scale", Range(1,50)) = 18
        _FlowSpeed ("Flow Speed", Range(0,0.05)) = 0.008
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

            float4 _FogColor;
            float _FogStrength;
            float _NoiseScale;
            float _FlowSpeed;

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
                float2 u = f * f * (3 - 2 * f);
                return lerp(a, b, u.x) + (c - a) * u.y * (1 - u.x) + (d - b) * u.x * u.y;
            }

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float2 uv = i.uv * _NoiseScale;
                uv.x += _Time.y * _FlowSpeed;   // trôi ngang
                uv.y += _Time.y * (_FlowSpeed * 0.5);

                float n = noise(uv);

                float alpha = n * _FogStrength;

                return half4(_FogColor.rgb, alpha);
            }
            ENDHLSL
        }
    }
}
