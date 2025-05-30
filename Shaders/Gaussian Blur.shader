Shader "Nexcide/Gaussian Blur" {

    Properties {
        _Spread("Standard Deviation (Spread)", Float) = 1.0
        _GridSize("Grid Size", Integer) = 2
    }

    SubShader {

        Tags {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
        }

        Blend One Zero

        HLSLINCLUDE

        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/PostProcessing/Common.hlsl"

        #define E 2.71828f

        CBUFFER_START(UnityPerMaterial)
            float _Spread;
            int _GridSize;
        CBUFFER_END

        float gaussian(int x) {
            float sigmaSqu = _Spread * _Spread;
            return (1 / sqrt(TWO_PI * sigmaSqu)) * pow(E, -(x*x) / (2 * sigmaSqu));
        }

        ENDHLSL

        Pass {
            Name "Horizontal"

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment FragHorizontal

            float4 FragHorizontal(Varyings input) : SV_Target {
                float3 col = float3(0.0f, 0.0f, 0.0f);
                float gridSum = 0.0f;

                int upper = ((_GridSize - 1) * 0.5f);
                int lower = -upper;

                for (int x = lower; x <= upper; ++x) {
                    float gauss = gaussian(x);
                    gridSum += gauss;
                    float2 uv = input.texcoord;
                    uv.x += (_BlitTexture_TexelSize.x * x);
                    col += gauss * SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv).rgb;
                }

                col /= gridSum;
                return float4(col, 1.0f);
            }

            ENDHLSL
        }

        Pass {
            Name "Vertical"

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment FragVertical

            float4 FragVertical(Varyings input) : SV_Target {
                float3 col = float3(0.0f, 0.0f, 0.0f);
                float gridSum = 0.0f;

                int upper = ((_GridSize - 1) * 0.5f);
                int lower = -upper;

                for (int y = lower; y <= upper; ++y) {
                    float gauss = gaussian(y);
                    gridSum += gauss;
                    float2 uv = input.texcoord;
                    uv.y += (_BlitTexture_TexelSize.y * y);
                    col += gauss * SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, uv).rgb;
                }

                col /= gridSum;
                return float4(col, 1.0f);
            }

            ENDHLSL
        }
    }
}
