//////////////////////////////////////////
///////////////////////////////////////////
Shader "Hidden/InternalSplashShadowReceiver" {
SubShader { 
 Pass {
  Cull Off
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1.w = 1.0;
  tmpvar_1.xyz = _glesVertex.xyz;
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesNormal;
  xlv_TEXCOORD1 = tmpvar_1;
  xlv_COLOR = _glesColor;
}


#endif
#ifdef FRAGMENT
uniform mediump vec3 unity_LightColor0;
uniform mediump vec3 unity_LightColor1;
uniform highp mat4 unity_World2Shadow[4];
uniform sampler2D unity_SplashScreenShadowTex0;
uniform sampler2D unity_SplashScreenShadowTex1;
uniform sampler2D unity_SplashScreenShadowTex2;
uniform sampler2D unity_SplashScreenShadowTex3;
uniform sampler2D unity_SplashScreenShadowTex4;
uniform sampler2D unity_SplashScreenShadowTex5;
uniform sampler2D unity_SplashScreenShadowTex6;
uniform sampler2D unity_SplashScreenShadowTex7;
uniform sampler2D unity_SplashScreenShadowTex8;
uniform highp vec3 unity_LightPosition0;
varying highp vec3 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
varying lowp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec4 tmpvar_2;
  tmpvar_2 = xlv_TEXCOORD1;
  lowp float shadowedIntensity_3;
  highp float biasedDepth_5;
  highp int equationIndex_6;
  highp int equationMatrixIndex_7;
  lowp vec4 weightedShadowSample_8;
  highp vec4 planeShadows3_9;
  highp vec4 planeShadows2_10;
  highp vec4 planeShadows1_11;
  lowp vec4 shadowSample8_12;
  lowp vec4 shadowSample7_13;
  lowp vec4 shadowSample6_14;
  lowp vec4 shadowSample5_15;
  lowp vec4 shadowSample4_16;
  lowp vec4 shadowSample3_17;
  lowp vec4 shadowSample2_18;
  lowp vec4 shadowSample1_19;
  lowp vec4 shadowSample0_20;
  highp vec4 tmpvar_21;
  tmpvar_21 = (unity_World2Shadow[0] * xlv_TEXCOORD1);
  highp vec2 tmpvar_22;
  tmpvar_22 = (((tmpvar_21.xy / tmpvar_21.w) * 0.5) + 0.5);
  shadowSample0_20 = texture2D (unity_SplashScreenShadowTex0, tmpvar_22);
  shadowSample1_19 = texture2D (unity_SplashScreenShadowTex1, tmpvar_22);
  shadowSample2_18 = texture2D (unity_SplashScreenShadowTex2, tmpvar_22);
  shadowSample3_17 = texture2D (unity_SplashScreenShadowTex3, tmpvar_22);
  shadowSample4_16 = texture2D (unity_SplashScreenShadowTex4, tmpvar_22);
  shadowSample5_15 = texture2D (unity_SplashScreenShadowTex5, tmpvar_22);
  shadowSample6_14 = texture2D (unity_SplashScreenShadowTex6, tmpvar_22);
  shadowSample7_13 = texture2D (unity_SplashScreenShadowTex7, tmpvar_22);
  shadowSample8_12 = texture2D (unity_SplashScreenShadowTex8, tmpvar_22);
  planeShadows1_11 = vec4(0.0, 0.0, 0.0, 0.0);
  planeShadows2_10 = vec4(0.0, 0.0, 0.0, 0.0);
  planeShadows3_9 = vec4(0.0, 0.0, 0.0, 0.0);
  equationMatrixIndex_7 = 1;
  equationIndex_6 = 0;
  for (highp int planeIndex_4 = 0; planeIndex_4 < 12; planeIndex_4++) {
    equationIndex_6 = planeIndex_4;
    if ((planeIndex_4 >= 8)) {
      equationMatrixIndex_7 = 3;
      equationIndex_6 = (planeIndex_4 - 8);
    } else {
      if ((planeIndex_4 >= 4)) {
        equationMatrixIndex_7 = 2;
        equationIndex_6 = (equationIndex_6 - 4);
      };
    };
    highp mat4 m_23;
    m_23 = unity_World2Shadow[equationMatrixIndex_7];
    highp vec4 v_24;
    v_24.x = m_23[0][equationIndex_6];
    v_24.y = m_23[1][equationIndex_6];
    v_24.z = m_23[2][equationIndex_6];
    v_24.w = m_23[3][equationIndex_6];
    biasedDepth_5 = (dot (v_24, tmpvar_2) - 1.0);
    if ((biasedDepth_5 > 0.0)) {
      highp float tmpvar_25;
      tmpvar_25 = clamp ((biasedDepth_5 * 0.5), 0.0, 1.0);
      highp float tmpvar_26;
      tmpvar_26 = clamp (((biasedDepth_5 - 2.0) / 48.0), 0.0, 1.0);
      highp float tmpvar_27;
      tmpvar_27 = (biasedDepth_5 * 0.25);
      highp float tmpvar_28;
      tmpvar_28 = clamp ((1.0 - tmpvar_27), 0.0, 1.0);
      highp float tmpvar_29;
      tmpvar_29 = clamp ((tmpvar_27 - 1.0), 0.0, 1.0);
      highp float tmpvar_30;
      tmpvar_30 = (1.0 - (tmpvar_28 + tmpvar_29));
      if ((equationMatrixIndex_7 == 1)) {
        weightedShadowSample_8 = (((shadowSample0_20 * tmpvar_28) + (shadowSample1_19 * tmpvar_30)) + (shadowSample2_18 * tmpvar_29));
        planeShadows1_11[equationIndex_6] = (((
          weightedShadowSample_8[equationIndex_6]
         * tmpvar_25) - tmpvar_26) * (1.0 + tmpvar_26));
      } else {
        if ((equationMatrixIndex_7 == 2)) {
          weightedShadowSample_8 = (((shadowSample3_17 * tmpvar_28) + (shadowSample4_16 * tmpvar_30)) + (shadowSample5_15 * tmpvar_29));
          planeShadows2_10[equationIndex_6] = (((
            weightedShadowSample_8[equationIndex_6]
           * tmpvar_25) - tmpvar_26) * (1.0 + tmpvar_26));
        } else {
          weightedShadowSample_8 = (((shadowSample6_14 * tmpvar_28) + (shadowSample7_13 * tmpvar_30)) + (shadowSample8_12 * tmpvar_29));
          planeShadows3_9[equationIndex_6] = (((
            weightedShadowSample_8[equationIndex_6]
           * tmpvar_25) - tmpvar_26) * (1.0 + tmpvar_26));
        };
      };
    };
  };
  highp float tmpvar_31;
  tmpvar_31 = (pow (clamp (
    dot (xlv_TEXCOORD0, normalize((unity_LightPosition0 - xlv_TEXCOORD1.xyz)))
  , 0.0, 1.0), 3.0) * (1.0 - clamp (
    max (max (max (max (planeShadows1_11.x, planeShadows1_11.y), max (planeShadows1_11.z, planeShadows1_11.w)), max (max (planeShadows2_10.x, planeShadows2_10.y), max (planeShadows2_10.z, planeShadows2_10.w))), max (max (planeShadows3_9.x, planeShadows3_9.y), max (planeShadows3_9.z, planeShadows3_9.w)))
  , 0.0, 1.0)));
  shadowedIntensity_3 = tmpvar_31;
  mediump vec4 tmpvar_32;
  tmpvar_32.w = 1.0;
  tmpvar_32.xyz = mix (unity_LightColor1, unity_LightColor0, vec3(shadowedIntensity_3));
  tmpvar_1 = (tmpvar_32 * xlv_COLOR);
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	mat4x4 glstate_matrix_mvp;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in lowp vec4 in_COLOR0;
out highp vec3 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
out lowp vec4 vs_COLOR0;
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
    vs_COLOR0 = in_COLOR0;
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
uniform lowp sampler2D unity_SplashScreenShadowTex2;
uniform lowp sampler2D unity_SplashScreenShadowTex3;
uniform lowp sampler2D unity_SplashScreenShadowTex4;
uniform lowp sampler2D unity_SplashScreenShadowTex5;
uniform lowp sampler2D unity_SplashScreenShadowTex6;
uniform lowp sampler2D unity_SplashScreenShadowTex7;
uniform lowp sampler2D unity_SplashScreenShadowTex8;
in highp vec3 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
in lowp vec4 vs_COLOR0;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
vec4 u_xlat1;
vec3 u_xlat2;
lowp vec4 u_xlat10_2;
lowp vec4 u_xlat10_3;
lowp vec4 u_xlat10_4;
lowp vec4 u_xlat10_5;
mediump vec3 u_xlat16_6;
vec3 u_xlat7;
float u_xlat8;
float u_xlat14;
float u_xlat15;
bool u_xlatb15;
float u_xlat16;
float u_xlat21;
bool u_xlatb21;
float u_xlat22;
void main()
{
    u_xlat0.x = unity_World2Shadow[1][0].x;
    u_xlat0.y = unity_World2Shadow[1][1].x;
    u_xlat0.z = unity_World2Shadow[1][2].x;
    u_xlat0.w = unity_World2Shadow[1][3].x;
    u_xlat0.x = dot(u_xlat0, vs_TEXCOORD1);
    u_xlat0.xyz = u_xlat0.xxx + vec3(-1.0, -3.0, -1.0);
#ifdef UNITY_ADRENO_ES3
    u_xlatb21 = !!(0.0<u_xlat0.z);
#else
    u_xlatb21 = 0.0<u_xlat0.z;
#endif
    u_xlat0.xy = u_xlat0.xy * vec2(0.5, 0.020833334);
    u_xlat0.xy = u_xlat0.xy;
#ifdef UNITY_ADRENO_ES3
    u_xlat0.xy = min(max(u_xlat0.xy, 0.0), 1.0);
#else
    u_xlat0.xy = clamp(u_xlat0.xy, 0.0, 1.0);
#endif
    u_xlat1.x = u_xlat0.y + 1.0;
    u_xlat8 = u_xlat0.z * 0.25 + -1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat8 = min(max(u_xlat8, 0.0), 1.0);
#else
    u_xlat8 = clamp(u_xlat8, 0.0, 1.0);
#endif
    u_xlat14 = (-u_xlat0.z) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat14 = min(max(u_xlat14, 0.0), 1.0);
#else
    u_xlat14 = clamp(u_xlat14, 0.0, 1.0);
#endif
    u_xlat15 = u_xlat8 + u_xlat14;
    u_xlat15 = (-u_xlat15) + 1.0;
    u_xlat2.xyz = vs_TEXCOORD1.yyy * unity_World2Shadow[0][1].xyw;
    u_xlat2.xyz = unity_World2Shadow[0][0].xyw * vs_TEXCOORD1.xxx + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[0][2].xyw * vs_TEXCOORD1.zzz + u_xlat2.xyz;
    u_xlat2.xyz = unity_World2Shadow[0][3].xyw * vs_TEXCOORD1.www + u_xlat2.xyz;
    u_xlat2.xy = u_xlat2.xy / u_xlat2.zz;
    u_xlat2.xy = u_xlat2.xy * vec2(0.5, 0.5) + vec2(0.5, 0.5);
    u_xlat10_3 = texture(unity_SplashScreenShadowTex1, u_xlat2.xy);
    u_xlat15 = u_xlat15 * u_xlat10_3.x;
    u_xlat10_4 = texture(unity_SplashScreenShadowTex0, u_xlat2.xy);
    u_xlat14 = u_xlat10_4.x * u_xlat14 + u_xlat15;
    u_xlat10_5 = texture(unity_SplashScreenShadowTex2, u_xlat2.xy);
    u_xlat14 = u_xlat10_5.x * u_xlat8 + u_xlat14;
    u_xlat0.x = u_xlat14 * u_xlat0.x + (-u_xlat0.y);
    u_xlat0.x = u_xlat1.x * u_xlat0.x;
    u_xlat0.x = u_xlatb21 ? u_xlat0.x : float(0.0);
    u_xlat1.x = unity_World2Shadow[1][0].y;
    u_xlat1.y = unity_World2Shadow[1][1].y;
    u_xlat1.z = unity_World2Shadow[1][2].y;
    u_xlat1.w = unity_World2Shadow[1][3].y;
    u_xlat7.x = dot(u_xlat1, vs_TEXCOORD1);
    u_xlat7.xyz = u_xlat7.xxx + vec3(-1.0, -3.0, -1.0);
    u_xlat1.x = (-u_xlat7.z) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.x = min(max(u_xlat1.x, 0.0), 1.0);
#else
    u_xlat1.x = clamp(u_xlat1.x, 0.0, 1.0);
#endif
    u_xlat8 = u_xlat7.z * 0.25 + -1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat8 = min(max(u_xlat8, 0.0), 1.0);
#else
    u_xlat8 = clamp(u_xlat8, 0.0, 1.0);
#endif
    u_xlat15 = u_xlat8 + u_xlat1.x;
    u_xlat15 = (-u_xlat15) + 1.0;
    u_xlat15 = u_xlat15 * u_xlat10_3.y;
    u_xlat1.x = u_xlat10_4.y * u_xlat1.x + u_xlat15;
    u_xlat1.x = u_xlat10_5.y * u_xlat8 + u_xlat1.x;
    u_xlat7.xy = u_xlat7.xy * vec2(0.5, 0.020833334);
#ifdef UNITY_ADRENO_ES3
    u_xlatb21 = !!(0.0<u_xlat7.z);
#else
    u_xlatb21 = 0.0<u_xlat7.z;
#endif
    u_xlat7.xy = u_xlat7.xy;
#ifdef UNITY_ADRENO_ES3
    u_xlat7.xy = min(max(u_xlat7.xy, 0.0), 1.0);
#else
    u_xlat7.xy = clamp(u_xlat7.xy, 0.0, 1.0);
#endif
    u_xlat7.x = u_xlat1.x * u_xlat7.x + (-u_xlat7.y);
    u_xlat14 = u_xlat7.y + 1.0;
    u_xlat7.x = u_xlat14 * u_xlat7.x;
    u_xlat7.x = u_xlatb21 ? u_xlat7.x : float(0.0);
    u_xlat0.x = max(u_xlat7.x, u_xlat0.x);
    u_xlat1.x = unity_World2Shadow[1][0].z;
    u_xlat1.y = unity_World2Shadow[1][1].z;
    u_xlat1.z = unity_World2Shadow[1][2].z;
    u_xlat1.w = unity_World2Shadow[1][3].z;
    u_xlat7.x = dot(u_xlat1, vs_TEXCOORD1);
    u_xlat7.xyz = u_xlat7.xxx + vec3(-1.0, -3.0, -1.0);
    u_xlat1.x = (-u_xlat7.z) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.x = min(max(u_xlat1.x, 0.0), 1.0);
#else
    u_xlat1.x = clamp(u_xlat1.x, 0.0, 1.0);
#endif
    u_xlat8 = u_xlat7.z * 0.25 + -1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat8 = min(max(u_xlat8, 0.0), 1.0);
#else
    u_xlat8 = clamp(u_xlat8, 0.0, 1.0);
#endif
    u_xlat15 = u_xlat8 + u_xlat1.x;
    u_xlat15 = (-u_xlat15) + 1.0;
    u_xlat15 = u_xlat15 * u_xlat10_3.z;
    u_xlat1.x = u_xlat10_4.z * u_xlat1.x + u_xlat15;
    u_xlat1.x = u_xlat10_5.z * u_xlat8 + u_xlat1.x;
    u_xlat7.xy = u_xlat7.xy * vec2(0.5, 0.020833334);
#ifdef UNITY_ADRENO_ES3
    u_xlatb21 = !!(0.0<u_xlat7.z);
#else
    u_xlatb21 = 0.0<u_xlat7.z;
#endif
    u_xlat7.xy = u_xlat7.xy;
#ifdef UNITY_ADRENO_ES3
    u_xlat7.xy = min(max(u_xlat7.xy, 0.0), 1.0);
#else
    u_xlat7.xy = clamp(u_xlat7.xy, 0.0, 1.0);
#endif
    u_xlat7.x = u_xlat1.x * u_xlat7.x + (-u_xlat7.y);
    u_xlat14 = u_xlat7.y + 1.0;
    u_xlat7.x = u_xlat14 * u_xlat7.x;
    u_xlat7.x = u_xlatb21 ? u_xlat7.x : float(0.0);
    u_xlat1.x = unity_World2Shadow[1][0].w;
    u_xlat1.y = unity_World2Shadow[1][1].w;
    u_xlat1.z = unity_World2Shadow[1][2].w;
    u_xlat1.w = unity_World2Shadow[1][3].w;
    u_xlat14 = dot(u_xlat1, vs_TEXCOORD1);
    u_xlat1.xyz = vec3(u_xlat14) + vec3(-1.0, -3.0, -1.0);
    u_xlat14 = (-u_xlat1.z) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat14 = min(max(u_xlat14, 0.0), 1.0);
#else
    u_xlat14 = clamp(u_xlat14, 0.0, 1.0);
#endif
    u_xlat21 = u_xlat1.z * 0.25 + -1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat21 = min(max(u_xlat21, 0.0), 1.0);
#else
    u_xlat21 = clamp(u_xlat21, 0.0, 1.0);
#endif
    u_xlat22 = u_xlat21 + u_xlat14;
    u_xlat22 = (-u_xlat22) + 1.0;
    u_xlat22 = u_xlat22 * u_xlat10_3.w;
    u_xlat14 = u_xlat10_4.w * u_xlat14 + u_xlat22;
    u_xlat14 = u_xlat10_5.w * u_xlat21 + u_xlat14;
    u_xlat1.xy = u_xlat1.xy * vec2(0.5, 0.020833334);
#ifdef UNITY_ADRENO_ES3
    u_xlatb21 = !!(0.0<u_xlat1.z);
#else
    u_xlatb21 = 0.0<u_xlat1.z;
#endif
    u_xlat1.xy = u_xlat1.xy;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.xy = min(max(u_xlat1.xy, 0.0), 1.0);
#else
    u_xlat1.xy = clamp(u_xlat1.xy, 0.0, 1.0);
#endif
    u_xlat14 = u_xlat14 * u_xlat1.x + (-u_xlat1.y);
    u_xlat1.x = u_xlat1.y + 1.0;
    u_xlat14 = u_xlat14 * u_xlat1.x;
    u_xlat14 = u_xlatb21 ? u_xlat14 : float(0.0);
    u_xlat7.x = max(u_xlat14, u_xlat7.x);
    u_xlat0.x = max(u_xlat7.x, u_xlat0.x);
    u_xlat1.x = unity_World2Shadow[2][0].x;
    u_xlat1.y = unity_World2Shadow[2][1].x;
    u_xlat1.z = unity_World2Shadow[2][2].x;
    u_xlat1.w = unity_World2Shadow[2][3].x;
    u_xlat7.x = dot(u_xlat1, vs_TEXCOORD1);
    u_xlat7.xyz = u_xlat7.xxx + vec3(-1.0, -3.0, -1.0);
    u_xlat1.x = (-u_xlat7.z) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.x = min(max(u_xlat1.x, 0.0), 1.0);
#else
    u_xlat1.x = clamp(u_xlat1.x, 0.0, 1.0);
#endif
    u_xlat8 = u_xlat7.z * 0.25 + -1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat8 = min(max(u_xlat8, 0.0), 1.0);
#else
    u_xlat8 = clamp(u_xlat8, 0.0, 1.0);
#endif
    u_xlat15 = u_xlat8 + u_xlat1.x;
    u_xlat15 = (-u_xlat15) + 1.0;
    u_xlat10_3 = texture(unity_SplashScreenShadowTex4, u_xlat2.xy);
    u_xlat15 = u_xlat15 * u_xlat10_3.x;
    u_xlat10_4 = texture(unity_SplashScreenShadowTex3, u_xlat2.xy);
    u_xlat1.x = u_xlat10_4.x * u_xlat1.x + u_xlat15;
    u_xlat10_5 = texture(unity_SplashScreenShadowTex5, u_xlat2.xy);
    u_xlat1.x = u_xlat10_5.x * u_xlat8 + u_xlat1.x;
    u_xlat7.xy = u_xlat7.xy * vec2(0.5, 0.020833334);
#ifdef UNITY_ADRENO_ES3
    u_xlatb21 = !!(0.0<u_xlat7.z);
#else
    u_xlatb21 = 0.0<u_xlat7.z;
#endif
    u_xlat7.xy = u_xlat7.xy;
#ifdef UNITY_ADRENO_ES3
    u_xlat7.xy = min(max(u_xlat7.xy, 0.0), 1.0);
#else
    u_xlat7.xy = clamp(u_xlat7.xy, 0.0, 1.0);
#endif
    u_xlat7.x = u_xlat1.x * u_xlat7.x + (-u_xlat7.y);
    u_xlat14 = u_xlat7.y + 1.0;
    u_xlat7.x = u_xlat14 * u_xlat7.x;
    u_xlat7.x = u_xlatb21 ? u_xlat7.x : float(0.0);
    u_xlat1.x = unity_World2Shadow[2][0].y;
    u_xlat1.y = unity_World2Shadow[2][1].y;
    u_xlat1.z = unity_World2Shadow[2][2].y;
    u_xlat1.w = unity_World2Shadow[2][3].y;
    u_xlat14 = dot(u_xlat1, vs_TEXCOORD1);
    u_xlat1.xyz = vec3(u_xlat14) + vec3(-1.0, -3.0, -1.0);
    u_xlat14 = (-u_xlat1.z) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat14 = min(max(u_xlat14, 0.0), 1.0);
#else
    u_xlat14 = clamp(u_xlat14, 0.0, 1.0);
#endif
    u_xlat21 = u_xlat1.z * 0.25 + -1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat21 = min(max(u_xlat21, 0.0), 1.0);
#else
    u_xlat21 = clamp(u_xlat21, 0.0, 1.0);
#endif
    u_xlat22 = u_xlat21 + u_xlat14;
    u_xlat22 = (-u_xlat22) + 1.0;
    u_xlat22 = u_xlat22 * u_xlat10_3.y;
    u_xlat14 = u_xlat10_4.y * u_xlat14 + u_xlat22;
    u_xlat14 = u_xlat10_5.y * u_xlat21 + u_xlat14;
    u_xlat1.xy = u_xlat1.xy * vec2(0.5, 0.020833334);
#ifdef UNITY_ADRENO_ES3
    u_xlatb21 = !!(0.0<u_xlat1.z);
#else
    u_xlatb21 = 0.0<u_xlat1.z;
#endif
    u_xlat1.xy = u_xlat1.xy;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.xy = min(max(u_xlat1.xy, 0.0), 1.0);
#else
    u_xlat1.xy = clamp(u_xlat1.xy, 0.0, 1.0);
#endif
    u_xlat14 = u_xlat14 * u_xlat1.x + (-u_xlat1.y);
    u_xlat1.x = u_xlat1.y + 1.0;
    u_xlat14 = u_xlat14 * u_xlat1.x;
    u_xlat14 = u_xlatb21 ? u_xlat14 : float(0.0);
    u_xlat7.x = max(u_xlat14, u_xlat7.x);
    u_xlat1.x = unity_World2Shadow[2][0].z;
    u_xlat1.y = unity_World2Shadow[2][1].z;
    u_xlat1.z = unity_World2Shadow[2][2].z;
    u_xlat1.w = unity_World2Shadow[2][3].z;
    u_xlat14 = dot(u_xlat1, vs_TEXCOORD1);
    u_xlat1.xyz = vec3(u_xlat14) + vec3(-1.0, -3.0, -1.0);
    u_xlat14 = (-u_xlat1.z) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat14 = min(max(u_xlat14, 0.0), 1.0);
#else
    u_xlat14 = clamp(u_xlat14, 0.0, 1.0);
#endif
    u_xlat21 = u_xlat1.z * 0.25 + -1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat21 = min(max(u_xlat21, 0.0), 1.0);
#else
    u_xlat21 = clamp(u_xlat21, 0.0, 1.0);
#endif
    u_xlat22 = u_xlat21 + u_xlat14;
    u_xlat22 = (-u_xlat22) + 1.0;
    u_xlat22 = u_xlat22 * u_xlat10_3.z;
    u_xlat14 = u_xlat10_4.z * u_xlat14 + u_xlat22;
    u_xlat14 = u_xlat10_5.z * u_xlat21 + u_xlat14;
    u_xlat1.xy = u_xlat1.xy * vec2(0.5, 0.020833334);
#ifdef UNITY_ADRENO_ES3
    u_xlatb21 = !!(0.0<u_xlat1.z);
#else
    u_xlatb21 = 0.0<u_xlat1.z;
#endif
    u_xlat1.xy = u_xlat1.xy;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.xy = min(max(u_xlat1.xy, 0.0), 1.0);
#else
    u_xlat1.xy = clamp(u_xlat1.xy, 0.0, 1.0);
#endif
    u_xlat14 = u_xlat14 * u_xlat1.x + (-u_xlat1.y);
    u_xlat1.x = u_xlat1.y + 1.0;
    u_xlat14 = u_xlat14 * u_xlat1.x;
    u_xlat14 = u_xlatb21 ? u_xlat14 : float(0.0);
    u_xlat1.x = unity_World2Shadow[2][0].w;
    u_xlat1.y = unity_World2Shadow[2][1].w;
    u_xlat1.z = unity_World2Shadow[2][2].w;
    u_xlat1.w = unity_World2Shadow[2][3].w;
    u_xlat21 = dot(u_xlat1, vs_TEXCOORD1);
    u_xlat1.xyz = vec3(u_xlat21) + vec3(-1.0, -3.0, -1.0);
    u_xlat21 = (-u_xlat1.z) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat21 = min(max(u_xlat21, 0.0), 1.0);
#else
    u_xlat21 = clamp(u_xlat21, 0.0, 1.0);
#endif
    u_xlat22 = u_xlat1.z * 0.25 + -1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat22 = min(max(u_xlat22, 0.0), 1.0);
#else
    u_xlat22 = clamp(u_xlat22, 0.0, 1.0);
#endif
    u_xlat16 = u_xlat21 + u_xlat22;
    u_xlat16 = (-u_xlat16) + 1.0;
    u_xlat16 = u_xlat16 * u_xlat10_3.w;
    u_xlat21 = u_xlat10_4.w * u_xlat21 + u_xlat16;
    u_xlat21 = u_xlat10_5.w * u_xlat22 + u_xlat21;
    u_xlat1.xy = u_xlat1.xy * vec2(0.5, 0.020833334);
#ifdef UNITY_ADRENO_ES3
    u_xlatb15 = !!(0.0<u_xlat1.z);
#else
    u_xlatb15 = 0.0<u_xlat1.z;
#endif
    u_xlat1.xy = u_xlat1.xy;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.xy = min(max(u_xlat1.xy, 0.0), 1.0);
#else
    u_xlat1.xy = clamp(u_xlat1.xy, 0.0, 1.0);
#endif
    u_xlat21 = u_xlat21 * u_xlat1.x + (-u_xlat1.y);
    u_xlat1.x = u_xlat1.y + 1.0;
    u_xlat21 = u_xlat21 * u_xlat1.x;
    u_xlat21 = u_xlatb15 ? u_xlat21 : float(0.0);
    u_xlat14 = max(u_xlat21, u_xlat14);
    u_xlat7.x = max(u_xlat14, u_xlat7.x);
    u_xlat0.x = max(u_xlat7.x, u_xlat0.x);
    u_xlat1.x = unity_World2Shadow[3][0].x;
    u_xlat1.y = unity_World2Shadow[3][1].x;
    u_xlat1.z = unity_World2Shadow[3][2].x;
    u_xlat1.w = unity_World2Shadow[3][3].x;
    u_xlat7.x = dot(u_xlat1, vs_TEXCOORD1);
    u_xlat7.xyz = u_xlat7.xxx + vec3(-1.0, -3.0, -1.0);
    u_xlat1.x = (-u_xlat7.z) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.x = min(max(u_xlat1.x, 0.0), 1.0);
#else
    u_xlat1.x = clamp(u_xlat1.x, 0.0, 1.0);
#endif
    u_xlat8 = u_xlat7.z * 0.25 + -1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat8 = min(max(u_xlat8, 0.0), 1.0);
#else
    u_xlat8 = clamp(u_xlat8, 0.0, 1.0);
#endif
    u_xlat15 = u_xlat8 + u_xlat1.x;
    u_xlat15 = (-u_xlat15) + 1.0;
    u_xlat10_3 = texture(unity_SplashScreenShadowTex7, u_xlat2.xy);
    u_xlat15 = u_xlat15 * u_xlat10_3.x;
    u_xlat10_4 = texture(unity_SplashScreenShadowTex6, u_xlat2.xy);
    u_xlat10_2 = texture(unity_SplashScreenShadowTex8, u_xlat2.xy);
    u_xlat1.x = u_xlat10_4.x * u_xlat1.x + u_xlat15;
    u_xlat1.x = u_xlat10_2.x * u_xlat8 + u_xlat1.x;
    u_xlat7.xy = u_xlat7.xy * vec2(0.5, 0.020833334);
#ifdef UNITY_ADRENO_ES3
    u_xlatb21 = !!(0.0<u_xlat7.z);
#else
    u_xlatb21 = 0.0<u_xlat7.z;
#endif
    u_xlat7.xy = u_xlat7.xy;
#ifdef UNITY_ADRENO_ES3
    u_xlat7.xy = min(max(u_xlat7.xy, 0.0), 1.0);
#else
    u_xlat7.xy = clamp(u_xlat7.xy, 0.0, 1.0);
#endif
    u_xlat7.x = u_xlat1.x * u_xlat7.x + (-u_xlat7.y);
    u_xlat14 = u_xlat7.y + 1.0;
    u_xlat7.x = u_xlat14 * u_xlat7.x;
    u_xlat7.x = u_xlatb21 ? u_xlat7.x : float(0.0);
    u_xlat1.x = unity_World2Shadow[3][0].y;
    u_xlat1.y = unity_World2Shadow[3][1].y;
    u_xlat1.z = unity_World2Shadow[3][2].y;
    u_xlat1.w = unity_World2Shadow[3][3].y;
    u_xlat14 = dot(u_xlat1, vs_TEXCOORD1);
    u_xlat1.xyz = vec3(u_xlat14) + vec3(-1.0, -3.0, -1.0);
    u_xlat14 = (-u_xlat1.z) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat14 = min(max(u_xlat14, 0.0), 1.0);
#else
    u_xlat14 = clamp(u_xlat14, 0.0, 1.0);
#endif
    u_xlat21 = u_xlat1.z * 0.25 + -1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat21 = min(max(u_xlat21, 0.0), 1.0);
#else
    u_xlat21 = clamp(u_xlat21, 0.0, 1.0);
#endif
    u_xlat22 = u_xlat21 + u_xlat14;
    u_xlat22 = (-u_xlat22) + 1.0;
    u_xlat22 = u_xlat22 * u_xlat10_3.y;
    u_xlat14 = u_xlat10_4.y * u_xlat14 + u_xlat22;
    u_xlat14 = u_xlat10_2.y * u_xlat21 + u_xlat14;
    u_xlat1.xy = u_xlat1.xy * vec2(0.5, 0.020833334);
#ifdef UNITY_ADRENO_ES3
    u_xlatb21 = !!(0.0<u_xlat1.z);
#else
    u_xlatb21 = 0.0<u_xlat1.z;
#endif
    u_xlat1.xy = u_xlat1.xy;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.xy = min(max(u_xlat1.xy, 0.0), 1.0);
#else
    u_xlat1.xy = clamp(u_xlat1.xy, 0.0, 1.0);
#endif
    u_xlat14 = u_xlat14 * u_xlat1.x + (-u_xlat1.y);
    u_xlat1.x = u_xlat1.y + 1.0;
    u_xlat14 = u_xlat14 * u_xlat1.x;
    u_xlat14 = u_xlatb21 ? u_xlat14 : float(0.0);
    u_xlat7.x = max(u_xlat14, u_xlat7.x);
    u_xlat1.x = unity_World2Shadow[3][0].z;
    u_xlat1.y = unity_World2Shadow[3][1].z;
    u_xlat1.z = unity_World2Shadow[3][2].z;
    u_xlat1.w = unity_World2Shadow[3][3].z;
    u_xlat14 = dot(u_xlat1, vs_TEXCOORD1);
    u_xlat1.xyz = vec3(u_xlat14) + vec3(-1.0, -3.0, -1.0);
    u_xlat14 = (-u_xlat1.z) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat14 = min(max(u_xlat14, 0.0), 1.0);
#else
    u_xlat14 = clamp(u_xlat14, 0.0, 1.0);
#endif
    u_xlat21 = u_xlat1.z * 0.25 + -1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat21 = min(max(u_xlat21, 0.0), 1.0);
#else
    u_xlat21 = clamp(u_xlat21, 0.0, 1.0);
#endif
    u_xlat22 = u_xlat21 + u_xlat14;
    u_xlat22 = (-u_xlat22) + 1.0;
    u_xlat22 = u_xlat22 * u_xlat10_3.z;
    u_xlat14 = u_xlat10_4.z * u_xlat14 + u_xlat22;
    u_xlat14 = u_xlat10_2.z * u_xlat21 + u_xlat14;
    u_xlat1.xy = u_xlat1.xy * vec2(0.5, 0.020833334);
#ifdef UNITY_ADRENO_ES3
    u_xlatb21 = !!(0.0<u_xlat1.z);
#else
    u_xlatb21 = 0.0<u_xlat1.z;
#endif
    u_xlat1.xy = u_xlat1.xy;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.xy = min(max(u_xlat1.xy, 0.0), 1.0);
#else
    u_xlat1.xy = clamp(u_xlat1.xy, 0.0, 1.0);
#endif
    u_xlat14 = u_xlat14 * u_xlat1.x + (-u_xlat1.y);
    u_xlat1.x = u_xlat1.y + 1.0;
    u_xlat14 = u_xlat14 * u_xlat1.x;
    u_xlat14 = u_xlatb21 ? u_xlat14 : float(0.0);
    u_xlat1.x = unity_World2Shadow[3][0].w;
    u_xlat1.y = unity_World2Shadow[3][1].w;
    u_xlat1.z = unity_World2Shadow[3][2].w;
    u_xlat1.w = unity_World2Shadow[3][3].w;
    u_xlat21 = dot(u_xlat1, vs_TEXCOORD1);
    u_xlat1.xyz = vec3(u_xlat21) + vec3(-1.0, -3.0, -1.0);
    u_xlat21 = (-u_xlat1.z) * 0.25 + 1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat21 = min(max(u_xlat21, 0.0), 1.0);
#else
    u_xlat21 = clamp(u_xlat21, 0.0, 1.0);
#endif
    u_xlat22 = u_xlat1.z * 0.25 + -1.0;
#ifdef UNITY_ADRENO_ES3
    u_xlat22 = min(max(u_xlat22, 0.0), 1.0);
#else
    u_xlat22 = clamp(u_xlat22, 0.0, 1.0);
#endif
    u_xlat2.x = u_xlat21 + u_xlat22;
    u_xlat2.x = (-u_xlat2.x) + 1.0;
    u_xlat2.x = u_xlat2.x * u_xlat10_3.w;
    u_xlat21 = u_xlat10_4.w * u_xlat21 + u_xlat2.x;
    u_xlat21 = u_xlat10_2.w * u_xlat22 + u_xlat21;
    u_xlat1.xy = u_xlat1.xy * vec2(0.5, 0.020833334);
#ifdef UNITY_ADRENO_ES3
    u_xlatb15 = !!(0.0<u_xlat1.z);
#else
    u_xlatb15 = 0.0<u_xlat1.z;
#endif
    u_xlat1.xy = u_xlat1.xy;
#ifdef UNITY_ADRENO_ES3
    u_xlat1.xy = min(max(u_xlat1.xy, 0.0), 1.0);
#else
    u_xlat1.xy = clamp(u_xlat1.xy, 0.0, 1.0);
#endif
    u_xlat21 = u_xlat21 * u_xlat1.x + (-u_xlat1.y);
    u_xlat1.x = u_xlat1.y + 1.0;
    u_xlat21 = u_xlat21 * u_xlat1.x;
    u_xlat21 = u_xlatb15 ? u_xlat21 : float(0.0);
    u_xlat14 = max(u_xlat21, u_xlat14);
    u_xlat7.x = max(u_xlat14, u_xlat7.x);
    u_xlat0.x = max(u_xlat7.x, u_xlat0.x);
#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    u_xlat0.x = (-u_xlat0.x) + 1.0;
    u_xlat7.xyz = (-vs_TEXCOORD1.xyz) + unity_LightPosition0.xyz;
    u_xlat1.x = dot(u_xlat7.xyz, u_xlat7.xyz);
    u_xlat1.x = inversesqrt(u_xlat1.x);
    u_xlat7.xyz = u_xlat7.xyz * u_xlat1.xxx;
    u_xlat7.x = dot(vs_TEXCOORD0.xyz, u_xlat7.xyz);
#ifdef UNITY_ADRENO_ES3
    u_xlat7.x = min(max(u_xlat7.x, 0.0), 1.0);
#else
    u_xlat7.x = clamp(u_xlat7.x, 0.0, 1.0);
#endif
    u_xlat14 = u_xlat7.x * u_xlat7.x;
    u_xlat7.x = u_xlat14 * u_xlat7.x;
    u_xlat0.x = u_xlat0.x * u_xlat7.x;
    u_xlat16_6.xyz = unity_LightColor0.xyz + (-unity_LightColor1.xyz);
    u_xlat16_6.xyz = u_xlat0.xxx * u_xlat16_6.xyz + unity_LightColor1.xyz;
    SV_Target0.xyz = u_xlat16_6.xyz * vs_COLOR0.xyz;
    SV_Target0.w = vs_COLOR0.w;
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