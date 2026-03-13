//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Water/Flowing Water" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _WaterTex ("Normal (RG) Waves (B)", 2D) = "white" { }
 _ColorLookup ("Color Lookup (RGB)", 2D) = "white" { }
 _AlphaTex ("Alpha Mask (Grey)", 2D) = "white" { }
 _Speed ("Speed", Float) = 5
 _Distance ("Distance", Float) = 10
 _Distortion ("Distortion", Float) = 0.1
 _WavePower ("Wave Power", Float) = 0.25
 _MaxIntensity ("Max Intensity", Float) = 0.5
 _AlphaScalar ("Alpha Scalar", Float) = 1
 _LightmapDistort ("Lightmap Distortion", Float) = 0.1
 _BlendedColor ("Blended Color", Color) = (0,0,0,0)
[PerRendererData]  _FadeAlpha ("Fade Alpha", Range(0,1)) = 1
 _Color ("Color", Color) = (1,1,1,1)
[Enum(Kampai.Util.Graphics.ColorMask)]  _ColorMask ("Color Mask", Float) = 15
[HideInInspector] [Enum(Kampai.Util.Graphics.BlendMode)]  _Mode ("Rendering Queue", Float) = 0
[HideInInspector]  _LayerIndex ("Layer index", Float) = 0
 _OffsetFactor ("Offset Factor", Float) = 0
 _OffsetUnits ("Offset Units", Float) = 0
[Enum(UnityEngine.Rendering.BlendMode)]  _SrcBlend ("Source Blend mode", Float) = 5
[Enum(UnityEngine.Rendering.BlendMode)]  _DstBlend ("Dest Blend mode", Float) = 10
[Enum(Kampai.Util.Graphics.CompareFunction)]  _ZTest ("ZTest", Float) = 4
[Enum(Kampai.Editor.ToggleValue)]  _ZWrite ("ZWrite", Float) = 1
[Enum(UnityEngine.Rendering.CullMode)]  _Cull ("Cull", Float) = 2
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="ForwardBase" "SHADOWSUPPORT"="true" "RenderType"="Opaque" }
  ZTest [_ZTest]
  ZWrite [_ZWrite]
  Cull [_Cull]
  Blend [_SrcBlend] [_DstBlend]
  ColorMask [_ColorMask]
  Offset [_OffsetFactor], [_OffsetUnits]
