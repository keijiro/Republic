Shader "Hidden/Republic/PostFx"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _LineColor("", Color) = (0, 0, 0, 1)
        _FillColor1("", Color) = (0, 0, 1)
        _FillColor2("", Color) = (1, 0, 0)
        _FillColor3("", Color) = (1, 1, 1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _CameraGBufferTexture2;

    sampler2D _MainTex;
    float4 _MainTex_TexelSize;

    float4 _LineColor;
    float3 _FillColor1;
    float3 _FillColor2;
    float3 _FillColor3;
    float2 _Threshold;

    fixed RobertsCross(sampler2D tex, float2 uv)
    {
        float4 duv = float4(0, 0, _MainTex_TexelSize.xy);
        half n11 = tex2D(tex, uv + duv.xy).g;
        half n12 = tex2D(tex, uv + duv.zy).g;
        half n21 = tex2D(tex, uv + duv.xw).g;
        half n22 = tex2D(tex, uv + duv.zw).g;
        half g = length(half2(n11 - n22, n12 - n21));
        return saturate((g - _Threshold.x) * _Threshold.y);
    }

    fixed4 frag(v2f_img i) : SV_Target
    {
        fixed edge = RobertsCross(_CameraGBufferTexture2, i.uv);
        fixed luma = dot(tex2D(_MainTex, i.uv).rgb, 1.0 / 3);
        fixed3 fill = luma > 0.66 ? _FillColor3 : (luma > 0.33 ? _FillColor2 : _FillColor1);
        return fixed4(lerp(fill, _LineColor.rgb, edge * _LineColor.a), 1);
    }

    ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            ENDCG
        }
    }
}
