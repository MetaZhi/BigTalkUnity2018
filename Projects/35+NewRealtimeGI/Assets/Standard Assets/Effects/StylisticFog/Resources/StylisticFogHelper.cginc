// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

#include "UnityCG.cginc"

half4 _MainTex_TexelSize;

struct v2f_multitex
{
    float4 pos : SV_POSITION;
    float4 uv : TEXCOORD0;
};

v2f_multitex vert_img_fog(appdata_img v)
{
    // Handles vertically-flipped case.
    float vflip = sign(_MainTex_TexelSize.y);

    v2f_multitex o;
    o.pos = UnityObjectToClipPos(v.vertex);
    o.uv.xy = v.texcoord.xy;
    o.uv.zw = (v.texcoord.xy - 0.5) * float2(1, vflip) + 0.5;
    return o;
}

// from https://github.com/keijiro/DepthToWorldPos
float4 DepthToWorld(float depth, float2 uv, float4x4 inverseViewMatrix)
{
    float viewDepth = LinearEyeDepth(depth);
    float2 p11_22 = float2(unity_CameraProjection._11, unity_CameraProjection._22);
    float3 vpos = float3((uv * 2 - 1) / p11_22, -1) * viewDepth;
    float4 wpos = mul(inverseViewMatrix, float4(vpos, 1));
    return wpos;
}

// Interleaved gradient function from Jimenez 2014 http://goo.gl/eomGso
half GradientNoise(float2 uv)
{
    uv = floor(uv * _ScreenParams.xy);
    float f = dot(float2(0.06711056f, 0.00583715f), uv);
    return frac(52.9829189f * frac(f));
}

