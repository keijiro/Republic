Shader "Hidden/Republic/WallFx/RandomBlock"
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
    int _RandomSeed;

    half4 frag(v2f_img i) : SV_TARGET
    {
        float2 uv = i.uv * _Dimension;

        int id = (int)uv.x + (int)uv.y * (int)_Dimension.x;

        float pt = Random(_RandomSeed + id);
        float alpha = pt < _Progress;

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
