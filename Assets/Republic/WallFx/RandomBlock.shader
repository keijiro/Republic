Shader "Hidden/Republic/WallFx/RandomBlock"
{
    CGINCLUDE

    #include "Common.hlsl"

    float _Progress;
    int _RandomSeed;

    half4 frag(v2f_img i) : SV_TARGET
    {
        float2 uv = i.uv * _ScreenParams.xy;
        int id = (int)uv.x + (int)uv.y * (int)_ScreenParams.x;
        return Random(_RandomSeed + id) < _Progress;
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
