Shader "Hidden/Republic/WallFx/CombNoise"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
    }

    CGINCLUDE

    #include "UnityCG.cginc"
    #include "SimplexNoise2D.hlsl"

    sampler2D _MainTex;

    half3 _Color;
    float _Density;
    float _Speed;
    float _Thickness;
    float _RowRepeat;

    half4 frag(v2f_img i) : SV_TARGET
    {
        float2 uv = i.uv - 0.5;

        float row = floor(uv.y * _RowRepeat + 0.5);

        float x = uv.x * _Density + row * 100;
        float t = _Time.y * _Speed;

        float n;
        n  = snoise(float2(x * 1, t)).z;
        n += snoise(float2(x * 2, t)).z * 0.5;

        float thresh = _Thickness * 1.4;
        n = (-thresh < n) * (n < thresh);

        half4 csrc = tex2D(_MainTex, i.uv);
        half3 cout = _Color * n;

        return half4(csrc.rgb + cout, csrc.a);
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
