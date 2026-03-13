//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Standard/Specular" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB) Gloss (A)", 2D) = "white" { }
 _Boost ("Boost", Float) = 1
 _Spec ("Specular", Float) = 0.078125
 _UVScroll ("UV Scroll", Vector) = (0,0,0,0)
 _OffsetFactor ("Offset Factor", Float) = 0
 _OffsetUnits ("Offset Units", Float) = 0
[Enum(UnityEngine.Rendering.BlendMode)]  _SrcBlend ("Source Blend mode", Float) = 5
[Enum(UnityEngine.Rendering.BlendMode)]  _DstBlend ("Dest Blend mode", Float) = 10
[Enum(Kampai.Util.Graphics.CompareFunction)]  _ZTest ("ZTest", Float) = 4
[Enum(Kampai.Editor.AlphaMode)]  _Alpha ("Transparent", Float) = 1
}
SubShader { 
 Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "QUEUE"="Geometry" }
  ZTest [_ZTest]
  Blend [_SrcBlend] [_DstBlend]
  Offset [_OffsetFactor], [_OffsetUnits]
GLSLPROGRAM
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec3 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  highp vec4 tmpvar_4;
  tmpvar_4.w = 0.0;
  tmpvar_4.xyz = _glesNormal;
  tmpvar_2 = (tmpvar_4 * _World2Object).xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = (_WorldSpaceCameraPos - (_Object2World * _glesVertex).xyz);
  highp vec3 I_6;
  I_6 = -(tmpvar_5);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = (I_6 - (2.0 * (
    dot (tmpvar_2, I_6)
   * tmpvar_2)));
  xlv_TEXCOORD3 = tmpvar_5;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _WorldSpaceLightPos0;
uniform sampler2D _MainTex;
uniform highp vec4 _LightColor0;
uniform lowp vec4 _Color;
uniform lowp float _Alpha;
uniform mediump float _Spec;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec3 specularColor_1;
  lowp vec4 c_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  c_2.w = tmpvar_3.w;
  mediump float y_4;
  y_4 = (_Spec * 128.0);
  highp vec3 tmpvar_5;
  tmpvar_5 = ((tmpvar_3.w * pow (
    max (0.0, dot (normalize((xlv_TEXCOORD3 + _WorldSpaceLightPos0.xyz)), normalize(xlv_TEXCOORD1)))
  , y_4)) * _LightColor0.xyz);
  specularColor_1 = tmpvar_5;
  c_2.xyz = (tmpvar_3.xyz + specularColor_1);
  c_2 = (c_2 * _Color);
  lowp vec4 tmpvar_6;
  tmpvar_6.xyz = c_2.xyz;
  tmpvar_6.w = (_Alpha + c_2.w);
  gl_FragData[0] = tmpvar_6;
}


#endif

ENDGLSL
 }
}
}