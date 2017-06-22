Shader "Hidden/Republic/WallFx/SimpleFill"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _Color("", Color) = (1, 1, 1, 1)
    }

    CGINCLUDE

    #include "UnityCG.cginc"

    sampler2D _MainTex;
    half4 _Color;

    half4 frag(v2f_img i) : SV_TARGET
    {
        half4 csrc = tex2D(_MainTex, i.uv);
        return lerp(csrc, _Color, _Color.a);
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
