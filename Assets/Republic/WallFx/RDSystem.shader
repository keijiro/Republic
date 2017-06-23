Shader "Hidden/Republic/WallFx/RDSystem"
{
    Properties
    {
        _MainTex("", 2D) = "" {}
        _StateTex("", 2D) = "" {}
    }

    CGINCLUDE

    #include "Common.hlsl"

    sampler2D _MainTex;
    float4 _MainTex_TexelSize;

    // Initialization

    float _SeedProb;
    uint _RandomSeed;

    half4 fragInit(v2f_img i) : SV_TARGET
    {
        float2 coord = i.uv * _ScreenParams.xy;
        uint id = coord.x + coord.y * _ScreenParams.x;
        return half4(1, Random(_RandomSeed + id) < _SeedProb, 0, 0);
    }

    // Rehashing

    float _RehashProb;

    half4 fragRehash(v2f_img i) : SV_TARGET
    {
        half2 q = tex2D(_MainTex, i.uv).xy;
        uint id = (uint)(i.uv.x * 20) + (uint)(i.uv.y * 20) * 20;
        q.y *= Random(_RandomSeed + id) > _RehashProb;
        return half4(q, 0, 0);
    }

    // State update

    half _Du, _Dv;
    half _Feed, _Kill;

    half4 fragUpdate(v2f_img i) : SV_TARGET
    {
        float4 duv = _MainTex_TexelSize.xyxy * float4(1, 1, -1, 0);

        half2 q = tex2D(_MainTex, i.uv).xy;

        half2 dq = -q;
        dq += tex2D(_MainTex, i.uv - duv.xy).xy * 0.05;
        dq += tex2D(_MainTex, i.uv - duv.wy).xy * 0.20;
        dq += tex2D(_MainTex, i.uv - duv.zy).xy * 0.05;
        dq += tex2D(_MainTex, i.uv + duv.zw).xy * 0.20;
        dq += tex2D(_MainTex, i.uv + duv.xw).xy * 0.20;
        dq += tex2D(_MainTex, i.uv + duv.zy).xy * 0.05;
        dq += tex2D(_MainTex, i.uv + duv.wy).xy * 0.20;
        dq += tex2D(_MainTex, i.uv + duv.xy).xy * 0.05;

        half ABB = q.x * q.y * q.y;

        q += float2(dq.x * _Du - ABB + _Feed * (1 - q.x),
                    dq.y * _Dv + ABB - (_Kill + _Feed) * q.y);

        return half4(saturate(q), 0, 0);
    }

    // Rendering

    sampler2D _StateTex;
    half _Threshold;
    half _Fading;
    half4 _Color;

    half4 fragRender(v2f_img i) : SV_TARGET
    {
        half p = tex2D(_StateTex, i.uv).y;
        p = smoothstep(_Threshold, _Threshold + _Fading, p);
        half4 src = tex2D(_MainTex, i.uv);
        return lerp(src, _Color, _Color.a * p);
    }

    ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment fragInit
            #pragma target 3.0
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment fragRehash
            #pragma target 3.0
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment fragUpdate
            #pragma target 3.0
            ENDCG
        }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment fragRender
            #pragma target 3.0
            ENDCG
        }
    }
}
