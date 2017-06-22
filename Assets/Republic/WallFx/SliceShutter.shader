Shader "Hidden/Republic/WallFx/Shutter"
{
    CGINCLUDE

    #include "Common.hlsl"

    float _Columns;
    float _Progress;
    int _RandomSeed;

    half4 frag(v2f_img i) : SV_TARGET
    {
        float2 uv = (i.uv - 0.5) * float2(_Columns, _ScreenParams.y);
        int id = (int)uv.x + (int)uv.y * (int)_Columns;

        float thresh = (Random(_RandomSeed + id) + 1) * (1 - _Progress);
        return abs(frac(uv.x) - 0.5) * 2 > thresh;
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
