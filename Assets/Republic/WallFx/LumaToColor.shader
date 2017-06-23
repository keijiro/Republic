Shader "Hidden/Republic/WallFx/LumaToColor"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _Color1("", Color) = (1, 1, 1, 1)
        _Color2("", Color) = (1, 1, 1, 1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    half4 _Color1;
    half4 _Color2;
    half _Blend;

    half4 frag(v2f_img i) : SV_TARGET
    {
        half4 src = tex2D(_MainTex, i.uv);
        half lum = dot(src.rgb, 1.0 / 3);
        half4 cout = lerp(_Color1, _Color2, lum);
        return lerp(src, cout, _Blend);
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
