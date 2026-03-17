Shader "Kampai/Transparent/Glass_Unity53" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Mask (A)", 2D) = "white" { }
        _MatCap ("MatCap (RGB)", 2D) = "gray" { }
        _Boost ("Boost", Float) = 1
        [PerRendererData] _FadeAlpha ("Fade Alpha", Range(0,1)) = 1
    }

    SubShader { 
        // L'ordre de rendu est CRUCIAL ici
        Tags { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True"
        }
        
        Pass {
            ZWrite Off
            Cull Off 
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc" // Corrigé ici

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

            sampler2D _MainTex; 
            float4 _MainTex_ST;
            sampler2D _MatCap;
            float4 _Color;
            float _Boost;
            float _FadeAlpha;

            v2f vert (appdata v) {
                v2f o;
                // Version Unity 5.3 standard
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                
                // On applique le Tiling/Offset de la texture
                o.uvMain = TRANSFORM_TEX(v.uv, _MainTex);
                
                // Calcul Matcap compatible Unity 5.x
                float3 viewNormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
                o.uvMatCap = normalize(viewNormal).xy * 0.5 + 0.5;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 mainTex = tex2D(_MainTex, i.uvMain);
                fixed3 matcap = tex2D(_MatCap, i.uvMatCap).rgb;
                
                // Calcul de la couleur finale
                fixed3 finalRGB = matcap * mainTex.rgb * _Boost * _Color.rgb;
                
                // GESTION DE L'ALPHA (Le point sensible)
                // Si ta texture a un fond noir et pas d'alpha, utilise mainTex.r
                // Si ta texture a de la vraie transparence, utilise mainTex.a
                fixed finalAlpha = mainTex.a * _Color.a * _FadeAlpha;
                
                return fixed4(finalRGB, finalAlpha);
            }
            ENDCG
        }
    }
}