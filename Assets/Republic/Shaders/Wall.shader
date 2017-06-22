Shader "Republic/Wall"
{
    Properties
    {
        _MainTex("Texture", 2D) = ""{}
        _Color("Color", Color) = (1,1,1,1)
        _Metallic("Metallic", Range(0, 1)) = 0
        _Smoothness("Smoothness", Range(0, 1)) = 0
        _Emission("Emission Amplitude", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard nolightmap nolppv
        #pragma target 3.0

        struct Input
        {
            float3 worldPos;
        };

        sampler2D _MainTex;
        float4 _MainTex_ST;

        half4 _Color;
        half _Metallic;
        half _Smoothness;
        half _Emission;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv = IN.worldPos.xy * _MainTex_ST.xy + _MainTex_ST.zw;
            half3 tc = tex2D(_MainTex, uv).rgb;

            o.Albedo = _Color.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
            o.Emission = tc * _Emission;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
