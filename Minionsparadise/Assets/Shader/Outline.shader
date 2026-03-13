//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Standard/Outline" {
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
 UsePass "Kampai/Standard/BluePrint/BASE"
 Pass {
  Tags { "LIGHTMODE"="Always" "CanUseSpriteAtlas"="true" }
  ZTest [_ZTest]
  ZWrite [_ZWrite]
  Cull Front
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
uniform highp mat4 glstate_matrix_mvp;
uniform lowp float _Outline;
uniform lowp float _VertexColor;
varying lowp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  lowp vec4 tmpvar_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  tmpvar_1.w = _glesVertex.w;
  tmpvar_1.xyz = (_glesVertex.xyz + ((_glesNormal * _Outline) * (tmpvar_3.y * 0.01)));
  tmpvar_1 = (glstate_matrix_mvp * tmpvar_1);
  gl_Position = tmpvar_1;
  xlv_COLOR = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform lowp float _Alpha;
uniform lowp vec4 _OutlineColor;
uniform lowp float _FadeAlpha;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.xyz = _OutlineColor.xyz;
  tmpvar_1.w = (_Alpha * _FadeAlpha);
  gl_FragData[0] = tmpvar_1;
}


#endif

ENDGLSL
 }
}
Fallback "Unlit/Texture"
}