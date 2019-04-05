Shader "Hidden/Image Effects/StylisticFog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    CGINCLUDE
    #include "UnityCG.cginc"
    #include "StylisticFogHelper.cginc"

    #define FOG_HANDLE_GAMMA_CORRECTION
    #define SKYBOX_THREASHOLD_VALUE 0.9999
    #define FOG_AMOUNT_CONTRIBUTION_THREASHOLD 0.0001

    // x: Apply distance fog. y: Apply height fog. z: Distance fog applies to skybox. w: Height fog applies to skybox.
    bool4 _FogModeSettings;
    // x: Fog end distance. y: Fog Height. z: Base density. w: Density Falloff.
    float4 _FogParameters;

    bool _SharedColor;

    sampler2D _MainTex;
    sampler2D _CameraDepthTexture;
    sampler2D _FogColorTexture0;
    sampler2D _FogColorTexture1;
    float4x4 _InverseViewMatrix;

    // Computes the amount of fog treversed based on a desnity function d(h)
    // where d(h) = _BaseDensity * exp2(-DensityFalloff * h) <=> d(h) = a * exp2(b * h)
    half ComputeHeightFogAmount(float viewDirY, float effectiveDistance)
    {
        float relativeHeight = _WorldSpaceCameraPos.y - _FogParameters.y;
        return saturate(_FogParameters.z * (exp2(-relativeHeight * _FogParameters.w) - exp2((-effectiveDistance * viewDirY - relativeHeight) * _FogParameters.w)) / viewDirY);
    }

    half4 GetColorFromTexture(sampler2D source, float fogAmount)
    {
        half4 textureColor = tex2D(source, float2(fogAmount, 0));
#if defined(UNITY_COLORSPACE_GAMMA) && defined(FOG_HANDLE_GAMMA_CORRECTION)
        textureColor.rgb = GammaToLinearSpace(textureColor.rgb);
#endif
        return textureColor;
    }

    half4 FragmentFog(v2f_multitex i) : SV_Target
    {
        half4 sceneColor = min(1., tex2D(_MainTex, i.uv.xy));
#if defined(UNITY_COLORSPACE_GAMMA) && defined(FOG_HANDLE_GAMMA_CORRECTION)
        sceneColor.rgb = GammaToLinearSpace(sceneColor.rgb);
#endif
        float depth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv.zw);
        float4 wpos = DepthToWorld(depth, i.uv.zw, _InverseViewMatrix);
        float4 cameraToFragment = wpos - float4(_WorldSpaceCameraPos, 1.);
        float totalDistance = length(cameraToFragment);
        half3 viewDirection = normalize(cameraToFragment);

        half onSkybox = Linear01Depth(depth) < SKYBOX_THREASHOLD_VALUE;
        half2 fogAmount = 0.; // x is distance fog, y is height fog

        if (_FogModeSettings.x)
            fogAmount.x = saturate(totalDistance / _FogParameters.x) * max(_FogModeSettings.z, onSkybox);

        if (_FogModeSettings.y)
            fogAmount.y = ComputeHeightFogAmount(viewDirection.y, totalDistance) * max(_FogModeSettings.w, onSkybox);

        half4 fogColor;
        if (_SharedColor)
        {
            half fogFactor = saturate(fogAmount.x + fogAmount.y);
            fogColor = GetColorFromTexture(_FogColorTexture0, fogFactor) * step(FOG_AMOUNT_CONTRIBUTION_THREASHOLD, fogFactor);
        } 
        else
        {
            half4 distanceFogColor = GetColorFromTexture(_FogColorTexture0, fogAmount.x) * step(FOG_AMOUNT_CONTRIBUTION_THREASHOLD, fogAmount.x);
            half4 heightFogColor = GetColorFromTexture(_FogColorTexture1, fogAmount.y) * step(FOG_AMOUNT_CONTRIBUTION_THREASHOLD, fogAmount.y);
            fogColor = lerp(distanceFogColor, heightFogColor, mad(fogAmount.y - fogAmount.x, .5, .5));
            fogColor.a = saturate(distanceFogColor.a + heightFogColor.a);
        }

        half blendFactor = fogColor.a;
        blendFactor += mad(GradientNoise(i.uv.xy), 2., - 1.) * 0.01 * step(FOG_AMOUNT_CONTRIBUTION_THREASHOLD, fogColor.a);;

        half3 blended = lerp(sceneColor.xyz, half4(fogColor.xyz, 1.), saturate(blendFactor));
#if defined(UNITY_COLORSPACE_GAMMA) && defined(FOG_HANDLE_GAMMA_CORRECTION)
        blended.rgb = LinearToGammaSpace(blended.rgb);
#endif
        return half4(blended, 1.);
    }

    ENDCG
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma multi_compile __ UNITY_COLORSPACE_GAMMA
            #pragma vertex vert_img_fog
            #pragma fragment FragmentFog
            ENDCG
        }
    }
}
