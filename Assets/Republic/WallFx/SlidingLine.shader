Shader "Hidden/Republic/WallFx/SlidingLine"
{
    CGINCLUDE

    #include "UnityCG.cginc"
    #include "SimplexNoise2D.hlsl"

    float _Density;
    float _Offset;
    float _Thickness;

    half4 frag(v2f_img i) : SV_TARGET
    {
        float2 uv = i.uv - 0.5;

        float row = floor(uv.y * _ScreenParams.y + 0.5);
        float x = uv.x * _Density + row * 100;

        float n;
        n  = snoise(float2(x * 1, _Offset)).z;
        n += snoise(float2(x * 2, _Offset)).z * 0.5;

        float thresh = _Thickness * 1.4;
        return (-thresh < n) * (n < thresh);
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
