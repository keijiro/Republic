Shader "Hidden/Republic/WallFx/TextureOverlay"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _OverlayTex("", 2D) = "" {}
        _Color("", Color) = (1, 1, 1, 1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    sampler2D _OverlayTex;
    half4 _Color;

    half4 frag(v2f_img i) : SV_TARGET
    {
        half4 src = tex2D(_MainTex, i.uv);
        half ovr = tex2D(_OverlayTex, i.uv).a;
        return lerp(src, _Color, ovr * _Color.a);
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
