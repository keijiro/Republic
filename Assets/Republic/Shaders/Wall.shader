Shader "Republic/Wall"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _Metallic("Metallic", Range(0, 1)) = 0
        _Smoothness("Smoothness", Range(0, 1)) = 0

        [Header(Emission)]
        _MainTex("Texture", 2D) = ""{}
        _Emission("Amplitude", Float) = 1

        [Header(Projection)]
        [Toggle] _EnableProjection("Enable", Float) = 1
        _ProjectionScaleX("Scale X", Float) = 0.1875
        _ProjectionScaleY("Scale Y", Float) = 0.3333
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM

        #pragma surface surf Standard nolightmap nolppv
        #pragma target 3.0

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half4 _Color;
        half _Metallic;
        half _Smoothness;

        sampler2D _MainTex;
        half _Emission;

        float _EnableProjection;
        float _ProjectionScaleX;
        float _ProjectionScaleY;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float2 uv1 = IN.uv_MainTex;
            float2 uv2 = IN.worldPos.xy;
            uv2 = uv2 * float2(_ProjectionScaleX, _ProjectionScaleY) + 0.5;

            half3 tc = tex2D(_MainTex, lerp(uv1, uv2, _EnableProjection)).rgb;

            o.Albedo = _Color.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
            o.Emission = tc * _Emission;
        }

        ENDCG
    }
    FallBack "Diffuse"
}
