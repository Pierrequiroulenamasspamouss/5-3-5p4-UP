//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Standard/Vert Animation" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _Scale ("Vertex Color Scale", Float) = 0
 _Speed ("Animation Speed", Float) = 1
 _BlendedColor ("Blended Color", Color) = (0,0,0,0)
[PerRendererData]  _FadeAlpha ("Fade Alpha", Range(0,1)) = 1
 _OffsetFactor ("Offset Factor", Float) = 0
 _OffsetUnits ("Offset Units", Float) = 0
[Enum(UnityEngine.Rendering.BlendMode)]  _SrcBlend ("Source Blend mode", Float) = 5
[Enum(UnityEngine.Rendering.BlendMode)]  _DstBlend ("Dest Blend mode", Float) = 10
[Enum(Kampai.Util.Graphics.CompareFunction)]  _ZTest ("ZTest", Float) = 4
[Enum(Kampai.Editor.AlphaMode)]  _Alpha ("Transparent", Float) = 1
}
SubShader { 
 Tags { "QUEUE"="Geometry+1" }
 Pass {
  Tags { "QUEUE"="Geometry+1" }
  ZTest [_ZTest]
  Blend [_SrcBlend] [_DstBlend]
  Offset [_OffsetFactor], [_OffsetUnits]
GLSLPROGRAM
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _MainTex_ST;
uniform lowp float _Scale;
uniform lowp float _Speed;
varying mediump vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.w = _glesVertex.w;
  mediump vec2 tmpvar_2;
  tmpvar_1.xyz = (_glesVertex.xyz + ((
    (2.0 * _glesColor.xyz)
   - 1.0) * (
    sin((_Time.y * _Speed))
   * _Scale)));
  tmpvar_2 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  gl_Position = (glstate_matrix_mvp * tmpvar_1);
  xlv_TEXCOORD0 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = (texture2D (_MainTex, xlv_TEXCOORD0) * _Color);
  lowp vec4 tmpvar_2;
  tmpvar_2.xyz = mix (tmpvar_1.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_2.w = (_Alpha + (tmpvar_1.w * _FadeAlpha));
  gl_FragData[0] = tmpvar_2;
}


#endif

ENDGLSL
 }
}
Fallback "Diffuse"
}