//////////////////////////////////////////
///////////////////////////////////////////
Shader "Hidden/Internal-DeferredReflections" {
Properties {
 _SrcBlend ("", Float) = 1
 _DstBlend ("", Float) = 1
}
SubShader { 
 Pass {
  ZWrite Off
  Blend [_SrcBlend] [_DstBlend]
GLSLPROGRAM
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp float _LightAsQuad;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec3 tmpvar_2;
  tmpvar_1 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (tmpvar_1 * 0.5);
  highp vec2 tmpvar_5;
  tmpvar_5.x = tmpvar_4.x;
  tmpvar_5.y = (tmpvar_4.y * _ProjectionParams.x);
  o_3.xy = (tmpvar_5 + tmpvar_4.w);
  o_3.zw = tmpvar_1.zw;
  tmpvar_2 = ((glstate_matrix_modelview0 * _glesVertex).xyz * vec3(-1.0, -1.0, 1.0));
  highp vec3 tmpvar_6;
  tmpvar_6 = mix (tmpvar_2, _glesNormal, vec3(_LightAsQuad));
  tmpvar_2 = tmpvar_6;
  gl_Position = tmpvar_1;
  xlv_TEXCOORD0 = o_3;
  xlv_TEXCOORD1 = tmpvar_6;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shader_texture_lod : enable
lowp vec4 impl_low_textureCubeLodEXT(lowp samplerCube sampler, highp vec3 coord, mediump float lod)
{
#if defined(GL_EXT_shader_texture_lod)
	return textureCubeLodEXT(sampler, coord, lod);
#else
	return textureCube(sampler, coord, lod);
#endif
}

uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform lowp samplerCube unity_SpecCube0;
uniform highp vec4 unity_SpecCube0_BoxMax;
uniform highp vec4 unity_SpecCube0_BoxMin;
uniform mediump vec4 unity_SpecCube0_HDR;
uniform highp vec4 unity_SpecCube1_ProbePosition;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump float tmpvar_1;
  mediump vec3 worldNormalRefl_2;
  mediump vec4 gbuffer2_3;
  mediump vec4 gbuffer1_4;
  mediump vec4 gbuffer0_5;
  highp vec2 tmpvar_6;
  tmpvar_6 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_6).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_8;
  tmpvar_8 = (_CameraToWorld * tmpvar_7).xyz;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_CameraGBufferTexture0, tmpvar_6);
  gbuffer0_5 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_CameraGBufferTexture1, tmpvar_6);
  gbuffer1_4 = tmpvar_10;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_CameraGBufferTexture2, tmpvar_6);
  gbuffer2_3 = tmpvar_11;
  mediump vec3 tmpvar_12;
  tmpvar_12 = normalize(((gbuffer2_3.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize((tmpvar_8 - _WorldSpaceCameraPos));
  highp vec3 tmpvar_14;
  tmpvar_14 = (tmpvar_13 - (2.0 * (
    dot (tmpvar_12, tmpvar_13)
   * tmpvar_12)));
  worldNormalRefl_2 = tmpvar_14;
  tmpvar_1 = (1.0 - gbuffer1_4.w);
  mediump vec4 tmpvar_15;
  tmpvar_15.xyz = worldNormalRefl_2;
  tmpvar_15.w = ((tmpvar_1 * (1.7 - 
    (0.7 * tmpvar_1)
  )) * 6.0);
  lowp vec4 tmpvar_16;
  tmpvar_16 = impl_low_textureCubeLodEXT (unity_SpecCube0, worldNormalRefl_2, tmpvar_15.w);
  mediump vec4 tmpvar_17;
  tmpvar_17 = tmpvar_16;
  mediump vec3 viewDir_18;
  viewDir_18 = -(tmpvar_13);
  mediump float tmpvar_19;
  tmpvar_19 = (1.0 - gbuffer1_4.w);
  mediump float x_20;
  x_20 = (1.0 - max (0.0, dot (tmpvar_12, viewDir_18)));
  mediump vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = (((1.0 - 
    ((tmpvar_19 * tmpvar_19) * (tmpvar_19 * 0.28))
  ) * (
    ((unity_SpecCube0_HDR.x * tmpvar_17.w) * tmpvar_17.xyz)
   * gbuffer0_5.w)) * mix (gbuffer1_4.xyz, vec3(clamp (
    (gbuffer1_4.w + (1.0 - (1.0 - max (
      max (gbuffer1_4.x, gbuffer1_4.y)
    , gbuffer1_4.z))))
  , 0.0, 1.0)), vec3((
    (x_20 * x_20)
   * 
    (x_20 * x_20)
  ))));
  mediump vec3 p_22;
  p_22 = tmpvar_8;
  mediump vec3 aabbMin_23;
  aabbMin_23 = unity_SpecCube0_BoxMin.xyz;
  mediump vec3 aabbMax_24;
  aabbMax_24 = unity_SpecCube0_BoxMax.xyz;
  mediump vec3 tmpvar_25;
  tmpvar_25 = max (max ((p_22 - aabbMax_24), (aabbMin_23 - p_22)), vec3(0.0, 0.0, 0.0));
  mediump float tmpvar_26;
  tmpvar_26 = sqrt(dot (tmpvar_25, tmpvar_25));
  mediump float tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = clamp ((1.0 - (tmpvar_26 / unity_SpecCube1_ProbePosition.w)), 0.0, 1.0);
  tmpvar_27 = tmpvar_28;
  mediump vec4 tmpvar_29;
  tmpvar_29.xyz = tmpvar_21.xyz;
  tmpvar_29.w = tmpvar_27;
  gl_FragData[0] = tmpvar_29;
}


#endif

ENDGLSL
 }
 Pass {
  ZTest Always
  ZWrite Off
  Blend [_SrcBlend] [_DstBlend]
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (tmpvar_1 * 0.5);
  highp vec2 tmpvar_4;
  tmpvar_4.x = tmpvar_3.x;
  tmpvar_4.y = (tmpvar_3.y * _ProjectionParams.x);
  o_2.xy = (tmpvar_4 + tmpvar_3.w);
  o_2.zw = tmpvar_1.zw;
  xlv_TEXCOORD0 = o_2.xy;
  gl_Position = tmpvar_1;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _CameraReflectionsTexture;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_CameraReflectionsTexture, xlv_TEXCOORD0);
  c_1 = tmpvar_2;
  mediump vec4 tmpvar_3;
  tmpvar_3.w = 0.0;
  tmpvar_3.xyz = exp2(-(c_1.xyz));
  gl_FragData[0] = tmpvar_3;
}


#endif
"
}
SubProgram "gles " {
Keywords { "UNITY_HDR_ON" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = (glstate_matrix_mvp * _glesVertex);
  highp vec4 o_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = (tmpvar_1 * 0.5);
  highp vec2 tmpvar_4;
  tmpvar_4.x = tmpvar_3.x;
  tmpvar_4.y = (tmpvar_3.y * _ProjectionParams.x);
  o_2.xy = (tmpvar_4 + tmpvar_3.w);
  o_2.zw = tmpvar_1.zw;
  xlv_TEXCOORD0 = o_2.xy;
  gl_Position = tmpvar_1;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _CameraReflectionsTexture;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec4 c_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_CameraReflectionsTexture, xlv_TEXCOORD0);
  c_1 = tmpvar_2;
  mediump vec4 tmpvar_3;
  tmpvar_3.w = 0.0;
  tmpvar_3.xyz = c_1.xyz;
  gl_FragData[0] = tmpvar_3;
}


#endif
"
}
}
Program "fp" {
SubProgram "gles " {
""
}
SubProgram "gles " {
Keywords { "UNITY_HDR_ON" }
""
}
}
 }
}
Fallback Off
}