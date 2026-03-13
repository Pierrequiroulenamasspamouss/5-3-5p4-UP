//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Standard/Texture" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _AlphaTex ("Alpha Mask (Grey)", 2D) = "white" { }
 _Boost ("Boost", Float) = 1
 _UVScroll ("UV Scroll", Vector) = (0,0,0,0)
 _BlendedColor ("Blended Color", Color) = (0,0,0,0)
 _Saturation ("Saturation", Float) = 1
 _AlphaClip ("Alpha Clip", Range(0,1)) = 0
[PerRendererData]  _FadeAlpha ("Fade Alpha", Range(0,1)) = 1
[HideInInspector] [Enum(Kampai.Util.Graphics.BlendMode)]  _Mode ("Rendering Queue", Float) = 0
[HideInInspector]  _LayerIndex ("Layer index", Float) = 0
 _TransparencyLM ("Transmissive Color", 2D) = "white" { }
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
 Tags { "CanUseSpriteAtlas"="true" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "SHADOWSUPPORT"="true" "CanUseSpriteAtlas"="true" }
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
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "TEXTURE_ALPHA" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_2));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_3.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "TEXTURE_ALPHA" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD1).x > 
    (xlv_TEXCOORD1.z / xlv_TEXCOORD1.w)
  )), _LightShadowData.x);
  tmpvar_3 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * tmpvar_3);
  tmpvar_5.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "TEXTURE_ALPHA" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_2));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_3.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "TEXTURE_ALPHA" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD1).x > 
    (xlv_TEXCOORD1.z / xlv_TEXCOORD1.w)
  )), _LightShadowData.x);
  tmpvar_3 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * tmpvar_3);
  tmpvar_5.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "TEXTURE_ALPHA" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float shadow_3;
  shadow_3 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_4;
  tmpvar_4.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * shadow_3);
  tmpvar_4.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_4;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "TEXTURE_ALPHA" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float shadow_3;
  shadow_3 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_4;
  tmpvar_4.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * shadow_3);
  tmpvar_4.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_4;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_MASK" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_2));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_3.w = (((texture2D (_AlphaTex, xlv_TEXCOORD0).x * tmpvar_2.w) + _Alpha) * _FadeAlpha);
  gl_FragData[0] = tmpvar_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_MASK" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD1).x > 
    (xlv_TEXCOORD1.z / xlv_TEXCOORD1.w)
  )), _LightShadowData.x);
  tmpvar_3 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * tmpvar_3);
  tmpvar_5.w = (((texture2D (_AlphaTex, xlv_TEXCOORD0).x * tmpvar_2.w) + _Alpha) * _FadeAlpha);
  gl_FragData[0] = tmpvar_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "ALPHA_MASK" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_2));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_3.w = (((texture2D (_AlphaTex, xlv_TEXCOORD0).x * tmpvar_2.w) + _Alpha) * _FadeAlpha);
  gl_FragData[0] = tmpvar_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "ALPHA_MASK" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD1).x > 
    (xlv_TEXCOORD1.z / xlv_TEXCOORD1.w)
  )), _LightShadowData.x);
  tmpvar_3 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * tmpvar_3);
  tmpvar_5.w = (((texture2D (_AlphaTex, xlv_TEXCOORD0).x * tmpvar_2.w) + _Alpha) * _FadeAlpha);
  gl_FragData[0] = tmpvar_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_MASK" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float shadow_3;
  shadow_3 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_4;
  tmpvar_4.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * shadow_3);
  tmpvar_4.w = (((texture2D (_AlphaTex, xlv_TEXCOORD0).x * tmpvar_2.w) + _Alpha) * _FadeAlpha);
  gl_FragData[0] = tmpvar_4;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "ALPHA_MASK" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float shadow_3;
  shadow_3 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_4;
  tmpvar_4.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * shadow_3);
  tmpvar_4.w = (((texture2D (_AlphaTex, xlv_TEXCOORD0).x * tmpvar_2.w) + _Alpha) * _FadeAlpha);
  gl_FragData[0] = tmpvar_4;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_CLIP" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_2));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform sampler2D _MainTex;
