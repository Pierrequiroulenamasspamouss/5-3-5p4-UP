//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Standard/Minion_LOD1" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "gray" { }
 _MatCapBase ("MatCapBase (RGB)", 2D) = "gray" { }
 _LightColor ("Light Color (RGB)", Color) = (0.733,0.706,0.525,1)
[HideInInspector]  _Mode ("Rendering Queue", Float) = 0
[HideInInspector]  _LayerIndex ("Layer index", Float) = 0
 __Stencil ("Ref", Float) = 0
[Enum(UnityEngine.Rendering.CompareFunction)]  __StencilComp ("Comparison", Float) = 8
 __StencilReadMask ("Read Mask", Float) = 255
 __StencilWriteMask ("Write Mask", Float) = 255
[Enum(UnityEngine.Rendering.StencilOp)]  __StencilPassOp ("Pass Operation", Float) = 0
[Enum(UnityEngine.Rendering.StencilOp)]  __StencilFailOp ("Fail Operation", Float) = 0
[Enum(UnityEngine.Rendering.StencilOp)]  __StencilZFailOp ("ZFail Operation", Float) = 0
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="Always" "RenderType"="Opaque" }
  Stencil {
   Ref [__Stencil]
   ReadMask [__StencilReadMask]
   WriteMask [__StencilWriteMask]
   Comp [__StencilComp]
   Pass [__StencilPassOp]
   Fail [__StencilFailOp]
   ZFail [__StencilZFailOp]
  }
GLSLPROGRAM
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp vec4 _MainTex_ST;
varying lowp vec2 xlv_TEXCOORD0;
varying lowp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec2 capCoord_1;
  lowp vec2 tmpvar_2;
  tmpvar_2 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  lowp vec3 tmpvar_3;
  tmpvar_3 = normalize(_glesNormal);
  highp vec4 v_4;
  v_4.x = glstate_matrix_invtrans_modelview0[0].x;
  v_4.y = glstate_matrix_invtrans_modelview0[1].x;
  v_4.z = glstate_matrix_invtrans_modelview0[2].x;
  v_4.w = glstate_matrix_invtrans_modelview0[3].x;
  highp vec3 tmpvar_5;
  tmpvar_5 = normalize(v_4.xyz);
  capCoord_1.x = dot (tmpvar_5, tmpvar_3);
  highp vec4 v_6;
  v_6.x = glstate_matrix_invtrans_modelview0[0].y;
  v_6.y = glstate_matrix_invtrans_modelview0[1].y;
  v_6.z = glstate_matrix_invtrans_modelview0[2].y;
  v_6.w = glstate_matrix_invtrans_modelview0[3].y;
  highp vec3 tmpvar_7;
  tmpvar_7 = normalize(v_6.xyz);
  capCoord_1.y = dot (tmpvar_7, tmpvar_3);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_TEXCOORD1 = ((capCoord_1 * 0.5) + 0.5);
  xlv_COLOR = _glesColor;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 _LightColor;
uniform sampler2D _MainTex;
uniform sampler2D _MatCapBase;
varying lowp vec2 xlv_TEXCOORD0;
varying lowp vec2 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec3 tmpvar_1;
  tmpvar_1 = (_LightColor + (xlv_COLOR.z * 0.25)).xyz;
  lowp vec4 tmpvar_2;
  tmpvar_2.w = 1.0;
  tmpvar_2.xyz = (((
    (texture2D (_MatCapBase, xlv_TEXCOORD1).y * xlv_COLOR.x)
   * 1.75) + pow (texture2D (_MainTex, xlv_TEXCOORD0).xyz, vec3(2.25, 2.25, 2.25))) * (tmpvar_1 + (tmpvar_1 * 0.6)));
  gl_FragData[0] = tmpvar_2;
}


#endif

ENDGLSL
 }
}
Fallback "VertexLit"
}