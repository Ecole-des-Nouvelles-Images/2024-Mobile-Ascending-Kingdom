#pragma multi_compile_fog
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

struct appdata
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
    float4 tangent : TANGENT;
};

struct v2f
{
    float4 positionHCS : SV_POSITION;
    float4 fogCoord : TEXCOORD0;
};

CBUFFER_START(Properties)
    float _OutlineWidth;
float4 _OutlineColor;
CBUFFER_END

v2f OutlineVert(appdata v)
{
    v2f o;
    VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
    o.positionHCS = TransformObjectToHClip(float4(v.vertex.xyz + v.normal * _OutlineWidth * 0.1, 1));
    o.fogCoord = ComputeFogFactor(vertexInput.positionCS.z);
    return o;
}

float3 OutlineFrag(v2f i) : SV_Target
{
    float3 finalColor = MixFog(_OutlineColor, i.fogCoord);
    return finalColor;
}
