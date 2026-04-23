Shader "Custom/Realistic_Water_URP"
{
    Properties
    {
        _DeepColor ("Deep Water Color", Color) = (0.01, 0.05, 0.1, 1)
        _ShallowColor ("Shallow Water Color", Color) = (0.1, 0.3, 0.3, 1)
        _Glossiness ("Smoothness", Range(0,1)) = 0.9
        _Metallic ("Metallic (Reflection)", Range(0,1)) = 0.8
        
        [Header(Wave Control)]
        _NormalMap ("Normal Map (Water Ripples)", 2D) = "bump" {}
        _NormalStrength ("Normal Strength", Range(0, 2)) = 0.5
        _WaveSpeed ("Wave Speed", Vector) = (0.05, 0.05, 0, 0)
        
        [Header(Depth and Transparency)]
        _DepthDistance ("Depth Fade Distance", Float) = 5
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" "Queue"="Transparent" "RenderType"="Transparent" }
        
        Pass
        {
            ZWrite On
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _DeepColor, _ShallowColor;
                float _Glossiness, _Metallic, _NormalStrength, _DepthDistance;
                float4 _WaveSpeed;
            CBUFFER_END

            TEXTURE2D(_NormalMap); SAMPLER(sampler_NormalMap);

            Varyings vert(Attributes IN) {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                OUT.screenPos = ComputeScreenPos(OUT.positionCS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target {
                // 1. Tính toán độ sâu (Depth) để tạo màu nước mờ ảo
                float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
                float rawDepth = SampleSceneDepth(screenUV);
                float sceneZ = LinearEyeDepth(rawDepth, _ZBufferParams);
                float waterZ = IN.screenPos.w;
                float depthAlpha = saturate((sceneZ - waterZ) / _DepthDistance);

                // 2. Normal Mapping (Đây là thứ tạo ra sự óng ánh)
                float2 timeUV = IN.uv + _Time.y * _WaveSpeed.xy;
                float3 normalSample = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, timeUV));
                float3 normalWS = normalize(TransformObjectToWorldNormal(normalSample));
                normalWS = lerp(float3(0, 1, 0), normalWS, _NormalStrength);

                // 3. Phản xạ Skybox (Crucial for Realism)
                float3 viewDirWS = normalize(GetWorldSpaceViewDir(IN.positionWS));
                float3 reflectDir = reflect(-viewDirWS, normalWS);
                half3 reflection = GlossyEnvironmentReflection(reflectDir, 1.0 - _Glossiness, 1.0);

                // 4. Fresnel Effect (Góc nhìn nghiêng thì thấy phản xạ nhiều hơn)
                float fresnel = pow(1.0 - saturate(dot(normalWS, viewDirWS)), 5.0);

                // 5. Lighting & Color Mix
                float3 baseColor = lerp(_ShallowColor.rgb, _DeepColor.rgb, depthAlpha);
                
                // Mix màu nước với sự phản xạ của bầu trời
                float3 finalColor = lerp(baseColor, reflection, fresnel * _Metallic);
                
                // Thêm Specular từ mặt trời (Directional Light)
                Light mainLight = GetMainLight();
                float3 halfDir = normalize(mainLight.direction + viewDirWS);
                float spec = pow(saturate(dot(normalWS, halfDir)), 128.0) * _Glossiness;
                finalColor += mainLight.color * spec;

                return half4(finalColor, lerp(0.5, 1.0, depthAlpha));
            }
            ENDHLSL
        }
    }
}