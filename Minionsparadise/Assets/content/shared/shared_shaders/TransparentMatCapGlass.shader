Shader "Kampai/Transparent/Glass" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB)", 2D) = "gray" { }
        _MatCap ("MatCap (RGB)", 2D) = "gray" { }
        _Boost ("Boost", Float) = 1
        [PerRendererData] _FadeAlpha ("Fade Alpha", Range(0,1)) = 1
    }

    SubShader { 
        Tags { "QUEUE"="Transparent" }
        Pass {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uvMain : TEXCOORD0;
                float2 uvMatCap : TEXCOORD1;
            };

            sampler2D _MainTex; float4 _MainTex_ST;
            sampler2D _MatCap;
            float4 _Color;
            float _Boost;
            float _FadeAlpha;

            v2f vert (appdata v) {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                
                // Dans le code original, ils omettent _MainTex_ST.zw (l'offset)
                o.uvMain = v.uv * _MainTex_ST.xy;
                
                // Calcul Matcap classique
                float3 viewNormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                o.uvMatCap = normalize(viewNormal).xy * 0.5 + 0.5;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed3 matcap = tex2D(_MatCap, i.uvMatCap).rgb;
                fixed maskAlpha = tex2D(_MainTex, i.uvMain).r;
                
                fixed3 finalRGB = matcap * _Boost * _Color.rgb;
                fixed finalAlpha = maskAlpha * _Color.a * _FadeAlpha;
                
                return fixed4(finalRGB, finalAlpha);
            }
            ENDCG
        }
    }
}