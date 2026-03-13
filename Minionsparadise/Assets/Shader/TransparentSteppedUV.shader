//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Transparent/Vertex Color Stepped Anim" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGBA)", 2D) = "white" { }
 _Boost ("Boost", Float) = 1
 _NumFrames ("Number Frames", Float) = 4
 _ScrollSpeed ("Scroll Speed", Float) = 4
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
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump float _ScrollSpeed;
uniform mediump float _NumFrames;
varying lowp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec2 tmpvar_2;
  tmpvar_2.y = 0.0;
  tmpvar_2.x = (1.0/(_NumFrames));
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_glesMultiTexCoord0.xy + (
    floor((_Time.y * _ScrollSpeed))
   * tmpvar_2)));
  tmpvar_1 = tmpvar_3;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = tmpvar_1;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform lowp float _Boost;
uniform lowp vec4 _Color;
varying lowp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0) * _Boost) * (_Color * xlv_COLOR));
  gl_FragData[0] = tmpvar_1;
}


#endif

ENDGLSL
 }
}
}