uniform lowp float _AlphaClip;
uniform lowp float _Saturation;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  mediump vec3 c_3;
  c_3 = tmpvar_2.xyz;
  mediump float x_4;
  x_4 = (clamp ((_Saturation * 
    dot (c_3, unity_ColorSpaceLuminance.xyz)
  ), 0.0, 1.0) * 0.9);
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_5.w = (((1.0 - 
    float((0.0 >= (pow (x_4, 
      (_AlphaClip * _AlphaClip)
    ) - 0.95)))
  ) * tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_CLIP" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp float _AlphaClip;
uniform lowp float _Saturation;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  mediump vec3 c_3;
  c_3 = tmpvar_2.xyz;
  mediump float x_4;
  x_4 = (clamp ((_Saturation * 
    dot (c_3, unity_ColorSpaceLuminance.xyz)
  ), 0.0, 1.0) * 0.9);
  lowp float tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD1).x > 
    (xlv_TEXCOORD1.z / xlv_TEXCOORD1.w)
  )), _LightShadowData.x);
  tmpvar_5 = tmpvar_6;
  lowp vec4 tmpvar_7;
  tmpvar_7.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * tmpvar_5);
  tmpvar_7.w = (((1.0 - 
    float((0.0 >= (pow (x_4, 
      (_AlphaClip * _AlphaClip)
    ) - 0.95)))
  ) * tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_7;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "ALPHA_CLIP" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_2));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform sampler2D _MainTex;
uniform lowp float _AlphaClip;
uniform lowp float _Saturation;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  mediump vec3 c_3;
  c_3 = tmpvar_2.xyz;
  mediump float x_4;
  x_4 = (clamp ((_Saturation * 
    dot (c_3, unity_ColorSpaceLuminance.xyz)
  ), 0.0, 1.0) * 0.9);
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_5.w = (((1.0 - 
    float((0.0 >= (pow (x_4, 
      (_AlphaClip * _AlphaClip)
    ) - 0.95)))
  ) * tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "ALPHA_CLIP" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp float _AlphaClip;
uniform lowp float _Saturation;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  mediump vec3 c_3;
  c_3 = tmpvar_2.xyz;
  mediump float x_4;
  x_4 = (clamp ((_Saturation * 
    dot (c_3, unity_ColorSpaceLuminance.xyz)
  ), 0.0, 1.0) * 0.9);
  lowp float tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD1).x > 
    (xlv_TEXCOORD1.z / xlv_TEXCOORD1.w)
  )), _LightShadowData.x);
  tmpvar_5 = tmpvar_6;
  lowp vec4 tmpvar_7;
  tmpvar_7.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * tmpvar_5);
  tmpvar_7.w = (((1.0 - 
    float((0.0 >= (pow (x_4, 
      (_AlphaClip * _AlphaClip)
    ) - 0.95)))
  ) * tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_7;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_CLIP" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp float _AlphaClip;
uniform lowp float _Saturation;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  mediump vec3 c_3;
  c_3 = tmpvar_2.xyz;
  mediump float x_4;
  x_4 = (clamp ((_Saturation * 
    dot (c_3, unity_ColorSpaceLuminance.xyz)
  ), 0.0, 1.0) * 0.9);
  lowp float shadow_5;
  shadow_5 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_6;
  tmpvar_6.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * shadow_5);
  tmpvar_6.w = (((1.0 - 
    float((0.0 >= (pow (x_4, 
      (_AlphaClip * _AlphaClip)
    ) - 0.95)))
  ) * tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_6;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "ALPHA_CLIP" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp float _AlphaClip;
uniform lowp float _Saturation;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  mediump vec3 c_3;
  c_3 = tmpvar_2.xyz;
  mediump float x_4;
  x_4 = (clamp ((_Saturation * 
    dot (c_3, unity_ColorSpaceLuminance.xyz)
  ), 0.0, 1.0) * 0.9);
  lowp float shadow_5;
  shadow_5 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_6;
  tmpvar_6.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * shadow_5);
  tmpvar_6.w = (((1.0 - 
    float((0.0 >= (pow (x_4, 
      (_AlphaClip * _AlphaClip)
    ) - 0.95)))
  ) * tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_6;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "TEXTURE_ALPHA" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_2));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_3.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "TEXTURE_ALPHA" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD1).x > 
    (xlv_TEXCOORD1.z / xlv_TEXCOORD1.w)
  )), _LightShadowData.x);
  tmpvar_3 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * tmpvar_3);
  tmpvar_5.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "TEXTURE_ALPHA" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_2));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_3.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "TEXTURE_ALPHA" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD1).x > 
    (xlv_TEXCOORD1.z / xlv_TEXCOORD1.w)
  )), _LightShadowData.x);
  tmpvar_3 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * tmpvar_3);
  tmpvar_5.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "TEXTURE_ALPHA" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float shadow_3;
  shadow_3 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_4;
  tmpvar_4.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * shadow_3);
  tmpvar_4.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_4;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "TEXTURE_ALPHA" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float shadow_3;
  shadow_3 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_4;
  tmpvar_4.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * shadow_3);
  tmpvar_4.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_4;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_MASK" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_2));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_3.w = (((texture2D (_AlphaTex, xlv_TEXCOORD0).x * tmpvar_2.w) + _Alpha) * _FadeAlpha);
  gl_FragData[0] = tmpvar_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_MASK" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD1).x > 
    (xlv_TEXCOORD1.z / xlv_TEXCOORD1.w)
  )), _LightShadowData.x);
  tmpvar_3 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * tmpvar_3);
  tmpvar_5.w = (((texture2D (_AlphaTex, xlv_TEXCOORD0).x * tmpvar_2.w) + _Alpha) * _FadeAlpha);
  gl_FragData[0] = tmpvar_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "ALPHA_MASK" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_2));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_3.w = (((texture2D (_AlphaTex, xlv_TEXCOORD0).x * tmpvar_2.w) + _Alpha) * _FadeAlpha);
  gl_FragData[0] = tmpvar_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "ALPHA_MASK" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float tmpvar_3;
  highp float tmpvar_4;
  tmpvar_4 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD1).x > 
    (xlv_TEXCOORD1.z / xlv_TEXCOORD1.w)
  )), _LightShadowData.x);
  tmpvar_3 = tmpvar_4;
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * tmpvar_3);
  tmpvar_5.w = (((texture2D (_AlphaTex, xlv_TEXCOORD0).x * tmpvar_2.w) + _Alpha) * _FadeAlpha);
  gl_FragData[0] = tmpvar_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_MASK" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float shadow_3;
  shadow_3 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_4;
  tmpvar_4.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * shadow_3);
  tmpvar_4.w = (((texture2D (_AlphaTex, xlv_TEXCOORD0).x * tmpvar_2.w) + _Alpha) * _FadeAlpha);
  gl_FragData[0] = tmpvar_4;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "ALPHA_MASK" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _AlphaTex;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Alpha;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  lowp float shadow_3;
  shadow_3 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_4;
  tmpvar_4.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * shadow_3);
  tmpvar_4.w = (((texture2D (_AlphaTex, xlv_TEXCOORD0).x * tmpvar_2.w) + _Alpha) * _FadeAlpha);
  gl_FragData[0] = tmpvar_4;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_CLIP" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_2));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform sampler2D _MainTex;
