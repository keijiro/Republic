Shader "Hidden/Republic/WallFx/WavyStripe"
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

    float _Frequency;
    float _Rows;

    float _WaveScroll;
    float _WaveAmplitude;
    float4 _Rotation;

    half4 frag(v2f_img i) : SV_TARGET
    {
        float2 uv = i.uv - 0.5;
        uv = float2(dot(_Rotation.xy, uv), dot(_Rotation.zw, uv));

        uv.x = uv.x * _Frequency + _WaveScroll;
        uv.y = (uv.y + sin(uv.x) * _WaveAmplitude) * _Rows;

        half4 c = frac(uv.y) < 0.5 ? _Color1 : _Color2;

        half4 src = tex2D(_MainTex, i.uv);
        return lerp(src, c, c.a);
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
