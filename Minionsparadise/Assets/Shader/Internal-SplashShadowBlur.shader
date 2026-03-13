//////////////////////////////////////////
///////////////////////////////////////////
Shader "Hidden/InternalSplashShadowBlur" {
SubShader { 
 Pass {
  ZTest Always
  Cull Off
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_TexelSize;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec4 average_2;
  highp vec2 tmpvar_3;
  tmpvar_3.y = 0.0;
  tmpvar_3.x = (-2.25 * _MainTex_TexelSize.x);
  highp vec2 P_4;
  P_4 = (xlv_TEXCOORD0 + tmpvar_3);
  highp vec2 tmpvar_5;
  tmpvar_5.y = 0.0;
  tmpvar_5.x = (-0.4473684 * _MainTex_TexelSize.x);
  highp vec2 P_6;
  P_6 = (xlv_TEXCOORD0 + tmpvar_5);
  highp vec2 tmpvar_7;
  tmpvar_7.y = 0.0;
  tmpvar_7.x = (1.306122 * _MainTex_TexelSize.x);
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD0 + tmpvar_7);
  highp vec2 tmpvar_9;
  tmpvar_9.y = 0.0;
  tmpvar_9.x = (3.0 * _MainTex_TexelSize.x);
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD0 + tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = (((
    (texture2D (_MainTex, P_4) * 0.1333333)
   + 
    (texture2D (_MainTex, P_6) * 0.5066667)
  ) + (texture2D (_MainTex, P_8) * 0.3266667)) + (texture2D (_MainTex, P_10) * 0.03333334));
  average_2 = tmpvar_11;
  tmpvar_1 = average_2;
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
in highp vec2 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
vec4 u_xlat0;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	vec4 _MainTex_TexelSize;
uniform lowp sampler2D _MainTex;
in highp vec2 vs_TEXCOORD0;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
lowp vec4 u_xlat10_0;
vec4 u_xlat1;
lowp vec4 u_xlat10_1;
lowp vec4 u_xlat10_2;
void main()
{
    u_xlat0.xz = _MainTex_TexelSize.xx * vec2(-2.25, -0.447368413);
    u_xlat0.y = float(0.0);
    u_xlat0.w = float(0.0);
    u_xlat0 = u_xlat0 + vs_TEXCOORD0.xyxy;
    u_xlat10_1 = texture(_MainTex, u_xlat0.zw);
    u_xlat10_0 = texture(_MainTex, u_xlat0.xy);
    u_xlat10_1 = u_xlat10_1 * vec4(0.50666666, 0.50666666, 0.50666666, 0.50666666);
    u_xlat10_0 = u_xlat10_0 * vec4(0.13333334, 0.13333334, 0.13333334, 0.13333334) + u_xlat10_1;
    u_xlat1.xz = _MainTex_TexelSize.xx * vec2(1.30612242, 3.0);
    u_xlat1.y = float(0.0);
    u_xlat1.w = float(0.0);
    u_xlat1 = u_xlat1 + vs_TEXCOORD0.xyxy;
    u_xlat10_2 = texture(_MainTex, u_xlat1.xy);
    u_xlat10_1 = texture(_MainTex, u_xlat1.zw);
    u_xlat10_0 = u_xlat10_2 * vec4(0.326666653, 0.326666653, 0.326666653, 0.326666653) + u_xlat10_0;
    SV_Target0 = u_xlat10_1 * vec4(0.0333333351, 0.0333333351, 0.0333333351, 0.0333333351) + u_xlat10_0;
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
 Pass {
  ZTest Always
  Cull Off
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform highp vec4 _MainTex_TexelSize;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec4 average_2;
  highp vec2 tmpvar_3;
  tmpvar_3.x = 0.0;
  tmpvar_3.y = (-2.25 * _MainTex_TexelSize.y);
  highp vec2 P_4;
  P_4 = (xlv_TEXCOORD0 + tmpvar_3);
  highp vec2 tmpvar_5;
  tmpvar_5.x = 0.0;
  tmpvar_5.y = (-0.4473684 * _MainTex_TexelSize.y);
  highp vec2 P_6;
  P_6 = (xlv_TEXCOORD0 + tmpvar_5);
  highp vec2 tmpvar_7;
  tmpvar_7.x = 0.0;
  tmpvar_7.y = (1.306122 * _MainTex_TexelSize.y);
  highp vec2 P_8;
  P_8 = (xlv_TEXCOORD0 + tmpvar_7);
  highp vec2 tmpvar_9;
  tmpvar_9.x = 0.0;
  tmpvar_9.y = (3.0 * _MainTex_TexelSize.y);
  highp vec2 P_10;
  P_10 = (xlv_TEXCOORD0 + tmpvar_9);
  lowp vec4 tmpvar_11;
  tmpvar_11 = (((
    (texture2D (_MainTex, P_4) * 0.1333333)
   + 
    (texture2D (_MainTex, P_6) * 0.5066667)
  ) + (texture2D (_MainTex, P_8) * 0.3266667)) + (texture2D (_MainTex, P_10) * 0.03333334));
  average_2 = tmpvar_11;
  tmpvar_1 = average_2;
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
in highp vec2 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
vec4 u_xlat0;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	vec4 _MainTex_TexelSize;
uniform lowp sampler2D _MainTex;
in highp vec2 vs_TEXCOORD0;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
lowp vec4 u_xlat10_0;
vec4 u_xlat1;
lowp vec4 u_xlat10_1;
lowp vec4 u_xlat10_2;
void main()
{
    u_xlat0.x = float(0.0);
    u_xlat0.z = float(0.0);
    u_xlat0.yw = _MainTex_TexelSize.yy * vec2(-2.25, -0.447368413);
    u_xlat0 = u_xlat0 + vs_TEXCOORD0.xyxy;
    u_xlat10_1 = texture(_MainTex, u_xlat0.zw);
    u_xlat10_0 = texture(_MainTex, u_xlat0.xy);
    u_xlat10_1 = u_xlat10_1 * vec4(0.50666666, 0.50666666, 0.50666666, 0.50666666);
    u_xlat10_0 = u_xlat10_0 * vec4(0.13333334, 0.13333334, 0.13333334, 0.13333334) + u_xlat10_1;
    u_xlat1.x = float(0.0);
    u_xlat1.z = float(0.0);
    u_xlat1.yw = _MainTex_TexelSize.yy * vec2(1.30612242, 3.0);
    u_xlat1 = u_xlat1 + vs_TEXCOORD0.xyxy;
    u_xlat10_2 = texture(_MainTex, u_xlat1.xy);
    u_xlat10_1 = texture(_MainTex, u_xlat1.zw);
    u_xlat10_0 = u_xlat10_2 * vec4(0.326666653, 0.326666653, 0.326666653, 0.326666653) + u_xlat10_0;
    SV_Target0 = u_xlat10_1 * vec4(0.0333333351, 0.0333333351, 0.0333333351, 0.0333333351) + u_xlat10_0;
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