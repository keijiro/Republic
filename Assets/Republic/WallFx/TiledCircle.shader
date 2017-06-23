Shader "Hidden/Republic/WallFx/TiledCircle"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _Color("", Color) = (1, 1, 1, 1)
    }

    CGINCLUDE

    #include "Common.hlsl"

    sampler2D _MainTex;

    half4 _Color;
    float2 _Density;
    float _Repeat;
    float _Displace;

    float _AAFactor;
    int _randomSeed;

    half4 frag(v2f_img i) : SV_TARGET
    {
        float2 uv = (i.uv - 0.5) * _Density;
        int id = _randomSeed + (int)uv.x + (int)uv.y * _Density.x;

        uint hash = Hash(id) & 3;
        float2 d1 = float2((hash == 0) * 2 - 1, 0);
        float2 d2 = float2(0, (hash == 1) * 2 - 1);
        float2 dir = lerp(d1, d2, (hash & 1) == 0);

        float2 center = floor(uv) + 0.5 + dir * _Displace;
        float d = length(uv - center);

        half alpha = abs(0.5 - frac(d * _Repeat)) * 2;
        alpha = saturate((alpha - 0.5) * _AAFactor);

        half4 src = tex2D(_MainTex, i.uv);
        return lerp(src, _Color, alpha);
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