uniform lowp float _AlphaClip;
uniform lowp float _Saturation;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  mediump vec3 c_3;
  c_3 = tmpvar_2.xyz;
  mediump float x_4;
  x_4 = (clamp ((_Saturation * 
    dot (c_3, unity_ColorSpaceLuminance.xyz)
  ), 0.0, 1.0) * 0.9);
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_5.w = (((1.0 - 
    float((0.0 >= (pow (x_4, 
      (_AlphaClip * _AlphaClip)
    ) - 0.95)))
  ) * tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_CLIP" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp float _AlphaClip;
uniform lowp float _Saturation;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  mediump vec3 c_3;
  c_3 = tmpvar_2.xyz;
  mediump float x_4;
  x_4 = (clamp ((_Saturation * 
    dot (c_3, unity_ColorSpaceLuminance.xyz)
  ), 0.0, 1.0) * 0.9);
  lowp float tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD1).x > 
    (xlv_TEXCOORD1.z / xlv_TEXCOORD1.w)
  )), _LightShadowData.x);
  tmpvar_5 = tmpvar_6;
  lowp vec4 tmpvar_7;
  tmpvar_7.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * tmpvar_5);
  tmpvar_7.w = (((1.0 - 
    float((0.0 >= (pow (x_4, 
      (_AlphaClip * _AlphaClip)
    ) - 0.95)))
  ) * tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_7;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "ALPHA_CLIP" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec2 tmpvar_2;
  tmpvar_2 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_2));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform sampler2D _MainTex;
