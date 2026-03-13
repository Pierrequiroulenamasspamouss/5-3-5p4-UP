//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Water/Terrain Waves" {
Properties {
 _Diffuse ("Diffuse", 2D) = "white" { }
 _AlphaMask ("Alpha Mask", 2D) = "white" { }
 _Color ("Color", Color) = (0.5,0.5,0.5,1)
 _Speed ("Speed", Float) = 1
 _FadeAlpha ("Fade Alpha", Range(0,1)) = 1
[HideInInspector]  _Mode ("Rendering Queue", Float) = 0
[HideInInspector]  _LayerIndex ("Layer index", Float) = 0
}
SubShader { 
 Tags { "LIGHTMODE"="Always" "QUEUE"="Background+1" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Tags { "LIGHTMODE"="Always" "QUEUE"="Background+1" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZTest Always
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
GLSLPROGRAM
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _Diffuse_ST;
uniform highp vec4 _AlphaMask_ST;
uniform mediump float _Speed;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec2 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3.x = 0.0;
  tmpvar_3.y = fract((_Time.y * _Speed));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _Diffuse_ST.xy) + (_Diffuse_ST.zw + tmpvar_3));
  tmpvar_2 = ((_glesMultiTexCoord1.xy * _AlphaMask_ST.xy) + _AlphaMask_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _Diffuse;
uniform sampler2D _AlphaMask;
uniform lowp vec4 _Color;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_Diffuse, xlv_TEXCOORD0);
  lowp vec4 tmpvar_2;
  tmpvar_2.xyz = (_Color.xyz + tmpvar_1.xyz);
  tmpvar_2.w = ((tmpvar_1.x * texture2D (_AlphaMask, xlv_TEXCOORD1).x) * (_Color.w * _FadeAlpha));
  gl_FragData[0] = tmpvar_2;
}


#endif

ENDGLSL
 }
}
Fallback "Diffuse"
}