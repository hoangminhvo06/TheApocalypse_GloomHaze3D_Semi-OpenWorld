Shader "Custom/Water_SurfaceRipples_Fog_URP"
{
    Properties
    {
        [Header(Ripples)]
        _RippleColor ("Ripple Color", Color) = (0.08, 0.18, 0.22, 1)
        _Alpha ("Ripple Alpha", Range(0,1)) = 0.22

        _RippleScale ("Ripple Density", Float) = 18
        _RippleSpeed ("Ripple Speed", Float) = 0.18
        _RippleStrength ("Ripple Height", Range(0,0.15)) = 0.045
        _Sharpness ("Ripple Sharpness", Range(1,6)) = 3.5

        [Header(Fog)]
        _FogColor ("Fog Color", Color) = (0.25, 0.25, 0.25, 1)
        _FogDensity ("Fog Density", Range(0,1)) = 0.35
        _FogSpeed ("Fog Speed", Range(0,1)) = 0.08
        _FogScale ("Fog Scale", Float) = 2.5
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

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float4 _RippleColor;
            float _Alpha;
            float _RippleScale;
            float _RippleSpeed;
            float _RippleStrength;
            float _Sharpness;

            float4 _FogColor;
            float _FogDensity;
            float _FogSpeed;
            float _FogScale;

            // noise nhẹ cho fog
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
                return lerp(a,b,u.x) + (c-a)*u.y*(1-u.x) + (d-b)*u.x*u.y;
            }

            Varyings vert (Attributes v)
            {
                Varyings o;

                float t = _Time.y * _RippleSpeed;

                float waveX = sin((v.uv.x + t) * _RippleScale);
                float waveY = sin((v.uv.y - t * 0.8) * _RippleScale);

                float ripple = (waveX + waveY) * 0.5;
                ripple = sign(ripple) * pow(abs(ripple), _Sharpness);

                v.positionOS.y += ripple * _RippleStrength;

                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = v.uv;
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float t = _Time.y * _RippleSpeed;

                float waveX = sin((i.uv.x + t) * _RippleScale);
                float waveY = sin((i.uv.y - t * 0.8) * _RippleScale);

                float ripple = (waveX + waveY) * 0.5;
                ripple = sign(ripple) * pow(abs(ripple), _Sharpness);

                float rippleMask = saturate(abs(ripple));

                // Ripple color
                float3 rippleCol =
                    _RippleColor.rgb * (0.6 + rippleMask * 0.5);

                float rippleAlpha =
                    _Alpha * (0.4 + rippleMask * 0.6);

                // Fog layer
                float2 fogUV =
                    i.uv * _FogScale +
                    float2(_Time.y * _FogSpeed, _Time.y * _FogSpeed * 0.6);

                float fogNoise = noise(fogUV);
                float fogMask = smoothstep(0.4, 0.75, fogNoise) * _FogDensity;

                float3 finalColor =
                    lerp(rippleCol, _FogColor.rgb, fogMask);

                float finalAlpha =
                    saturate(rippleAlpha + fogMask * 0.25);

                return float4(finalColor, finalAlpha);
            }
            ENDHLSL
        }
    }
}
