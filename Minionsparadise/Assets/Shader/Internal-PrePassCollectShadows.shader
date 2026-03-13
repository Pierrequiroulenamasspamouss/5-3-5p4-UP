//////////////////////////////////////////
///////////////////////////////////////////
Shader "Hidden/Internal-PrePassCollectShadows" {
Properties {
 _ShadowMapTexture ("", any) = "" { }
}
SubShader { 
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_NONATIVE" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform highp sampler2D _ShadowMapTexture;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 res_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec3 tmpvar_4;
  tmpvar_4 = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = tmpvar_4;
  highp vec4 tmpvar_6;
  tmpvar_6 = (_CameraToWorld * tmpvar_5);
  bvec4 tmpvar_7;
  tmpvar_7 = greaterThanEqual (tmpvar_4.zzzz, _LightSplitsNear);
  bvec4 tmpvar_8;
  tmpvar_8 = lessThan (tmpvar_4.zzzz, _LightSplitsFar);
  lowp vec4 tmpvar_9;
  tmpvar_9 = (vec4(tmpvar_7) * vec4(tmpvar_8));
  highp vec4 tmpvar_10;
  tmpvar_10.w = 1.0;
  tmpvar_10.xyz = (((
    ((unity_World2Shadow[0] * tmpvar_6).xyz * tmpvar_9.x)
   + 
    ((unity_World2Shadow[1] * tmpvar_6).xyz * tmpvar_9.y)
  ) + (
    (unity_World2Shadow[2] * tmpvar_6)
  .xyz * tmpvar_9.z)) + ((unity_World2Shadow[3] * tmpvar_6).xyz * tmpvar_9.w));
  highp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_ShadowMapTexture, tmpvar_10.xy);
  mediump float tmpvar_12;
  if ((tmpvar_11.x < tmpvar_10.z)) {
    tmpvar_12 = 0.0;
  } else {
    tmpvar_12 = 1.0;
  };
  highp float tmpvar_13;
  tmpvar_13 = clamp (((tmpvar_4.z * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  shadow_2 = (mix (_LightShadowData.x, 1.0, tmpvar_12) + tmpvar_13);
  mediump vec4 tmpvar_14;
  tmpvar_14 = vec4(shadow_2);
  res_1 = tmpvar_14;
  gl_FragData[0] = res_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform lowp sampler2DShadow _ShadowMapTexture;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 res_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec3 tmpvar_4;
  tmpvar_4 = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = tmpvar_4;
  highp vec4 tmpvar_6;
  tmpvar_6 = (_CameraToWorld * tmpvar_5);
  bvec4 tmpvar_7;
  tmpvar_7 = greaterThanEqual (tmpvar_4.zzzz, _LightSplitsNear);
  bvec4 tmpvar_8;
  tmpvar_8 = lessThan (tmpvar_4.zzzz, _LightSplitsFar);
  lowp vec4 tmpvar_9;
  tmpvar_9 = (vec4(tmpvar_7) * vec4(tmpvar_8));
  highp vec4 tmpvar_10;
  tmpvar_10.w = 1.0;
  tmpvar_10.xyz = (((
    ((unity_World2Shadow[0] * tmpvar_6).xyz * tmpvar_9.x)
   + 
    ((unity_World2Shadow[1] * tmpvar_6).xyz * tmpvar_9.y)
  ) + (
    (unity_World2Shadow[2] * tmpvar_6)
  .xyz * tmpvar_9.z)) + ((unity_World2Shadow[3] * tmpvar_6).xyz * tmpvar_9.w));
  lowp float tmpvar_11;
  tmpvar_11 = shadow2DEXT (_ShadowMapTexture, tmpvar_10.xyz);
  mediump float tmpvar_12;
  tmpvar_12 = tmpvar_11;
  highp float tmpvar_13;
  tmpvar_13 = clamp (((tmpvar_4.z * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  shadow_2 = (mix (_LightShadowData.x, 1.0, tmpvar_12) + tmpvar_13);
  mediump vec4 tmpvar_14;
  tmpvar_14 = vec4(shadow_2);
  res_1 = tmpvar_14;
  gl_FragData[0] = res_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_NATIVE" }
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 unity_CameraInvProjection;
uniform 	mat4x4 glstate_matrix_mvp;
in highp vec4 in_POSITION0;
in highp vec2 in_TEXCOORD0;
in highp vec3 in_NORMAL0;
out highp vec2 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
vec4 u_xlat0;
vec4 u_xlat1;
void main()
{
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD1.xyz = in_NORMAL0.xyz;
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    u_xlat1.x = u_xlat0.y * _ProjectionParams.x;
    u_xlat1.xyz = u_xlat1.xxx * unity_CameraInvProjection[1].xyz;
    u_xlat1.xyz = unity_CameraInvProjection[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
    gl_Position = u_xlat0;
    u_xlat0.xyz = u_xlat1.xyz + (-unity_CameraInvProjection[2].xyz);
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[2].xyz;
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.xyz = u_xlat0.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.w = (-u_xlat0.z);
    vs_TEXCOORD2.xyz = u_xlat0.xyw;
    u_xlat1.w = (-u_xlat1.z);
    vs_TEXCOORD3.xyz = u_xlat1.xyw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4x4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	mat4x4 _CameraToWorld;
uniform highp sampler2D _CameraDepthTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
vec4 u_xlat1;
lowp vec4 u_xlat10_1;
bvec4 u_xlatb1;
vec4 u_xlat2;
bvec4 u_xlatb2;
vec3 u_xlat3;
mediump float u_xlat16_4;
vec3 u_xlat5;
lowp float u_xlat10_5;
float u_xlat10;
void main()
{
    u_xlat0.x = texture(_CameraDepthTexture, vs_TEXCOORD0.xy).x;
    u_xlat5.x = _ZBufferParams.x * u_xlat0.x + _ZBufferParams.y;
    u_xlat5.x = float(1.0) / u_xlat5.x;
    u_xlat10 = (-u_xlat5.x) + u_xlat0.x;
    u_xlat5.x = unity_OrthoParams.w * u_xlat10 + u_xlat5.x;
    u_xlat1.xyz = (-vs_TEXCOORD2.xyz) + vs_TEXCOORD3.xyz;
    u_xlat0.xzw = u_xlat0.xxx * u_xlat1.xyz + vs_TEXCOORD2.xyz;
    u_xlat0.xzw = (-vs_TEXCOORD1.xyz) * u_xlat5.xxx + u_xlat0.xzw;
    u_xlat1.xyz = u_xlat5.xxx * vs_TEXCOORD1.xyz;
    u_xlat0.xyz = unity_OrthoParams.www * u_xlat0.xzw + u_xlat1.xyz;
    u_xlatb1 = greaterThanEqual(u_xlat0.zzzz, _LightSplitsNear);
    u_xlat1 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(u_xlatb1));
    u_xlatb2 = lessThan(u_xlat0.zzzz, _LightSplitsFar);
    u_xlat2 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(u_xlatb2));
    u_xlat10_1 = u_xlat1 * u_xlat2;
    u_xlat2 = u_xlat0.yyyy * _CameraToWorld[1];
    u_xlat2 = _CameraToWorld[0] * u_xlat0.xxxx + u_xlat2;
    u_xlat2 = _CameraToWorld[2] * u_xlat0.zzzz + u_xlat2;
    u_xlat0.x = u_xlat0.z * _LightShadowData.z + _LightShadowData.w;
#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    u_xlat2 = u_xlat2 + _CameraToWorld[3];
    u_xlat5.xyz = u_xlat2.yyy * unity_World2Shadow[1][1].xyz;
    u_xlat5.xyz = unity_World2Shadow[1][0].xyz * u_xlat2.xxx + u_xlat5.xyz;
    u_xlat5.xyz = unity_World2Shadow[1][2].xyz * u_xlat2.zzz + u_xlat5.xyz;
    u_xlat5.xyz = unity_World2Shadow[1][3].xyz * u_xlat2.www + u_xlat5.xyz;
    u_xlat5.xyz = u_xlat10_1.yyy * u_xlat5.xyz;
    u_xlat3.xyz = u_xlat2.yyy * unity_World2Shadow[0][1].xyz;
    u_xlat3.xyz = unity_World2Shadow[0][0].xyz * u_xlat2.xxx + u_xlat3.xyz;
    u_xlat3.xyz = unity_World2Shadow[0][2].xyz * u_xlat2.zzz + u_xlat3.xyz;
    u_xlat3.xyz = unity_World2Shadow[0][3].xyz * u_xlat2.www + u_xlat3.xyz;
    u_xlat5.xyz = u_xlat3.xyz * u_xlat10_1.xxx + u_xlat5.xyz;
    u_xlat3.xyz = u_xlat2.yyy * unity_World2Shadow[2][1].xyz;
    u_xlat3.xyz = unity_World2Shadow[2][0].xyz * u_xlat2.xxx + u_xlat3.xyz;
    u_xlat3.xyz = unity_World2Shadow[2][2].xyz * u_xlat2.zzz + u_xlat3.xyz;
    u_xlat3.xyz = unity_World2Shadow[2][3].xyz * u_xlat2.www + u_xlat3.xyz;
    u_xlat5.xyz = u_xlat3.xyz * u_xlat10_1.zzz + u_xlat5.xyz;
    u_xlat3.xyz = u_xlat2.yyy * unity_World2Shadow[3][1].xyz;
    u_xlat3.xyz = unity_World2Shadow[3][0].xyz * u_xlat2.xxx + u_xlat3.xyz;
    u_xlat2.xyz = unity_World2Shadow[3][2].xyz * u_xlat2.zzz + u_xlat3.xyz;
    u_xlat2.xyz = unity_World2Shadow[3][3].xyz * u_xlat2.www + u_xlat2.xyz;
    u_xlat5.xyz = u_xlat2.xyz * u_xlat10_1.www + u_xlat5.xyz;
    vec3 txVec3 = vec3(u_xlat5.xy,u_xlat5.z);
    u_xlat10_5 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec3, 0.0);
    u_xlat16_4 = (-_LightShadowData.x) + 1.0;
    u_xlat16_4 = u_xlat10_5 * u_xlat16_4 + _LightShadowData.x;
    u_xlat0 = u_xlat0.xxxx + vec4(u_xlat16_4);
    SV_Target0 = u_xlat0;
    return;
}
#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NONATIVE" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform highp sampler2D _ShadowMapTexture;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 res_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5 = (_CameraToWorld * tmpvar_4);
  lowp vec4 weights_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_11;
  tmpvar_11.x = dot (tmpvar_7, tmpvar_7);
  tmpvar_11.y = dot (tmpvar_8, tmpvar_8);
  tmpvar_11.z = dot (tmpvar_9, tmpvar_9);
  tmpvar_11.w = dot (tmpvar_10, tmpvar_10);
  bvec4 tmpvar_12;
  tmpvar_12 = lessThan (tmpvar_11, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_13;
  tmpvar_13 = vec4(tmpvar_12);
  weights_6.x = tmpvar_13.x;
  weights_6.yzw = clamp ((tmpvar_13.yzw - tmpvar_13.xyz), 0.0, 1.0);
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = (((
    ((unity_World2Shadow[0] * tmpvar_5).xyz * tmpvar_13.x)
   + 
    ((unity_World2Shadow[1] * tmpvar_5).xyz * weights_6.y)
  ) + (
    (unity_World2Shadow[2] * tmpvar_5)
  .xyz * weights_6.z)) + ((unity_World2Shadow[3] * tmpvar_5).xyz * weights_6.w));
  highp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_14.xy);
  mediump float tmpvar_16;
  if ((tmpvar_15.x < tmpvar_14.z)) {
    tmpvar_16 = 0.0;
  } else {
    tmpvar_16 = 1.0;
  };
  highp float tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_18 = (tmpvar_5.xyz - unity_ShadowFadeCenterAndType.xyz);
  mediump float tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = clamp (((
    sqrt(dot (tmpvar_18, tmpvar_18))
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  tmpvar_19 = tmpvar_20;
  tmpvar_17 = tmpvar_19;
  shadow_2 = (mix (_LightShadowData.x, 1.0, tmpvar_16) + tmpvar_17);
  mediump vec4 tmpvar_21;
  tmpvar_21 = vec4(shadow_2);
  res_1 = tmpvar_21;
  gl_FragData[0] = res_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform lowp sampler2DShadow _ShadowMapTexture;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 res_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5 = (_CameraToWorld * tmpvar_4);
  lowp vec4 weights_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_11;
  tmpvar_11.x = dot (tmpvar_7, tmpvar_7);
  tmpvar_11.y = dot (tmpvar_8, tmpvar_8);
  tmpvar_11.z = dot (tmpvar_9, tmpvar_9);
  tmpvar_11.w = dot (tmpvar_10, tmpvar_10);
  bvec4 tmpvar_12;
  tmpvar_12 = lessThan (tmpvar_11, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_13;
  tmpvar_13 = vec4(tmpvar_12);
  weights_6.x = tmpvar_13.x;
  weights_6.yzw = clamp ((tmpvar_13.yzw - tmpvar_13.xyz), 0.0, 1.0);
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = (((
    ((unity_World2Shadow[0] * tmpvar_5).xyz * tmpvar_13.x)
   + 
    ((unity_World2Shadow[1] * tmpvar_5).xyz * weights_6.y)
  ) + (
    (unity_World2Shadow[2] * tmpvar_5)
  .xyz * weights_6.z)) + ((unity_World2Shadow[3] * tmpvar_5).xyz * weights_6.w));
  lowp float tmpvar_15;
  tmpvar_15 = shadow2DEXT (_ShadowMapTexture, tmpvar_14.xyz);
  mediump float tmpvar_16;
  tmpvar_16 = tmpvar_15;
  highp float tmpvar_17;
  highp vec3 tmpvar_18;
  tmpvar_18 = (tmpvar_5.xyz - unity_ShadowFadeCenterAndType.xyz);
  mediump float tmpvar_19;
  highp float tmpvar_20;
  tmpvar_20 = clamp (((
    sqrt(dot (tmpvar_18, tmpvar_18))
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  tmpvar_19 = tmpvar_20;
  tmpvar_17 = tmpvar_19;
  shadow_2 = (mix (_LightShadowData.x, 1.0, tmpvar_16) + tmpvar_17);
  mediump vec4 tmpvar_21;
  tmpvar_21 = vec4(shadow_2);
  res_1 = tmpvar_21;
  gl_FragData[0] = res_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 unity_CameraInvProjection;
uniform 	mat4x4 glstate_matrix_mvp;
in highp vec4 in_POSITION0;
in highp vec2 in_TEXCOORD0;
in highp vec3 in_NORMAL0;
out highp vec2 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
vec4 u_xlat0;
vec4 u_xlat1;
void main()
{
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD1.xyz = in_NORMAL0.xyz;
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    u_xlat1.x = u_xlat0.y * _ProjectionParams.x;
    u_xlat1.xyz = u_xlat1.xxx * unity_CameraInvProjection[1].xyz;
    u_xlat1.xyz = unity_CameraInvProjection[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
    gl_Position = u_xlat0;
    u_xlat0.xyz = u_xlat1.xyz + (-unity_CameraInvProjection[2].xyz);
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[2].xyz;
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.xyz = u_xlat0.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.w = (-u_xlat0.z);
    vs_TEXCOORD2.xyz = u_xlat0.xyw;
    u_xlat1.w = (-u_xlat1.z);
    vs_TEXCOORD3.xyz = u_xlat1.xyw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	mat4x4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4x4 _CameraToWorld;
uniform highp sampler2D _CameraDepthTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
vec4 u_xlat1;
bvec4 u_xlatb1;
vec3 u_xlat2;
lowp vec3 u_xlat10_3;
mediump float u_xlat16_4;
vec3 u_xlat5;
lowp float u_xlat10_5;
vec3 u_xlat6;
float u_xlat10;
void main()
{
    u_xlat0.x = texture(_CameraDepthTexture, vs_TEXCOORD0.xy).x;
    u_xlat5.x = _ZBufferParams.x * u_xlat0.x + _ZBufferParams.y;
    u_xlat5.x = float(1.0) / u_xlat5.x;
    u_xlat10 = (-u_xlat5.x) + u_xlat0.x;
    u_xlat5.x = unity_OrthoParams.w * u_xlat10 + u_xlat5.x;
    u_xlat1.xyz = (-vs_TEXCOORD2.xyz) + vs_TEXCOORD3.xyz;
    u_xlat0.xzw = u_xlat0.xxx * u_xlat1.xyz + vs_TEXCOORD2.xyz;
    u_xlat0.xzw = (-vs_TEXCOORD1.xyz) * u_xlat5.xxx + u_xlat0.xzw;
    u_xlat1.xyz = u_xlat5.xxx * vs_TEXCOORD1.xyz;
    u_xlat0.xyz = unity_OrthoParams.www * u_xlat0.xzw + u_xlat1.xyz;
    u_xlat1 = u_xlat0.yyyy * _CameraToWorld[1];
    u_xlat1 = _CameraToWorld[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat0 = _CameraToWorld[2] * u_xlat0.zzzz + u_xlat1;
    u_xlat0 = u_xlat0 + _CameraToWorld[3];
    u_xlat1.xyz = u_xlat0.xyz + (-unity_ShadowSplitSpheres[0].xyz);
    u_xlat1.x = dot(u_xlat1.xyz, u_xlat1.xyz);
    u_xlat2.xyz = u_xlat0.xyz + (-unity_ShadowSplitSpheres[1].xyz);
    u_xlat1.y = dot(u_xlat2.xyz, u_xlat2.xyz);
    u_xlat2.xyz = u_xlat0.xyz + (-unity_ShadowSplitSpheres[2].xyz);
    u_xlat1.z = dot(u_xlat2.xyz, u_xlat2.xyz);
    u_xlat2.xyz = u_xlat0.xyz + (-unity_ShadowSplitSpheres[3].xyz);
    u_xlat1.w = dot(u_xlat2.xyz, u_xlat2.xyz);
    u_xlatb1 = lessThan(u_xlat1, unity_ShadowSplitSqRadii);
    u_xlat10_3.x = (u_xlatb1.x) ? float(-1.0) : float(-0.0);
    u_xlat10_3.y = (u_xlatb1.y) ? float(-1.0) : float(-0.0);
    u_xlat10_3.z = (u_xlatb1.z) ? float(-1.0) : float(-0.0);
    u_xlat1 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(u_xlatb1));
    u_xlat10_3.xyz = vec3(u_xlat10_3.x + u_xlat1.y, u_xlat10_3.y + u_xlat1.z, u_xlat10_3.z + u_xlat1.w);
    u_xlat10_3.xyz = max(u_xlat10_3.xyz, vec3(0.0, 0.0, 0.0));
    u_xlat6.xyz = u_xlat0.yyy * unity_World2Shadow[1][1].xyz;
    u_xlat6.xyz = unity_World2Shadow[1][0].xyz * u_xlat0.xxx + u_xlat6.xyz;
    u_xlat6.xyz = unity_World2Shadow[1][2].xyz * u_xlat0.zzz + u_xlat6.xyz;
    u_xlat6.xyz = unity_World2Shadow[1][3].xyz * u_xlat0.www + u_xlat6.xyz;
    u_xlat6.xyz = u_xlat10_3.xxx * u_xlat6.xyz;
    u_xlat2.xyz = u_xlat0.yyy * unity_World2Shadow[0][1].xyz;
    u_xlat2.xyz = unity_World2Shadow[0][0].xyz * u_xlat0.xxx + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[0][2].xyz * u_xlat0.zzz + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[0][3].xyz * u_xlat0.www + u_xlat2.xyz;
    u_xlat1.xyz = u_xlat2.xyz * u_xlat1.xxx + u_xlat6.xyz;
    u_xlat2.xyz = u_xlat0.yyy * unity_World2Shadow[2][1].xyz;
    u_xlat2.xyz = unity_World2Shadow[2][0].xyz * u_xlat0.xxx + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[2][2].xyz * u_xlat0.zzz + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[2][3].xyz * u_xlat0.www + u_xlat2.xyz;
    u_xlat1.xyz = u_xlat2.xyz * u_xlat10_3.yyy + u_xlat1.xyz;
    u_xlat2.xyz = u_xlat0.yyy * unity_World2Shadow[3][1].xyz;
    u_xlat2.xyz = unity_World2Shadow[3][0].xyz * u_xlat0.xxx + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[3][2].xyz * u_xlat0.zzz + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[3][3].xyz * u_xlat0.www + u_xlat2.xyz;
    u_xlat0.xyz = u_xlat0.xyz + (-unity_ShadowFadeCenterAndType.xyz);
    u_xlat0.x = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat0.x = sqrt(u_xlat0.x);
    u_xlat0.x = u_xlat0.x * _LightShadowData.z + _LightShadowData.w;
#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    u_xlat5.xyz = u_xlat2.xyz * u_xlat10_3.zzz + u_xlat1.xyz;
    vec3 txVec2 = vec3(u_xlat5.xy,u_xlat5.z);
    u_xlat10_5 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec2, 0.0);
    u_xlat16_4 = (-_LightShadowData.x) + 1.0;
    u_xlat16_4 = u_xlat10_5 * u_xlat16_4 + _LightShadowData.x;
    u_xlat0 = u_xlat0.xxxx + vec4(u_xlat16_4);
    SV_Target0 = u_xlat0;
    return;
}
#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SINGLE_CASCADE" "SHADOWS_NONATIVE" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform highp sampler2D _ShadowMapTexture;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 res_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec3 tmpvar_4;
  tmpvar_4 = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = tmpvar_4;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = (unity_World2Shadow[0] * (_CameraToWorld * tmpvar_5)).xyz;
  highp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_ShadowMapTexture, tmpvar_6.xy);
  mediump float tmpvar_8;
  if ((tmpvar_7.x < tmpvar_6.z)) {
    tmpvar_8 = 0.0;
  } else {
    tmpvar_8 = 1.0;
  };
  highp float tmpvar_9;
  tmpvar_9 = clamp (((tmpvar_4.z * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  shadow_2 = (mix (_LightShadowData.x, 1.0, tmpvar_8) + tmpvar_9);
  mediump vec4 tmpvar_10;
  tmpvar_10 = vec4(shadow_2);
  res_1 = tmpvar_10;
  gl_FragData[0] = res_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform lowp sampler2DShadow _ShadowMapTexture;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 res_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec3 tmpvar_4;
  tmpvar_4 = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = tmpvar_4;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = (unity_World2Shadow[0] * (_CameraToWorld * tmpvar_5)).xyz;
  lowp float tmpvar_7;
  tmpvar_7 = shadow2DEXT (_ShadowMapTexture, tmpvar_6.xyz);
  mediump float tmpvar_8;
  tmpvar_8 = tmpvar_7;
  highp float tmpvar_9;
  tmpvar_9 = clamp (((tmpvar_4.z * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  shadow_2 = (mix (_LightShadowData.x, 1.0, tmpvar_8) + tmpvar_9);
  mediump vec4 tmpvar_10;
  tmpvar_10 = vec4(shadow_2);
  res_1 = tmpvar_10;
  gl_FragData[0] = res_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 unity_CameraInvProjection;
uniform 	mat4x4 glstate_matrix_mvp;
in highp vec4 in_POSITION0;
in highp vec2 in_TEXCOORD0;
in highp vec3 in_NORMAL0;
out highp vec2 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
vec4 u_xlat0;
vec4 u_xlat1;
void main()
{
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD1.xyz = in_NORMAL0.xyz;
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    u_xlat1.x = u_xlat0.y * _ProjectionParams.x;
    u_xlat1.xyz = u_xlat1.xxx * unity_CameraInvProjection[1].xyz;
    u_xlat1.xyz = unity_CameraInvProjection[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
    gl_Position = u_xlat0;
    u_xlat0.xyz = u_xlat1.xyz + (-unity_CameraInvProjection[2].xyz);
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[2].xyz;
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.xyz = u_xlat0.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.w = (-u_xlat0.z);
    vs_TEXCOORD2.xyz = u_xlat0.xyw;
    u_xlat1.w = (-u_xlat1.z);
    vs_TEXCOORD3.xyz = u_xlat1.xyw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	mat4x4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	mat4x4 _CameraToWorld;
uniform highp sampler2D _CameraDepthTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
vec4 u_xlat1;
mediump float u_xlat16_2;
vec3 u_xlat3;
lowp float u_xlat10_3;
float u_xlat6;
void main()
{
    u_xlat0.x = texture(_CameraDepthTexture, vs_TEXCOORD0.xy).x;
    u_xlat3.x = _ZBufferParams.x * u_xlat0.x + _ZBufferParams.y;
    u_xlat3.x = float(1.0) / u_xlat3.x;
    u_xlat6 = (-u_xlat3.x) + u_xlat0.x;
    u_xlat3.x = unity_OrthoParams.w * u_xlat6 + u_xlat3.x;
    u_xlat1.xyz = (-vs_TEXCOORD2.xyz) + vs_TEXCOORD3.xyz;
    u_xlat0.xzw = u_xlat0.xxx * u_xlat1.xyz + vs_TEXCOORD2.xyz;
    u_xlat0.xzw = (-vs_TEXCOORD1.xyz) * u_xlat3.xxx + u_xlat0.xzw;
    u_xlat1.xyz = u_xlat3.xxx * vs_TEXCOORD1.xyz;
    u_xlat0.xyz = unity_OrthoParams.www * u_xlat0.xzw + u_xlat1.xyz;
    u_xlat1 = u_xlat0.yyyy * _CameraToWorld[1];
    u_xlat1 = _CameraToWorld[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = _CameraToWorld[2] * u_xlat0.zzzz + u_xlat1;
    u_xlat0.x = u_xlat0.z * _LightShadowData.z + _LightShadowData.w;
#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    u_xlat1 = u_xlat1 + _CameraToWorld[3];
    u_xlat3.xyz = u_xlat1.yyy * unity_World2Shadow[0][1].xyz;
    u_xlat3.xyz = unity_World2Shadow[0][0].xyz * u_xlat1.xxx + u_xlat3.xyz;
    u_xlat3.xyz = unity_World2Shadow[0][2].xyz * u_xlat1.zzz + u_xlat3.xyz;
    u_xlat3.xyz = unity_World2Shadow[0][3].xyz * u_xlat1.www + u_xlat3.xyz;
    vec3 txVec0 = vec3(u_xlat3.xy,u_xlat3.z);
    u_xlat10_3 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec0, 0.0);
    u_xlat16_2 = (-_LightShadowData.x) + 1.0;
    u_xlat16_2 = u_xlat10_3 * u_xlat16_2 + _LightShadowData.x;
    u_xlat0 = u_xlat0.xxxx + vec4(u_xlat16_2);
    SV_Target0 = u_xlat0;
    return;
}
#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_SINGLE_CASCADE" "SHADOWS_NONATIVE" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform highp sampler2D _ShadowMapTexture;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 res_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5 = (_CameraToWorld * tmpvar_4);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = (unity_World2Shadow[0] * tmpvar_5).xyz;
  highp vec4 tmpvar_7;
  tmpvar_7 = texture2D (_ShadowMapTexture, tmpvar_6.xy);
  mediump float tmpvar_8;
  if ((tmpvar_7.x < tmpvar_6.z)) {
    tmpvar_8 = 0.0;
  } else {
    tmpvar_8 = 1.0;
  };
  highp float tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_5.xyz - unity_ShadowFadeCenterAndType.xyz);
  mediump float tmpvar_11;
  highp float tmpvar_12;
  tmpvar_12 = clamp (((
    sqrt(dot (tmpvar_10, tmpvar_10))
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  tmpvar_11 = tmpvar_12;
  tmpvar_9 = tmpvar_11;
  shadow_2 = (mix (_LightShadowData.x, 1.0, tmpvar_8) + tmpvar_9);
  mediump vec4 tmpvar_13;
  tmpvar_13 = vec4(shadow_2);
  res_1 = tmpvar_13;
  gl_FragData[0] = res_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform lowp sampler2DShadow _ShadowMapTexture;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 res_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5 = (_CameraToWorld * tmpvar_4);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = (unity_World2Shadow[0] * tmpvar_5).xyz;
  lowp float tmpvar_7;
  tmpvar_7 = shadow2DEXT (_ShadowMapTexture, tmpvar_6.xyz);
  mediump float tmpvar_8;
  tmpvar_8 = tmpvar_7;
  highp float tmpvar_9;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_5.xyz - unity_ShadowFadeCenterAndType.xyz);
  mediump float tmpvar_11;
  highp float tmpvar_12;
  tmpvar_12 = clamp (((
    sqrt(dot (tmpvar_10, tmpvar_10))
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  tmpvar_11 = tmpvar_12;
  tmpvar_9 = tmpvar_11;
  shadow_2 = (mix (_LightShadowData.x, 1.0, tmpvar_8) + tmpvar_9);
  mediump vec4 tmpvar_13;
  tmpvar_13 = vec4(shadow_2);
  res_1 = tmpvar_13;
  gl_FragData[0] = res_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 unity_CameraInvProjection;
uniform 	mat4x4 glstate_matrix_mvp;
in highp vec4 in_POSITION0;
in highp vec2 in_TEXCOORD0;
in highp vec3 in_NORMAL0;
out highp vec2 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
vec4 u_xlat0;
vec4 u_xlat1;
void main()
{
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD1.xyz = in_NORMAL0.xyz;
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    u_xlat1.x = u_xlat0.y * _ProjectionParams.x;
    u_xlat1.xyz = u_xlat1.xxx * unity_CameraInvProjection[1].xyz;
    u_xlat1.xyz = unity_CameraInvProjection[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
    gl_Position = u_xlat0;
    u_xlat0.xyz = u_xlat1.xyz + (-unity_CameraInvProjection[2].xyz);
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[2].xyz;
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.xyz = u_xlat0.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.w = (-u_xlat0.z);
    vs_TEXCOORD2.xyz = u_xlat0.xyw;
    u_xlat1.w = (-u_xlat1.z);
    vs_TEXCOORD3.xyz = u_xlat1.xyw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	mat4x4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4x4 _CameraToWorld;
uniform highp sampler2D _CameraDepthTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
vec4 u_xlat1;
mediump float u_xlat16_2;
float u_xlat3;
lowp float u_xlat10_3;
float u_xlat6;
void main()
{
    u_xlat0.x = texture(_CameraDepthTexture, vs_TEXCOORD0.xy).x;
    u_xlat3 = _ZBufferParams.x * u_xlat0.x + _ZBufferParams.y;
    u_xlat3 = float(1.0) / u_xlat3;
    u_xlat6 = (-u_xlat3) + u_xlat0.x;
    u_xlat3 = unity_OrthoParams.w * u_xlat6 + u_xlat3;
    u_xlat1.xyz = (-vs_TEXCOORD2.xyz) + vs_TEXCOORD3.xyz;
    u_xlat0.xzw = u_xlat0.xxx * u_xlat1.xyz + vs_TEXCOORD2.xyz;
    u_xlat0.xzw = (-vs_TEXCOORD1.xyz) * vec3(u_xlat3) + u_xlat0.xzw;
    u_xlat1.xyz = vec3(u_xlat3) * vs_TEXCOORD1.xyz;
    u_xlat0.xyz = unity_OrthoParams.www * u_xlat0.xzw + u_xlat1.xyz;
    u_xlat1 = u_xlat0.yyyy * _CameraToWorld[1];
    u_xlat1 = _CameraToWorld[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat0 = _CameraToWorld[2] * u_xlat0.zzzz + u_xlat1;
    u_xlat0 = u_xlat0 + _CameraToWorld[3];
    u_xlat1.xyz = u_xlat0.yyy * unity_World2Shadow[0][1].xyz;
    u_xlat1.xyz = unity_World2Shadow[0][0].xyz * u_xlat0.xxx + u_xlat1.xyz;
    u_xlat1.xyz = unity_World2Shadow[0][2].xyz * u_xlat0.zzz + u_xlat1.xyz;
    u_xlat1.xyz = unity_World2Shadow[0][3].xyz * u_xlat0.www + u_xlat1.xyz;
    u_xlat0.xyz = u_xlat0.xyz + (-unity_ShadowFadeCenterAndType.xyz);
    u_xlat0.x = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat0.x = sqrt(u_xlat0.x);
    u_xlat0.x = u_xlat0.x * _LightShadowData.z + _LightShadowData.w;
#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    vec3 txVec19 = vec3(u_xlat1.xy,u_xlat1.z);
    u_xlat10_3 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec19, 0.0);
    u_xlat16_2 = (-_LightShadowData.x) + 1.0;
    u_xlat16_2 = u_xlat10_3 * u_xlat16_2 + _LightShadowData.x;
    u_xlat0 = u_xlat0.xxxx + vec4(u_xlat16_2);
    SV_Target0 = u_xlat0;
    return;
}
#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" }
""
}
SubProgram "gles3 " {
Keywords { "SHADOWS_NATIVE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
""
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_SINGLE_CASCADE" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
""
}
SubProgram "gles3 " {
Keywords { "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_SINGLE_CASCADE" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
""
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
""
}
}
 }
}
SubShader { 
 Tags { "ShadowmapFilter"="PCF_5x5" }
 Pass {
  Tags { "ShadowmapFilter"="PCF_5x5" }
  ZTest Always
  ZWrite Off
  Cull Off
Program "vp" {
SubProgram "gles " {
Keywords { "SHADOWS_NONATIVE" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform highp sampler2D _ShadowMapTexture;
uniform highp vec4 _ShadowMapTexture_TexelSize;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec3 tmpvar_4;
  tmpvar_4 = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = tmpvar_4;
  highp vec4 tmpvar_6;
  tmpvar_6 = (_CameraToWorld * tmpvar_5);
  bvec4 tmpvar_7;
  tmpvar_7 = greaterThanEqual (tmpvar_4.zzzz, _LightSplitsNear);
  bvec4 tmpvar_8;
  tmpvar_8 = lessThan (tmpvar_4.zzzz, _LightSplitsFar);
  lowp vec4 tmpvar_9;
  tmpvar_9 = (vec4(tmpvar_7) * vec4(tmpvar_8));
  highp vec4 tmpvar_10;
  tmpvar_10.w = 1.0;
  tmpvar_10.xyz = (((
    ((unity_World2Shadow[0] * tmpvar_6).xyz * tmpvar_9.x)
   + 
    ((unity_World2Shadow[1] * tmpvar_6).xyz * tmpvar_9.y)
  ) + (
    (unity_World2Shadow[2] * tmpvar_6)
  .xyz * tmpvar_9.z)) + ((unity_World2Shadow[3] * tmpvar_6).xyz * tmpvar_9.w));
  mediump float shadow_11;
  shadow_11 = 0.0;
  highp vec2 tmpvar_12;
  tmpvar_12 = _ShadowMapTexture_TexelSize.xy;
  highp vec3 tmpvar_13;
  tmpvar_13.xy = (tmpvar_10.xy - _ShadowMapTexture_TexelSize.xy);
  tmpvar_13.z = tmpvar_10.z;
  highp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_ShadowMapTexture, tmpvar_13.xy);
  mediump float tmpvar_15;
  if ((tmpvar_14.x < tmpvar_10.z)) {
    tmpvar_15 = 0.0;
  } else {
    tmpvar_15 = 1.0;
  };
  shadow_11 = tmpvar_15;
  highp vec2 tmpvar_16;
  tmpvar_16.x = 0.0;
  tmpvar_16.y = -(_ShadowMapTexture_TexelSize.y);
  highp vec3 tmpvar_17;
  tmpvar_17.xy = (tmpvar_10.xy + tmpvar_16);
  tmpvar_17.z = tmpvar_10.z;
  highp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_ShadowMapTexture, tmpvar_17.xy);
  highp float tmpvar_19;
  if ((tmpvar_18.x < tmpvar_10.z)) {
    tmpvar_19 = 0.0;
  } else {
    tmpvar_19 = 1.0;
  };
  shadow_11 = (tmpvar_15 + tmpvar_19);
  highp vec2 tmpvar_20;
  tmpvar_20.x = tmpvar_12.x;
  tmpvar_20.y = -(_ShadowMapTexture_TexelSize.y);
  highp vec3 tmpvar_21;
  tmpvar_21.xy = (tmpvar_10.xy + tmpvar_20);
  tmpvar_21.z = tmpvar_10.z;
  highp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_ShadowMapTexture, tmpvar_21.xy);
  highp float tmpvar_23;
  if ((tmpvar_22.x < tmpvar_10.z)) {
    tmpvar_23 = 0.0;
  } else {
    tmpvar_23 = 1.0;
  };
  shadow_11 = (shadow_11 + tmpvar_23);
  highp vec2 tmpvar_24;
  tmpvar_24.y = 0.0;
  tmpvar_24.x = -(_ShadowMapTexture_TexelSize.x);
  highp vec3 tmpvar_25;
  tmpvar_25.xy = (tmpvar_10.xy + tmpvar_24);
  tmpvar_25.z = tmpvar_10.z;
  highp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_ShadowMapTexture, tmpvar_25.xy);
  highp float tmpvar_27;
  if ((tmpvar_26.x < tmpvar_10.z)) {
    tmpvar_27 = 0.0;
  } else {
    tmpvar_27 = 1.0;
  };
  shadow_11 = (shadow_11 + tmpvar_27);
  highp vec4 tmpvar_28;
  tmpvar_28 = texture2D (_ShadowMapTexture, tmpvar_10.xy);
  highp float tmpvar_29;
  if ((tmpvar_28.x < tmpvar_10.z)) {
    tmpvar_29 = 0.0;
  } else {
    tmpvar_29 = 1.0;
  };
  shadow_11 = (shadow_11 + tmpvar_29);
  highp vec2 tmpvar_30;
  tmpvar_30.y = 0.0;
  tmpvar_30.x = tmpvar_12.x;
  highp vec3 tmpvar_31;
  tmpvar_31.xy = (tmpvar_10.xy + tmpvar_30);
  tmpvar_31.z = tmpvar_10.z;
  highp vec4 tmpvar_32;
  tmpvar_32 = texture2D (_ShadowMapTexture, tmpvar_31.xy);
  highp float tmpvar_33;
  if ((tmpvar_32.x < tmpvar_10.z)) {
    tmpvar_33 = 0.0;
  } else {
    tmpvar_33 = 1.0;
  };
  shadow_11 = (shadow_11 + tmpvar_33);
  highp vec2 tmpvar_34;
  tmpvar_34.x = -(_ShadowMapTexture_TexelSize.x);
  tmpvar_34.y = tmpvar_12.y;
  highp vec3 tmpvar_35;
  tmpvar_35.xy = (tmpvar_10.xy + tmpvar_34);
  tmpvar_35.z = tmpvar_10.z;
  highp vec4 tmpvar_36;
  tmpvar_36 = texture2D (_ShadowMapTexture, tmpvar_35.xy);
  highp float tmpvar_37;
  if ((tmpvar_36.x < tmpvar_10.z)) {
    tmpvar_37 = 0.0;
  } else {
    tmpvar_37 = 1.0;
  };
  shadow_11 = (shadow_11 + tmpvar_37);
  highp vec2 tmpvar_38;
  tmpvar_38.x = 0.0;
  tmpvar_38.y = tmpvar_12.y;
  highp vec3 tmpvar_39;
  tmpvar_39.xy = (tmpvar_10.xy + tmpvar_38);
  tmpvar_39.z = tmpvar_10.z;
  highp vec4 tmpvar_40;
  tmpvar_40 = texture2D (_ShadowMapTexture, tmpvar_39.xy);
  highp float tmpvar_41;
  if ((tmpvar_40.x < tmpvar_10.z)) {
    tmpvar_41 = 0.0;
  } else {
    tmpvar_41 = 1.0;
  };
  shadow_11 = (shadow_11 + tmpvar_41);
  highp vec3 tmpvar_42;
  tmpvar_42.xy = (tmpvar_10.xy + _ShadowMapTexture_TexelSize.xy);
  tmpvar_42.z = tmpvar_10.z;
  highp vec4 tmpvar_43;
  tmpvar_43 = texture2D (_ShadowMapTexture, tmpvar_42.xy);
  highp float tmpvar_44;
  if ((tmpvar_43.x < tmpvar_10.z)) {
    tmpvar_44 = 0.0;
  } else {
    tmpvar_44 = 1.0;
  };
  shadow_11 = (shadow_11 + tmpvar_44);
  shadow_11 = (shadow_11 / 9.0);
  mediump float tmpvar_45;
  tmpvar_45 = mix (_LightShadowData.x, 1.0, shadow_11);
  shadow_11 = tmpvar_45;
  highp float tmpvar_46;
  tmpvar_46 = clamp (((tmpvar_4.z * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  shadow_2 = (tmpvar_45 + tmpvar_46);
  mediump vec4 tmpvar_47;
  tmpvar_47 = vec4(shadow_2);
  tmpvar_1 = tmpvar_47;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp vec4 _LightSplitsNear;
uniform highp vec4 _LightSplitsFar;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _ShadowMapTexture_TexelSize;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec3 tmpvar_4;
  tmpvar_4 = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = tmpvar_4;
  highp vec4 tmpvar_6;
  tmpvar_6 = (_CameraToWorld * tmpvar_5);
  bvec4 tmpvar_7;
  tmpvar_7 = greaterThanEqual (tmpvar_4.zzzz, _LightSplitsNear);
  bvec4 tmpvar_8;
  tmpvar_8 = lessThan (tmpvar_4.zzzz, _LightSplitsFar);
  lowp vec4 tmpvar_9;
  tmpvar_9 = (vec4(tmpvar_7) * vec4(tmpvar_8));
  highp vec4 tmpvar_10;
  tmpvar_10.w = 1.0;
  tmpvar_10.xyz = (((
    ((unity_World2Shadow[0] * tmpvar_6).xyz * tmpvar_9.x)
   + 
    ((unity_World2Shadow[1] * tmpvar_6).xyz * tmpvar_9.y)
  ) + (
    (unity_World2Shadow[2] * tmpvar_6)
  .xyz * tmpvar_9.z)) + ((unity_World2Shadow[3] * tmpvar_6).xyz * tmpvar_9.w));
  mediump vec3 accum_11;
  mediump float sum_12;
  mediump float shadow_13;
  highp vec3 v_14;
  highp vec3 u_15;
  highp vec2 tmpvar_16;
  tmpvar_16 = ((tmpvar_10.xy * _ShadowMapTexture_TexelSize.zw) + vec2(0.5, 0.5));
  highp vec2 tmpvar_17;
  tmpvar_17 = ((floor(tmpvar_16) - vec2(0.5, 0.5)) * _ShadowMapTexture_TexelSize.xy);
  highp vec2 tmpvar_18;
  tmpvar_18 = fract(tmpvar_16);
  highp vec3 tmpvar_19;
  tmpvar_19.y = 7.0;
  tmpvar_19.x = (4.0 - (3.0 * tmpvar_18.x));
  tmpvar_19.z = (1.0 + (3.0 * tmpvar_18.x));
  highp vec3 tmpvar_20;
  tmpvar_20.x = (((3.0 - 
    (2.0 * tmpvar_18.x)
  ) / tmpvar_19.x) - 2.0);
  tmpvar_20.y = ((3.0 + tmpvar_18.x) / 7.0);
  tmpvar_20.z = ((tmpvar_18.x / tmpvar_19.z) + 2.0);
  u_15 = (tmpvar_20 * _ShadowMapTexture_TexelSize.x);
  highp vec3 tmpvar_21;
  tmpvar_21.y = 7.0;
  tmpvar_21.x = (4.0 - (3.0 * tmpvar_18.y));
  tmpvar_21.z = (1.0 + (3.0 * tmpvar_18.y));
  highp vec3 tmpvar_22;
  tmpvar_22.x = (((3.0 - 
    (2.0 * tmpvar_18.y)
  ) / tmpvar_21.x) - 2.0);
  tmpvar_22.y = ((3.0 + tmpvar_18.y) / 7.0);
  tmpvar_22.z = ((tmpvar_18.y / tmpvar_21.z) + 2.0);
  v_14 = (tmpvar_22 * _ShadowMapTexture_TexelSize.y);
  highp vec3 tmpvar_23;
  tmpvar_23 = (tmpvar_19 * tmpvar_21.x);
  accum_11 = tmpvar_23;
  highp vec2 tmpvar_24;
  tmpvar_24.x = u_15.x;
  tmpvar_24.y = v_14.x;
  highp float depth_25;
  depth_25 = tmpvar_10.z;
  highp vec3 uv_26;
  highp vec3 tmpvar_27;
  tmpvar_27.xy = (tmpvar_17 + tmpvar_24);
  tmpvar_27.z = depth_25;
  uv_26.xy = tmpvar_27.xy;
  uv_26.z = depth_25;
  lowp float tmpvar_28;
  tmpvar_28 = shadow2DEXT (_ShadowMapTexture, uv_26);
  sum_12 = (accum_11.x * tmpvar_28);
  highp vec2 tmpvar_29;
  tmpvar_29.x = u_15.y;
  tmpvar_29.y = v_14.x;
  highp float depth_30;
  depth_30 = tmpvar_10.z;
  highp vec3 uv_31;
  highp vec3 tmpvar_32;
  tmpvar_32.xy = (tmpvar_17 + tmpvar_29);
  tmpvar_32.z = depth_30;
  uv_31.xy = tmpvar_32.xy;
  uv_31.z = depth_30;
  lowp float tmpvar_33;
  tmpvar_33 = shadow2DEXT (_ShadowMapTexture, uv_31);
  sum_12 = (sum_12 + (accum_11.y * tmpvar_33));
  highp vec2 tmpvar_34;
  tmpvar_34.x = u_15.z;
  tmpvar_34.y = v_14.x;
  highp float depth_35;
  depth_35 = tmpvar_10.z;
  highp vec3 uv_36;
  highp vec3 tmpvar_37;
  tmpvar_37.xy = (tmpvar_17 + tmpvar_34);
  tmpvar_37.z = depth_35;
  uv_36.xy = tmpvar_37.xy;
  uv_36.z = depth_35;
  lowp float tmpvar_38;
  tmpvar_38 = shadow2DEXT (_ShadowMapTexture, uv_36);
  sum_12 = (sum_12 + (accum_11.z * tmpvar_38));
  accum_11 = (tmpvar_19 * 7.0);
  highp vec2 tmpvar_39;
  tmpvar_39.x = u_15.x;
  tmpvar_39.y = v_14.y;
  highp float depth_40;
  depth_40 = tmpvar_10.z;
  highp vec3 uv_41;
  highp vec3 tmpvar_42;
  tmpvar_42.xy = (tmpvar_17 + tmpvar_39);
  tmpvar_42.z = depth_40;
  uv_41.xy = tmpvar_42.xy;
  uv_41.z = depth_40;
  lowp float tmpvar_43;
  tmpvar_43 = shadow2DEXT (_ShadowMapTexture, uv_41);
  sum_12 = (sum_12 + (accum_11.x * tmpvar_43));
  highp vec2 tmpvar_44;
  tmpvar_44.x = u_15.y;
  tmpvar_44.y = v_14.y;
  highp float depth_45;
  depth_45 = tmpvar_10.z;
  highp vec3 uv_46;
  highp vec3 tmpvar_47;
  tmpvar_47.xy = (tmpvar_17 + tmpvar_44);
  tmpvar_47.z = depth_45;
  uv_46.xy = tmpvar_47.xy;
  uv_46.z = depth_45;
  lowp float tmpvar_48;
  tmpvar_48 = shadow2DEXT (_ShadowMapTexture, uv_46);
  sum_12 = (sum_12 + (accum_11.y * tmpvar_48));
  highp vec2 tmpvar_49;
  tmpvar_49.x = u_15.z;
  tmpvar_49.y = v_14.y;
  highp float depth_50;
  depth_50 = tmpvar_10.z;
  highp vec3 uv_51;
  highp vec3 tmpvar_52;
  tmpvar_52.xy = (tmpvar_17 + tmpvar_49);
  tmpvar_52.z = depth_50;
  uv_51.xy = tmpvar_52.xy;
  uv_51.z = depth_50;
  lowp float tmpvar_53;
  tmpvar_53 = shadow2DEXT (_ShadowMapTexture, uv_51);
  sum_12 = (sum_12 + (accum_11.z * tmpvar_53));
  accum_11 = (tmpvar_19 * tmpvar_21.z);
  highp vec2 tmpvar_54;
  tmpvar_54.x = u_15.x;
  tmpvar_54.y = v_14.z;
  highp float depth_55;
  depth_55 = tmpvar_10.z;
  highp vec3 uv_56;
  highp vec3 tmpvar_57;
  tmpvar_57.xy = (tmpvar_17 + tmpvar_54);
  tmpvar_57.z = depth_55;
  uv_56.xy = tmpvar_57.xy;
  uv_56.z = depth_55;
  lowp float tmpvar_58;
  tmpvar_58 = shadow2DEXT (_ShadowMapTexture, uv_56);
  sum_12 = (sum_12 + (accum_11.x * tmpvar_58));
  highp vec2 tmpvar_59;
  tmpvar_59.x = u_15.y;
  tmpvar_59.y = v_14.z;
  highp float depth_60;
  depth_60 = tmpvar_10.z;
  highp vec3 uv_61;
  highp vec3 tmpvar_62;
  tmpvar_62.xy = (tmpvar_17 + tmpvar_59);
  tmpvar_62.z = depth_60;
  uv_61.xy = tmpvar_62.xy;
  uv_61.z = depth_60;
  lowp float tmpvar_63;
  tmpvar_63 = shadow2DEXT (_ShadowMapTexture, uv_61);
  sum_12 = (sum_12 + (accum_11.y * tmpvar_63));
  highp vec2 tmpvar_64;
  tmpvar_64.x = u_15.z;
  tmpvar_64.y = v_14.z;
  highp float depth_65;
  depth_65 = tmpvar_10.z;
  highp vec3 uv_66;
  highp vec3 tmpvar_67;
  tmpvar_67.xy = (tmpvar_17 + tmpvar_64);
  tmpvar_67.z = depth_65;
  uv_66.xy = tmpvar_67.xy;
  uv_66.z = depth_65;
  lowp float tmpvar_68;
  tmpvar_68 = shadow2DEXT (_ShadowMapTexture, uv_66);
  sum_12 = (sum_12 + (accum_11.z * tmpvar_68));
  shadow_13 = (sum_12 / 144.0);
  mediump float tmpvar_69;
  tmpvar_69 = mix (_LightShadowData.x, 1.0, shadow_13);
  shadow_13 = tmpvar_69;
  highp float tmpvar_70;
  tmpvar_70 = clamp (((tmpvar_4.z * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  shadow_2 = (tmpvar_69 + tmpvar_70);
  mediump vec4 tmpvar_71;
  tmpvar_71 = vec4(shadow_2);
  tmpvar_1 = tmpvar_71;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_NATIVE" }
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 unity_CameraInvProjection;
uniform 	mat4x4 glstate_matrix_mvp;
in highp vec4 in_POSITION0;
in highp vec2 in_TEXCOORD0;
in highp vec3 in_NORMAL0;
out highp vec2 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
vec4 u_xlat0;
vec4 u_xlat1;
void main()
{
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD1.xyz = in_NORMAL0.xyz;
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    u_xlat1.x = u_xlat0.y * _ProjectionParams.x;
    u_xlat1.xyz = u_xlat1.xxx * unity_CameraInvProjection[1].xyz;
    u_xlat1.xyz = unity_CameraInvProjection[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
    gl_Position = u_xlat0;
    u_xlat0.xyz = u_xlat1.xyz + (-unity_CameraInvProjection[2].xyz);
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[2].xyz;
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.xyz = u_xlat0.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.w = (-u_xlat0.z);
    vs_TEXCOORD2.xyz = u_xlat0.xyw;
    u_xlat1.w = (-u_xlat1.z);
    vs_TEXCOORD3.xyz = u_xlat1.xyw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 _LightSplitsNear;
uniform 	vec4 _LightSplitsFar;
uniform 	mat4x4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	mat4x4 _CameraToWorld;
uniform 	vec4 _ShadowMapTexture_TexelSize;
uniform highp sampler2D _CameraDepthTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
vec4 u_xlat1;
lowp vec4 u_xlat10_1;
bvec4 u_xlatb1;
vec4 u_xlat2;
lowp float u_xlat10_2;
bvec4 u_xlatb2;
vec4 u_xlat3;
vec4 u_xlat4;
vec3 u_xlat5;
vec4 u_xlat6;
vec3 u_xlat7;
vec3 u_xlat8;
lowp float u_xlat10_8;
float u_xlat16;
mediump float u_xlat16_16;
lowp float u_xlat10_16;
vec2 u_xlat18;
lowp float u_xlat10_18;
lowp float u_xlat10_24;
void main()
{
    u_xlat0.x = texture(_CameraDepthTexture, vs_TEXCOORD0.xy).x;
    u_xlat8.x = _ZBufferParams.x * u_xlat0.x + _ZBufferParams.y;
    u_xlat8.x = float(1.0) / u_xlat8.x;
    u_xlat16 = (-u_xlat8.x) + u_xlat0.x;
    u_xlat8.x = unity_OrthoParams.w * u_xlat16 + u_xlat8.x;
    u_xlat1.xyz = (-vs_TEXCOORD2.xyz) + vs_TEXCOORD3.xyz;
    u_xlat0.xzw = u_xlat0.xxx * u_xlat1.xyz + vs_TEXCOORD2.xyz;
    u_xlat0.xzw = (-vs_TEXCOORD1.xyz) * u_xlat8.xxx + u_xlat0.xzw;
    u_xlat1.xyz = u_xlat8.xxx * vs_TEXCOORD1.xyz;
    u_xlat0.xyz = unity_OrthoParams.www * u_xlat0.xzw + u_xlat1.xyz;
    u_xlatb1 = greaterThanEqual(u_xlat0.zzzz, _LightSplitsNear);
    u_xlat1 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(u_xlatb1));
    u_xlatb2 = lessThan(u_xlat0.zzzz, _LightSplitsFar);
    u_xlat2 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(u_xlatb2));
    u_xlat10_1 = u_xlat1 * u_xlat2;
    u_xlat2 = u_xlat0.yyyy * _CameraToWorld[1];
    u_xlat2 = _CameraToWorld[0] * u_xlat0.xxxx + u_xlat2;
    u_xlat2 = _CameraToWorld[2] * u_xlat0.zzzz + u_xlat2;
    u_xlat0.x = u_xlat0.z * _LightShadowData.z + _LightShadowData.w;
#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    u_xlat2 = u_xlat2 + _CameraToWorld[3];
    u_xlat8.xyz = u_xlat2.yyy * unity_World2Shadow[1][1].xyz;
    u_xlat8.xyz = unity_World2Shadow[1][0].xyz * u_xlat2.xxx + u_xlat8.xyz;
    u_xlat8.xyz = unity_World2Shadow[1][2].xyz * u_xlat2.zzz + u_xlat8.xyz;
    u_xlat8.xyz = unity_World2Shadow[1][3].xyz * u_xlat2.www + u_xlat8.xyz;
    u_xlat8.xyz = u_xlat10_1.yyy * u_xlat8.xyz;
    u_xlat3.xyz = u_xlat2.yyy * unity_World2Shadow[0][1].xyz;
    u_xlat3.xyz = unity_World2Shadow[0][0].xyz * u_xlat2.xxx + u_xlat3.xyz;
    u_xlat3.xyz = unity_World2Shadow[0][2].xyz * u_xlat2.zzz + u_xlat3.xyz;
    u_xlat3.xyz = unity_World2Shadow[0][3].xyz * u_xlat2.www + u_xlat3.xyz;
    u_xlat8.xyz = u_xlat3.xyz * u_xlat10_1.xxx + u_xlat8.xyz;
    u_xlat3.xyz = u_xlat2.yyy * unity_World2Shadow[2][1].xyz;
    u_xlat3.xyz = unity_World2Shadow[2][0].xyz * u_xlat2.xxx + u_xlat3.xyz;
    u_xlat3.xyz = unity_World2Shadow[2][2].xyz * u_xlat2.zzz + u_xlat3.xyz;
    u_xlat3.xyz = unity_World2Shadow[2][3].xyz * u_xlat2.www + u_xlat3.xyz;
    u_xlat8.xyz = u_xlat3.xyz * u_xlat10_1.zzz + u_xlat8.xyz;
    u_xlat3.xyz = u_xlat2.yyy * unity_World2Shadow[3][1].xyz;
    u_xlat3.xyz = unity_World2Shadow[3][0].xyz * u_xlat2.xxx + u_xlat3.xyz;
    u_xlat2.xyz = unity_World2Shadow[3][2].xyz * u_xlat2.zzz + u_xlat3.xyz;
    u_xlat2.xyz = unity_World2Shadow[3][3].xyz * u_xlat2.www + u_xlat2.xyz;
    u_xlat8.xyz = u_xlat2.xyz * u_xlat10_1.www + u_xlat8.xyz;
    u_xlat8.xy = u_xlat8.xy * _ShadowMapTexture_TexelSize.zw + vec2(0.5, 0.5);
    u_xlat2.xy = floor(u_xlat8.xy);
    u_xlat8.xy = fract(u_xlat8.xy);
    u_xlat2.xy = u_xlat2.xy + vec2(-0.5, -0.5);
    u_xlat18.xy = (-u_xlat8.xy) * vec2(2.0, 2.0) + vec2(3.0, 3.0);
    u_xlat3.xy = (-u_xlat8.xy) * vec2(3.0, 3.0) + vec2(4.0, 4.0);
    u_xlat18.xy = u_xlat18.xy / u_xlat3.xy;
    u_xlat1.xy = u_xlat18.xy + vec2(-2.0, -2.0);
    u_xlat4.z = u_xlat1.y;
    u_xlat18.xy = u_xlat8.xy * vec2(3.0, 3.0) + vec2(1.0, 1.0);
    u_xlat3.xz = u_xlat8.xy / u_xlat18.xy;
    u_xlat4.xw = u_xlat3.xz + vec2(2.0, 2.0);
    u_xlat1.w = u_xlat4.x;
    u_xlat3.xz = u_xlat8.xy + vec2(3.0, 3.0);
    u_xlat8.x = u_xlat8.x * 3.0;
    u_xlat5.xz = u_xlat8.xx * vec2(-1.0, 1.0) + vec2(4.0, 1.0);
    u_xlat4.xy = u_xlat3.xz * _ShadowMapTexture_TexelSize.xy;
    u_xlat6.xz = _ShadowMapTexture_TexelSize.yy;
    u_xlat6.y = 0.142857149;
    u_xlat6.xyz = vec3(u_xlat4.z * u_xlat6.x, u_xlat4.y * u_xlat6.y, u_xlat4.w * u_xlat6.z);
    u_xlat1.z = u_xlat4.x;
    u_xlat4.w = u_xlat6.x;
    u_xlat7.xz = _ShadowMapTexture_TexelSize.xx;
    u_xlat7.y = 0.142857149;
    u_xlat4.xyz = u_xlat1.zxw * u_xlat7.yxz;
    u_xlat1 = u_xlat2.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat4.ywxw;
    u_xlat8.xy = u_xlat2.xy * _ShadowMapTexture_TexelSize.xy + u_xlat4.zw;
    vec3 txVec1 = vec3(u_xlat8.xy,u_xlat8.z);
    u_xlat10_8 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec1, 0.0);
    vec3 txVec2 = vec3(u_xlat1.xy,u_xlat8.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec2, 0.0);
    vec3 txVec3 = vec3(u_xlat1.zw,u_xlat8.z);
    u_xlat10_18 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec3, 0.0);
    u_xlat5.y = 7.0;
    u_xlat3.xyz = u_xlat3.yyy * u_xlat5.xyz;
    u_xlat7.xyz = u_xlat18.yyy * u_xlat5.xyz;
    u_xlat5.xy = u_xlat5.xz * vec2(7.0, 7.0);
    u_xlat18.x = u_xlat10_18 * u_xlat3.y;
    u_xlat16 = u_xlat3.x * u_xlat10_16 + u_xlat18.x;
    u_xlat8.x = u_xlat3.z * u_xlat10_8 + u_xlat16;
    u_xlat6.w = u_xlat4.y;
    u_xlat1 = u_xlat2.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat6.wywz;
    u_xlat4.yw = u_xlat6.yz;
    vec3 txVec4 = vec3(u_xlat1.xy,u_xlat8.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec4, 0.0);
    vec3 txVec5 = vec3(u_xlat1.zw,u_xlat8.z);
    u_xlat10_18 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec5, 0.0);
    u_xlat8.x = u_xlat5.x * u_xlat10_16 + u_xlat8.x;
    u_xlat1 = u_xlat2.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat4.xyzy;
    u_xlat3 = u_xlat2.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat4.xwzw;
    vec3 txVec6 = vec3(u_xlat1.xy,u_xlat8.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec6, 0.0);
    vec3 txVec7 = vec3(u_xlat1.zw,u_xlat8.z);
    u_xlat10_2 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec7, 0.0);
    u_xlat8.x = u_xlat10_16 * 49.0 + u_xlat8.x;
    u_xlat8.x = u_xlat5.y * u_xlat10_2 + u_xlat8.x;
    u_xlat8.x = u_xlat7.x * u_xlat10_18 + u_xlat8.x;
    vec3 txVec8 = vec3(u_xlat3.xy,u_xlat8.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec8, 0.0);
    vec3 txVec9 = vec3(u_xlat3.zw,u_xlat8.z);
    u_xlat10_24 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec9, 0.0);
    u_xlat8.x = u_xlat7.y * u_xlat10_16 + u_xlat8.x;
    u_xlat8.x = u_xlat7.z * u_xlat10_24 + u_xlat8.x;
    u_xlat8.x = u_xlat8.x * 0.0069444445;
    u_xlat16_16 = (-_LightShadowData.x) + 1.0;
    u_xlat8.x = u_xlat8.x * u_xlat16_16 + _LightShadowData.x;
    u_xlat0 = u_xlat0.xxxx + u_xlat8.xxxx;
    SV_Target0 = u_xlat0;
    return;
}
#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NONATIVE" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform highp sampler2D _ShadowMapTexture;
uniform highp vec4 _ShadowMapTexture_TexelSize;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5 = (_CameraToWorld * tmpvar_4);
  lowp vec4 weights_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_11;
  tmpvar_11.x = dot (tmpvar_7, tmpvar_7);
  tmpvar_11.y = dot (tmpvar_8, tmpvar_8);
  tmpvar_11.z = dot (tmpvar_9, tmpvar_9);
  tmpvar_11.w = dot (tmpvar_10, tmpvar_10);
  bvec4 tmpvar_12;
  tmpvar_12 = lessThan (tmpvar_11, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_13;
  tmpvar_13 = vec4(tmpvar_12);
  weights_6.x = tmpvar_13.x;
  weights_6.yzw = clamp ((tmpvar_13.yzw - tmpvar_13.xyz), 0.0, 1.0);
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = (((
    ((unity_World2Shadow[0] * tmpvar_5).xyz * tmpvar_13.x)
   + 
    ((unity_World2Shadow[1] * tmpvar_5).xyz * weights_6.y)
  ) + (
    (unity_World2Shadow[2] * tmpvar_5)
  .xyz * weights_6.z)) + ((unity_World2Shadow[3] * tmpvar_5).xyz * weights_6.w));
  mediump float shadow_15;
  shadow_15 = 0.0;
  highp vec2 tmpvar_16;
  tmpvar_16 = _ShadowMapTexture_TexelSize.xy;
  highp vec3 tmpvar_17;
  tmpvar_17.xy = (tmpvar_14.xy - _ShadowMapTexture_TexelSize.xy);
  tmpvar_17.z = tmpvar_14.z;
  highp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_ShadowMapTexture, tmpvar_17.xy);
  mediump float tmpvar_19;
  if ((tmpvar_18.x < tmpvar_14.z)) {
    tmpvar_19 = 0.0;
  } else {
    tmpvar_19 = 1.0;
  };
  shadow_15 = tmpvar_19;
  highp vec2 tmpvar_20;
  tmpvar_20.x = 0.0;
  tmpvar_20.y = -(_ShadowMapTexture_TexelSize.y);
  highp vec3 tmpvar_21;
  tmpvar_21.xy = (tmpvar_14.xy + tmpvar_20);
  tmpvar_21.z = tmpvar_14.z;
  highp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_ShadowMapTexture, tmpvar_21.xy);
  highp float tmpvar_23;
  if ((tmpvar_22.x < tmpvar_14.z)) {
    tmpvar_23 = 0.0;
  } else {
    tmpvar_23 = 1.0;
  };
  shadow_15 = (tmpvar_19 + tmpvar_23);
  highp vec2 tmpvar_24;
  tmpvar_24.x = tmpvar_16.x;
  tmpvar_24.y = -(_ShadowMapTexture_TexelSize.y);
  highp vec3 tmpvar_25;
  tmpvar_25.xy = (tmpvar_14.xy + tmpvar_24);
  tmpvar_25.z = tmpvar_14.z;
  highp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_ShadowMapTexture, tmpvar_25.xy);
  highp float tmpvar_27;
  if ((tmpvar_26.x < tmpvar_14.z)) {
    tmpvar_27 = 0.0;
  } else {
    tmpvar_27 = 1.0;
  };
  shadow_15 = (shadow_15 + tmpvar_27);
  highp vec2 tmpvar_28;
  tmpvar_28.y = 0.0;
  tmpvar_28.x = -(_ShadowMapTexture_TexelSize.x);
  highp vec3 tmpvar_29;
  tmpvar_29.xy = (tmpvar_14.xy + tmpvar_28);
  tmpvar_29.z = tmpvar_14.z;
  highp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_ShadowMapTexture, tmpvar_29.xy);
  highp float tmpvar_31;
  if ((tmpvar_30.x < tmpvar_14.z)) {
    tmpvar_31 = 0.0;
  } else {
    tmpvar_31 = 1.0;
  };
  shadow_15 = (shadow_15 + tmpvar_31);
  highp vec4 tmpvar_32;
  tmpvar_32 = texture2D (_ShadowMapTexture, tmpvar_14.xy);
  highp float tmpvar_33;
  if ((tmpvar_32.x < tmpvar_14.z)) {
    tmpvar_33 = 0.0;
  } else {
    tmpvar_33 = 1.0;
  };
  shadow_15 = (shadow_15 + tmpvar_33);
  highp vec2 tmpvar_34;
  tmpvar_34.y = 0.0;
  tmpvar_34.x = tmpvar_16.x;
  highp vec3 tmpvar_35;
  tmpvar_35.xy = (tmpvar_14.xy + tmpvar_34);
  tmpvar_35.z = tmpvar_14.z;
  highp vec4 tmpvar_36;
  tmpvar_36 = texture2D (_ShadowMapTexture, tmpvar_35.xy);
  highp float tmpvar_37;
  if ((tmpvar_36.x < tmpvar_14.z)) {
    tmpvar_37 = 0.0;
  } else {
    tmpvar_37 = 1.0;
  };
  shadow_15 = (shadow_15 + tmpvar_37);
  highp vec2 tmpvar_38;
  tmpvar_38.x = -(_ShadowMapTexture_TexelSize.x);
  tmpvar_38.y = tmpvar_16.y;
  highp vec3 tmpvar_39;
  tmpvar_39.xy = (tmpvar_14.xy + tmpvar_38);
  tmpvar_39.z = tmpvar_14.z;
  highp vec4 tmpvar_40;
  tmpvar_40 = texture2D (_ShadowMapTexture, tmpvar_39.xy);
  highp float tmpvar_41;
  if ((tmpvar_40.x < tmpvar_14.z)) {
    tmpvar_41 = 0.0;
  } else {
    tmpvar_41 = 1.0;
  };
  shadow_15 = (shadow_15 + tmpvar_41);
  highp vec2 tmpvar_42;
  tmpvar_42.x = 0.0;
  tmpvar_42.y = tmpvar_16.y;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = (tmpvar_14.xy + tmpvar_42);
  tmpvar_43.z = tmpvar_14.z;
  highp vec4 tmpvar_44;
  tmpvar_44 = texture2D (_ShadowMapTexture, tmpvar_43.xy);
  highp float tmpvar_45;
  if ((tmpvar_44.x < tmpvar_14.z)) {
    tmpvar_45 = 0.0;
  } else {
    tmpvar_45 = 1.0;
  };
  shadow_15 = (shadow_15 + tmpvar_45);
  highp vec3 tmpvar_46;
  tmpvar_46.xy = (tmpvar_14.xy + _ShadowMapTexture_TexelSize.xy);
  tmpvar_46.z = tmpvar_14.z;
  highp vec4 tmpvar_47;
  tmpvar_47 = texture2D (_ShadowMapTexture, tmpvar_46.xy);
  highp float tmpvar_48;
  if ((tmpvar_47.x < tmpvar_14.z)) {
    tmpvar_48 = 0.0;
  } else {
    tmpvar_48 = 1.0;
  };
  shadow_15 = (shadow_15 + tmpvar_48);
  shadow_15 = (shadow_15 / 9.0);
  mediump float tmpvar_49;
  tmpvar_49 = mix (_LightShadowData.x, 1.0, shadow_15);
  shadow_15 = tmpvar_49;
  highp float tmpvar_50;
  highp vec3 tmpvar_51;
  tmpvar_51 = (tmpvar_5.xyz - unity_ShadowFadeCenterAndType.xyz);
  mediump float tmpvar_52;
  highp float tmpvar_53;
  tmpvar_53 = clamp (((
    sqrt(dot (tmpvar_51, tmpvar_51))
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  tmpvar_52 = tmpvar_53;
  tmpvar_50 = tmpvar_52;
  shadow_2 = (tmpvar_49 + tmpvar_50);
  mediump vec4 tmpvar_54;
  tmpvar_54 = vec4(shadow_2);
  tmpvar_1 = tmpvar_54;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp vec4 unity_ShadowSplitSpheres[4];
uniform highp vec4 unity_ShadowSplitSqRadii;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _ShadowMapTexture_TexelSize;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5 = (_CameraToWorld * tmpvar_4);
  lowp vec4 weights_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[0].xyz);
  highp vec3 tmpvar_8;
  tmpvar_8 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[1].xyz);
  highp vec3 tmpvar_9;
  tmpvar_9 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[2].xyz);
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_5.xyz - unity_ShadowSplitSpheres[3].xyz);
  highp vec4 tmpvar_11;
  tmpvar_11.x = dot (tmpvar_7, tmpvar_7);
  tmpvar_11.y = dot (tmpvar_8, tmpvar_8);
  tmpvar_11.z = dot (tmpvar_9, tmpvar_9);
  tmpvar_11.w = dot (tmpvar_10, tmpvar_10);
  bvec4 tmpvar_12;
  tmpvar_12 = lessThan (tmpvar_11, unity_ShadowSplitSqRadii);
  lowp vec4 tmpvar_13;
  tmpvar_13 = vec4(tmpvar_12);
  weights_6.x = tmpvar_13.x;
  weights_6.yzw = clamp ((tmpvar_13.yzw - tmpvar_13.xyz), 0.0, 1.0);
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = (((
    ((unity_World2Shadow[0] * tmpvar_5).xyz * tmpvar_13.x)
   + 
    ((unity_World2Shadow[1] * tmpvar_5).xyz * weights_6.y)
  ) + (
    (unity_World2Shadow[2] * tmpvar_5)
  .xyz * weights_6.z)) + ((unity_World2Shadow[3] * tmpvar_5).xyz * weights_6.w));
  mediump vec3 accum_15;
  mediump float sum_16;
  mediump float shadow_17;
  highp vec3 v_18;
  highp vec3 u_19;
  highp vec2 tmpvar_20;
  tmpvar_20 = ((tmpvar_14.xy * _ShadowMapTexture_TexelSize.zw) + vec2(0.5, 0.5));
  highp vec2 tmpvar_21;
  tmpvar_21 = ((floor(tmpvar_20) - vec2(0.5, 0.5)) * _ShadowMapTexture_TexelSize.xy);
  highp vec2 tmpvar_22;
  tmpvar_22 = fract(tmpvar_20);
  highp vec3 tmpvar_23;
  tmpvar_23.y = 7.0;
  tmpvar_23.x = (4.0 - (3.0 * tmpvar_22.x));
  tmpvar_23.z = (1.0 + (3.0 * tmpvar_22.x));
  highp vec3 tmpvar_24;
  tmpvar_24.x = (((3.0 - 
    (2.0 * tmpvar_22.x)
  ) / tmpvar_23.x) - 2.0);
  tmpvar_24.y = ((3.0 + tmpvar_22.x) / 7.0);
  tmpvar_24.z = ((tmpvar_22.x / tmpvar_23.z) + 2.0);
  u_19 = (tmpvar_24 * _ShadowMapTexture_TexelSize.x);
  highp vec3 tmpvar_25;
  tmpvar_25.y = 7.0;
  tmpvar_25.x = (4.0 - (3.0 * tmpvar_22.y));
  tmpvar_25.z = (1.0 + (3.0 * tmpvar_22.y));
  highp vec3 tmpvar_26;
  tmpvar_26.x = (((3.0 - 
    (2.0 * tmpvar_22.y)
  ) / tmpvar_25.x) - 2.0);
  tmpvar_26.y = ((3.0 + tmpvar_22.y) / 7.0);
  tmpvar_26.z = ((tmpvar_22.y / tmpvar_25.z) + 2.0);
  v_18 = (tmpvar_26 * _ShadowMapTexture_TexelSize.y);
  highp vec3 tmpvar_27;
  tmpvar_27 = (tmpvar_23 * tmpvar_25.x);
  accum_15 = tmpvar_27;
  highp vec2 tmpvar_28;
  tmpvar_28.x = u_19.x;
  tmpvar_28.y = v_18.x;
  highp float depth_29;
  depth_29 = tmpvar_14.z;
  highp vec3 uv_30;
  highp vec3 tmpvar_31;
  tmpvar_31.xy = (tmpvar_21 + tmpvar_28);
  tmpvar_31.z = depth_29;
  uv_30.xy = tmpvar_31.xy;
  uv_30.z = depth_29;
  lowp float tmpvar_32;
  tmpvar_32 = shadow2DEXT (_ShadowMapTexture, uv_30);
  sum_16 = (accum_15.x * tmpvar_32);
  highp vec2 tmpvar_33;
  tmpvar_33.x = u_19.y;
  tmpvar_33.y = v_18.x;
  highp float depth_34;
  depth_34 = tmpvar_14.z;
  highp vec3 uv_35;
  highp vec3 tmpvar_36;
  tmpvar_36.xy = (tmpvar_21 + tmpvar_33);
  tmpvar_36.z = depth_34;
  uv_35.xy = tmpvar_36.xy;
  uv_35.z = depth_34;
  lowp float tmpvar_37;
  tmpvar_37 = shadow2DEXT (_ShadowMapTexture, uv_35);
  sum_16 = (sum_16 + (accum_15.y * tmpvar_37));
  highp vec2 tmpvar_38;
  tmpvar_38.x = u_19.z;
  tmpvar_38.y = v_18.x;
  highp float depth_39;
  depth_39 = tmpvar_14.z;
  highp vec3 uv_40;
  highp vec3 tmpvar_41;
  tmpvar_41.xy = (tmpvar_21 + tmpvar_38);
  tmpvar_41.z = depth_39;
  uv_40.xy = tmpvar_41.xy;
  uv_40.z = depth_39;
  lowp float tmpvar_42;
  tmpvar_42 = shadow2DEXT (_ShadowMapTexture, uv_40);
  sum_16 = (sum_16 + (accum_15.z * tmpvar_42));
  accum_15 = (tmpvar_23 * 7.0);
  highp vec2 tmpvar_43;
  tmpvar_43.x = u_19.x;
  tmpvar_43.y = v_18.y;
  highp float depth_44;
  depth_44 = tmpvar_14.z;
  highp vec3 uv_45;
  highp vec3 tmpvar_46;
  tmpvar_46.xy = (tmpvar_21 + tmpvar_43);
  tmpvar_46.z = depth_44;
  uv_45.xy = tmpvar_46.xy;
  uv_45.z = depth_44;
  lowp float tmpvar_47;
  tmpvar_47 = shadow2DEXT (_ShadowMapTexture, uv_45);
  sum_16 = (sum_16 + (accum_15.x * tmpvar_47));
  highp vec2 tmpvar_48;
  tmpvar_48.x = u_19.y;
  tmpvar_48.y = v_18.y;
  highp float depth_49;
  depth_49 = tmpvar_14.z;
  highp vec3 uv_50;
  highp vec3 tmpvar_51;
  tmpvar_51.xy = (tmpvar_21 + tmpvar_48);
  tmpvar_51.z = depth_49;
  uv_50.xy = tmpvar_51.xy;
  uv_50.z = depth_49;
  lowp float tmpvar_52;
  tmpvar_52 = shadow2DEXT (_ShadowMapTexture, uv_50);
  sum_16 = (sum_16 + (accum_15.y * tmpvar_52));
  highp vec2 tmpvar_53;
  tmpvar_53.x = u_19.z;
  tmpvar_53.y = v_18.y;
  highp float depth_54;
  depth_54 = tmpvar_14.z;
  highp vec3 uv_55;
  highp vec3 tmpvar_56;
  tmpvar_56.xy = (tmpvar_21 + tmpvar_53);
  tmpvar_56.z = depth_54;
  uv_55.xy = tmpvar_56.xy;
  uv_55.z = depth_54;
  lowp float tmpvar_57;
  tmpvar_57 = shadow2DEXT (_ShadowMapTexture, uv_55);
  sum_16 = (sum_16 + (accum_15.z * tmpvar_57));
  accum_15 = (tmpvar_23 * tmpvar_25.z);
  highp vec2 tmpvar_58;
  tmpvar_58.x = u_19.x;
  tmpvar_58.y = v_18.z;
  highp float depth_59;
  depth_59 = tmpvar_14.z;
  highp vec3 uv_60;
  highp vec3 tmpvar_61;
  tmpvar_61.xy = (tmpvar_21 + tmpvar_58);
  tmpvar_61.z = depth_59;
  uv_60.xy = tmpvar_61.xy;
  uv_60.z = depth_59;
  lowp float tmpvar_62;
  tmpvar_62 = shadow2DEXT (_ShadowMapTexture, uv_60);
  sum_16 = (sum_16 + (accum_15.x * tmpvar_62));
  highp vec2 tmpvar_63;
  tmpvar_63.x = u_19.y;
  tmpvar_63.y = v_18.z;
  highp float depth_64;
  depth_64 = tmpvar_14.z;
  highp vec3 uv_65;
  highp vec3 tmpvar_66;
  tmpvar_66.xy = (tmpvar_21 + tmpvar_63);
  tmpvar_66.z = depth_64;
  uv_65.xy = tmpvar_66.xy;
  uv_65.z = depth_64;
  lowp float tmpvar_67;
  tmpvar_67 = shadow2DEXT (_ShadowMapTexture, uv_65);
  sum_16 = (sum_16 + (accum_15.y * tmpvar_67));
  highp vec2 tmpvar_68;
  tmpvar_68.x = u_19.z;
  tmpvar_68.y = v_18.z;
  highp float depth_69;
  depth_69 = tmpvar_14.z;
  highp vec3 uv_70;
  highp vec3 tmpvar_71;
  tmpvar_71.xy = (tmpvar_21 + tmpvar_68);
  tmpvar_71.z = depth_69;
  uv_70.xy = tmpvar_71.xy;
  uv_70.z = depth_69;
  lowp float tmpvar_72;
  tmpvar_72 = shadow2DEXT (_ShadowMapTexture, uv_70);
  sum_16 = (sum_16 + (accum_15.z * tmpvar_72));
  shadow_17 = (sum_16 / 144.0);
  mediump float tmpvar_73;
  tmpvar_73 = mix (_LightShadowData.x, 1.0, shadow_17);
  shadow_17 = tmpvar_73;
  highp float tmpvar_74;
  highp vec3 tmpvar_75;
  tmpvar_75 = (tmpvar_5.xyz - unity_ShadowFadeCenterAndType.xyz);
  mediump float tmpvar_76;
  highp float tmpvar_77;
  tmpvar_77 = clamp (((
    sqrt(dot (tmpvar_75, tmpvar_75))
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  tmpvar_76 = tmpvar_77;
  tmpvar_74 = tmpvar_76;
  shadow_2 = (tmpvar_73 + tmpvar_74);
  mediump vec4 tmpvar_78;
  tmpvar_78 = vec4(shadow_2);
  tmpvar_1 = tmpvar_78;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 unity_CameraInvProjection;
uniform 	mat4x4 glstate_matrix_mvp;
in highp vec4 in_POSITION0;
in highp vec2 in_TEXCOORD0;
in highp vec3 in_NORMAL0;
out highp vec2 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
vec4 u_xlat0;
vec4 u_xlat1;
void main()
{
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD1.xyz = in_NORMAL0.xyz;
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    u_xlat1.x = u_xlat0.y * _ProjectionParams.x;
    u_xlat1.xyz = u_xlat1.xxx * unity_CameraInvProjection[1].xyz;
    u_xlat1.xyz = unity_CameraInvProjection[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
    gl_Position = u_xlat0;
    u_xlat0.xyz = u_xlat1.xyz + (-unity_CameraInvProjection[2].xyz);
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[2].xyz;
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.xyz = u_xlat0.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.w = (-u_xlat0.z);
    vs_TEXCOORD2.xyz = u_xlat0.xyw;
    u_xlat1.w = (-u_xlat1.z);
    vs_TEXCOORD3.xyz = u_xlat1.xyw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	vec4 unity_ShadowSplitSpheres[4];
uniform 	vec4 unity_ShadowSplitSqRadii;
uniform 	mat4x4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4x4 _CameraToWorld;
uniform 	vec4 _ShadowMapTexture_TexelSize;
uniform highp sampler2D _CameraDepthTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
vec4 u_xlat1;
lowp float u_xlat10_1;
bvec4 u_xlatb1;
vec4 u_xlat2;
vec4 u_xlat3;
lowp vec3 u_xlat10_3;
vec4 u_xlat4;
vec3 u_xlat5;
vec4 u_xlat6;
vec3 u_xlat7;
vec3 u_xlat8;
lowp float u_xlat10_8;
vec3 u_xlat9;
float u_xlat16;
mediump float u_xlat16_16;
lowp float u_xlat10_16;
vec2 u_xlat17;
lowp float u_xlat10_17;
lowp float u_xlat10_24;
void main()
{
    u_xlat0.x = texture(_CameraDepthTexture, vs_TEXCOORD0.xy).x;
    u_xlat8.x = _ZBufferParams.x * u_xlat0.x + _ZBufferParams.y;
    u_xlat8.x = float(1.0) / u_xlat8.x;
    u_xlat16 = (-u_xlat8.x) + u_xlat0.x;
    u_xlat8.x = unity_OrthoParams.w * u_xlat16 + u_xlat8.x;
    u_xlat1.xyz = (-vs_TEXCOORD2.xyz) + vs_TEXCOORD3.xyz;
    u_xlat0.xzw = u_xlat0.xxx * u_xlat1.xyz + vs_TEXCOORD2.xyz;
    u_xlat0.xzw = (-vs_TEXCOORD1.xyz) * u_xlat8.xxx + u_xlat0.xzw;
    u_xlat1.xyz = u_xlat8.xxx * vs_TEXCOORD1.xyz;
    u_xlat0.xyz = unity_OrthoParams.www * u_xlat0.xzw + u_xlat1.xyz;
    u_xlat1 = u_xlat0.yyyy * _CameraToWorld[1];
    u_xlat1 = _CameraToWorld[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat0 = _CameraToWorld[2] * u_xlat0.zzzz + u_xlat1;
    u_xlat0 = u_xlat0 + _CameraToWorld[3];
    u_xlat1.xyz = u_xlat0.xyz + (-unity_ShadowSplitSpheres[0].xyz);
    u_xlat1.x = dot(u_xlat1.xyz, u_xlat1.xyz);
    u_xlat2.xyz = u_xlat0.xyz + (-unity_ShadowSplitSpheres[1].xyz);
    u_xlat1.y = dot(u_xlat2.xyz, u_xlat2.xyz);
    u_xlat2.xyz = u_xlat0.xyz + (-unity_ShadowSplitSpheres[2].xyz);
    u_xlat1.z = dot(u_xlat2.xyz, u_xlat2.xyz);
    u_xlat2.xyz = u_xlat0.xyz + (-unity_ShadowSplitSpheres[3].xyz);
    u_xlat1.w = dot(u_xlat2.xyz, u_xlat2.xyz);
    u_xlatb1 = lessThan(u_xlat1, unity_ShadowSplitSqRadii);
    u_xlat10_3.x = (u_xlatb1.x) ? float(-1.0) : float(-0.0);
    u_xlat10_3.y = (u_xlatb1.y) ? float(-1.0) : float(-0.0);
    u_xlat10_3.z = (u_xlatb1.z) ? float(-1.0) : float(-0.0);
    u_xlat1 = mix(vec4(0.0, 0.0, 0.0, 0.0), vec4(1.0, 1.0, 1.0, 1.0), vec4(u_xlatb1));
    u_xlat10_3.xyz = vec3(u_xlat10_3.x + u_xlat1.y, u_xlat10_3.y + u_xlat1.z, u_xlat10_3.z + u_xlat1.w);
    u_xlat10_3.xyz = max(u_xlat10_3.xyz, vec3(0.0, 0.0, 0.0));
    u_xlat9.xyz = u_xlat0.yyy * unity_World2Shadow[1][1].xyz;
    u_xlat9.xyz = unity_World2Shadow[1][0].xyz * u_xlat0.xxx + u_xlat9.xyz;
    u_xlat9.xyz = unity_World2Shadow[1][2].xyz * u_xlat0.zzz + u_xlat9.xyz;
    u_xlat9.xyz = unity_World2Shadow[1][3].xyz * u_xlat0.www + u_xlat9.xyz;
    u_xlat9.xyz = u_xlat10_3.xxx * u_xlat9.xyz;
    u_xlat2.xyz = u_xlat0.yyy * unity_World2Shadow[0][1].xyz;
    u_xlat2.xyz = unity_World2Shadow[0][0].xyz * u_xlat0.xxx + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[0][2].xyz * u_xlat0.zzz + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[0][3].xyz * u_xlat0.www + u_xlat2.xyz;
    u_xlat1.xyz = u_xlat2.xyz * u_xlat1.xxx + u_xlat9.xyz;
    u_xlat2.xyz = u_xlat0.yyy * unity_World2Shadow[2][1].xyz;
    u_xlat2.xyz = unity_World2Shadow[2][0].xyz * u_xlat0.xxx + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[2][2].xyz * u_xlat0.zzz + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[2][3].xyz * u_xlat0.www + u_xlat2.xyz;
    u_xlat1.xyz = u_xlat2.xyz * u_xlat10_3.yyy + u_xlat1.xyz;
    u_xlat2.xyz = u_xlat0.yyy * unity_World2Shadow[3][1].xyz;
    u_xlat2.xyz = unity_World2Shadow[3][0].xyz * u_xlat0.xxx + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[3][2].xyz * u_xlat0.zzz + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[3][3].xyz * u_xlat0.www + u_xlat2.xyz;
    u_xlat0.xyz = u_xlat0.xyz + (-unity_ShadowFadeCenterAndType.xyz);
    u_xlat0.x = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat0.x = sqrt(u_xlat0.x);
    u_xlat0.x = u_xlat0.x * _LightShadowData.z + _LightShadowData.w;
#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    u_xlat8.xyz = u_xlat2.xyz * u_xlat10_3.zzz + u_xlat1.xyz;
    u_xlat8.xy = u_xlat8.xy * _ShadowMapTexture_TexelSize.zw + vec2(0.5, 0.5);
    u_xlat1.xy = floor(u_xlat8.xy);
    u_xlat8.xy = fract(u_xlat8.xy);
    u_xlat1.xy = u_xlat1.xy + vec2(-0.5, -0.5);
    u_xlat17.xy = (-u_xlat8.xy) * vec2(2.0, 2.0) + vec2(3.0, 3.0);
    u_xlat2.xy = (-u_xlat8.xy) * vec2(3.0, 3.0) + vec2(4.0, 4.0);
    u_xlat17.xy = u_xlat17.xy / u_xlat2.xy;
    u_xlat3.xy = u_xlat17.xy + vec2(-2.0, -2.0);
    u_xlat4.z = u_xlat3.y;
    u_xlat17.xy = u_xlat8.xy * vec2(3.0, 3.0) + vec2(1.0, 1.0);
    u_xlat2.xz = u_xlat8.xy / u_xlat17.xy;
    u_xlat4.xw = u_xlat2.xz + vec2(2.0, 2.0);
    u_xlat3.w = u_xlat4.x;
    u_xlat2.xz = u_xlat8.xy + vec2(3.0, 3.0);
    u_xlat8.x = u_xlat8.x * 3.0;
    u_xlat5.xz = u_xlat8.xx * vec2(-1.0, 1.0) + vec2(4.0, 1.0);
    u_xlat4.xy = u_xlat2.xz * _ShadowMapTexture_TexelSize.xy;
    u_xlat6.xz = _ShadowMapTexture_TexelSize.yy;
    u_xlat6.y = 0.142857149;
    u_xlat6.xyz = vec3(u_xlat4.z * u_xlat6.x, u_xlat4.y * u_xlat6.y, u_xlat4.w * u_xlat6.z);
    u_xlat3.z = u_xlat4.x;
    u_xlat4.w = u_xlat6.x;
    u_xlat7.xz = _ShadowMapTexture_TexelSize.xx;
    u_xlat7.y = 0.142857149;
    u_xlat4.xyz = u_xlat3.zxw * u_xlat7.yxz;
    u_xlat3 = u_xlat1.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat4.ywxw;
    u_xlat8.xy = u_xlat1.xy * _ShadowMapTexture_TexelSize.xy + u_xlat4.zw;
    vec3 txVec9 = vec3(u_xlat8.xy,u_xlat8.z);
    u_xlat10_8 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec9, 0.0);
    vec3 txVec10 = vec3(u_xlat3.xy,u_xlat8.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec10, 0.0);
    vec3 txVec11 = vec3(u_xlat3.zw,u_xlat8.z);
    u_xlat10_17 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec11, 0.0);
    u_xlat5.y = 7.0;
    u_xlat2.xyz = u_xlat2.yyy * u_xlat5.xyz;
    u_xlat7.xyz = u_xlat17.yyy * u_xlat5.xyz;
    u_xlat5.xy = u_xlat5.xz * vec2(7.0, 7.0);
    u_xlat17.x = u_xlat10_17 * u_xlat2.y;
    u_xlat16 = u_xlat2.x * u_xlat10_16 + u_xlat17.x;
    u_xlat8.x = u_xlat2.z * u_xlat10_8 + u_xlat16;
    u_xlat6.w = u_xlat4.y;
    u_xlat2 = u_xlat1.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat6.wywz;
    u_xlat4.yw = u_xlat6.yz;
    vec3 txVec12 = vec3(u_xlat2.xy,u_xlat8.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec12, 0.0);
    vec3 txVec13 = vec3(u_xlat2.zw,u_xlat8.z);
    u_xlat10_17 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec13, 0.0);
    u_xlat8.x = u_xlat5.x * u_xlat10_16 + u_xlat8.x;
    u_xlat2 = u_xlat1.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat4.xyzy;
    u_xlat3 = u_xlat1.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat4.xwzw;
    vec3 txVec14 = vec3(u_xlat2.xy,u_xlat8.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec14, 0.0);
    vec3 txVec15 = vec3(u_xlat2.zw,u_xlat8.z);
    u_xlat10_1 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec15, 0.0);
    u_xlat8.x = u_xlat10_16 * 49.0 + u_xlat8.x;
    u_xlat8.x = u_xlat5.y * u_xlat10_1 + u_xlat8.x;
    u_xlat8.x = u_xlat7.x * u_xlat10_17 + u_xlat8.x;
    vec3 txVec16 = vec3(u_xlat3.xy,u_xlat8.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec16, 0.0);
    vec3 txVec17 = vec3(u_xlat3.zw,u_xlat8.z);
    u_xlat10_24 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec17, 0.0);
    u_xlat8.x = u_xlat7.y * u_xlat10_16 + u_xlat8.x;
    u_xlat8.x = u_xlat7.z * u_xlat10_24 + u_xlat8.x;
    u_xlat8.x = u_xlat8.x * 0.0069444445;
    u_xlat16_16 = (-_LightShadowData.x) + 1.0;
    u_xlat8.x = u_xlat8.x * u_xlat16_16 + _LightShadowData.x;
    u_xlat0 = u_xlat0.xxxx + u_xlat8.xxxx;
    SV_Target0 = u_xlat0;
    return;
}
#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SINGLE_CASCADE" "SHADOWS_NONATIVE" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform highp sampler2D _ShadowMapTexture;
uniform highp vec4 _ShadowMapTexture_TexelSize;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec3 tmpvar_4;
  tmpvar_4 = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = tmpvar_4;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = (unity_World2Shadow[0] * (_CameraToWorld * tmpvar_5)).xyz;
  mediump float shadow_7;
  shadow_7 = 0.0;
  highp vec2 tmpvar_8;
  tmpvar_8 = _ShadowMapTexture_TexelSize.xy;
  highp vec3 tmpvar_9;
  tmpvar_9.xy = (tmpvar_6.xy - _ShadowMapTexture_TexelSize.xy);
  tmpvar_9.z = tmpvar_6.z;
  highp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_ShadowMapTexture, tmpvar_9.xy);
  mediump float tmpvar_11;
  if ((tmpvar_10.x < tmpvar_6.z)) {
    tmpvar_11 = 0.0;
  } else {
    tmpvar_11 = 1.0;
  };
  shadow_7 = tmpvar_11;
  highp vec2 tmpvar_12;
  tmpvar_12.x = 0.0;
  tmpvar_12.y = -(_ShadowMapTexture_TexelSize.y);
  highp vec3 tmpvar_13;
  tmpvar_13.xy = (tmpvar_6.xy + tmpvar_12);
  tmpvar_13.z = tmpvar_6.z;
  highp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_ShadowMapTexture, tmpvar_13.xy);
  highp float tmpvar_15;
  if ((tmpvar_14.x < tmpvar_6.z)) {
    tmpvar_15 = 0.0;
  } else {
    tmpvar_15 = 1.0;
  };
  shadow_7 = (tmpvar_11 + tmpvar_15);
  highp vec2 tmpvar_16;
  tmpvar_16.x = tmpvar_8.x;
  tmpvar_16.y = -(_ShadowMapTexture_TexelSize.y);
  highp vec3 tmpvar_17;
  tmpvar_17.xy = (tmpvar_6.xy + tmpvar_16);
  tmpvar_17.z = tmpvar_6.z;
  highp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_ShadowMapTexture, tmpvar_17.xy);
  highp float tmpvar_19;
  if ((tmpvar_18.x < tmpvar_6.z)) {
    tmpvar_19 = 0.0;
  } else {
    tmpvar_19 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_19);
  highp vec2 tmpvar_20;
  tmpvar_20.y = 0.0;
  tmpvar_20.x = -(_ShadowMapTexture_TexelSize.x);
  highp vec3 tmpvar_21;
  tmpvar_21.xy = (tmpvar_6.xy + tmpvar_20);
  tmpvar_21.z = tmpvar_6.z;
  highp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_ShadowMapTexture, tmpvar_21.xy);
  highp float tmpvar_23;
  if ((tmpvar_22.x < tmpvar_6.z)) {
    tmpvar_23 = 0.0;
  } else {
    tmpvar_23 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_23);
  highp vec4 tmpvar_24;
  tmpvar_24 = texture2D (_ShadowMapTexture, tmpvar_6.xy);
  highp float tmpvar_25;
  if ((tmpvar_24.x < tmpvar_6.z)) {
    tmpvar_25 = 0.0;
  } else {
    tmpvar_25 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_25);
  highp vec2 tmpvar_26;
  tmpvar_26.y = 0.0;
  tmpvar_26.x = tmpvar_8.x;
  highp vec3 tmpvar_27;
  tmpvar_27.xy = (tmpvar_6.xy + tmpvar_26);
  tmpvar_27.z = tmpvar_6.z;
  highp vec4 tmpvar_28;
  tmpvar_28 = texture2D (_ShadowMapTexture, tmpvar_27.xy);
  highp float tmpvar_29;
  if ((tmpvar_28.x < tmpvar_6.z)) {
    tmpvar_29 = 0.0;
  } else {
    tmpvar_29 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_29);
  highp vec2 tmpvar_30;
  tmpvar_30.x = -(_ShadowMapTexture_TexelSize.x);
  tmpvar_30.y = tmpvar_8.y;
  highp vec3 tmpvar_31;
  tmpvar_31.xy = (tmpvar_6.xy + tmpvar_30);
  tmpvar_31.z = tmpvar_6.z;
  highp vec4 tmpvar_32;
  tmpvar_32 = texture2D (_ShadowMapTexture, tmpvar_31.xy);
  highp float tmpvar_33;
  if ((tmpvar_32.x < tmpvar_6.z)) {
    tmpvar_33 = 0.0;
  } else {
    tmpvar_33 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_33);
  highp vec2 tmpvar_34;
  tmpvar_34.x = 0.0;
  tmpvar_34.y = tmpvar_8.y;
  highp vec3 tmpvar_35;
  tmpvar_35.xy = (tmpvar_6.xy + tmpvar_34);
  tmpvar_35.z = tmpvar_6.z;
  highp vec4 tmpvar_36;
  tmpvar_36 = texture2D (_ShadowMapTexture, tmpvar_35.xy);
  highp float tmpvar_37;
  if ((tmpvar_36.x < tmpvar_6.z)) {
    tmpvar_37 = 0.0;
  } else {
    tmpvar_37 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_37);
  highp vec3 tmpvar_38;
  tmpvar_38.xy = (tmpvar_6.xy + _ShadowMapTexture_TexelSize.xy);
  tmpvar_38.z = tmpvar_6.z;
  highp vec4 tmpvar_39;
  tmpvar_39 = texture2D (_ShadowMapTexture, tmpvar_38.xy);
  highp float tmpvar_40;
  if ((tmpvar_39.x < tmpvar_6.z)) {
    tmpvar_40 = 0.0;
  } else {
    tmpvar_40 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_40);
  shadow_7 = (shadow_7 / 9.0);
  mediump float tmpvar_41;
  tmpvar_41 = mix (_LightShadowData.x, 1.0, shadow_7);
  shadow_7 = tmpvar_41;
  highp float tmpvar_42;
  tmpvar_42 = clamp (((tmpvar_4.z * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  shadow_2 = (tmpvar_41 + tmpvar_42);
  mediump vec4 tmpvar_43;
  tmpvar_43 = vec4(shadow_2);
  tmpvar_1 = tmpvar_43;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _ShadowMapTexture_TexelSize;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec3 tmpvar_4;
  tmpvar_4 = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 1.0;
  tmpvar_5.xyz = tmpvar_4;
  highp vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = (unity_World2Shadow[0] * (_CameraToWorld * tmpvar_5)).xyz;
  mediump vec3 accum_7;
  mediump float sum_8;
  mediump float shadow_9;
  highp vec3 v_10;
  highp vec3 u_11;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((tmpvar_6.xy * _ShadowMapTexture_TexelSize.zw) + vec2(0.5, 0.5));
  highp vec2 tmpvar_13;
  tmpvar_13 = ((floor(tmpvar_12) - vec2(0.5, 0.5)) * _ShadowMapTexture_TexelSize.xy);
  highp vec2 tmpvar_14;
  tmpvar_14 = fract(tmpvar_12);
  highp vec3 tmpvar_15;
  tmpvar_15.y = 7.0;
  tmpvar_15.x = (4.0 - (3.0 * tmpvar_14.x));
  tmpvar_15.z = (1.0 + (3.0 * tmpvar_14.x));
  highp vec3 tmpvar_16;
  tmpvar_16.x = (((3.0 - 
    (2.0 * tmpvar_14.x)
  ) / tmpvar_15.x) - 2.0);
  tmpvar_16.y = ((3.0 + tmpvar_14.x) / 7.0);
  tmpvar_16.z = ((tmpvar_14.x / tmpvar_15.z) + 2.0);
  u_11 = (tmpvar_16 * _ShadowMapTexture_TexelSize.x);
  highp vec3 tmpvar_17;
  tmpvar_17.y = 7.0;
  tmpvar_17.x = (4.0 - (3.0 * tmpvar_14.y));
  tmpvar_17.z = (1.0 + (3.0 * tmpvar_14.y));
  highp vec3 tmpvar_18;
  tmpvar_18.x = (((3.0 - 
    (2.0 * tmpvar_14.y)
  ) / tmpvar_17.x) - 2.0);
  tmpvar_18.y = ((3.0 + tmpvar_14.y) / 7.0);
  tmpvar_18.z = ((tmpvar_14.y / tmpvar_17.z) + 2.0);
  v_10 = (tmpvar_18 * _ShadowMapTexture_TexelSize.y);
  highp vec3 tmpvar_19;
  tmpvar_19 = (tmpvar_15 * tmpvar_17.x);
  accum_7 = tmpvar_19;
  highp vec2 tmpvar_20;
  tmpvar_20.x = u_11.x;
  tmpvar_20.y = v_10.x;
  highp float depth_21;
  depth_21 = tmpvar_6.z;
  highp vec3 uv_22;
  highp vec3 tmpvar_23;
  tmpvar_23.xy = (tmpvar_13 + tmpvar_20);
  tmpvar_23.z = depth_21;
  uv_22.xy = tmpvar_23.xy;
  uv_22.z = depth_21;
  lowp float tmpvar_24;
  tmpvar_24 = shadow2DEXT (_ShadowMapTexture, uv_22);
  sum_8 = (accum_7.x * tmpvar_24);
  highp vec2 tmpvar_25;
  tmpvar_25.x = u_11.y;
  tmpvar_25.y = v_10.x;
  highp float depth_26;
  depth_26 = tmpvar_6.z;
  highp vec3 uv_27;
  highp vec3 tmpvar_28;
  tmpvar_28.xy = (tmpvar_13 + tmpvar_25);
  tmpvar_28.z = depth_26;
  uv_27.xy = tmpvar_28.xy;
  uv_27.z = depth_26;
  lowp float tmpvar_29;
  tmpvar_29 = shadow2DEXT (_ShadowMapTexture, uv_27);
  sum_8 = (sum_8 + (accum_7.y * tmpvar_29));
  highp vec2 tmpvar_30;
  tmpvar_30.x = u_11.z;
  tmpvar_30.y = v_10.x;
  highp float depth_31;
  depth_31 = tmpvar_6.z;
  highp vec3 uv_32;
  highp vec3 tmpvar_33;
  tmpvar_33.xy = (tmpvar_13 + tmpvar_30);
  tmpvar_33.z = depth_31;
  uv_32.xy = tmpvar_33.xy;
  uv_32.z = depth_31;
  lowp float tmpvar_34;
  tmpvar_34 = shadow2DEXT (_ShadowMapTexture, uv_32);
  sum_8 = (sum_8 + (accum_7.z * tmpvar_34));
  accum_7 = (tmpvar_15 * 7.0);
  highp vec2 tmpvar_35;
  tmpvar_35.x = u_11.x;
  tmpvar_35.y = v_10.y;
  highp float depth_36;
  depth_36 = tmpvar_6.z;
  highp vec3 uv_37;
  highp vec3 tmpvar_38;
  tmpvar_38.xy = (tmpvar_13 + tmpvar_35);
  tmpvar_38.z = depth_36;
  uv_37.xy = tmpvar_38.xy;
  uv_37.z = depth_36;
  lowp float tmpvar_39;
  tmpvar_39 = shadow2DEXT (_ShadowMapTexture, uv_37);
  sum_8 = (sum_8 + (accum_7.x * tmpvar_39));
  highp vec2 tmpvar_40;
  tmpvar_40.x = u_11.y;
  tmpvar_40.y = v_10.y;
  highp float depth_41;
  depth_41 = tmpvar_6.z;
  highp vec3 uv_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = (tmpvar_13 + tmpvar_40);
  tmpvar_43.z = depth_41;
  uv_42.xy = tmpvar_43.xy;
  uv_42.z = depth_41;
  lowp float tmpvar_44;
  tmpvar_44 = shadow2DEXT (_ShadowMapTexture, uv_42);
  sum_8 = (sum_8 + (accum_7.y * tmpvar_44));
  highp vec2 tmpvar_45;
  tmpvar_45.x = u_11.z;
  tmpvar_45.y = v_10.y;
  highp float depth_46;
  depth_46 = tmpvar_6.z;
  highp vec3 uv_47;
  highp vec3 tmpvar_48;
  tmpvar_48.xy = (tmpvar_13 + tmpvar_45);
  tmpvar_48.z = depth_46;
  uv_47.xy = tmpvar_48.xy;
  uv_47.z = depth_46;
  lowp float tmpvar_49;
  tmpvar_49 = shadow2DEXT (_ShadowMapTexture, uv_47);
  sum_8 = (sum_8 + (accum_7.z * tmpvar_49));
  accum_7 = (tmpvar_15 * tmpvar_17.z);
  highp vec2 tmpvar_50;
  tmpvar_50.x = u_11.x;
  tmpvar_50.y = v_10.z;
  highp float depth_51;
  depth_51 = tmpvar_6.z;
  highp vec3 uv_52;
  highp vec3 tmpvar_53;
  tmpvar_53.xy = (tmpvar_13 + tmpvar_50);
  tmpvar_53.z = depth_51;
  uv_52.xy = tmpvar_53.xy;
  uv_52.z = depth_51;
  lowp float tmpvar_54;
  tmpvar_54 = shadow2DEXT (_ShadowMapTexture, uv_52);
  sum_8 = (sum_8 + (accum_7.x * tmpvar_54));
  highp vec2 tmpvar_55;
  tmpvar_55.x = u_11.y;
  tmpvar_55.y = v_10.z;
  highp float depth_56;
  depth_56 = tmpvar_6.z;
  highp vec3 uv_57;
  highp vec3 tmpvar_58;
  tmpvar_58.xy = (tmpvar_13 + tmpvar_55);
  tmpvar_58.z = depth_56;
  uv_57.xy = tmpvar_58.xy;
  uv_57.z = depth_56;
  lowp float tmpvar_59;
  tmpvar_59 = shadow2DEXT (_ShadowMapTexture, uv_57);
  sum_8 = (sum_8 + (accum_7.y * tmpvar_59));
  highp vec2 tmpvar_60;
  tmpvar_60.x = u_11.z;
  tmpvar_60.y = v_10.z;
  highp float depth_61;
  depth_61 = tmpvar_6.z;
  highp vec3 uv_62;
  highp vec3 tmpvar_63;
  tmpvar_63.xy = (tmpvar_13 + tmpvar_60);
  tmpvar_63.z = depth_61;
  uv_62.xy = tmpvar_63.xy;
  uv_62.z = depth_61;
  lowp float tmpvar_64;
  tmpvar_64 = shadow2DEXT (_ShadowMapTexture, uv_62);
  sum_8 = (sum_8 + (accum_7.z * tmpvar_64));
  shadow_9 = (sum_8 / 144.0);
  mediump float tmpvar_65;
  tmpvar_65 = mix (_LightShadowData.x, 1.0, shadow_9);
  shadow_9 = tmpvar_65;
  highp float tmpvar_66;
  tmpvar_66 = clamp (((tmpvar_4.z * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  shadow_2 = (tmpvar_65 + tmpvar_66);
  mediump vec4 tmpvar_67;
  tmpvar_67 = vec4(shadow_2);
  tmpvar_1 = tmpvar_67;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 unity_CameraInvProjection;
uniform 	mat4x4 glstate_matrix_mvp;
in highp vec4 in_POSITION0;
in highp vec2 in_TEXCOORD0;
in highp vec3 in_NORMAL0;
out highp vec2 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
vec4 u_xlat0;
vec4 u_xlat1;
void main()
{
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD1.xyz = in_NORMAL0.xyz;
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    u_xlat1.x = u_xlat0.y * _ProjectionParams.x;
    u_xlat1.xyz = u_xlat1.xxx * unity_CameraInvProjection[1].xyz;
    u_xlat1.xyz = unity_CameraInvProjection[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
    gl_Position = u_xlat0;
    u_xlat0.xyz = u_xlat1.xyz + (-unity_CameraInvProjection[2].xyz);
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[2].xyz;
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.xyz = u_xlat0.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.w = (-u_xlat0.z);
    vs_TEXCOORD2.xyz = u_xlat0.xyw;
    u_xlat1.w = (-u_xlat1.z);
    vs_TEXCOORD3.xyz = u_xlat1.xyw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	mat4x4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	mat4x4 _CameraToWorld;
uniform 	vec4 _ShadowMapTexture_TexelSize;
uniform highp sampler2D _CameraDepthTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
lowp float u_xlat10_0;
vec4 u_xlat1;
lowp float u_xlat10_1;
vec4 u_xlat2;
vec3 u_xlat3;
vec4 u_xlat4;
vec4 u_xlat5;
vec3 u_xlat6;
vec4 u_xlat7;
float u_xlat8;
mediump float u_xlat16_8;
lowp float u_xlat10_8;
vec3 u_xlat9;
vec2 u_xlat10;
float u_xlat16;
lowp float u_xlat10_16;
vec2 u_xlat18;
float u_xlat24;
float u_xlat25;
void main()
{
    u_xlat0.xz = _ShadowMapTexture_TexelSize.yy;
    u_xlat0.y = 0.142857149;
    u_xlat24 = texture(_CameraDepthTexture, vs_TEXCOORD0.xy).x;
    u_xlat1.x = _ZBufferParams.x * u_xlat24 + _ZBufferParams.y;
    u_xlat1.x = float(1.0) / u_xlat1.x;
    u_xlat9.x = u_xlat24 + (-u_xlat1.x);
    u_xlat1.x = unity_OrthoParams.w * u_xlat9.x + u_xlat1.x;
    u_xlat9.xyz = (-vs_TEXCOORD2.xyz) + vs_TEXCOORD3.xyz;
    u_xlat9.xyz = vec3(u_xlat24) * u_xlat9.xyz + vs_TEXCOORD2.xyz;
    u_xlat9.xyz = (-vs_TEXCOORD1.xyz) * u_xlat1.xxx + u_xlat9.xyz;
    u_xlat2.xyz = u_xlat1.xxx * vs_TEXCOORD1.xyz;
    u_xlat1.xyz = unity_OrthoParams.www * u_xlat9.xyz + u_xlat2.xyz;
    u_xlat2 = u_xlat1.yyyy * _CameraToWorld[1];
    u_xlat2 = _CameraToWorld[0] * u_xlat1.xxxx + u_xlat2;
    u_xlat2 = _CameraToWorld[2] * u_xlat1.zzzz + u_xlat2;
    u_xlat24 = u_xlat1.z * _LightShadowData.z + _LightShadowData.w;
#ifdef UNITY_ADRENO_ES3
    u_xlat24 = min(max(u_xlat24, 0.0), 1.0);
#else
    u_xlat24 = clamp(u_xlat24, 0.0, 1.0);
#endif
    u_xlat1 = u_xlat2 + _CameraToWorld[3];
    u_xlat2.xyz = u_xlat1.yyy * unity_World2Shadow[0][1].xyz;
    u_xlat2.xyz = unity_World2Shadow[0][0].xyz * u_xlat1.xxx + u_xlat2.xyz;
    u_xlat1.xyz = unity_World2Shadow[0][2].xyz * u_xlat1.zzz + u_xlat2.xyz;
    u_xlat1.xyz = unity_World2Shadow[0][3].xyz * u_xlat1.www + u_xlat1.xyz;
    u_xlat1.xy = u_xlat1.xy * _ShadowMapTexture_TexelSize.zw + vec2(0.5, 0.5);
    u_xlat2.xy = fract(u_xlat1.xy);
    u_xlat1.xy = floor(u_xlat1.xy);
    u_xlat1.xy = u_xlat1.xy + vec2(-0.5, -0.5);
    u_xlat18.xy = (-u_xlat2.xy) * vec2(2.0, 2.0) + vec2(3.0, 3.0);
    u_xlat3.xy = (-u_xlat2.xy) * vec2(3.0, 3.0) + vec2(4.0, 4.0);
    u_xlat18.xy = u_xlat18.xy / u_xlat3.xy;
    u_xlat4.xy = u_xlat18.xy + vec2(-2.0, -2.0);
    u_xlat5.z = u_xlat4.y;
    u_xlat18.xy = u_xlat2.xy * vec2(3.0, 3.0) + vec2(1.0, 1.0);
    u_xlat3.xz = u_xlat2.xy / u_xlat18.xy;
    u_xlat5.xw = u_xlat3.xz + vec2(2.0, 2.0);
    u_xlat4.w = u_xlat5.x;
    u_xlat10.xy = u_xlat2.xy + vec2(3.0, 3.0);
    u_xlat25 = u_xlat2.x * 3.0;
    u_xlat6.xz = vec2(u_xlat25) * vec2(-1.0, 1.0) + vec2(4.0, 1.0);
    u_xlat5.xy = u_xlat10.xy * _ShadowMapTexture_TexelSize.xy;
    u_xlat7.xyz = vec3(u_xlat0.x * u_xlat5.z, u_xlat0.y * u_xlat5.y, u_xlat0.z * u_xlat5.w);
    u_xlat4.z = u_xlat5.x;
    u_xlat5.w = u_xlat7.x;
    u_xlat0.xz = _ShadowMapTexture_TexelSize.xx;
    u_xlat0.y = 0.142857149;
    u_xlat5.xyz = u_xlat0.yxz * u_xlat4.zxw;
    u_xlat4 = u_xlat1.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat5.ywxw;
    u_xlat0.xy = u_xlat1.xy * _ShadowMapTexture_TexelSize.xy + u_xlat5.zw;
    vec3 txVec27 = vec3(u_xlat0.xy,u_xlat1.z);
    u_xlat10_0 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec27, 0.0);
    vec3 txVec28 = vec3(u_xlat4.xy,u_xlat1.z);
    u_xlat10_8 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec28, 0.0);
    vec3 txVec29 = vec3(u_xlat4.zw,u_xlat1.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec29, 0.0);
    u_xlat6.y = 7.0;
    u_xlat2.xyz = u_xlat3.yyy * u_xlat6.xyz;
    u_xlat3.xyz = u_xlat18.yyy * u_xlat6.xyz;
    u_xlat4.xy = u_xlat6.xz * vec2(7.0, 7.0);
    u_xlat16 = u_xlat10_16 * u_xlat2.y;
    u_xlat8 = u_xlat2.x * u_xlat10_8 + u_xlat16;
    u_xlat0.x = u_xlat2.z * u_xlat10_0 + u_xlat8;
    u_xlat7.w = u_xlat5.y;
    u_xlat2 = u_xlat1.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat7.wywz;
    u_xlat5.yw = u_xlat7.yz;
    vec3 txVec30 = vec3(u_xlat2.xy,u_xlat1.z);
    u_xlat10_8 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec30, 0.0);
    vec3 txVec31 = vec3(u_xlat2.zw,u_xlat1.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec31, 0.0);
    u_xlat0.x = u_xlat4.x * u_xlat10_8 + u_xlat0.x;
    u_xlat2 = u_xlat1.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat5.xyzy;
    u_xlat5 = u_xlat1.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat5.xwzw;
    vec3 txVec32 = vec3(u_xlat2.xy,u_xlat1.z);
    u_xlat10_8 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec32, 0.0);
    vec3 txVec33 = vec3(u_xlat2.zw,u_xlat1.z);
    u_xlat10_1 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec33, 0.0);
    u_xlat0.x = u_xlat10_8 * 49.0 + u_xlat0.x;
    u_xlat0.x = u_xlat4.y * u_xlat10_1 + u_xlat0.x;
    u_xlat0.x = u_xlat3.x * u_xlat10_16 + u_xlat0.x;
    vec3 txVec34 = vec3(u_xlat5.xy,u_xlat1.z);
    u_xlat10_8 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec34, 0.0);
    vec3 txVec35 = vec3(u_xlat5.zw,u_xlat1.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec35, 0.0);
    u_xlat0.x = u_xlat3.y * u_xlat10_8 + u_xlat0.x;
    u_xlat0.x = u_xlat3.z * u_xlat10_16 + u_xlat0.x;
    u_xlat0.x = u_xlat0.x * 0.0069444445;
    u_xlat16_8 = (-_LightShadowData.x) + 1.0;
    u_xlat0.x = u_xlat0.x * u_xlat16_8 + _LightShadowData.x;
    u_xlat0 = vec4(u_xlat24) + u_xlat0.xxxx;
    SV_Target0 = u_xlat0;
    return;
}
#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_SINGLE_CASCADE" "SHADOWS_NONATIVE" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform highp sampler2D _ShadowMapTexture;
uniform highp vec4 _ShadowMapTexture_TexelSize;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5 = (_CameraToWorld * tmpvar_4);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = (unity_World2Shadow[0] * tmpvar_5).xyz;
  mediump float shadow_7;
  shadow_7 = 0.0;
  highp vec2 tmpvar_8;
  tmpvar_8 = _ShadowMapTexture_TexelSize.xy;
  highp vec3 tmpvar_9;
  tmpvar_9.xy = (tmpvar_6.xy - _ShadowMapTexture_TexelSize.xy);
  tmpvar_9.z = tmpvar_6.z;
  highp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_ShadowMapTexture, tmpvar_9.xy);
  mediump float tmpvar_11;
  if ((tmpvar_10.x < tmpvar_6.z)) {
    tmpvar_11 = 0.0;
  } else {
    tmpvar_11 = 1.0;
  };
  shadow_7 = tmpvar_11;
  highp vec2 tmpvar_12;
  tmpvar_12.x = 0.0;
  tmpvar_12.y = -(_ShadowMapTexture_TexelSize.y);
  highp vec3 tmpvar_13;
  tmpvar_13.xy = (tmpvar_6.xy + tmpvar_12);
  tmpvar_13.z = tmpvar_6.z;
  highp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_ShadowMapTexture, tmpvar_13.xy);
  highp float tmpvar_15;
  if ((tmpvar_14.x < tmpvar_6.z)) {
    tmpvar_15 = 0.0;
  } else {
    tmpvar_15 = 1.0;
  };
  shadow_7 = (tmpvar_11 + tmpvar_15);
  highp vec2 tmpvar_16;
  tmpvar_16.x = tmpvar_8.x;
  tmpvar_16.y = -(_ShadowMapTexture_TexelSize.y);
  highp vec3 tmpvar_17;
  tmpvar_17.xy = (tmpvar_6.xy + tmpvar_16);
  tmpvar_17.z = tmpvar_6.z;
  highp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_ShadowMapTexture, tmpvar_17.xy);
  highp float tmpvar_19;
  if ((tmpvar_18.x < tmpvar_6.z)) {
    tmpvar_19 = 0.0;
  } else {
    tmpvar_19 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_19);
  highp vec2 tmpvar_20;
  tmpvar_20.y = 0.0;
  tmpvar_20.x = -(_ShadowMapTexture_TexelSize.x);
  highp vec3 tmpvar_21;
  tmpvar_21.xy = (tmpvar_6.xy + tmpvar_20);
  tmpvar_21.z = tmpvar_6.z;
  highp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_ShadowMapTexture, tmpvar_21.xy);
  highp float tmpvar_23;
  if ((tmpvar_22.x < tmpvar_6.z)) {
    tmpvar_23 = 0.0;
  } else {
    tmpvar_23 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_23);
  highp vec4 tmpvar_24;
  tmpvar_24 = texture2D (_ShadowMapTexture, tmpvar_6.xy);
  highp float tmpvar_25;
  if ((tmpvar_24.x < tmpvar_6.z)) {
    tmpvar_25 = 0.0;
  } else {
    tmpvar_25 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_25);
  highp vec2 tmpvar_26;
  tmpvar_26.y = 0.0;
  tmpvar_26.x = tmpvar_8.x;
  highp vec3 tmpvar_27;
  tmpvar_27.xy = (tmpvar_6.xy + tmpvar_26);
  tmpvar_27.z = tmpvar_6.z;
  highp vec4 tmpvar_28;
  tmpvar_28 = texture2D (_ShadowMapTexture, tmpvar_27.xy);
  highp float tmpvar_29;
  if ((tmpvar_28.x < tmpvar_6.z)) {
    tmpvar_29 = 0.0;
  } else {
    tmpvar_29 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_29);
  highp vec2 tmpvar_30;
  tmpvar_30.x = -(_ShadowMapTexture_TexelSize.x);
  tmpvar_30.y = tmpvar_8.y;
  highp vec3 tmpvar_31;
  tmpvar_31.xy = (tmpvar_6.xy + tmpvar_30);
  tmpvar_31.z = tmpvar_6.z;
  highp vec4 tmpvar_32;
  tmpvar_32 = texture2D (_ShadowMapTexture, tmpvar_31.xy);
  highp float tmpvar_33;
  if ((tmpvar_32.x < tmpvar_6.z)) {
    tmpvar_33 = 0.0;
  } else {
    tmpvar_33 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_33);
  highp vec2 tmpvar_34;
  tmpvar_34.x = 0.0;
  tmpvar_34.y = tmpvar_8.y;
  highp vec3 tmpvar_35;
  tmpvar_35.xy = (tmpvar_6.xy + tmpvar_34);
  tmpvar_35.z = tmpvar_6.z;
  highp vec4 tmpvar_36;
  tmpvar_36 = texture2D (_ShadowMapTexture, tmpvar_35.xy);
  highp float tmpvar_37;
  if ((tmpvar_36.x < tmpvar_6.z)) {
    tmpvar_37 = 0.0;
  } else {
    tmpvar_37 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_37);
  highp vec3 tmpvar_38;
  tmpvar_38.xy = (tmpvar_6.xy + _ShadowMapTexture_TexelSize.xy);
  tmpvar_38.z = tmpvar_6.z;
  highp vec4 tmpvar_39;
  tmpvar_39 = texture2D (_ShadowMapTexture, tmpvar_38.xy);
  highp float tmpvar_40;
  if ((tmpvar_39.x < tmpvar_6.z)) {
    tmpvar_40 = 0.0;
  } else {
    tmpvar_40 = 1.0;
  };
  shadow_7 = (shadow_7 + tmpvar_40);
  shadow_7 = (shadow_7 / 9.0);
  mediump float tmpvar_41;
  tmpvar_41 = mix (_LightShadowData.x, 1.0, shadow_7);
  shadow_7 = tmpvar_41;
  highp float tmpvar_42;
  highp vec3 tmpvar_43;
  tmpvar_43 = (tmpvar_5.xyz - unity_ShadowFadeCenterAndType.xyz);
  mediump float tmpvar_44;
  highp float tmpvar_45;
  tmpvar_45 = clamp (((
    sqrt(dot (tmpvar_43, tmpvar_43))
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  tmpvar_44 = tmpvar_45;
  tmpvar_42 = tmpvar_44;
  shadow_2 = (tmpvar_41 + tmpvar_42);
  mediump vec4 tmpvar_46;
  tmpvar_46 = vec4(shadow_2);
  tmpvar_1 = tmpvar_46;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 unity_CameraInvProjection;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  highp vec3 orthoPosFar_1;
  highp vec3 orthoPosNear_2;
  highp vec4 clipPos_3;
  highp vec4 tmpvar_4;
  tmpvar_4 = (glstate_matrix_mvp * _glesVertex);
  clipPos_3.xzw = tmpvar_4.xzw;
  clipPos_3.y = (tmpvar_4.y * _ProjectionParams.x);
  highp vec4 tmpvar_5;
  tmpvar_5.zw = vec2(-1.0, 1.0);
  tmpvar_5.xy = clipPos_3.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (unity_CameraInvProjection * tmpvar_5).xyz;
  orthoPosNear_2.xy = tmpvar_6.xy;
  highp vec4 tmpvar_7;
  tmpvar_7.zw = vec2(1.0, 1.0);
  tmpvar_7.xy = clipPos_3.xy;
  highp vec3 tmpvar_8;
  tmpvar_8 = (unity_CameraInvProjection * tmpvar_7).xyz;
  orthoPosFar_1.xy = tmpvar_8.xy;
  orthoPosNear_2.z = -(tmpvar_6.z);
  orthoPosFar_1.z = -(tmpvar_8.z);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = _glesNormal;
  xlv_TEXCOORD2 = orthoPosNear_2;
  xlv_TEXCOORD3 = orthoPosFar_1;
  gl_Position = tmpvar_4;
}


#endif
#ifdef FRAGMENT
#extension GL_EXT_shadow_samplers : enable
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_OrthoParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform highp sampler2D _CameraDepthTexture;
uniform highp mat4 _CameraToWorld;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _ShadowMapTexture_TexelSize;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
varying highp vec3 xlv_TEXCOORD2;
varying highp vec3 xlv_TEXCOORD3;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump float shadow_2;
  highp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_CameraDepthTexture, xlv_TEXCOORD0);
  highp vec4 tmpvar_4;
  tmpvar_4.w = 1.0;
  tmpvar_4.xyz = mix ((xlv_TEXCOORD1 * mix (
    (1.0/(((_ZBufferParams.x * tmpvar_3.x) + _ZBufferParams.y)))
  , tmpvar_3.x, unity_OrthoParams.w)), mix (xlv_TEXCOORD2, xlv_TEXCOORD3, tmpvar_3.xxx), unity_OrthoParams.www);
  highp vec4 tmpvar_5;
  tmpvar_5 = (_CameraToWorld * tmpvar_4);
  highp vec4 tmpvar_6;
  tmpvar_6.w = 0.0;
  tmpvar_6.xyz = (unity_World2Shadow[0] * tmpvar_5).xyz;
  mediump vec3 accum_7;
  mediump float sum_8;
  mediump float shadow_9;
  highp vec3 v_10;
  highp vec3 u_11;
  highp vec2 tmpvar_12;
  tmpvar_12 = ((tmpvar_6.xy * _ShadowMapTexture_TexelSize.zw) + vec2(0.5, 0.5));
  highp vec2 tmpvar_13;
  tmpvar_13 = ((floor(tmpvar_12) - vec2(0.5, 0.5)) * _ShadowMapTexture_TexelSize.xy);
  highp vec2 tmpvar_14;
  tmpvar_14 = fract(tmpvar_12);
  highp vec3 tmpvar_15;
  tmpvar_15.y = 7.0;
  tmpvar_15.x = (4.0 - (3.0 * tmpvar_14.x));
  tmpvar_15.z = (1.0 + (3.0 * tmpvar_14.x));
  highp vec3 tmpvar_16;
  tmpvar_16.x = (((3.0 - 
    (2.0 * tmpvar_14.x)
  ) / tmpvar_15.x) - 2.0);
  tmpvar_16.y = ((3.0 + tmpvar_14.x) / 7.0);
  tmpvar_16.z = ((tmpvar_14.x / tmpvar_15.z) + 2.0);
  u_11 = (tmpvar_16 * _ShadowMapTexture_TexelSize.x);
  highp vec3 tmpvar_17;
  tmpvar_17.y = 7.0;
  tmpvar_17.x = (4.0 - (3.0 * tmpvar_14.y));
  tmpvar_17.z = (1.0 + (3.0 * tmpvar_14.y));
  highp vec3 tmpvar_18;
  tmpvar_18.x = (((3.0 - 
    (2.0 * tmpvar_14.y)
  ) / tmpvar_17.x) - 2.0);
  tmpvar_18.y = ((3.0 + tmpvar_14.y) / 7.0);
  tmpvar_18.z = ((tmpvar_14.y / tmpvar_17.z) + 2.0);
  v_10 = (tmpvar_18 * _ShadowMapTexture_TexelSize.y);
  highp vec3 tmpvar_19;
  tmpvar_19 = (tmpvar_15 * tmpvar_17.x);
  accum_7 = tmpvar_19;
  highp vec2 tmpvar_20;
  tmpvar_20.x = u_11.x;
  tmpvar_20.y = v_10.x;
  highp float depth_21;
  depth_21 = tmpvar_6.z;
  highp vec3 uv_22;
  highp vec3 tmpvar_23;
  tmpvar_23.xy = (tmpvar_13 + tmpvar_20);
  tmpvar_23.z = depth_21;
  uv_22.xy = tmpvar_23.xy;
  uv_22.z = depth_21;
  lowp float tmpvar_24;
  tmpvar_24 = shadow2DEXT (_ShadowMapTexture, uv_22);
  sum_8 = (accum_7.x * tmpvar_24);
  highp vec2 tmpvar_25;
  tmpvar_25.x = u_11.y;
  tmpvar_25.y = v_10.x;
  highp float depth_26;
  depth_26 = tmpvar_6.z;
  highp vec3 uv_27;
  highp vec3 tmpvar_28;
  tmpvar_28.xy = (tmpvar_13 + tmpvar_25);
  tmpvar_28.z = depth_26;
  uv_27.xy = tmpvar_28.xy;
  uv_27.z = depth_26;
  lowp float tmpvar_29;
  tmpvar_29 = shadow2DEXT (_ShadowMapTexture, uv_27);
  sum_8 = (sum_8 + (accum_7.y * tmpvar_29));
  highp vec2 tmpvar_30;
  tmpvar_30.x = u_11.z;
  tmpvar_30.y = v_10.x;
  highp float depth_31;
  depth_31 = tmpvar_6.z;
  highp vec3 uv_32;
  highp vec3 tmpvar_33;
  tmpvar_33.xy = (tmpvar_13 + tmpvar_30);
  tmpvar_33.z = depth_31;
  uv_32.xy = tmpvar_33.xy;
  uv_32.z = depth_31;
  lowp float tmpvar_34;
  tmpvar_34 = shadow2DEXT (_ShadowMapTexture, uv_32);
  sum_8 = (sum_8 + (accum_7.z * tmpvar_34));
  accum_7 = (tmpvar_15 * 7.0);
  highp vec2 tmpvar_35;
  tmpvar_35.x = u_11.x;
  tmpvar_35.y = v_10.y;
  highp float depth_36;
  depth_36 = tmpvar_6.z;
  highp vec3 uv_37;
  highp vec3 tmpvar_38;
  tmpvar_38.xy = (tmpvar_13 + tmpvar_35);
  tmpvar_38.z = depth_36;
  uv_37.xy = tmpvar_38.xy;
  uv_37.z = depth_36;
  lowp float tmpvar_39;
  tmpvar_39 = shadow2DEXT (_ShadowMapTexture, uv_37);
  sum_8 = (sum_8 + (accum_7.x * tmpvar_39));
  highp vec2 tmpvar_40;
  tmpvar_40.x = u_11.y;
  tmpvar_40.y = v_10.y;
  highp float depth_41;
  depth_41 = tmpvar_6.z;
  highp vec3 uv_42;
  highp vec3 tmpvar_43;
  tmpvar_43.xy = (tmpvar_13 + tmpvar_40);
  tmpvar_43.z = depth_41;
  uv_42.xy = tmpvar_43.xy;
  uv_42.z = depth_41;
  lowp float tmpvar_44;
  tmpvar_44 = shadow2DEXT (_ShadowMapTexture, uv_42);
  sum_8 = (sum_8 + (accum_7.y * tmpvar_44));
  highp vec2 tmpvar_45;
  tmpvar_45.x = u_11.z;
  tmpvar_45.y = v_10.y;
  highp float depth_46;
  depth_46 = tmpvar_6.z;
  highp vec3 uv_47;
  highp vec3 tmpvar_48;
  tmpvar_48.xy = (tmpvar_13 + tmpvar_45);
  tmpvar_48.z = depth_46;
  uv_47.xy = tmpvar_48.xy;
  uv_47.z = depth_46;
  lowp float tmpvar_49;
  tmpvar_49 = shadow2DEXT (_ShadowMapTexture, uv_47);
  sum_8 = (sum_8 + (accum_7.z * tmpvar_49));
  accum_7 = (tmpvar_15 * tmpvar_17.z);
  highp vec2 tmpvar_50;
  tmpvar_50.x = u_11.x;
  tmpvar_50.y = v_10.z;
  highp float depth_51;
  depth_51 = tmpvar_6.z;
  highp vec3 uv_52;
  highp vec3 tmpvar_53;
  tmpvar_53.xy = (tmpvar_13 + tmpvar_50);
  tmpvar_53.z = depth_51;
  uv_52.xy = tmpvar_53.xy;
  uv_52.z = depth_51;
  lowp float tmpvar_54;
  tmpvar_54 = shadow2DEXT (_ShadowMapTexture, uv_52);
  sum_8 = (sum_8 + (accum_7.x * tmpvar_54));
  highp vec2 tmpvar_55;
  tmpvar_55.x = u_11.y;
  tmpvar_55.y = v_10.z;
  highp float depth_56;
  depth_56 = tmpvar_6.z;
  highp vec3 uv_57;
  highp vec3 tmpvar_58;
  tmpvar_58.xy = (tmpvar_13 + tmpvar_55);
  tmpvar_58.z = depth_56;
  uv_57.xy = tmpvar_58.xy;
  uv_57.z = depth_56;
  lowp float tmpvar_59;
  tmpvar_59 = shadow2DEXT (_ShadowMapTexture, uv_57);
  sum_8 = (sum_8 + (accum_7.y * tmpvar_59));
  highp vec2 tmpvar_60;
  tmpvar_60.x = u_11.z;
  tmpvar_60.y = v_10.z;
  highp float depth_61;
  depth_61 = tmpvar_6.z;
  highp vec3 uv_62;
  highp vec3 tmpvar_63;
  tmpvar_63.xy = (tmpvar_13 + tmpvar_60);
  tmpvar_63.z = depth_61;
  uv_62.xy = tmpvar_63.xy;
  uv_62.z = depth_61;
  lowp float tmpvar_64;
  tmpvar_64 = shadow2DEXT (_ShadowMapTexture, uv_62);
  sum_8 = (sum_8 + (accum_7.z * tmpvar_64));
  shadow_9 = (sum_8 / 144.0);
  mediump float tmpvar_65;
  tmpvar_65 = mix (_LightShadowData.x, 1.0, shadow_9);
  shadow_9 = tmpvar_65;
  highp float tmpvar_66;
  highp vec3 tmpvar_67;
  tmpvar_67 = (tmpvar_5.xyz - unity_ShadowFadeCenterAndType.xyz);
  mediump float tmpvar_68;
  highp float tmpvar_69;
  tmpvar_69 = clamp (((
    sqrt(dot (tmpvar_67, tmpvar_67))
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  tmpvar_68 = tmpvar_69;
  tmpvar_66 = tmpvar_68;
  shadow_2 = (tmpvar_65 + tmpvar_66);
  mediump vec4 tmpvar_70;
  tmpvar_70 = vec4(shadow_2);
  tmpvar_1 = tmpvar_70;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 unity_CameraInvProjection;
uniform 	mat4x4 glstate_matrix_mvp;
in highp vec4 in_POSITION0;
in highp vec2 in_TEXCOORD0;
in highp vec3 in_NORMAL0;
out highp vec2 vs_TEXCOORD0;
out highp vec3 vs_TEXCOORD1;
out highp vec3 vs_TEXCOORD2;
out highp vec3 vs_TEXCOORD3;
vec4 u_xlat0;
vec4 u_xlat1;
void main()
{
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    vs_TEXCOORD1.xyz = in_NORMAL0.xyz;
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    u_xlat1.x = u_xlat0.y * _ProjectionParams.x;
    u_xlat1.xyz = u_xlat1.xxx * unity_CameraInvProjection[1].xyz;
    u_xlat1.xyz = unity_CameraInvProjection[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
    gl_Position = u_xlat0;
    u_xlat0.xyz = u_xlat1.xyz + (-unity_CameraInvProjection[2].xyz);
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[2].xyz;
    u_xlat1.xyz = u_xlat1.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.xyz = u_xlat0.xyz + unity_CameraInvProjection[3].xyz;
    u_xlat0.w = (-u_xlat0.z);
    vs_TEXCOORD2.xyz = u_xlat0.xyw;
    u_xlat1.w = (-u_xlat1.z);
    vs_TEXCOORD3.xyz = u_xlat1.xyw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	vec4 _ZBufferParams;
uniform 	vec4 unity_OrthoParams;
uniform 	mat4x4 unity_World2Shadow[4];
uniform 	mediump vec4 _LightShadowData;
uniform 	vec4 unity_ShadowFadeCenterAndType;
uniform 	mat4x4 _CameraToWorld;
uniform 	vec4 _ShadowMapTexture_TexelSize;
uniform highp sampler2D _CameraDepthTexture;
uniform lowp sampler2DShadow hlslcc_zcmp_ShadowMapTexture;
uniform lowp sampler2D _ShadowMapTexture;
in highp vec2 vs_TEXCOORD0;
in highp vec3 vs_TEXCOORD1;
in highp vec3 vs_TEXCOORD2;
in highp vec3 vs_TEXCOORD3;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
lowp float u_xlat10_0;
vec4 u_xlat1;
lowp float u_xlat10_1;
vec4 u_xlat2;
vec4 u_xlat3;
vec4 u_xlat4;
vec4 u_xlat5;
vec3 u_xlat6;
vec4 u_xlat7;
float u_xlat8;
mediump float u_xlat16_8;
lowp float u_xlat10_8;
vec3 u_xlat9;
float u_xlat16;
lowp float u_xlat10_16;
vec2 u_xlat17;
float u_xlat24;
void main()
{
    u_xlat0.xz = _ShadowMapTexture_TexelSize.yy;
    u_xlat0.y = 0.142857149;
    u_xlat24 = texture(_CameraDepthTexture, vs_TEXCOORD0.xy).x;
    u_xlat1.x = _ZBufferParams.x * u_xlat24 + _ZBufferParams.y;
    u_xlat1.x = float(1.0) / u_xlat1.x;
    u_xlat9.x = u_xlat24 + (-u_xlat1.x);
    u_xlat1.x = unity_OrthoParams.w * u_xlat9.x + u_xlat1.x;
    u_xlat9.xyz = (-vs_TEXCOORD2.xyz) + vs_TEXCOORD3.xyz;
    u_xlat9.xyz = vec3(u_xlat24) * u_xlat9.xyz + vs_TEXCOORD2.xyz;
    u_xlat9.xyz = (-vs_TEXCOORD1.xyz) * u_xlat1.xxx + u_xlat9.xyz;
    u_xlat2.xyz = u_xlat1.xxx * vs_TEXCOORD1.xyz;
    u_xlat1.xyz = unity_OrthoParams.www * u_xlat9.xyz + u_xlat2.xyz;
    u_xlat2 = u_xlat1.yyyy * _CameraToWorld[1];
    u_xlat2 = _CameraToWorld[0] * u_xlat1.xxxx + u_xlat2;
    u_xlat1 = _CameraToWorld[2] * u_xlat1.zzzz + u_xlat2;
    u_xlat1 = u_xlat1 + _CameraToWorld[3];
    u_xlat2.xyz = u_xlat1.yyy * unity_World2Shadow[0][1].xyz;
    u_xlat2.xyz = unity_World2Shadow[0][0].xyz * u_xlat1.xxx + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[0][2].xyz * u_xlat1.zzz + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[0][3].xyz * u_xlat1.www + u_xlat2.xyz;
    u_xlat1.xyz = u_xlat1.xyz + (-unity_ShadowFadeCenterAndType.xyz);
    u_xlat24 = dot(u_xlat1.xyz, u_xlat1.xyz);
    u_xlat24 = sqrt(u_xlat24);
    u_xlat24 = u_xlat24 * _LightShadowData.z + _LightShadowData.w;
#ifdef UNITY_ADRENO_ES3
    u_xlat24 = min(max(u_xlat24, 0.0), 1.0);
#else
    u_xlat24 = clamp(u_xlat24, 0.0, 1.0);
#endif
    u_xlat1.xy = u_xlat2.xy * _ShadowMapTexture_TexelSize.zw + vec2(0.5, 0.5);
    u_xlat17.xy = fract(u_xlat1.xy);
    u_xlat1.xy = floor(u_xlat1.xy);
    u_xlat1.xy = u_xlat1.xy + vec2(-0.5, -0.5);
    u_xlat2.xy = (-u_xlat17.xy) * vec2(2.0, 2.0) + vec2(3.0, 3.0);
    u_xlat3.xy = (-u_xlat17.xy) * vec2(3.0, 3.0) + vec2(4.0, 4.0);
    u_xlat2.xy = u_xlat2.xy / u_xlat3.xy;
    u_xlat4.xy = u_xlat2.xy + vec2(-2.0, -2.0);
    u_xlat5.z = u_xlat4.y;
    u_xlat2.xy = u_xlat17.xy * vec2(3.0, 3.0) + vec2(1.0, 1.0);
    u_xlat2.xw = u_xlat17.xy / u_xlat2.xy;
    u_xlat5.xw = u_xlat2.xw + vec2(2.0, 2.0);
    u_xlat4.w = u_xlat5.x;
    u_xlat2.xw = u_xlat17.xy + vec2(3.0, 3.0);
    u_xlat17.x = u_xlat17.x * 3.0;
    u_xlat6.xz = u_xlat17.xx * vec2(-1.0, 1.0) + vec2(4.0, 1.0);
    u_xlat5.xy = u_xlat2.xw * _ShadowMapTexture_TexelSize.xy;
    u_xlat7.xyz = vec3(u_xlat0.x * u_xlat5.z, u_xlat0.y * u_xlat5.y, u_xlat0.z * u_xlat5.w);
    u_xlat4.z = u_xlat5.x;
    u_xlat5.w = u_xlat7.x;
    u_xlat0.xz = _ShadowMapTexture_TexelSize.xx;
    u_xlat0.y = 0.142857149;
    u_xlat5.xyz = u_xlat0.yxz * u_xlat4.zxw;
    u_xlat4 = u_xlat1.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat5.ywxw;
    u_xlat0.xy = u_xlat1.xy * _ShadowMapTexture_TexelSize.xy + u_xlat5.zw;
    vec3 txVec3 = vec3(u_xlat0.xy,u_xlat2.z);
    u_xlat10_0 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec3, 0.0);
    vec3 txVec4 = vec3(u_xlat4.xy,u_xlat2.z);
    u_xlat10_8 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec4, 0.0);
    vec3 txVec5 = vec3(u_xlat4.zw,u_xlat2.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec5, 0.0);
    u_xlat6.y = 7.0;
    u_xlat3.xyz = u_xlat3.yyy * u_xlat6.xyz;
    u_xlat2.xyw = u_xlat2.yyy * u_xlat6.xyz;
    u_xlat17.xy = u_xlat6.xz * vec2(7.0, 7.0);
    u_xlat16 = u_xlat10_16 * u_xlat3.y;
    u_xlat8 = u_xlat3.x * u_xlat10_8 + u_xlat16;
    u_xlat0.x = u_xlat3.z * u_xlat10_0 + u_xlat8;
    u_xlat7.w = u_xlat5.y;
    u_xlat3 = u_xlat1.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat7.wywz;
    u_xlat5.yw = u_xlat7.yz;
    vec3 txVec6 = vec3(u_xlat3.xy,u_xlat2.z);
    u_xlat10_8 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec6, 0.0);
    vec3 txVec7 = vec3(u_xlat3.zw,u_xlat2.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec7, 0.0);
    u_xlat0.x = u_xlat17.x * u_xlat10_8 + u_xlat0.x;
    u_xlat3 = u_xlat1.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat5.xyzy;
    u_xlat4 = u_xlat1.xyxy * _ShadowMapTexture_TexelSize.xyxy + u_xlat5.xwzw;
    vec3 txVec8 = vec3(u_xlat3.xy,u_xlat2.z);
    u_xlat10_8 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec8, 0.0);
    vec3 txVec9 = vec3(u_xlat3.zw,u_xlat2.z);
    u_xlat10_1 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec9, 0.0);
    u_xlat0.x = u_xlat10_8 * 49.0 + u_xlat0.x;
    u_xlat0.x = u_xlat17.y * u_xlat10_1 + u_xlat0.x;
    u_xlat0.x = u_xlat2.x * u_xlat10_16 + u_xlat0.x;
    vec3 txVec10 = vec3(u_xlat4.xy,u_xlat2.z);
    u_xlat10_8 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec10, 0.0);
    vec3 txVec11 = vec3(u_xlat4.zw,u_xlat2.z);
    u_xlat10_16 = textureLod(hlslcc_zcmp_ShadowMapTexture, txVec11, 0.0);
    u_xlat0.x = u_xlat2.y * u_xlat10_8 + u_xlat0.x;
    u_xlat0.x = u_xlat2.w * u_xlat10_16 + u_xlat0.x;
    u_xlat0.x = u_xlat0.x * 0.0069444445;
    u_xlat16_8 = (-_LightShadowData.x) + 1.0;
    u_xlat0.x = u_xlat0.x * u_xlat16_8 + _LightShadowData.x;
    u_xlat0 = vec4(u_xlat24) + u_xlat0.xxxx;
    SV_Target0 = u_xlat0;
    return;
}
#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" }
""
}
SubProgram "gles3 " {
Keywords { "SHADOWS_NATIVE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
""
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_SINGLE_CASCADE" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
""
}
SubProgram "gles3 " {
Keywords { "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_SINGLE_CASCADE" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
""
}
SubProgram "gles3 " {
Keywords { "SHADOWS_SPLIT_SPHERES" "SHADOWS_NATIVE" "SHADOWS_SINGLE_CASCADE" }
""
}
}
 }
}
Fallback Off
}