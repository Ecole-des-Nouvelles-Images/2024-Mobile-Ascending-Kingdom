Shader "Elk/HandPaintedV4+Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Paint Mask", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _OutlineWidth ("Outline Width", Float) = 0.1
        _OutlineScale ("Outline Scale", Float) = 1.0
        _OutlineColor ("Outline Color", Color) = (1,1,1,1)
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _PaintColor ("Paint Color", Color) = (1,1,1,1)
        _NormalIntensity ("Normal Intensity", Range(0, 1)) = 1
        _MaskOpacity ("Mask Opacity", Range(0, 1)) = 1
        _AmbientLight ("Ambient Light", Color) = (0.1, 0.1, 0.1, 1) // Ajout de la lumière ambiante
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_MaskTex);
            SAMPLER(sampler_MaskTex);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 normalWS     : TEXCOORD1;
                float3 viewDirWS    : TEXCOORD2;
                float4 fogCoord     : TEXCOORD3;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _MaskTex_ST;
                float4 _NormalMap_ST;
                float _OutlineWidth;
                float _OutlineScale;
                float4 _OutlineColor;
                float4 _BaseColor;
                float4 _PaintColor;
                float _NormalIntensity;
                float _MaskOpacity;
                float4 _AmbientLight; // Ajout de la lumière ambiante
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.viewDirWS = GetWorldSpaceViewDir(output.positionHCS);
                output.fogCoord = ComputeFogFactor(input.positionOS);
                return output;
            }

            // Simple Lambertian lighting function with ambient light
            float3 LightingLambert(float3 normal, float3 lightDir, float3 lightColor, float3 ambientLight)
            {
                float3 N = normalize(normal);
                float3 L = normalize(lightDir);
                float diff = max(dot(N, L), 0.0);
                return lightColor * diff + ambientLight;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                float4 maskColor = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, input.uv);
                float4 normalMapColor = SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, input.uv);

                // Convert normal map to world space normal
                float3 normal = UnpackNormal(normalMapColor).rgb;
                normal = mul(normal, (float3x3)UNITY_MATRIX_IT_MV).rgb;

                // Apply normal intensity progressively
                float3 originalNormal = normalize(input.normalWS);
                normal = normalize(normal) * _NormalIntensity + originalNormal * (1.0 - _NormalIntensity);

                // Simple Lambertian lighting with a single light source and ambient light
                float3 lightDir = normalize(float3(0.5, 0.5, -0.5)); // Example light direction
                float3 lightColor = float3(1.0, 1.0, 1.0); // Example light color
                float3 ambientLight = _AmbientLight.rgb; // Ajout de la lumière ambiante
                float3 lighting = LightingLambert(normal, lightDir, lightColor, ambientLight);

                // Mix base color and paint color based on mask
                float3 baseColor = _BaseColor.rgb * texColor.rgb;
                float3 paintColor = _PaintColor.rgb * maskColor.rgb;
                float3 finalColor = lerp(baseColor, paintColor, maskColor.a * _MaskOpacity) * lighting;

                return half4(finalColor, texColor.a);
            }
            ENDHLSL
        }

        // Outline Pass
        Pass
        {
            Name "Outline"
            Cull Front
            Tags { "LightMode" = "SRPDefaultUnlit" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
                float4 fogCoord     : TEXCOORD0;
            };

            CBUFFER_START(UnityPerMaterial)
                float _OutlineWidth;
                float _OutlineScale;
                float4 _OutlineColor;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);

                // Calculate the scale based on the distance from the original position
                float3 scaledNormal = input.normalOS * _OutlineWidth * _OutlineScale;
                float3 scaledPosition = input.positionOS.xyz + scaledNormal;
                output.positionHCS = TransformObjectToHClip(float4(scaledPosition, 1));
                output.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float3 finalColor = MixFog(_OutlineColor, input.fogCoord);
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }

        UsePass "Universal Render Pipeline/Lit/ShadowCaster"
    }
    FallBack "Diffuse"
}
