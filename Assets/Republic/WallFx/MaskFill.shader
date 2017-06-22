Shader "Hidden/Republic/WallFx/MaskFill"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _MaskTex("", 2D) = "" {}
        _Color("", Color) = (1, 1, 1, 1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    sampler2D _MaskTex;
    half4 _Color;

    half4 frag(v2f_img i) : SV_TARGET
    {
        half4 src = tex2D(_MainTex, i.uv);
        half mask = tex2D(_MaskTex, i.uv).r;
        return lerp(src, _Color, mask * _Color.a);
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
            #pragma target 3.0
            ENDCG
        }
    }
}
