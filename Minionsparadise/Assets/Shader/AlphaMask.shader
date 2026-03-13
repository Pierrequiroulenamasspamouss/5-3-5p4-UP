//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/UI/AlphaMask" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _AlphaTex ("Alpha mask (R)", 2D) = "white" { }
 _Color ("Tint", Color) = (1,1,1,1)
 _Overlay ("Overlay", Color) = (0,0,0,0)
 _Desaturation ("Desaturation", Float) = 0
 _StencilComp ("Stencil Comparison", Float) = 8
 _Stencil ("Stencil ID", Float) = 0
 _StencilOp ("Stencil Operation", Float) = 0
 _StencilWriteMask ("Stencil Write Mask", Float) = 255
 _StencilReadMask ("Stencil Read Mask", Float) = 255
[Enum(Kampai.Util.Graphics.ColorMask)]  _ColorMask ("Color Mask", Float) = 15
}
SubShader { 
 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CanUseSpriteAtlas"="true" "PreviewType"="Plane" }
 Pass {
  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "CanUseSpriteAtlas"="true" "PreviewType"="Plane" }
  ZTest [unity_GUIZTestMode]
  ZWrite Off
  Cull Off
  Stencil {
   Ref [_Stencil]
   ReadMask [_StencilReadMask]
   WriteMask [_StencilWriteMask]
   Comp [_StencilComp]
   Pass [_StencilOp]
  }
  Blend SrcAlpha OneMinusSrcAlpha
  ColorMask [_ColorMask]
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

struct appdata_t {
    float4 vertex : POSITION;
    float4 color : COLOR;
    float2 texcoord : TEXCOORD0;
    float2 texcoord1 : TEXCOORD1;
};

struct v2f {
    float4 vertex : SV_POSITION;
    fixed4 color : COLOR;
    float2 texcoord : TEXCOORD0;
    float2 texcoord1 : TEXCOORD1;
};

sampler2D _MainTex;
sampler2D _AlphaTex;
float4 _MainTex_ST;
float4 _AlphaTex_ST;
fixed4 _Color;
fixed4 _Overlay;
float _Desaturation;

v2f vert (appdata_t v)
{
    v2f o;
    o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
    o.texcoord1 = TRANSFORM_TEX(v.texcoord1, _AlphaTex);
    
    fixed4 col = v.color * _Color;
    o.color.rgb = (_Overlay.rgb * _Overlay.a) + (col.rgb * (1.0 - _Overlay.a));
    o.color.a = col.a;
    return o;
}

fixed4 frag (v2f i) : SV_Target
{
    fixed4 tex = tex2D(_MainTex, i.texcoord);
    fixed3 col = tex.rgb * i.color.rgb;
    
    float lum = dot(col, fixed3(0.222, 0.707, 0.071));
    col = lerp(col, fixed3(lum, lum, lum), _Desaturation);
    
    fixed alpha = tex2D(_AlphaTex, i.texcoord1).r * i.color.a;
    return fixed4(col, alpha);
}
ENDCG

GLSLPROGRAM
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
uniform highp vec4 _AlphaTex_ST;
uniform lowp vec4 _Color;
uniform lowp vec4 _Overlay;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec2 tmpvar_2;
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  tmpvar_2 = ((_glesMultiTexCoord1.xy * _AlphaTex_ST.xy) + _AlphaTex_ST.zw);
  lowp vec4 tmpvar_3;
  tmpvar_3 = (_glesColor * _Color);
  lowp vec4 tmpvar_4;
  tmpvar_4.xyz = ((_Overlay.xyz * _Overlay.w) + (tmpvar_3.xyz * (1.0 - _Overlay.w)));
  tmpvar_4.w = tmpvar_3.w;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_COLOR = tmpvar_4;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform lowp float _Desaturation;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp float lum_1;
  lowp vec3 tmpvar_2;
  tmpvar_2 = (texture2D (_MainTex, xlv_TEXCOORD0).xyz * xlv_COLOR.xyz);
  mediump vec3 c_3;
  c_3 = tmpvar_2;
  mediump float tmpvar_4;
  tmpvar_4 = dot (c_3, unity_ColorSpaceLuminance.xyz);
  lum_1 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = mix (tmpvar_2, vec3(lum_1), vec3(_Desaturation));
  tmpvar_5.w = (texture2D (_AlphaTex, xlv_TEXCOORD1).x * xlv_COLOR.w);
  gl_FragData[0] = tmpvar_5;
}


#endif

ENDGLSL
 }
}
}