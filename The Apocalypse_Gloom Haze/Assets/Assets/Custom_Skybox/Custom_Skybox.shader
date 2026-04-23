Shader "Custom/ProximitySky"
{
    Properties
    {
        // Thêm [HDR] để có thể edit Intensity
        // Cách viết RGB chuẩn
        [HDR]_SkyColor ("Sky Color (RGB)", Color) = (0.1, 0.1, 0.2, 1)
        [HDR]_HorizonColor ("Horizon Color (RGB)", Color) = (0.3, 0.3, 0.4, 1)
        [HDR]_CloudColor ("Cloud Color (RGB)", Color) = (1, 1, 1, 1)
        
        [HDR]_CloudSpeed ("Cloud Speed (RGB)", Float) = 0.1
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes {
                float4 positionOS : POSITION;
            };

            struct Varyings {
                float4 positionCS : SV_POSITION;
                float3 viewDir : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _SkyColor;
                float4 _HorizonColor;
                float4 _CloudColor;
                float _CloudSpeed;
            CBUFFER_END

            // Hàm tạo Noise đơn giản để làm mây
            float hash(float2 p) {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453123);
            }

            float noise(float2 p) {
                float2 i = floor(p);
                float2 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);
                return lerp(lerp(hash(i + float2(0,0)), hash(i + float2(1,0)), f.x),
                            lerp(hash(i + float2(0,1)), hash(i + float2(1,1)), f.x), f.y);
            }

            Varyings vert (Attributes IN) {
                Varyings OUT;
                // Chuyển tọa độ từ Object sang Clip space
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.viewDir = IN.positionOS.xyz; // Vector hướng nhìn chính là tọa độ đỉnh của Skybox
                return OUT;
            }

            float4 frag (Varyings IN) : SV_Target {
                float3 dir = normalize(IN.viewDir);
                
                // 1. Tạo Gradient cho bầu trời
                float skyPoint = pow(max(dir.y, 0.0), 0.5);
                float4 finalColor = lerp(_HorizonColor, _SkyColor, skyPoint);

                // 2. Tạo mây trôi (Dùng Noise + Time)
                float2 uvCloud = dir.xz / (dir.y + 0.01); // Projection lên mặt phẳng
                uvCloud += _Time.y * _CloudSpeed; // Mây trôi theo thời gian
                
                float cloudAlpha = noise(uvCloud * 2.0) * noise(uvCloud * 4.0);
                cloudAlpha *= smoothstep(0.1, 0.5, dir.y); // Chỉ hiện mây ở trên cao

                finalColor = lerp(finalColor, _CloudColor, cloudAlpha * 0.5);

                // Tạo hiệu ứng chớp giật ngẫu nhiên
                float lightning = sin(_Time.y * 10.0) * cos(_Time.y * 33.0);
                lightning = smoothstep(0.8, 1.0, lightning); // Chỉ lấy những lúc giá trị cực cao

                // Thêm ánh sáng chớp vào màu trời (nhân với màu trắng/xanh nhạt)
                finalColor.rgb += lightning * float3(0.8, 0.9, 1.0) * 0.5;

                return finalColor;
            }
            ENDHLSL
        }
    }
}