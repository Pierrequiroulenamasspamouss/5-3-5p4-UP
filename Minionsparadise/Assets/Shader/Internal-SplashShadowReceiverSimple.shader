//////////////////////////////////////////
///////////////////////////////////////////
Shader "Hidden/InternalSplashShadowReceiverSimple" {
SubShader { 
 Pass {
  Cull Off
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
uniform highp mat4 unity_World2Shadow[4];
uniform highp mat4 glstate_matrix_mvp;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.xyz = _glesVertex.xyz;
  highp vec4 tmpvar_2;
  tmpvar_2 = (unity_World2Shadow[0] * tmpvar_1);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesNormal;
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_TEXCOORD2 = (((tmpvar_2.xy / tmpvar_2.w) * 0.5) + 0.5);
}


#endif
#ifdef FRAGMENT
uniform mediump vec3 unity_LightColor0;
uniform mediump vec3 unity_LightColor1;
uniform highp mat4 unity_World2Shadow[4];
uniform sampler2D unity_SplashScreenShadowTex0;
uniform sampler2D unity_SplashScreenShadowTex1;
uniform highp vec3 unity_LightPosition0;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying highp vec2 xlv_TEXCOORD2;
void main ()
{
  lowp vec4 tmpvar_1;
  lowp float shadowedIntensity_2;
  highp vec4 planeShadows_3;
  lowp vec4 shadowSample1_4;
  lowp vec4 shadowSample0_5;
  shadowSample0_5 = texture2D (unity_SplashScreenShadowTex0, xlv_TEXCOORD2);
  shadowSample1_4 = texture2D (unity_SplashScreenShadowTex1, xlv_TEXCOORD2);
  planeShadows_3 = vec4(0.0, 0.0, 0.0, 0.0);
  highp mat4 m_6;
  m_6 = unity_World2Shadow[1];
  highp vec4 v_7;
  v_7.x = m_6[0].x;
  v_7.y = m_6[1].x;
  v_7.z = m_6[2].x;
  v_7.w = m_6[3].x;
  highp float tmpvar_8;
  tmpvar_8 = dot (v_7, xlv_TEXCOORD1);
  if ((tmpvar_8 > 0.5)) {
    lowp vec4 weightedShadowSample_9;
    highp float tmpvar_10;
    tmpvar_10 = clamp ((1.0 - (tmpvar_8 * 0.25)), 0.0, 1.0);
    highp vec4 tmpvar_11;
    tmpvar_11 = ((shadowSample0_5 * tmpvar_10) + (shadowSample1_4 * (1.0 - tmpvar_10)));
    weightedShadowSample_9 = tmpvar_11;
    lowp vec4 tmpvar_12;
    tmpvar_12.yzw = vec3(0.0, 0.0, 0.0);
    tmpvar_12.x = weightedShadowSample_9.x;
    planeShadows_3 = tmpvar_12;
  };
  highp mat4 m_13;
  m_13 = unity_World2Shadow[1];
  highp vec4 v_14;
  v_14.x = m_13[0].y;
  v_14.y = m_13[1].y;
  v_14.z = m_13[2].y;
  v_14.w = m_13[3].y;
  highp float tmpvar_15;
  tmpvar_15 = dot (v_14, xlv_TEXCOORD1);
  if ((tmpvar_15 > 0.5)) {
    lowp vec4 weightedShadowSample_16;
    highp float tmpvar_17;
    tmpvar_17 = clamp ((1.0 - (tmpvar_15 * 0.25)), 0.0, 1.0);
    highp vec4 tmpvar_18;
    tmpvar_18 = ((shadowSample0_5 * tmpvar_17) + (shadowSample1_4 * (1.0 - tmpvar_17)));
    weightedShadowSample_16 = tmpvar_18;
    highp vec4 tmpvar_19;
    tmpvar_19.xzw = planeShadows_3.xzw;
    tmpvar_19.y = weightedShadowSample_16.y;
    planeShadows_3 = tmpvar_19;
  };
  highp mat4 m_20;
  m_20 = unity_World2Shadow[1];
  highp vec4 v_21;
  v_21.x = m_20[0].z;
  v_21.y = m_20[1].z;
  v_21.z = m_20[2].z;
  v_21.w = m_20[3].z;
  highp float tmpvar_22;
  tmpvar_22 = dot (v_21, xlv_TEXCOORD1);
  if ((tmpvar_22 > 0.5)) {
    lowp vec4 weightedShadowSample_23;
    highp float tmpvar_24;
    tmpvar_24 = clamp ((1.0 - (tmpvar_22 * 0.25)), 0.0, 1.0);
    highp vec4 tmpvar_25;
    tmpvar_25 = ((shadowSample0_5 * tmpvar_24) + (shadowSample1_4 * (1.0 - tmpvar_24)));
    weightedShadowSample_23 = tmpvar_25;
    highp vec4 tmpvar_26;
    tmpvar_26.xyw = planeShadows_3.xyw;
    tmpvar_26.z = weightedShadowSample_23.z;
    planeShadows_3 = tmpvar_26;
  };
  highp mat4 m_27;
  m_27 = unity_World2Shadow[1];
  highp vec4 v_28;
  v_28.x = m_27[0].w;
  v_28.y = m_27[1].w;
  v_28.z = m_27[2].w;
  v_28.w = m_27[3].w;
  highp float tmpvar_29;
  tmpvar_29 = dot (v_28, xlv_TEXCOORD1);
  if ((tmpvar_29 > 0.5)) {
    lowp vec4 weightedShadowSample_30;
    highp float tmpvar_31;
    tmpvar_31 = clamp ((1.0 - (tmpvar_29 * 0.25)), 0.0, 1.0);
    highp vec4 tmpvar_32;
    tmpvar_32 = ((shadowSample0_5 * tmpvar_31) + (shadowSample1_4 * (1.0 - tmpvar_31)));
    weightedShadowSample_30 = tmpvar_32;
    highp vec4 tmpvar_33;
    tmpvar_33.xyz = planeShadows_3.xyz;
    tmpvar_33.w = weightedShadowSample_30.w;
    planeShadows_3 = tmpvar_33;
  };
  highp float tmpvar_34;
  tmpvar_34 = (pow (clamp (
    dot (xlv_TEXCOORD0, normalize((unity_LightPosition0 - xlv_TEXCOORD1.xyz)))
  , 0.0, 1.0), 3.0) * (1.0 - max (
    max (planeShadows_3.x, planeShadows_3.y)
  , 
    max (planeShadows_3.z, planeShadows_3.w)
  )));
  shadowedIntensity_2 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35.w = 1.0;
  tmpvar_35.xyz = mix (unity_LightColor1, unity_LightColor0, vec3(shadowedIntensity_2));
  tmpvar_1 = tmpvar_35;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	mat4x4 unity_World2Shadow[4];
uniform 	mat4x4 glstate_matrix_mvp;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
out highp vec3 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
out highp vec2 vs_TEXCOORD2;
vec4 u_xlat0;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    vs_TEXCOORD0.xyz = in_NORMAL0.xyz;
    vs_TEXCOORD1.xyz = in_POSITION0.xyz;
    vs_TEXCOORD1.w = 1.0;
    u_xlat0.xyz = in_POSITION0.yyy * unity_World2Shadow[0][1].xyw;
    u_xlat0.xyz = unity_World2Shadow[0][0].xyw * in_POSITION0.xxx + u_xlat0.xyz;
    u_xlat0.xyz = unity_World2Shadow[0][2].xyw * in_POSITION0.zzz + u_xlat0.xyz;
    u_xlat0.xyz = u_xlat0.xyz + unity_World2Shadow[0][3].xyw;
    u_xlat0.xy = u_xlat0.xy / u_xlat0.zz;
    vs_TEXCOORD2.xy = u_xlat0.xy * vec2(0.5, 0.5) + vec2(0.5, 0.5);
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	mediump vec3 unity_LightColor0;
uniform 	mediump vec3 unity_LightColor1;
uniform 	mat4x4 unity_World2Shadow[4];
uniform 	vec3 unity_LightPosition0;
uniform lowp sampler2D unity_SplashScreenShadowTex0;
uniform lowp sampler2D unity_SplashScreenShadowTex1;
in highp vec3 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
in highp vec2 vs_TEXCOORD2;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
float u_xlat1;
lowp vec4 u_xlat10_1;
lowp vec4 u_xlat10_2;
vec4 u_xlat3;
mediump vec3 u_xlat16_4;
vec3 u_xlat5;
bool u_xlatb5;
float u_xlat10;
bool u_xlatb10;
float u_xlat15;
void main()
{
    u_xlat0.x = unity_World2Shadow[1][0].x;
    u_xlat0.y = unity_World2Shadow[1][1].x;
    u_xlat0.z = unity_World2Shadow[1][2].x;
    u_xlat0.w = unity_World2Shadow[1][3].x;
    u_xlat0.x = dot(u_xlat0, vs_TEXCOORD1);
#ifdef UNITY_ADRENO_ES3
    u_xlatb5 = !!(0.5<u_xlat0.x);
#else
    u_xlatb5 = 0.5<u_xlat0.x;
#endif
    u_xlat0.x = (-u_xlat0.x) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    u_xlat10 = (-u_xlat0.x) + 1.0;
    u_xlat10_1 = texture(unity_SplashScreenShadowTex1, vs_TEXCOORD2.xy);
    u_xlat10 = u_xlat10 * u_xlat10_1.x;
    u_xlat10_2 = texture(unity_SplashScreenShadowTex0, vs_TEXCOORD2.xy);
    u_xlat0.x = u_xlat10_2.x * u_xlat0.x + u_xlat10;
    u_xlat0.x = u_xlatb5 ? u_xlat0.x : float(0.0);
    u_xlat3.x = unity_World2Shadow[1][0].y;
    u_xlat3.y = unity_World2Shadow[1][1].y;
    u_xlat3.z = unity_World2Shadow[1][2].y;
    u_xlat3.w = unity_World2Shadow[1][3].y;
    u_xlat5.x = dot(u_xlat3, vs_TEXCOORD1);
#ifdef UNITY_ADRENO_ES3
    u_xlatb10 = !!(0.5<u_xlat5.x);
#else
    u_xlatb10 = 0.5<u_xlat5.x;
#endif
    u_xlat5.x = (-u_xlat5.x) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat5.x = min(max(u_xlat5.x, 0.0), 1.0);
#else
    u_xlat5.x = clamp(u_xlat5.x, 0.0, 1.0);
#endif
    u_xlat15 = (-u_xlat5.x) + 1.0;
    u_xlat15 = u_xlat15 * u_xlat10_1.y;
    u_xlat5.x = u_xlat10_2.y * u_xlat5.x + u_xlat15;
    u_xlat5.x = u_xlatb10 ? u_xlat5.x : float(0.0);
    u_xlat0.x = max(u_xlat5.x, u_xlat0.x);
    u_xlat3.x = unity_World2Shadow[1][0].z;
    u_xlat3.y = unity_World2Shadow[1][1].z;
    u_xlat3.z = unity_World2Shadow[1][2].z;
    u_xlat3.w = unity_World2Shadow[1][3].z;
    u_xlat5.x = dot(u_xlat3, vs_TEXCOORD1);
    u_xlat10 = (-u_xlat5.x) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat10 = min(max(u_xlat10, 0.0), 1.0);
#else
    u_xlat10 = clamp(u_xlat10, 0.0, 1.0);
#endif
#ifdef UNITY_ADRENO_ES3
    u_xlatb5 = !!(0.5<u_xlat5.x);
#else
    u_xlatb5 = 0.5<u_xlat5.x;
#endif
    u_xlat15 = (-u_xlat10) + 1.0;
    u_xlat15 = u_xlat15 * u_xlat10_1.z;
    u_xlat10 = u_xlat10_2.z * u_xlat10 + u_xlat15;
    u_xlat5.x = u_xlatb5 ? u_xlat10 : float(0.0);
    u_xlat3.x = unity_World2Shadow[1][0].w;
    u_xlat3.y = unity_World2Shadow[1][1].w;
    u_xlat3.z = unity_World2Shadow[1][2].w;
    u_xlat3.w = unity_World2Shadow[1][3].w;
    u_xlat10 = dot(u_xlat3, vs_TEXCOORD1);
    u_xlat15 = (-u_xlat10) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat15 = min(max(u_xlat15, 0.0), 1.0);
#else
    u_xlat15 = clamp(u_xlat15, 0.0, 1.0);
#endif
#ifdef UNITY_ADRENO_ES3
    u_xlatb10 = !!(0.5<u_xlat10);
#else
    u_xlatb10 = 0.5<u_xlat10;
#endif
    u_xlat1 = (-u_xlat15) + 1.0;
    u_xlat1 = u_xlat1 * u_xlat10_1.w;
    u_xlat15 = u_xlat10_2.w * u_xlat15 + u_xlat1;
    u_xlat10 = u_xlatb10 ? u_xlat15 : float(0.0);
    u_xlat5.x = max(u_xlat10, u_xlat5.x);
    u_xlat0.x = max(u_xlat5.x, u_xlat0.x);
    u_xlat0.x = (-u_xlat0.x) + 1.0;
    u_xlat5.xyz = (-vs_TEXCOORD1.xyz) + unity_LightPosition0.xyz;
    u_xlat1 = dot(u_xlat5.xyz, u_xlat5.xyz);
    u_xlat1 = inversesqrt(u_xlat1);
    u_xlat5.xyz = u_xlat5.xyz * vec3(u_xlat1);
    u_xlat5.x = dot(vs_TEXCOORD0.xyz, u_xlat5.xyz);
#ifdef UNITY_ADRENO_ES3
    u_xlat5.x = min(max(u_xlat5.x, 0.0), 1.0);
#else
    u_xlat5.x = clamp(u_xlat5.x, 0.0, 1.0);
#endif
    u_xlat10 = u_xlat5.x * u_xlat5.x;
    u_xlat5.x = u_xlat10 * u_xlat5.x;
    u_xlat0.x = u_xlat0.x * u_xlat5.x;
    u_xlat16_4.xyz = unity_LightColor0.xyz + (-unity_LightColor1.xyz);
    u_xlat16_4.xyz = u_xlat0.xxx * u_xlat16_4.xyz + unity_LightColor1.xyz;
    SV_Target0.xyz = u_xlat16_4.xyz;
    SV_Target0.w = 1.0;
    return;
}
#endif
"
}
}
Program "fp" {
SubProgram "gles " {
""
}
SubProgram "gles3 " {
""
}
}
 }
}
Fallback Off
}