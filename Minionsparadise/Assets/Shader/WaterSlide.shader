//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Water/WaterSlide" {
Properties {
 _AlphaMask ("Alpha Mask", 2D) = "gray" { }
 _diffuse1 ("diffuse1", 2D) = "gray" { }
 _diffuse2 ("diffuse2", 2D) = "gray" { }
 _time_scale ("time_scale", Float) = 10
}
SubShader { 
 Tags { "QUEUE"="Geometry" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
 Pass {
  Name "FORWARDBASE"
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" }
  ZWrite Off
  Blend SrcAlpha OneMinusSrcAlpha
  Offset [_OffsetFactor], [_OffsetUnits]
GLSLPROGRAM
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _AlphaMask_ST;
uniform lowp vec4 _diffuse1_ST;
uniform lowp vec4 _diffuse2_ST;
uniform highp float _time_scale;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump float scroll_1;
  mediump vec2 tmpvar_2;
  mediump vec2 tmpvar_3;
  mediump vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = fract((_Time.x * _time_scale));
  scroll_1 = tmpvar_6;
  tmpvar_2 = (_glesMultiTexCoord0.xy * _diffuse1_ST.xy);
  tmpvar_2.x = (tmpvar_2.x - scroll_1);
  tmpvar_3 = (_glesMultiTexCoord0.xy * _diffuse2_ST.xy);
  tmpvar_3.x = (tmpvar_3.x - scroll_1);
  mediump vec2 tmpvar_7;
  tmpvar_7 = (_glesMultiTexCoord0.xy * _AlphaMask_ST.xy);
  tmpvar_4.y = tmpvar_7.y;
  tmpvar_4.x = (tmpvar_7.x - scroll_1);
  tmpvar_5.y = tmpvar_7.y;
  tmpvar_5.x = (tmpvar_7.x - scroll_1);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_TEXCOORD1 = tmpvar_3;
  xlv_TEXCOORD2 = tmpvar_4;
  xlv_TEXCOORD3 = tmpvar_5;
  xlv_COLOR = _glesColor;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _AlphaMask;
uniform sampler2D _diffuse1;
uniform sampler2D _diffuse2;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying mediump vec2 xlv_TEXCOORD2;
varying mediump vec2 xlv_TEXCOORD3;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 finalColor_1;
  finalColor_1.xyz = clamp ((1.0 - (
    (1.0 - texture2D (_diffuse1, xlv_TEXCOORD0))
   * 
    (1.0 - texture2D (_diffuse2, xlv_TEXCOORD1))
  )), 0.0, 1.0).xyz;
  finalColor_1.w = (xlv_COLOR.w * clamp ((texture2D (_AlphaMask, xlv_TEXCOORD2).x + texture2D (_AlphaMask, xlv_TEXCOORD3).z), 0.0, 1.0));
  gl_FragData[0] = finalColor_1;
}


#endif

ENDGLSL
 }
}
}