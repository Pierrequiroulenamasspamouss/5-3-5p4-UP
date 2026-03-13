//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Standard/BluePrint" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _OutlineColor ("Outline Color", Color) = (0,0,0,0)
 _Outline ("Outline width", Float) = 1
 _GradTex ("Gradients (RGB)", 2D) = "white" { }
 _MatCapTex ("MatCap (RGB)", 2D) = "white" { }
 _BlendedColor ("Blended Color", Color) = (0,0,0,0)
[PerRendererData]  _FadeAlpha ("Fade Alpha", Range(0,1)) = 1
[HideInInspector] [Enum(Kampai.Util.Graphics.BlendMode)]  _Mode ("Rendering Queue", Float) = 0
[HideInInspector]  _LayerIndex ("Layer index", Float) = 0
 _OffsetFactor ("Offset Factor", Float) = 0
 _OffsetUnits ("Offset Units", Float) = 0
[Enum(UnityEngine.Rendering.BlendMode)]  _SrcBlend ("Source Blend mode", Float) = 5
[Enum(UnityEngine.Rendering.BlendMode)]  _DstBlend ("Dest Blend mode", Float) = 10
[Enum(UnityEngine.Rendering.CompareFunction)]  _ZTest ("ZTest", Float) = 4
[Enum(Kampai.Editor.AlphaMode)]  _Alpha ("Transparent", Float) = 1
[Enum(Kampai.Editor.ToggleValue)]  _ZWrite ("ZWrite", Float) = 1
[Enum(UnityEngine.Rendering.CullMode)]  _Cull ("Cull", Float) = 2
[Enum(Kampai.Editor.ToggleValue)]  _VertexColor ("Vertex Color", Float) = 0
[Enum(Kampai.Util.Graphics.ColorMask)]  _ColorMask ("Color Mask", Float) = 15
 __Stencil ("Ref", Float) = 0
[Enum(UnityEngine.Rendering.CompareFunction)]  __StencilComp ("Comparison", Float) = 8
 __StencilReadMask ("Read Mask", Float) = 255
 __StencilWriteMask ("Write Mask", Float) = 255
[Enum(UnityEngine.Rendering.StencilOp)]  __StencilPassOp ("Pass Operation", Float) = 0
[Enum(UnityEngine.Rendering.StencilOp)]  __StencilFailOp ("Fail Operation", Float) = 0
[Enum(UnityEngine.Rendering.StencilOp)]  __StencilZFailOp ("ZFail Operation", Float) = 0
}
SubShader { 
 Tags { "LIGHTMODE"="Always" "CanUseSpriteAtlas"="true" }
 Pass {
  Name "BASE"
  Tags { "LIGHTMODE"="Always" "CanUseSpriteAtlas"="true" }
  ZTest [_ZTest]
  ZWrite [_ZWrite]
  Cull [_Cull]
  Stencil {
   Ref [__Stencil]
   ReadMask [__StencilReadMask]
   WriteMask [__StencilWriteMask]
   Comp [__StencilComp]
   Pass [__StencilPassOp]
   Fail [__StencilFailOp]
   ZFail [__StencilZFailOp]
  }
  Blend [_SrcBlend] [_DstBlend]
  ColorMask [_ColorMask]
  Offset [_OffsetFactor], [_OffsetUnits]
GLSLPROGRAM
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform lowp vec4 _GradTex_ST;
uniform lowp float _VertexColor;
varying lowp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec4 v_2;
  v_2.x = glstate_matrix_invtrans_modelview0[0].x;
  v_2.y = glstate_matrix_invtrans_modelview0[1].x;
  v_2.z = glstate_matrix_invtrans_modelview0[2].x;
  v_2.w = glstate_matrix_invtrans_modelview0[3].x;
  highp vec4 v_3;
  v_3.x = glstate_matrix_invtrans_modelview0[0].y;
  v_3.y = glstate_matrix_invtrans_modelview0[1].y;
  v_3.z = glstate_matrix_invtrans_modelview0[2].y;
  v_3.w = glstate_matrix_invtrans_modelview0[3].y;
  highp vec2 tmpvar_4;
  tmpvar_4.x = dot (v_2.xyz, _glesNormal);
  tmpvar_4.y = dot (v_3.xyz, _glesNormal);
  tmpvar_1 = ((tmpvar_4 * 0.5) + 0.5);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _GradTex_ST.xy) + _GradTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _GradTex;
uniform sampler2D _MatCapTex;
uniform lowp float _Alpha;
uniform lowp vec4 _Color;
uniform lowp float _FadeAlpha;
uniform lowp vec4 _BlendedColor;
varying lowp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec2 tmpvar_1;
  tmpvar_1.x = (texture2D (_MatCapTex, xlv_TEXCOORD1).x * xlv_COLOR.x);
  tmpvar_1.y = (1.0 - xlv_COLOR.z);
  lowp vec4 tmpvar_2;
  tmpvar_2.xyz = texture2D (_GradTex, tmpvar_1).xyz;
  tmpvar_2.w = _Color.w;
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_3.w = (_Alpha * _FadeAlpha);
  gl_FragData[0] = tmpvar_3;
}


#endif

ENDGLSL
 }
}
Fallback "Unlit/Texture"
}