Shader "Hidden/Republic/WallFx/Shutter"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _Color("", Color) = (1, 1, 1)
    }

    CGINCLUDE

    #include "Common.hlsl"

    sampler2D _MainTex;

    half4 _Color;
    float2 _Dimension;
    float _Progress;
    int _Step;

    half4 frag(v2f_img i) : SV_TARGET
    {
        float2 uv = (i.uv - 0.5) * _Dimension;
        float offs = Random(_Step + (int)uv.x + (int)uv.y * (int)_Dimension.x);
        float alpha = abs(frac(uv.x) - 0.5) * 2 - _Progress * (1 + offs) > 0;

        half4 csrc = tex2D(_MainTex, i.uv);
        return lerp(csrc, _Color, alpha * _Color.a);
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