Program "vp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _WaterTex_ST;
uniform mediump float _Speed;
uniform mediump float _Distance;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying highp vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float t_2;
  highp vec2 flowDir_3;
  highp vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  highp vec3 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mediump vec2 tmpvar_8;
  tmpvar_8 = ((_glesColor.xy - 0.5) * _Distance);
  flowDir_3 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * _WaterTex_ST.xy);
  highp float tmpvar_10;
  tmpvar_10 = fract((_Time.x * _Speed));
  t_2 = tmpvar_10;
  mediump float tmpvar_11;
  tmpvar_11 = fract((t_2 + 0.5));
  lowp vec2 tmpvar_12;
  tmpvar_12.y = 0.0;
  tmpvar_12.x = tmpvar_1.z;
  tmpvar_5 = tmpvar_12;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = (tmpvar_9 - (flowDir_3 * t_2));
  xlv_TEXCOORD2 = (tmpvar_9 - (flowDir_3 * tmpvar_11));
  xlv_TEXCOORD3 = ((cos(
    (6.283 * t_2)
  ) * 0.5) + 0.5);
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _WaterTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _ColorLookup;
uniform mediump float _WavePower;
uniform mediump float _Distortion;
uniform mediump float _MaxIntensity;
uniform mediump float _AlphaScalar;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _FadeAlpha;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
void main ()
{
  highp vec3 waterTint_1;
  lowp vec3 main_2;
  highp float cap_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_ColorLookup, xlv_TEXCOORD4);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_WaterTex, xlv_TEXCOORD1);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_WaterTex, xlv_TEXCOORD2);
  lowp float tmpvar_7;
  tmpvar_7 = clamp ((1.02 - xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_8;
  tmpvar_8 = (((
    mix (tmpvar_5.xy, tmpvar_6.xy, vec2(xlv_TEXCOORD3))
   * 2.0) - 1.0) * (tmpvar_7 * _MaxIntensity));
  lowp vec4 tmpvar_9;
  mediump vec2 P_10;
  P_10 = (tmpvar_8 - 0.5);
  tmpvar_9 = texture2D (_WaterTex, P_10);
  mediump float tmpvar_11;
  tmpvar_11 = (tmpvar_9.z * _WavePower);
  cap_3 = tmpvar_11;
  lowp float tmpvar_12;
  tmpvar_12 = clamp ((10.0 * xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_13;
  tmpvar_13 = ((tmpvar_8 * _Distortion) * tmpvar_12);
  lowp vec4 tmpvar_14;
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0 - tmpvar_13);
  tmpvar_14 = texture2D (_MainTex, P_15);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0 - tmpvar_13);
  waterTint_1 = (clamp ((
    (tmpvar_4.xyz * cap_3)
   * 2.0), 0.0, 1.0) * xlv_COLOR.w);
  waterTint_1 = (waterTint_1 * 4.0);
  highp vec3 tmpvar_17;
  tmpvar_17 = clamp (waterTint_1, 0.0, 1.0);
  waterTint_1 = tmpvar_17;
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((xlv_COLOR.z * _AlphaScalar), 0.0, 1.0);
  highp vec3 tmpvar_19;
  tmpvar_19 = mix (tmpvar_14.xyz, tmpvar_17, vec3(tmpvar_18));
  main_2 = tmpvar_19;
  lowp vec3 tmpvar_20;
  tmpvar_20 = mix (main_2, _BlendedColor.xyz, _BlendedColor.www);
  main_2 = tmpvar_20;
  lowp vec4 tmpvar_21;
  tmpvar_21.xyz = tmpvar_20;
  tmpvar_21.w = ((texture2D (_AlphaTex, P_16).x * _Color.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_21;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _WaterTex_ST;
uniform mediump float _Speed;
uniform mediump float _Distance;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying highp vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float t_2;
  highp vec2 flowDir_3;
  highp vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  highp vec3 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mediump vec2 tmpvar_8;
  tmpvar_8 = ((_glesColor.xy - 0.5) * _Distance);
  flowDir_3 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * _WaterTex_ST.xy);
  highp float tmpvar_10;
  tmpvar_10 = fract((_Time.x * _Speed));
  t_2 = tmpvar_10;
  mediump float tmpvar_11;
  tmpvar_11 = fract((t_2 + 0.5));
  lowp vec2 tmpvar_12;
  tmpvar_12.y = 0.0;
  tmpvar_12.x = tmpvar_1.z;
  tmpvar_5 = tmpvar_12;
  tmpvar_7 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = (tmpvar_9 - (flowDir_3 * t_2));
  xlv_TEXCOORD2 = (tmpvar_9 - (flowDir_3 * tmpvar_11));
  xlv_TEXCOORD3 = ((cos(
    (6.283 * t_2)
  ) * 0.5) + 0.5);
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _WaterTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _ColorLookup;
uniform mediump float _WavePower;
uniform mediump float _Distortion;
uniform mediump float _MaxIntensity;
uniform mediump float _AlphaScalar;
uniform mediump float _LightmapDistort;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _FadeAlpha;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec3 waterTint_2;
  lowp vec3 main_3;
  highp float cap_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_ColorLookup, xlv_TEXCOORD4);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_WaterTex, xlv_TEXCOORD1);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_WaterTex, xlv_TEXCOORD2);
  lowp float tmpvar_8;
  tmpvar_8 = clamp ((1.02 - xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_9;
  tmpvar_9 = (((
    mix (tmpvar_6.xy, tmpvar_7.xy, vec2(xlv_TEXCOORD3))
   * 2.0) - 1.0) * (tmpvar_8 * _MaxIntensity));
  lowp vec4 tmpvar_10;
  mediump vec2 P_11;
  P_11 = (tmpvar_9 - 0.5);
  tmpvar_10 = texture2D (_WaterTex, P_11);
  mediump float tmpvar_12;
  tmpvar_12 = (tmpvar_10.z * _WavePower);
  cap_4 = tmpvar_12;
  lowp float tmpvar_13;
  tmpvar_13 = clamp ((10.0 * xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_14;
  tmpvar_14 = ((tmpvar_9 * _Distortion) * tmpvar_13);
  lowp vec4 tmpvar_15;
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0 - tmpvar_14);
  tmpvar_15 = texture2D (_MainTex, P_16);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0 - tmpvar_14);
  waterTint_2 = (clamp ((
    (tmpvar_5.xyz * cap_4)
   * 2.0), 0.0, 1.0) * xlv_COLOR.w);
  waterTint_2 = (waterTint_2 * 4.0);
  highp vec3 tmpvar_18;
  tmpvar_18 = clamp (waterTint_2, 0.0, 1.0);
  waterTint_2 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = clamp ((xlv_COLOR.z * _AlphaScalar), 0.0, 1.0);
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (tmpvar_15.xyz, tmpvar_18, vec3(tmpvar_19));
  main_3 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = mix (main_3, _BlendedColor.xyz, _BlendedColor.www);
  main_3 = tmpvar_21;
  tmpvar_1.xy = (xlv_TEXCOORD6.xy - ((_LightmapDistort * tmpvar_14) * 4.0));
  tmpvar_1.zw = (xlv_TEXCOORD6.zw - ((_LightmapDistort * tmpvar_14) * 4.0));
  lowp float tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = max (float((texture2DProj (_ShadowMapTexture, tmpvar_1).x > 
    (tmpvar_1.z / tmpvar_1.w)
  )), _LightShadowData.x);
  tmpvar_22 = tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_24.xyz = (tmpvar_21 * tmpvar_22);
  tmpvar_24.w = ((texture2D (_AlphaTex, P_17).x * _Color.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_24;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _WaterTex_ST;
uniform mediump float _Speed;
uniform mediump float _Distance;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying highp vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float t_2;
  highp vec2 flowDir_3;
  highp vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  highp vec3 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mediump vec2 tmpvar_8;
  tmpvar_8 = ((_glesColor.xy - 0.5) * _Distance);
  flowDir_3 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * _WaterTex_ST.xy);
  highp float tmpvar_10;
  tmpvar_10 = fract((_Time.x * _Speed));
  t_2 = tmpvar_10;
  mediump float tmpvar_11;
  tmpvar_11 = fract((t_2 + 0.5));
  lowp vec2 tmpvar_12;
  tmpvar_12.y = 0.0;
  tmpvar_12.x = tmpvar_1.z;
  tmpvar_5 = tmpvar_12;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = (tmpvar_9 - (flowDir_3 * t_2));
  xlv_TEXCOORD2 = (tmpvar_9 - (flowDir_3 * tmpvar_11));
  xlv_TEXCOORD3 = ((cos(
    (6.283 * t_2)
  ) * 0.5) + 0.5);
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _WaterTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _ColorLookup;
uniform mediump float _WavePower;
uniform mediump float _Distortion;
uniform mediump float _MaxIntensity;
uniform mediump float _AlphaScalar;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _FadeAlpha;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
void main ()
{
  highp vec3 waterTint_1;
  lowp vec3 main_2;
  highp float cap_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_ColorLookup, xlv_TEXCOORD4);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_WaterTex, xlv_TEXCOORD1);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_WaterTex, xlv_TEXCOORD2);
  lowp float tmpvar_7;
  tmpvar_7 = clamp ((1.02 - xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_8;
  tmpvar_8 = (((
    mix (tmpvar_5.xy, tmpvar_6.xy, vec2(xlv_TEXCOORD3))
   * 2.0) - 1.0) * (tmpvar_7 * _MaxIntensity));
  lowp vec4 tmpvar_9;
  mediump vec2 P_10;
  P_10 = (tmpvar_8 - 0.5);
  tmpvar_9 = texture2D (_WaterTex, P_10);
  mediump float tmpvar_11;
  tmpvar_11 = (tmpvar_9.z * _WavePower);
  cap_3 = tmpvar_11;
  lowp float tmpvar_12;
  tmpvar_12 = clamp ((10.0 * xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_13;
  tmpvar_13 = ((tmpvar_8 * _Distortion) * tmpvar_12);
  lowp vec4 tmpvar_14;
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0 - tmpvar_13);
  tmpvar_14 = texture2D (_MainTex, P_15);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0 - tmpvar_13);
  waterTint_1 = (clamp ((
    (tmpvar_4.xyz * cap_3)
   * 2.0), 0.0, 1.0) * xlv_COLOR.w);
  waterTint_1 = (waterTint_1 * 4.0);
  highp vec3 tmpvar_17;
  tmpvar_17 = clamp (waterTint_1, 0.0, 1.0);
  waterTint_1 = tmpvar_17;
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((xlv_COLOR.z * _AlphaScalar), 0.0, 1.0);
  highp vec3 tmpvar_19;
  tmpvar_19 = mix (tmpvar_14.xyz, tmpvar_17, vec3(tmpvar_18));
  main_2 = tmpvar_19;
  lowp vec3 tmpvar_20;
  tmpvar_20 = mix (main_2, _BlendedColor.xyz, _BlendedColor.www);
  main_2 = tmpvar_20;
  lowp vec4 tmpvar_21;
  tmpvar_21.xyz = tmpvar_20;
  tmpvar_21.w = ((texture2D (_AlphaTex, P_16).x * _Color.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_21;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _WaterTex_ST;
uniform mediump float _Speed;
uniform mediump float _Distance;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying highp vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float t_2;
  highp vec2 flowDir_3;
  highp vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  highp vec3 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mediump vec2 tmpvar_8;
  tmpvar_8 = ((_glesColor.xy - 0.5) * _Distance);
  flowDir_3 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * _WaterTex_ST.xy);
  highp float tmpvar_10;
  tmpvar_10 = fract((_Time.x * _Speed));
  t_2 = tmpvar_10;
  mediump float tmpvar_11;
  tmpvar_11 = fract((t_2 + 0.5));
  lowp vec2 tmpvar_12;
  tmpvar_12.y = 0.0;
  tmpvar_12.x = tmpvar_1.z;
  tmpvar_5 = tmpvar_12;
  tmpvar_7 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = (tmpvar_9 - (flowDir_3 * t_2));
  xlv_TEXCOORD2 = (tmpvar_9 - (flowDir_3 * tmpvar_11));
  xlv_TEXCOORD3 = ((cos(
    (6.283 * t_2)
  ) * 0.5) + 0.5);
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _WaterTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _ColorLookup;
uniform mediump float _WavePower;
uniform mediump float _Distortion;
uniform mediump float _MaxIntensity;
uniform mediump float _AlphaScalar;
uniform mediump float _LightmapDistort;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _FadeAlpha;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec3 waterTint_2;
  lowp vec3 main_3;
  highp float cap_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_ColorLookup, xlv_TEXCOORD4);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_WaterTex, xlv_TEXCOORD1);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_WaterTex, xlv_TEXCOORD2);
  lowp float tmpvar_8;
  tmpvar_8 = clamp ((1.02 - xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_9;
  tmpvar_9 = (((
    mix (tmpvar_6.xy, tmpvar_7.xy, vec2(xlv_TEXCOORD3))
   * 2.0) - 1.0) * (tmpvar_8 * _MaxIntensity));
  lowp vec4 tmpvar_10;
  mediump vec2 P_11;
  P_11 = (tmpvar_9 - 0.5);
  tmpvar_10 = texture2D (_WaterTex, P_11);
  mediump float tmpvar_12;
  tmpvar_12 = (tmpvar_10.z * _WavePower);
  cap_4 = tmpvar_12;
  lowp float tmpvar_13;
  tmpvar_13 = clamp ((10.0 * xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_14;
  tmpvar_14 = ((tmpvar_9 * _Distortion) * tmpvar_13);
  lowp vec4 tmpvar_15;
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0 - tmpvar_14);
  tmpvar_15 = texture2D (_MainTex, P_16);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0 - tmpvar_14);
  waterTint_2 = (clamp ((
    (tmpvar_5.xyz * cap_4)
   * 2.0), 0.0, 1.0) * xlv_COLOR.w);
  waterTint_2 = (waterTint_2 * 4.0);
  highp vec3 tmpvar_18;
  tmpvar_18 = clamp (waterTint_2, 0.0, 1.0);
  waterTint_2 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = clamp ((xlv_COLOR.z * _AlphaScalar), 0.0, 1.0);
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (tmpvar_15.xyz, tmpvar_18, vec3(tmpvar_19));
  main_3 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = mix (main_3, _BlendedColor.xyz, _BlendedColor.www);
  main_3 = tmpvar_21;
  tmpvar_1.xy = (xlv_TEXCOORD6.xy - ((_LightmapDistort * tmpvar_14) * 4.0));
  tmpvar_1.zw = (xlv_TEXCOORD6.zw - ((_LightmapDistort * tmpvar_14) * 4.0));
  lowp float tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = max (float((texture2DProj (_ShadowMapTexture, tmpvar_1).x > 
    (tmpvar_1.z / tmpvar_1.w)
  )), _LightShadowData.x);
  tmpvar_22 = tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_24.xyz = (tmpvar_21 * tmpvar_22);
  tmpvar_24.w = ((texture2D (_AlphaTex, P_17).x * _Color.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_24;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
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
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _WaterTex_ST;
uniform mediump float _Speed;
uniform mediump float _Distance;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying mediump vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float t_2;
  highp vec2 flowDir_3;
  highp vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mediump vec2 tmpvar_8;
  tmpvar_8 = ((_glesColor.xy - 0.5) * _Distance);
  flowDir_3 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * _WaterTex_ST.xy);
  highp float tmpvar_10;
  tmpvar_10 = fract((_Time.x * _Speed));
  t_2 = tmpvar_10;
  mediump float tmpvar_11;
  tmpvar_11 = fract((t_2 + 0.5));
  lowp vec2 tmpvar_12;
  tmpvar_12.y = 0.0;
  tmpvar_12.x = tmpvar_1.z;
  tmpvar_5 = tmpvar_12;
  tmpvar_7 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = (tmpvar_9 - (flowDir_3 * t_2));
  xlv_TEXCOORD2 = (tmpvar_9 - (flowDir_3 * tmpvar_11));
  xlv_TEXCOORD3 = ((cos(
    (6.283 * t_2)
  ) * 0.5) + 0.5);
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _WaterTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _ColorLookup;
uniform mediump float _WavePower;
uniform mediump float _Distortion;
uniform mediump float _MaxIntensity;
uniform mediump float _AlphaScalar;
uniform mediump float _LightmapDistort;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _FadeAlpha;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec3 waterTint_2;
  lowp vec3 main_3;
  highp float cap_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_ColorLookup, xlv_TEXCOORD4);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_WaterTex, xlv_TEXCOORD1);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_WaterTex, xlv_TEXCOORD2);
  lowp float tmpvar_8;
  tmpvar_8 = clamp ((1.02 - xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_9;
  tmpvar_9 = (((
    mix (tmpvar_6.xy, tmpvar_7.xy, vec2(xlv_TEXCOORD3))
   * 2.0) - 1.0) * (tmpvar_8 * _MaxIntensity));
  lowp vec4 tmpvar_10;
  mediump vec2 P_11;
  P_11 = (tmpvar_9 - 0.5);
  tmpvar_10 = texture2D (_WaterTex, P_11);
  mediump float tmpvar_12;
  tmpvar_12 = (tmpvar_10.z * _WavePower);
  cap_4 = tmpvar_12;
  lowp float tmpvar_13;
  tmpvar_13 = clamp ((10.0 * xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_14;
  tmpvar_14 = ((tmpvar_9 * _Distortion) * tmpvar_13);
  lowp vec4 tmpvar_15;
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0 - tmpvar_14);
  tmpvar_15 = texture2D (_MainTex, P_16);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0 - tmpvar_14);
  waterTint_2 = (clamp ((
    (tmpvar_5.xyz * cap_4)
   * 2.0), 0.0, 1.0) * xlv_COLOR.w);
  waterTint_2 = (waterTint_2 * 4.0);
  highp vec3 tmpvar_18;
  tmpvar_18 = clamp (waterTint_2, 0.0, 1.0);
  waterTint_2 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = clamp ((xlv_COLOR.z * _AlphaScalar), 0.0, 1.0);
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (tmpvar_15.xyz, tmpvar_18, vec3(tmpvar_19));
  main_3 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = mix (main_3, _BlendedColor.xyz, _BlendedColor.www);
  main_3 = tmpvar_21;
  tmpvar_1.xy = (xlv_TEXCOORD6.xy - ((_LightmapDistort * tmpvar_14) * 4.0));
  tmpvar_1.zw = (xlv_TEXCOORD6.zw - ((_LightmapDistort * tmpvar_14) * 4.0));
  lowp float shadow_22;
  shadow_22 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, tmpvar_1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_23;
  tmpvar_23.xyz = (tmpvar_21 * shadow_22);
  tmpvar_23.w = ((texture2D (_AlphaTex, P_17).x * _Color.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_23;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
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
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _WaterTex_ST;
uniform mediump float _Speed;
uniform mediump float _Distance;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying mediump vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float t_2;
  highp vec2 flowDir_3;
  highp vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mediump vec2 tmpvar_8;
  tmpvar_8 = ((_glesColor.xy - 0.5) * _Distance);
  flowDir_3 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * _WaterTex_ST.xy);
  highp float tmpvar_10;
  tmpvar_10 = fract((_Time.x * _Speed));
  t_2 = tmpvar_10;
  mediump float tmpvar_11;
  tmpvar_11 = fract((t_2 + 0.5));
  lowp vec2 tmpvar_12;
  tmpvar_12.y = 0.0;
  tmpvar_12.x = tmpvar_1.z;
  tmpvar_5 = tmpvar_12;
  tmpvar_7 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = (tmpvar_9 - (flowDir_3 * t_2));
  xlv_TEXCOORD2 = (tmpvar_9 - (flowDir_3 * tmpvar_11));
  xlv_TEXCOORD3 = ((cos(
    (6.283 * t_2)
  ) * 0.5) + 0.5);
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _WaterTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _ColorLookup;
uniform mediump float _WavePower;
uniform mediump float _Distortion;
uniform mediump float _MaxIntensity;
uniform mediump float _AlphaScalar;
uniform mediump float _LightmapDistort;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _FadeAlpha;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec3 waterTint_2;
  lowp vec3 main_3;
  highp float cap_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_ColorLookup, xlv_TEXCOORD4);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_WaterTex, xlv_TEXCOORD1);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_WaterTex, xlv_TEXCOORD2);
  lowp float tmpvar_8;
  tmpvar_8 = clamp ((1.02 - xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_9;
  tmpvar_9 = (((
    mix (tmpvar_6.xy, tmpvar_7.xy, vec2(xlv_TEXCOORD3))
   * 2.0) - 1.0) * (tmpvar_8 * _MaxIntensity));
  lowp vec4 tmpvar_10;
  mediump vec2 P_11;
  P_11 = (tmpvar_9 - 0.5);
  tmpvar_10 = texture2D (_WaterTex, P_11);
  mediump float tmpvar_12;
  tmpvar_12 = (tmpvar_10.z * _WavePower);
  cap_4 = tmpvar_12;
  lowp float tmpvar_13;
  tmpvar_13 = clamp ((10.0 * xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_14;
  tmpvar_14 = ((tmpvar_9 * _Distortion) * tmpvar_13);
  lowp vec4 tmpvar_15;
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0 - tmpvar_14);
  tmpvar_15 = texture2D (_MainTex, P_16);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0 - tmpvar_14);
  waterTint_2 = (clamp ((
    (tmpvar_5.xyz * cap_4)
   * 2.0), 0.0, 1.0) * xlv_COLOR.w);
  waterTint_2 = (waterTint_2 * 4.0);
  highp vec3 tmpvar_18;
  tmpvar_18 = clamp (waterTint_2, 0.0, 1.0);
  waterTint_2 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = clamp ((xlv_COLOR.z * _AlphaScalar), 0.0, 1.0);
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (tmpvar_15.xyz, tmpvar_18, vec3(tmpvar_19));
  main_3 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = mix (main_3, _BlendedColor.xyz, _BlendedColor.www);
  main_3 = tmpvar_21;
  tmpvar_1.xy = (xlv_TEXCOORD6.xy - ((_LightmapDistort * tmpvar_14) * 4.0));
  tmpvar_1.zw = (xlv_TEXCOORD6.zw - ((_LightmapDistort * tmpvar_14) * 4.0));
  lowp float shadow_22;
  shadow_22 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, tmpvar_1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_23;
  tmpvar_23.xyz = (tmpvar_21 * shadow_22);
  tmpvar_23.w = ((texture2D (_AlphaTex, P_17).x * _Color.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_23;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _WaterTex_ST;
uniform mediump float _Speed;
uniform mediump float _Distance;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying highp vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float t_2;
  highp vec2 flowDir_3;
  highp vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  highp vec3 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mediump vec2 tmpvar_8;
  tmpvar_8 = ((_glesColor.xy - 0.5) * _Distance);
  flowDir_3 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * _WaterTex_ST.xy);
  highp float tmpvar_10;
  tmpvar_10 = fract((_Time.x * _Speed));
  t_2 = tmpvar_10;
  mediump float tmpvar_11;
  tmpvar_11 = fract((t_2 + 0.5));
  lowp vec2 tmpvar_12;
  tmpvar_12.y = 0.0;
  tmpvar_12.x = tmpvar_1.z;
  tmpvar_5 = tmpvar_12;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = (tmpvar_9 - (flowDir_3 * t_2));
  xlv_TEXCOORD2 = (tmpvar_9 - (flowDir_3 * tmpvar_11));
  xlv_TEXCOORD3 = ((cos(
    (6.283 * t_2)
  ) * 0.5) + 0.5);
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _WaterTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _ColorLookup;
uniform mediump float _WavePower;
uniform mediump float _Distortion;
uniform mediump float _MaxIntensity;
uniform mediump float _AlphaScalar;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _FadeAlpha;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
void main ()
{
  highp vec3 waterTint_1;
  lowp vec3 main_2;
  highp float cap_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_ColorLookup, xlv_TEXCOORD4);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_WaterTex, xlv_TEXCOORD1);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_WaterTex, xlv_TEXCOORD2);
  lowp float tmpvar_7;
  tmpvar_7 = clamp ((1.02 - xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_8;
  tmpvar_8 = (((
    mix (tmpvar_5.xy, tmpvar_6.xy, vec2(xlv_TEXCOORD3))
   * 2.0) - 1.0) * (tmpvar_7 * _MaxIntensity));
  lowp vec4 tmpvar_9;
  mediump vec2 P_10;
  P_10 = (tmpvar_8 - 0.5);
  tmpvar_9 = texture2D (_WaterTex, P_10);
  mediump float tmpvar_11;
  tmpvar_11 = (tmpvar_9.z * _WavePower);
  cap_3 = tmpvar_11;
  lowp float tmpvar_12;
  tmpvar_12 = clamp ((10.0 * xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_13;
  tmpvar_13 = ((tmpvar_8 * _Distortion) * tmpvar_12);
  lowp vec4 tmpvar_14;
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0 - tmpvar_13);
  tmpvar_14 = texture2D (_MainTex, P_15);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0 - tmpvar_13);
  waterTint_1 = (clamp ((
    (tmpvar_4.xyz * cap_3)
   * 2.0), 0.0, 1.0) * xlv_COLOR.w);
  waterTint_1 = (waterTint_1 * 4.0);
  highp vec3 tmpvar_17;
  tmpvar_17 = clamp (waterTint_1, 0.0, 1.0);
  waterTint_1 = tmpvar_17;
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((xlv_COLOR.z * _AlphaScalar), 0.0, 1.0);
  highp vec3 tmpvar_19;
  tmpvar_19 = mix (tmpvar_14.xyz, tmpvar_17, vec3(tmpvar_18));
  main_2 = tmpvar_19;
  lowp vec3 tmpvar_20;
  tmpvar_20 = mix (main_2, _BlendedColor.xyz, _BlendedColor.www);
  main_2 = tmpvar_20;
  lowp vec4 tmpvar_21;
  tmpvar_21.xyz = tmpvar_20;
  tmpvar_21.w = ((texture2D (_AlphaTex, P_16).x * _Color.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_21;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _WaterTex_ST;
uniform mediump float _Speed;
uniform mediump float _Distance;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying highp vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float t_2;
  highp vec2 flowDir_3;
  highp vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  highp vec3 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mediump vec2 tmpvar_8;
  tmpvar_8 = ((_glesColor.xy - 0.5) * _Distance);
  flowDir_3 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * _WaterTex_ST.xy);
  highp float tmpvar_10;
  tmpvar_10 = fract((_Time.x * _Speed));
  t_2 = tmpvar_10;
  mediump float tmpvar_11;
  tmpvar_11 = fract((t_2 + 0.5));
  lowp vec2 tmpvar_12;
  tmpvar_12.y = 0.0;
  tmpvar_12.x = tmpvar_1.z;
  tmpvar_5 = tmpvar_12;
  tmpvar_7 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = (tmpvar_9 - (flowDir_3 * t_2));
  xlv_TEXCOORD2 = (tmpvar_9 - (flowDir_3 * tmpvar_11));
  xlv_TEXCOORD3 = ((cos(
    (6.283 * t_2)
  ) * 0.5) + 0.5);
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _WaterTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _ColorLookup;
uniform mediump float _WavePower;
uniform mediump float _Distortion;
uniform mediump float _MaxIntensity;
uniform mediump float _AlphaScalar;
uniform mediump float _LightmapDistort;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _FadeAlpha;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec3 waterTint_2;
  lowp vec3 main_3;
  highp float cap_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_ColorLookup, xlv_TEXCOORD4);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_WaterTex, xlv_TEXCOORD1);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_WaterTex, xlv_TEXCOORD2);
  lowp float tmpvar_8;
  tmpvar_8 = clamp ((1.02 - xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_9;
  tmpvar_9 = (((
    mix (tmpvar_6.xy, tmpvar_7.xy, vec2(xlv_TEXCOORD3))
   * 2.0) - 1.0) * (tmpvar_8 * _MaxIntensity));
  lowp vec4 tmpvar_10;
  mediump vec2 P_11;
  P_11 = (tmpvar_9 - 0.5);
  tmpvar_10 = texture2D (_WaterTex, P_11);
  mediump float tmpvar_12;
  tmpvar_12 = (tmpvar_10.z * _WavePower);
  cap_4 = tmpvar_12;
  lowp float tmpvar_13;
  tmpvar_13 = clamp ((10.0 * xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_14;
  tmpvar_14 = ((tmpvar_9 * _Distortion) * tmpvar_13);
  lowp vec4 tmpvar_15;
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0 - tmpvar_14);
  tmpvar_15 = texture2D (_MainTex, P_16);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0 - tmpvar_14);
  waterTint_2 = (clamp ((
    (tmpvar_5.xyz * cap_4)
   * 2.0), 0.0, 1.0) * xlv_COLOR.w);
  waterTint_2 = (waterTint_2 * 4.0);
  highp vec3 tmpvar_18;
  tmpvar_18 = clamp (waterTint_2, 0.0, 1.0);
  waterTint_2 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = clamp ((xlv_COLOR.z * _AlphaScalar), 0.0, 1.0);
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (tmpvar_15.xyz, tmpvar_18, vec3(tmpvar_19));
  main_3 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = mix (main_3, _BlendedColor.xyz, _BlendedColor.www);
  main_3 = tmpvar_21;
  tmpvar_1.xy = (xlv_TEXCOORD6.xy - ((_LightmapDistort * tmpvar_14) * 4.0));
  tmpvar_1.zw = (xlv_TEXCOORD6.zw - ((_LightmapDistort * tmpvar_14) * 4.0));
  lowp float tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = max (float((texture2DProj (_ShadowMapTexture, tmpvar_1).x > 
    (tmpvar_1.z / tmpvar_1.w)
  )), _LightShadowData.x);
  tmpvar_22 = tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_24.xyz = (tmpvar_21 * tmpvar_22);
  tmpvar_24.w = ((texture2D (_AlphaTex, P_17).x * _Color.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_24;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 glstate_matrix_mvp;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _WaterTex_ST;
uniform mediump float _Speed;
uniform mediump float _Distance;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying highp vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float t_2;
  highp vec2 flowDir_3;
  highp vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  highp vec3 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mediump vec2 tmpvar_8;
  tmpvar_8 = ((_glesColor.xy - 0.5) * _Distance);
  flowDir_3 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * _WaterTex_ST.xy);
  highp float tmpvar_10;
  tmpvar_10 = fract((_Time.x * _Speed));
  t_2 = tmpvar_10;
  mediump float tmpvar_11;
  tmpvar_11 = fract((t_2 + 0.5));
  lowp vec2 tmpvar_12;
  tmpvar_12.y = 0.0;
  tmpvar_12.x = tmpvar_1.z;
  tmpvar_5 = tmpvar_12;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = (tmpvar_9 - (flowDir_3 * t_2));
  xlv_TEXCOORD2 = (tmpvar_9 - (flowDir_3 * tmpvar_11));
  xlv_TEXCOORD3 = ((cos(
    (6.283 * t_2)
  ) * 0.5) + 0.5);
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _WaterTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _ColorLookup;
uniform mediump float _WavePower;
uniform mediump float _Distortion;
uniform mediump float _MaxIntensity;
uniform mediump float _AlphaScalar;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _FadeAlpha;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
void main ()
{
  highp vec3 waterTint_1;
  lowp vec3 main_2;
  highp float cap_3;
  lowp vec4 tmpvar_4;
  tmpvar_4 = texture2D (_ColorLookup, xlv_TEXCOORD4);
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_WaterTex, xlv_TEXCOORD1);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_WaterTex, xlv_TEXCOORD2);
  lowp float tmpvar_7;
  tmpvar_7 = clamp ((1.02 - xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_8;
  tmpvar_8 = (((
    mix (tmpvar_5.xy, tmpvar_6.xy, vec2(xlv_TEXCOORD3))
   * 2.0) - 1.0) * (tmpvar_7 * _MaxIntensity));
  lowp vec4 tmpvar_9;
  mediump vec2 P_10;
  P_10 = (tmpvar_8 - 0.5);
  tmpvar_9 = texture2D (_WaterTex, P_10);
  mediump float tmpvar_11;
  tmpvar_11 = (tmpvar_9.z * _WavePower);
  cap_3 = tmpvar_11;
  lowp float tmpvar_12;
  tmpvar_12 = clamp ((10.0 * xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_13;
  tmpvar_13 = ((tmpvar_8 * _Distortion) * tmpvar_12);
  lowp vec4 tmpvar_14;
  highp vec2 P_15;
  P_15 = (xlv_TEXCOORD0 - tmpvar_13);
  tmpvar_14 = texture2D (_MainTex, P_15);
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0 - tmpvar_13);
  waterTint_1 = (clamp ((
    (tmpvar_4.xyz * cap_3)
   * 2.0), 0.0, 1.0) * xlv_COLOR.w);
  waterTint_1 = (waterTint_1 * 4.0);
  highp vec3 tmpvar_17;
  tmpvar_17 = clamp (waterTint_1, 0.0, 1.0);
  waterTint_1 = tmpvar_17;
  mediump float tmpvar_18;
  tmpvar_18 = clamp ((xlv_COLOR.z * _AlphaScalar), 0.0, 1.0);
  highp vec3 tmpvar_19;
  tmpvar_19 = mix (tmpvar_14.xyz, tmpvar_17, vec3(tmpvar_18));
  main_2 = tmpvar_19;
  lowp vec3 tmpvar_20;
  tmpvar_20 = mix (main_2, _BlendedColor.xyz, _BlendedColor.www);
  main_2 = tmpvar_20;
  lowp vec4 tmpvar_21;
  tmpvar_21.xyz = tmpvar_20;
  tmpvar_21.w = ((texture2D (_AlphaTex, P_16).x * _Color.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_21;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _Time;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 _Object2World;
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _WaterTex_ST;
uniform mediump float _Speed;
uniform mediump float _Distance;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying highp vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float t_2;
  highp vec2 flowDir_3;
  highp vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  highp vec3 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mediump vec2 tmpvar_8;
  tmpvar_8 = ((_glesColor.xy - 0.5) * _Distance);
  flowDir_3 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * _WaterTex_ST.xy);
  highp float tmpvar_10;
  tmpvar_10 = fract((_Time.x * _Speed));
  t_2 = tmpvar_10;
  mediump float tmpvar_11;
  tmpvar_11 = fract((t_2 + 0.5));
  lowp vec2 tmpvar_12;
  tmpvar_12.y = 0.0;
  tmpvar_12.x = tmpvar_1.z;
  tmpvar_5 = tmpvar_12;
  tmpvar_7 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = (tmpvar_9 - (flowDir_3 * t_2));
  xlv_TEXCOORD2 = (tmpvar_9 - (flowDir_3 * tmpvar_11));
  xlv_TEXCOORD3 = ((cos(
    (6.283 * t_2)
  ) * 0.5) + 0.5);
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _WaterTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _ColorLookup;
uniform mediump float _WavePower;
uniform mediump float _Distortion;
uniform mediump float _MaxIntensity;
uniform mediump float _AlphaScalar;
uniform mediump float _LightmapDistort;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _FadeAlpha;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec3 waterTint_2;
  lowp vec3 main_3;
  highp float cap_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_ColorLookup, xlv_TEXCOORD4);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_WaterTex, xlv_TEXCOORD1);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_WaterTex, xlv_TEXCOORD2);
  lowp float tmpvar_8;
  tmpvar_8 = clamp ((1.02 - xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_9;
  tmpvar_9 = (((
    mix (tmpvar_6.xy, tmpvar_7.xy, vec2(xlv_TEXCOORD3))
   * 2.0) - 1.0) * (tmpvar_8 * _MaxIntensity));
  lowp vec4 tmpvar_10;
  mediump vec2 P_11;
  P_11 = (tmpvar_9 - 0.5);
  tmpvar_10 = texture2D (_WaterTex, P_11);
  mediump float tmpvar_12;
  tmpvar_12 = (tmpvar_10.z * _WavePower);
  cap_4 = tmpvar_12;
  lowp float tmpvar_13;
  tmpvar_13 = clamp ((10.0 * xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_14;
  tmpvar_14 = ((tmpvar_9 * _Distortion) * tmpvar_13);
  lowp vec4 tmpvar_15;
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0 - tmpvar_14);
  tmpvar_15 = texture2D (_MainTex, P_16);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0 - tmpvar_14);
  waterTint_2 = (clamp ((
    (tmpvar_5.xyz * cap_4)
   * 2.0), 0.0, 1.0) * xlv_COLOR.w);
  waterTint_2 = (waterTint_2 * 4.0);
  highp vec3 tmpvar_18;
  tmpvar_18 = clamp (waterTint_2, 0.0, 1.0);
  waterTint_2 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = clamp ((xlv_COLOR.z * _AlphaScalar), 0.0, 1.0);
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (tmpvar_15.xyz, tmpvar_18, vec3(tmpvar_19));
  main_3 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = mix (main_3, _BlendedColor.xyz, _BlendedColor.www);
  main_3 = tmpvar_21;
  tmpvar_1.xy = (xlv_TEXCOORD6.xy - ((_LightmapDistort * tmpvar_14) * 4.0));
  tmpvar_1.zw = (xlv_TEXCOORD6.zw - ((_LightmapDistort * tmpvar_14) * 4.0));
  lowp float tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = max (float((texture2DProj (_ShadowMapTexture, tmpvar_1).x > 
    (tmpvar_1.z / tmpvar_1.w)
  )), _LightShadowData.x);
  tmpvar_22 = tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_24.xyz = (tmpvar_21 * tmpvar_22);
  tmpvar_24.w = ((texture2D (_AlphaTex, P_17).x * _Color.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_24;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
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
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _WaterTex_ST;
uniform mediump float _Speed;
uniform mediump float _Distance;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying mediump vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float t_2;
  highp vec2 flowDir_3;
  highp vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mediump vec2 tmpvar_8;
  tmpvar_8 = ((_glesColor.xy - 0.5) * _Distance);
  flowDir_3 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * _WaterTex_ST.xy);
  highp float tmpvar_10;
  tmpvar_10 = fract((_Time.x * _Speed));
  t_2 = tmpvar_10;
  mediump float tmpvar_11;
  tmpvar_11 = fract((t_2 + 0.5));
  lowp vec2 tmpvar_12;
  tmpvar_12.y = 0.0;
  tmpvar_12.x = tmpvar_1.z;
  tmpvar_5 = tmpvar_12;
  tmpvar_7 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = (tmpvar_9 - (flowDir_3 * t_2));
  xlv_TEXCOORD2 = (tmpvar_9 - (flowDir_3 * tmpvar_11));
  xlv_TEXCOORD3 = ((cos(
    (6.283 * t_2)
  ) * 0.5) + 0.5);
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _WaterTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _ColorLookup;
uniform mediump float _WavePower;
uniform mediump float _Distortion;
uniform mediump float _MaxIntensity;
uniform mediump float _AlphaScalar;
uniform mediump float _LightmapDistort;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _FadeAlpha;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec3 waterTint_2;
  lowp vec3 main_3;
  highp float cap_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_ColorLookup, xlv_TEXCOORD4);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_WaterTex, xlv_TEXCOORD1);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_WaterTex, xlv_TEXCOORD2);
  lowp float tmpvar_8;
  tmpvar_8 = clamp ((1.02 - xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_9;
  tmpvar_9 = (((
    mix (tmpvar_6.xy, tmpvar_7.xy, vec2(xlv_TEXCOORD3))
   * 2.0) - 1.0) * (tmpvar_8 * _MaxIntensity));
  lowp vec4 tmpvar_10;
  mediump vec2 P_11;
  P_11 = (tmpvar_9 - 0.5);
  tmpvar_10 = texture2D (_WaterTex, P_11);
  mediump float tmpvar_12;
  tmpvar_12 = (tmpvar_10.z * _WavePower);
  cap_4 = tmpvar_12;
  lowp float tmpvar_13;
  tmpvar_13 = clamp ((10.0 * xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_14;
  tmpvar_14 = ((tmpvar_9 * _Distortion) * tmpvar_13);
  lowp vec4 tmpvar_15;
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0 - tmpvar_14);
  tmpvar_15 = texture2D (_MainTex, P_16);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0 - tmpvar_14);
  waterTint_2 = (clamp ((
    (tmpvar_5.xyz * cap_4)
   * 2.0), 0.0, 1.0) * xlv_COLOR.w);
  waterTint_2 = (waterTint_2 * 4.0);
  highp vec3 tmpvar_18;
  tmpvar_18 = clamp (waterTint_2, 0.0, 1.0);
  waterTint_2 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = clamp ((xlv_COLOR.z * _AlphaScalar), 0.0, 1.0);
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (tmpvar_15.xyz, tmpvar_18, vec3(tmpvar_19));
  main_3 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = mix (main_3, _BlendedColor.xyz, _BlendedColor.www);
  main_3 = tmpvar_21;
  tmpvar_1.xy = (xlv_TEXCOORD6.xy - ((_LightmapDistort * tmpvar_14) * 4.0));
  tmpvar_1.zw = (xlv_TEXCOORD6.zw - ((_LightmapDistort * tmpvar_14) * 4.0));
  lowp float shadow_22;
  shadow_22 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, tmpvar_1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_23;
  tmpvar_23.xyz = (tmpvar_21 * shadow_22);
  tmpvar_23.w = ((texture2D (_AlphaTex, P_17).x * _Color.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_23;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" "VERTEXLIGHT_ON" }
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
uniform mediump vec4 _MainTex_ST;
uniform mediump vec4 _WaterTex_ST;
uniform mediump float _Speed;
uniform mediump float _Distance;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying mediump vec3 xlv_TEXCOORD5;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  mediump float t_2;
  highp vec2 flowDir_3;
  highp vec2 tmpvar_4;
  mediump vec2 tmpvar_5;
  mediump vec3 tmpvar_6;
  mediump vec4 tmpvar_7;
  tmpvar_4 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  mediump vec2 tmpvar_8;
  tmpvar_8 = ((_glesColor.xy - 0.5) * _Distance);
  flowDir_3 = tmpvar_8;
  highp vec2 tmpvar_9;
  tmpvar_9 = (tmpvar_4 * _WaterTex_ST.xy);
  highp float tmpvar_10;
  tmpvar_10 = fract((_Time.x * _Speed));
  t_2 = tmpvar_10;
  mediump float tmpvar_11;
  tmpvar_11 = fract((t_2 + 0.5));
  lowp vec2 tmpvar_12;
  tmpvar_12.y = 0.0;
  tmpvar_12.x = tmpvar_1.z;
  tmpvar_5 = tmpvar_12;
  tmpvar_7 = (unity_World2Shadow[0] * (_Object2World * _glesVertex));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_4;
  xlv_TEXCOORD1 = (tmpvar_9 - (flowDir_3 * t_2));
  xlv_TEXCOORD2 = (tmpvar_9 - (flowDir_3 * tmpvar_11));
  xlv_TEXCOORD3 = ((cos(
    (6.283 * t_2)
  ) * 0.5) + 0.5);
  xlv_TEXCOORD4 = tmpvar_5;
  xlv_COLOR = tmpvar_1;
  xlv_TEXCOORD5 = tmpvar_6;
  xlv_TEXCOORD6 = tmpvar_7;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform mediump vec4 _LightShadowData;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _MainTex;
uniform sampler2D _WaterTex;
uniform sampler2D _AlphaTex;
uniform sampler2D _ColorLookup;
uniform mediump float _WavePower;
uniform mediump float _Distortion;
uniform mediump float _MaxIntensity;
uniform mediump float _AlphaScalar;
uniform mediump float _LightmapDistort;
uniform lowp vec4 _Color;
uniform lowp vec4 _BlendedColor;
uniform lowp float _FadeAlpha;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
varying mediump float xlv_TEXCOORD3;
varying mediump vec2 xlv_TEXCOORD4;
varying lowp vec4 xlv_COLOR;
varying mediump vec4 xlv_TEXCOORD6;
void main ()
{
  mediump vec4 tmpvar_1;
  highp vec3 waterTint_2;
  lowp vec3 main_3;
  highp float cap_4;
  lowp vec4 tmpvar_5;
  tmpvar_5 = texture2D (_ColorLookup, xlv_TEXCOORD4);
  lowp vec4 tmpvar_6;
  tmpvar_6 = texture2D (_WaterTex, xlv_TEXCOORD1);
  lowp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_WaterTex, xlv_TEXCOORD2);
  lowp float tmpvar_8;
  tmpvar_8 = clamp ((1.02 - xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_9;
  tmpvar_9 = (((
    mix (tmpvar_6.xy, tmpvar_7.xy, vec2(xlv_TEXCOORD3))
   * 2.0) - 1.0) * (tmpvar_8 * _MaxIntensity));
  lowp vec4 tmpvar_10;
  mediump vec2 P_11;
  P_11 = (tmpvar_9 - 0.5);
  tmpvar_10 = texture2D (_WaterTex, P_11);
  mediump float tmpvar_12;
  tmpvar_12 = (tmpvar_10.z * _WavePower);
  cap_4 = tmpvar_12;
  lowp float tmpvar_13;
  tmpvar_13 = clamp ((10.0 * xlv_COLOR.z), 0.0, 1.0);
  mediump vec2 tmpvar_14;
  tmpvar_14 = ((tmpvar_9 * _Distortion) * tmpvar_13);
  lowp vec4 tmpvar_15;
  highp vec2 P_16;
  P_16 = (xlv_TEXCOORD0 - tmpvar_14);
  tmpvar_15 = texture2D (_MainTex, P_16);
  highp vec2 P_17;
  P_17 = (xlv_TEXCOORD0 - tmpvar_14);
  waterTint_2 = (clamp ((
    (tmpvar_5.xyz * cap_4)
   * 2.0), 0.0, 1.0) * xlv_COLOR.w);
  waterTint_2 = (waterTint_2 * 4.0);
  highp vec3 tmpvar_18;
  tmpvar_18 = clamp (waterTint_2, 0.0, 1.0);
  waterTint_2 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = clamp ((xlv_COLOR.z * _AlphaScalar), 0.0, 1.0);
  highp vec3 tmpvar_20;
  tmpvar_20 = mix (tmpvar_15.xyz, tmpvar_18, vec3(tmpvar_19));
  main_3 = tmpvar_20;
  lowp vec3 tmpvar_21;
  tmpvar_21 = mix (main_3, _BlendedColor.xyz, _BlendedColor.www);
  main_3 = tmpvar_21;
  tmpvar_1.xy = (xlv_TEXCOORD6.xy - ((_LightmapDistort * tmpvar_14) * 4.0));
  tmpvar_1.zw = (xlv_TEXCOORD6.zw - ((_LightmapDistort * tmpvar_14) * 4.0));
  lowp float shadow_22;
  shadow_22 = (_LightShadowData.x + (shadow2DEXT (_ShadowMapTexture, tmpvar_1.xyz) * (1.0 - _LightShadowData.x)));
  lowp vec4 tmpvar_23;
  tmpvar_23.xyz = (tmpvar_21 * shadow_22);
  tmpvar_23.w = ((texture2D (_AlphaTex, P_17).x * _Color.w) * _FadeAlpha);
  gl_FragData[0] = tmpvar_23;
}


#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "LIGHTMAP_OFF" "DIRLIGHTMAP_OFF" "DYNAMICLIGHTMAP_OFF" }
""
}
}
 }
 Pass {
  Name "SHADOWCASTER"
  Tags { "LIGHTMODE"="SHADOWCASTER" "SHADOWSUPPORT"="true" "RenderType"="Opaque" }
  Cull Off
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
}