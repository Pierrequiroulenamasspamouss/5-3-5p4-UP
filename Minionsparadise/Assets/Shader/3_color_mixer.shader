//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Transparent/3 Color Blend" {
Properties {
 _TransparencyMask ("Channel Map (RGBA)", 2D) = "gray" { }
 _colorR ("Color R", Color) = (0.0661765,0.0661765,0.0661765,1)
 _colorG ("Color G", Color) = (0.0205991,0.366995,0.933824,1)
 _colorB ("Color B", Color) = (0.522059,0.822008,1,1)
 _color_boost ("Color Boost", Float) = 1
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
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _TransparencyMask_ST;
varying mediump vec2 xlv_TEXCOORD0;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _TransparencyMask_ST.xy) + _TransparencyMask_ST.zw);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _TransparencyMask;
uniform lowp vec4 _colorG;
uniform lowp vec4 _colorB;
uniform lowp vec4 _colorR;
uniform lowp float _color_boost;
varying mediump vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_TransparencyMask, xlv_TEXCOORD0);
  lowp vec4 tmpvar_2;
  tmpvar_2.xyz = (mix (mix (_colorR.xyz, _colorG.xyz, tmpvar_1.yyy), _colorB.xyz, tmpvar_1.zzz) * _color_boost);
  tmpvar_2.w = (tmpvar_1.w * float((tmpvar_1.w >= 0.5)));
  gl_FragData[0] = tmpvar_2;
}


#endif

ENDGLSL
 }
}
Fallback "Diffuse"
}