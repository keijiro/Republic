Shader "Hidden/Republic/WallFx/DiagZoom"
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
    float _Density;
    float _Thickness;
    float _Progress;

    half4 frag(v2f_img i) : SV_TARGET
    {
        float2 uv = (i.uv - 0.5) * float2(16.0 / 9, 1);
        uv = mul(float2x2(0.707, -0.707, 0.707, 0.707), uv);
        uv = abs(uv * 2 * _Density) + _Progress;

        float lh = frac(uv.y) < _Thickness;
        float lv = frac(uv.x) < _Thickness;
        float alpha = lerp(lh, lv, uv.x > uv.y);

        half4 src = tex2D(_MainTex, i.uv);
        return lerp(src, _Color, alpha * _Color.a);
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
