Shader "Custom/DeepWater_URP_Fixed"
{
    Properties
    {
        _ShallowColor ("Shallow Color", Color) = (0.12,0.35,0.45,0.9)
        _DeepColor ("Deep Color", Color) = (0.01,0.04,0.08,1)
        _DepthPower ("Depth Power", Range(0.1,6)) = 3.5

        _WaveStrength ("Wave Strength", Range(0,0.1)) = 0.03
        _WaveSpeed ("Wave Speed", Range(0,5)) = 1
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _ShallowColor;
                float4 _DeepColor;
                float _DepthPower;
                float _WaveStrength;
                float _WaveSpeed;
            CBUFFER_END

            Varyings vert (Attributes v)
            {
                Varyings o;

                float wave =
                    sin(v.uv.x * 10 + _Time.y * _WaveSpeed) *
                    sin(v.uv.y * 10 + _Time.y * _WaveSpeed) *
                    _WaveStrength;

                v.positionOS.y += wave;

                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.positionCS);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float2 screenUV = i.screenPos.xy / i.screenPos.w;

                // Lấy depth scene (URP bắt buộc dùng macro này)
                float sceneDepth = LinearEyeDepth(
                    SampleSceneDepth(screenUV), _ZBufferParams);

                float waterDepth = sceneDepth - i.screenPos.w;

                float depthFactor = saturate(pow(waterDepth * 0.25, _DepthPower));

                float4 col = lerp(_ShallowColor, _DeepColor, depthFactor);
                col.a = lerp(0.85, 0.98, depthFactor);

                return col;
            }
            ENDHLSL
        }
    }
}
