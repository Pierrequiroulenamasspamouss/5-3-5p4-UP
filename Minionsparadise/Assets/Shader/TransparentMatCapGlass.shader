//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Transparent/Glass" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB)", 2D) = "gray" { }
 _MatCap ("MatCap (RGB)", 2D) = "gray" { }
 _Boost ("Boost", Float) = 1
[PerRendererData]  _FadeAlpha ("Fade Alpha", Range(0,1)) = 1
}
SubShader { 
 Tags { "QUEUE"="Transparent" }
 Pass {
  Tags { "QUEUE"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
GLSLPROGRAM
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform lowp vec4 _MainTex_ST;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec2 capCoord_1;
  mediump vec2 tmpvar_2;
  tmpvar_2 = (_glesMultiTexCoord0.xy * _MainTex_ST.xy);
  highp vec4 v_3;
  v_3.x = glstate_matrix_invtrans_modelview0[0].x;
  v_3.y = glstate_matrix_invtrans_modelview0[1].x;
  v_3.z = glstate_matrix_invtrans_modelview0[2].x;
  v_3.w = glstate_matrix_invtrans_modelview0[3].x;
  highp vec3 tmpvar_4;
  tmpvar_4 = normalize(v_3.xyz);
  capCoord_1.x = dot (tmpvar_4, _glesNormal);
  highp vec4 v_5;
  v_5.x = glstate_matrix_invtrans_modelview0[0].y;
  v_5.y = glstate_matrix_invtrans_modelview0[1].y;
  v_5.z = glstate_matrix_invtrans_modelview0[2].y;
  v_5.w = glstate_matrix_invtrans_modelview0[3].y;
  highp vec3 tmpvar_6;
  tmpvar_6 = normalize(v_5.xyz);
  capCoord_1.y = dot (tmpvar_6, _glesNormal);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_TEXCOORD1 = ((capCoord_1 * 0.5) + 0.5);
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 _Color;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
uniform sampler2D _MainTex;
uniform sampler2D _MatCap;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.xyz = ((texture2D (_MatCap, xlv_TEXCOORD1) * _Boost) * _Color).xyz;
  tmpvar_1.w = ((texture2D (_MainTex, xlv_TEXCOORD0).x * _Color.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_1;
}


#endif

ENDGLSL
 }
}
}