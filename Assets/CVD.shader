Shader "Hidden/Custom/CVD"
{
    HLSLINCLUDE
    // StdLib.hlsl holds pre-configured vertex shaders (VertDefault), varying structs (VaryingsDefault), and most of the data you need to write common effects.
    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"
    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
    // Lerp the pixel color with the luminance using the _Blend uniform.      
    // float _Blend;

    float3 R = float3(0.567, 0.433, 0.000);
    float3 G = float3(0.558, 0.442, 0.000);
    float3 B = float3(0.000, 0.242, 0.758);

    float4 Frag(VaryingsDefault i) : SV_Target
    {
        float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
        float r = dot(color.rgb, R);
        float g = dot(color.rgb, G);
        float b = dot(color.rgb, B);
        color.rgb = float3(r, g, b);
        return color;
    }
    ENDHLSL
    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
            #pragma vertex VertDefault
            #pragma fragment Frag
            ENDHLSL
        }
    }
}