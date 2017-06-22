Shader "Hidden/Republic/WallFx/SliceScroll"
{
    CGINCLUDE

    #include "Common.hlsl"

    float _Density;
    int _RandomSeed;
    float _Progress;
    float _Threshold;

    half4 frag(v2f_img i) : SV_TARGET
    {
        float2 uv = i.uv * float2(_Density, _ScreenParams.y);
        float speed = 0.5 + 2 * Random(_RandomSeed + (int)uv.y);
        return frac(uv.x + speed * _Density * _Progress) < _Threshold;
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
