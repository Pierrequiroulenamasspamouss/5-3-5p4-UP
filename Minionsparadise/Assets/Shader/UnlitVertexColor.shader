// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Kampai/Standard/Vertex Color" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _boost ("Boost", Float) = 0
    }
    SubShader { 
        Tags { "RenderType"="Opaque" }
        Pass {
            Name "FORWARDBASE"
            Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex; float4 _MainTex_ST;
            float _boost;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 tex = tex2D(_MainTex, i.uv);
                
                // Extraction du canal Rouge de la lumière ambiante Unity (glstate_lightmodel_ambient)
                float ambientLight = 1.0 + (UNITY_LIGHTMODEL_AMBIENT.r * 2.0);
                
                // Calcul restauré à partir de : c_1.xyz = diff_2 * ((tex - (v.color * -_boost)) * tex)
                fixed3 colorMix = tex.rgb + (i.color.rgb * _boost);
                fixed3 finalRGB = ambientLight * (colorMix * tex.rgb);
                
                return fixed4(finalRGB, 1.0);
            }
            ENDCG
        }
    }
    Fallback "Mobile/Diffuse"
}