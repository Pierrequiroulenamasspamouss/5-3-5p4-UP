//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Standard/Matcap" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _MatCapTex ("MatCap (RGB)", 2D) = "white" { }
 _Boost ("Boost", Float) = 1
 _MatCapBoost ("MatCap Boost", Float) = 1
 _BlendedColor ("Blended Color", Color) = (0,0,0,0)
[PerRendererData]  _FadeAlpha ("Fade Alpha", Range(0,1)) = 1
[HideInInspector] [Enum(Kampai.Util.Graphics.BlendMode)]  _Mode ("Rendering Queue", Float) = 0
[HideInInspector]  _LayerIndex ("Layer index", Float) = 0
 _OffsetFactor ("Offset Factor", Float) = 0
 _OffsetUnits ("Offset Units", Float) = 0
 _TransparencyLM ("Transmissive Color", 2D) = "white" { }
 _VertexAnimScale ("Vertex Color Scale", Float) = 0
 _VertexAnimSpeed ("Animation Speed", Float) = 1
[Enum(UnityEngine.Rendering.BlendMode)]  _SrcBlend ("Source Blend mode", Float) = 5
[Enum(UnityEngine.Rendering.BlendMode)]  _DstBlend ("Dest Blend mode", Float) = 10
[Enum(Kampai.Util.Graphics.CompareFunction)]  _ZTest ("ZTest", Float) = 4
[Enum(Kampai.Editor.AlphaMode)]  _Alpha ("Transparent", Float) = 1
[Enum(Kampai.Editor.ToggleValue)]  _ZWrite ("ZWrite", Float) = 1
[Enum(UnityEngine.Rendering.CullMode)]  _Cull ("Cull", Float) = 2
[Enum(Kampai.Editor.ToggleValue)]  _VertexColor ("Vertex Color", Float) = 0
[Enum(Kampai.Editor.MatCapBlend)]  _MatCapBlend ("MatCap Blend", Float) = 0
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
 Tags { "LIGHTMODE"="Always" }
 Pass {
  Tags { "LIGHTMODE"="Always" }
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
Keywords { "LIGHTMAP_OFF" "VERTEX_STANDARD" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform lowp vec4 _MainTex_ST;
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
  lowp vec3 tmpvar_3;
  tmpvar_3 = normalize(v_2.xyz);
  highp vec4 v_4;
  v_4.x = glstate_matrix_invtrans_modelview0[0].y;
  v_4.y = glstate_matrix_invtrans_modelview0[1].y;
  v_4.z = glstate_matrix_invtrans_modelview0[2].y;
  v_4.w = glstate_matrix_invtrans_modelview0[3].y;
  lowp vec3 tmpvar_5;
  tmpvar_5 = normalize(v_4.xyz);
  highp vec2 tmpvar_6;
  tmpvar_6.x = dot (tmpvar_3, _glesNormal);
  tmpvar_6.y = dot (tmpvar_5, _glesNormal);
  tmpvar_1 = ((tmpvar_6 * 0.5) + 0.5);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _MatCapTex;
uniform lowp float _Boost;
uniform lowp float _MatCapBoost;
uniform lowp float _MatCapBlend;
uniform lowp float _Alpha;
uniform lowp vec4 _Color;
uniform lowp float _FadeAlpha;
uniform lowp vec4 _BlendedColor;
varying lowp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0) * xlv_COLOR) * (_Color * _Boost));
  lowp vec3 tmpvar_2;
  tmpvar_2 = (texture2D (_MatCapTex, xlv_TEXCOORD1).xyz * _MatCapBoost);
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = mix (mix ((tmpvar_1.xyz * tmpvar_2), (tmpvar_1.xyz + tmpvar_2), vec3(_MatCapBlend)), _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_3.w = ((_Alpha + tmpvar_1.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "VERTEX_ANIM" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform lowp vec4 _MainTex_ST;
uniform lowp float _VertexAnimScale;
uniform lowp float _VertexAnimSpeed;
varying lowp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.w = _glesVertex.w;
  mediump vec2 tmpvar_2;
  tmpvar_1.xyz = (_glesVertex.xyz + ((
    (2.0 * _glesColor.xyz)
   - 1.0) * (
    sin((_Time.y * _VertexAnimSpeed))
   * _VertexAnimScale)));
  highp vec4 v_3;
  v_3.x = glstate_matrix_invtrans_modelview0[0].x;
  v_3.y = glstate_matrix_invtrans_modelview0[1].x;
  v_3.z = glstate_matrix_invtrans_modelview0[2].x;
  v_3.w = glstate_matrix_invtrans_modelview0[3].x;
  lowp vec3 tmpvar_4;
  tmpvar_4 = normalize(v_3.xyz);
  highp vec4 v_5;
  v_5.x = glstate_matrix_invtrans_modelview0[0].y;
  v_5.y = glstate_matrix_invtrans_modelview0[1].y;
  v_5.z = glstate_matrix_invtrans_modelview0[2].y;
  v_5.w = glstate_matrix_invtrans_modelview0[3].y;
  lowp vec3 tmpvar_6;
  tmpvar_6 = normalize(v_5.xyz);
  highp vec2 tmpvar_7;
  tmpvar_7.x = dot (tmpvar_4, _glesNormal);
  tmpvar_7.y = dot (tmpvar_6, _glesNormal);
  tmpvar_2 = ((tmpvar_7 * 0.5) + 0.5);
  gl_Position = (glstate_matrix_mvp * tmpvar_1);
  xlv_COLOR = vec4(1.0, 1.0, 1.0, 1.0);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _MatCapTex;
uniform lowp float _Boost;
uniform lowp float _MatCapBoost;
uniform lowp float _MatCapBlend;
uniform lowp float _Alpha;
uniform lowp vec4 _Color;
uniform lowp float _FadeAlpha;
uniform lowp vec4 _BlendedColor;
varying lowp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = ((texture2D (_MainTex, xlv_TEXCOORD0) * xlv_COLOR) * (_Color * _Boost));
  lowp vec3 tmpvar_2;
  tmpvar_2 = (texture2D (_MatCapTex, xlv_TEXCOORD1).xyz * _MatCapBoost);
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = mix (mix ((tmpvar_1.xyz * tmpvar_2), (tmpvar_1.xyz + tmpvar_2), vec3(_MatCapBlend)), _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_3.w = ((_Alpha + tmpvar_1.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "VERTEX_STANDARD" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp vec4 unity_LightmapST;
uniform lowp vec4 _MainTex_ST;
uniform lowp float _VertexColor;
varying lowp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  mediump vec2 tmpvar_1;
  highp vec4 v_2;
  v_2.x = glstate_matrix_invtrans_modelview0[0].x;
  v_2.y = glstate_matrix_invtrans_modelview0[1].x;
  v_2.z = glstate_matrix_invtrans_modelview0[2].x;
  v_2.w = glstate_matrix_invtrans_modelview0[3].x;
  lowp vec3 tmpvar_3;
  tmpvar_3 = normalize(v_2.xyz);
  highp vec4 v_4;
  v_4.x = glstate_matrix_invtrans_modelview0[0].y;
  v_4.y = glstate_matrix_invtrans_modelview0[1].y;
  v_4.z = glstate_matrix_invtrans_modelview0[2].y;
  v_4.w = glstate_matrix_invtrans_modelview0[3].y;
  lowp vec3 tmpvar_5;
  tmpvar_5 = normalize(v_4.xyz);
  highp vec2 tmpvar_6;
  tmpvar_6.x = dot (tmpvar_3, _glesNormal);
  tmpvar_6.y = dot (tmpvar_5, _glesNormal);
  tmpvar_1 = ((tmpvar_6 * 0.5) + 0.5);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = mix (vec4(1.0, 1.0, 1.0, 1.0), _glesColor, vec4(_VertexColor));
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform sampler2D _MatCapTex;
uniform lowp float _Boost;
uniform lowp float _MatCapBoost;
uniform lowp float _MatCapBlend;
uniform lowp float _Alpha;
uniform lowp vec4 _Color;
uniform lowp float _FadeAlpha;
uniform lowp vec4 _BlendedColor;
varying lowp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 lightMap_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * xlv_COLOR) * (_Color * _Boost));
  lowp vec3 tmpvar_3;
  tmpvar_3 = (texture2D (_MatCapTex, xlv_TEXCOORD1).xyz * _MatCapBoost);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (unity_Lightmap, xlv_TEXCOORD2);
  mediump vec3 tmpvar_5;
  tmpvar_5 = (2.0 * tmpvar_4.xyz);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_5;
  lightMap_1 = tmpvar_6;
  lowp vec4 tmpvar_7;
  tmpvar_7.xyz = mix (mix ((tmpvar_2.xyz * tmpvar_3), (tmpvar_2.xyz + tmpvar_3), vec3(_MatCapBlend)), _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_7.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  lowp vec4 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * lightMap_1);
  gl_FragData[0] = tmpvar_8;
}


#endif
"
}
SubProgram "gles " {
Keywords { "VERTEX_ANIM" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp vec4 unity_LightmapST;
uniform lowp vec4 _MainTex_ST;
uniform lowp float _VertexAnimScale;
uniform lowp float _VertexAnimSpeed;
varying lowp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.w = _glesVertex.w;
  mediump vec2 tmpvar_2;
  tmpvar_1.xyz = (_glesVertex.xyz + ((
    (2.0 * _glesColor.xyz)
   - 1.0) * (
    sin((_Time.y * _VertexAnimSpeed))
   * _VertexAnimScale)));
  highp vec4 v_3;
  v_3.x = glstate_matrix_invtrans_modelview0[0].x;
  v_3.y = glstate_matrix_invtrans_modelview0[1].x;
  v_3.z = glstate_matrix_invtrans_modelview0[2].x;
  v_3.w = glstate_matrix_invtrans_modelview0[3].x;
  lowp vec3 tmpvar_4;
  tmpvar_4 = normalize(v_3.xyz);
  highp vec4 v_5;
  v_5.x = glstate_matrix_invtrans_modelview0[0].y;
  v_5.y = glstate_matrix_invtrans_modelview0[1].y;
  v_5.z = glstate_matrix_invtrans_modelview0[2].y;
  v_5.w = glstate_matrix_invtrans_modelview0[3].y;
  lowp vec3 tmpvar_6;
  tmpvar_6 = normalize(v_5.xyz);
  highp vec2 tmpvar_7;
  tmpvar_7.x = dot (tmpvar_4, _glesNormal);
  tmpvar_7.y = dot (tmpvar_6, _glesNormal);
  tmpvar_2 = ((tmpvar_7 * 0.5) + 0.5);
  gl_Position = (glstate_matrix_mvp * tmpvar_1);
  xlv_COLOR = vec4(1.0, 1.0, 1.0, 1.0);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_2;
  xlv_TEXCOORD2 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
}


#endif
#ifdef FRAGMENT
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
uniform sampler2D _MatCapTex;
uniform lowp float _Boost;
uniform lowp float _MatCapBoost;
uniform lowp float _MatCapBlend;
uniform lowp float _Alpha;
uniform lowp vec4 _Color;
uniform lowp float _FadeAlpha;
uniform lowp vec4 _BlendedColor;
varying lowp vec4 xlv_COLOR;
varying mediump vec2 xlv_TEXCOORD0;
varying mediump vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 lightMap_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = ((texture2D (_MainTex, xlv_TEXCOORD0) * xlv_COLOR) * (_Color * _Boost));
  lowp vec3 tmpvar_3;
  tmpvar_3 = (texture2D (_MatCapTex, xlv_TEXCOORD1).xyz * _MatCapBoost);
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (unity_Lightmap, xlv_TEXCOORD2);
  mediump vec3 tmpvar_5;
  tmpvar_5 = (2.0 * tmpvar_4.xyz);
  mediump vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = tmpvar_5;
  lightMap_1 = tmpvar_6;
  lowp vec4 tmpvar_7;
  tmpvar_7.xyz = mix (mix ((tmpvar_2.xyz * tmpvar_3), (tmpvar_2.xyz + tmpvar_3), vec3(_MatCapBlend)), _BlendedColor.xyz, _BlendedColor.www);
  tmpvar_7.w = ((_Alpha + tmpvar_2.w) * _FadeAlpha);
  lowp vec4 tmpvar_8;
  tmpvar_8 = (tmpvar_7 * lightMap_1);
  gl_FragData[0] = tmpvar_8;
}


#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "VERTEX_STANDARD" }
""
}
SubProgram "gles " {
Keywords { "LIGHTMAP_OFF" "VERTEX_ANIM" }
""
}
SubProgram "gles " {
Keywords { "VERTEX_STANDARD" }
""
}
SubProgram "gles " {
Keywords { "VERTEX_ANIM" }
""
}
}
 }
}
Fallback "Unlit/Texture"
}