uniform lowp float _AlphaClip;
uniform lowp float _Saturation;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  mediump vec3 c_3;
  c_3 = tmpvar_2.xyz;
  mediump float x_4;
  x_4 = (clamp ((_Saturation * 
    dot (c_3, unity_ColorSpaceLuminance.xyz)
  ), 0.0, 1.0) * 0.9);
  lowp vec4 tmpvar_5;
  tmpvar_5.xyz = mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_5.w = (((1.0 - 
    float((0.0 >= (pow (x_4, 
      (_AlphaClip * _AlphaClip)
    ) - 0.95)))
  ) * tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_5;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "ALPHA_CLIP" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp float _AlphaClip;
uniform lowp float _Saturation;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  mediump vec3 c_3;
  c_3 = tmpvar_2.xyz;
  mediump float x_4;
  x_4 = (clamp ((_Saturation * 
    dot (c_3, unity_ColorSpaceLuminance.xyz)
  ), 0.0, 1.0) * 0.9);
  lowp float tmpvar_5;
  highp float tmpvar_6;
  tmpvar_6 = max (float((texture2DProj (_ShadowMapTexture, xlv_TEXCOORD1).x > 
    (xlv_TEXCOORD1.z / xlv_TEXCOORD1.w)
  )), _LightShadowData.x);
  tmpvar_5 = tmpvar_6;
  lowp vec4 tmpvar_7;
  tmpvar_7.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * tmpvar_5);
  tmpvar_7.w = (((1.0 - 
    float((0.0 >= (pow (x_4, 
      (_AlphaClip * _AlphaClip)
    ) - 0.95)))
  ) * tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_7;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_CLIP" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp float _AlphaClip;
uniform lowp float _Saturation;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  mediump vec3 c_3;
  c_3 = tmpvar_2.xyz;
  mediump float x_4;
  x_4 = (clamp ((_Saturation * 
    dot (c_3, unity_ColorSpaceLuminance.xyz)
  ), 0.0, 1.0) * 0.9);
  lowp float shadow_5;
  shadow_5 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_6;
  tmpvar_6.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * shadow_5);
  tmpvar_6.w = (((1.0 - 
    float((0.0 >= (pow (x_4, 
      (_AlphaClip * _AlphaClip)
    ) - 0.95)))
  ) * tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_6;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" "ALPHA_CLIP" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform lowp vec4 _MainTex_ST;
uniform highp vec4 _UVScroll;
uniform lowp float _VertexColor;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  mediump vec2 tmpvar_1;
  mediump vec4 tmpvar_2;
  highp vec2 tmpvar_3;
  tmpvar_3 = fract((_UVScroll.xy * _Time.x));
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + (_MainTex_ST.zw + tmpvar_3));
  tmpvar_2 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform lowp float _AlphaClip;
uniform lowp float _Saturation;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _Boost;
uniform lowp float _FadeAlpha;
varying mediump vec2 xlv_TEXCOORD0;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.x = _Boost;
  tmpvar_1.y = _Boost;
  tmpvar_1.z = _Boost;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * tmpvar_1) * (xlv_COLOR * _Color));
  mediump vec3 c_3;
  c_3 = tmpvar_2.xyz;
  mediump float x_4;
  x_4 = (clamp ((_Saturation * 
    dot (c_3, unity_ColorSpaceLuminance.xyz)
  ), 0.0, 1.0) * 0.9);
  lowp float shadow_5;
  shadow_5 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, xlv_TEXCOORD1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_6;
  tmpvar_6.xyz = (mix (tmpvar_2.xyz, _BlendedColor.xyz, _BlendedColor.www) * shadow_5);
  tmpvar_6.w = (((1.0 - 
    float((0.0 >= (pow (x_4, 
      (_AlphaClip * _AlphaClip)
    ) - 0.95)))
  ) * tmpvar_2.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_6;
}


#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "TEXTURE_ALPHA" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "TEXTURE_ALPHA" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "TEXTURE_ALPHA" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_MASK" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_MASK" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_MASK" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_CLIP" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_CLIP" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_CLIP" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "TEXTURE_ALPHA" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "TEXTURE_ALPHA" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "TEXTURE_ALPHA" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_MASK" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_MASK" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_MASK" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_CLIP" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_CLIP" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "ALPHA_CLIP" }
""
}
}
 }
 Pass {
  Name "SHADOWCASTER"
  Tags { "LIGHTMODE"="SHADOWCASTER" "SHADOWSUPPORT"="true" "CanUseSpriteAtlas"="true" }
  Cull Off
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
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_DEPTH" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
uniform highp vec4 _WorldSpaceLightPos0;
uniform highp vec4 unity_LightShadowBias;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform highp mat4 _World2Object;
uniform highp mat4 unity_MatrixVP;
void main ()
{
  highp vec3 vertex_1;
  vertex_1 = _glesVertex.xyz;
  highp vec4 clipPos_2;
  if ((unity_LightShadowBias.z != 0.0)) {
    highp vec4 tmpvar_3;
    tmpvar_3.w = 1.0;
    tmpvar_3.xyz = vertex_1;
    highp vec3 tmpvar_4;
    tmpvar_4 = (_Object2World * tmpvar_3).xyz;
    highp vec4 v_5;
    v_5.x = _World2Object[0].x;
    v_5.y = _World2Object[1].x;
    v_5.z = _World2Object[2].x;
    v_5.w = _World2Object[3].x;
    highp vec4 v_6;
    v_6.x = _World2Object[0].y;
    v_6.y = _World2Object[1].y;
    v_6.z = _World2Object[2].y;
    v_6.w = _World2Object[3].y;
    highp vec4 v_7;
    v_7.x = _World2Object[0].z;
    v_7.y = _World2Object[1].z;
    v_7.z = _World2Object[2].z;
    v_7.w = _World2Object[3].z;
    highp vec3 tmpvar_8;
    tmpvar_8 = normalize(((
      (v_5.xyz * _glesNormal.x)
     + 
      (v_6.xyz * _glesNormal.y)
    ) + (v_7.xyz * _glesNormal.z)));
    highp float tmpvar_9;
    tmpvar_9 = dot (tmpvar_8, normalize((_WorldSpaceLightPos0.xyz - 
      (tmpvar_4 * _WorldSpaceLightPos0.w)
    )));
    highp vec4 tmpvar_10;
    tmpvar_10.w = 1.0;
    tmpvar_10.xyz = (tmpvar_4 - (tmpvar_8 * (unity_LightShadowBias.z * 
      sqrt((1.0 - (tmpvar_9 * tmpvar_9)))
    )));
    clipPos_2 = (unity_MatrixVP * tmpvar_10);
  } else {
    highp vec4 tmpvar_11;
    tmpvar_11.w = 1.0;
    tmpvar_11.xyz = vertex_1;
    clipPos_2 = (glstate_matrix_mvp * tmpvar_11);
  };
  highp vec4 clipPos_12;
  clipPos_12.xyw = clipPos_2.xyw;
  clipPos_12.z = (clipPos_2.z + clamp ((unity_LightShadowBias.x / clipPos_2.w), 0.0, 1.0));
  clipPos_12.z = mix (clipPos_12.z, max (clipPos_12.z, -(clipPos_2.w)), unity_LightShadowBias.y);
  gl_Position = clipPos_12;
}


#endif
#ifdef FRAGMENT
void main ()
{
  gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_CUBE" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
uniform highp vec4 _LightPositionRange;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
varying highp vec3 xlv_TEXCOORD0;
void main ()
{
  xlv_TEXCOORD0 = ((_Object2World * _glesVertex).xyz - _LightPositionRange.xyz);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _LightPositionRange;
uniform highp vec4 unity_LightShadowBias;
varying highp vec3 xlv_TEXCOORD0;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = fract((vec4(1.0, 255.0, 65025.0, 1.658138e+07) * min (
    ((sqrt(dot (xlv_TEXCOORD0, xlv_TEXCOORD0)) + unity_LightShadowBias.x) * _LightPositionRange.w)
  , 0.999)));
  highp vec4 tmpvar_2;
  tmpvar_2 = (tmpvar_1 - (tmpvar_1.yzww * 0.003921569));
  gl_FragData[0] = tmpvar_2;
}


#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_DEPTH" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_CUBE" }
""
}
}
 }
}
Fallback "Unlit/Texture"
}