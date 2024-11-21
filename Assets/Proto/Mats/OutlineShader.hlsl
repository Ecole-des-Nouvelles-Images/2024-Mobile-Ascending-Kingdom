#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

CBUFFER_START(Properties)
    float _OutlineWidth;
float4 _OutlineColor;
CBUFFER_END

float4 OutlineVert(float4 vertex, float3 normal)
{
    VertexPositionInputs vertexInput = GetVertexPositionInputs(vertex.xyz);
    float4 positionHCS = TransformObjectToHClip(float4(vertex.xyz + normal * _OutlineWidth * 0.1, 1));
    float4 fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
    return float4(positionHCS, fogCoord);
}

float4 OutlineFrag(float4 fogCoord)
{
    float3 finalColor = MixFog(_OutlineColor, fogCoord);
    return float4(finalColor, 1.0);
}
