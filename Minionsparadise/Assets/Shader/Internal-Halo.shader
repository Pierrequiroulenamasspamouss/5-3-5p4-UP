//////////////////////////////////////////
///////////////////////////////////////////
Shader "Hidden/Internal-Halo" {
SubShader { 
 Tags { "RenderType"="Overlay" }
 Pass {
  Tags { "RenderType"="Overlay" }
  ZWrite Off
  Cull Off
  Blend OneMinusDstColor One
  ColorMask RGB
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _HaloFalloff_ST;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _HaloFalloff_ST.xy) + _HaloFalloff_ST.zw);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _HaloFalloff;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = texture2D (_HaloFalloff, xlv_TEXCOORD0);
  lowp vec4 tmpvar_2;
  tmpvar_2.xyz = (xlv_COLOR.xyz * tmpvar_1.w);
  tmpvar_2.w = tmpvar_1.w;
  gl_FragData[0] = tmpvar_2;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	vec4 _HaloFalloff_ST;
in highp vec4 in_POSITION0;
in lowp vec4 in_COLOR0;
in highp vec2 in_TEXCOORD0;
out lowp vec4 vs_COLOR0;
out highp vec2 vs_TEXCOORD0;
vec4 u_xlat0;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    vs_COLOR0 = in_COLOR0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _HaloFalloff_ST.xy + _HaloFalloff_ST.zw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform lowp sampler2D _HaloFalloff;
in lowp vec4 vs_COLOR0;
in highp vec2 vs_TEXCOORD0;
layout(location = 0) out lowp vec4 SV_Target0;
lowp float u_xlat10_0;
void main()
{
    u_xlat10_0 = texture(_HaloFalloff, vs_TEXCOORD0.xy).w;
    SV_Target0.xyz = vec3(u_xlat10_0) * vs_COLOR0.xyz;
    SV_Target0.w = u_xlat10_0;
    return;
}
#endif
"
}
SubProgram "gles " {
Keywords { "FOG_LINEAR" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_FogParams;
uniform highp vec4 _HaloFalloff_ST;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp float xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = (glstate_matrix_mvp * _glesVertex);
  gl_Position = tmpvar_1;
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _HaloFalloff_ST.xy) + _HaloFalloff_ST.zw);
  xlv_TEXCOORD1 = ((tmpvar_1.z * unity_FogParams.z) + unity_FogParams.w);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _HaloFalloff;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp float xlv_TEXCOORD1;
void main ()
{
  lowp vec4 col_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_HaloFalloff, xlv_TEXCOORD0);
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = (xlv_COLOR.xyz * tmpvar_2.w);
  tmpvar_3.w = tmpvar_2.w;
  col_1.w = tmpvar_3.w;
  highp float tmpvar_4;
  tmpvar_4 = clamp (xlv_TEXCOORD1, 0.0, 1.0);
  col_1.xyz = (tmpvar_3.xyz * vec3(tmpvar_4));
  gl_FragData[0] = col_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "FOG_LINEAR" }
"#ifdef VERTEX
#version 300 es
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	vec4 unity_FogParams;
uniform 	vec4 _HaloFalloff_ST;
in highp vec4 in_POSITION0;
in lowp vec4 in_COLOR0;
in highp vec2 in_TEXCOORD0;
out lowp vec4 vs_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp float vs_TEXCOORD1;
vec4 u_xlat0;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    gl_Position = u_xlat0;
    vs_TEXCOORD1 = u_xlat0.z * unity_FogParams.z + unity_FogParams.w;
    vs_COLOR0 = in_COLOR0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _HaloFalloff_ST.xy + _HaloFalloff_ST.zw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform lowp sampler2D _HaloFalloff;
in lowp vec4 vs_COLOR0;
in highp vec2 vs_TEXCOORD0;
in highp float vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
float u_xlat0;
vec4 u_xlat1;
lowp vec3 u_xlat10_2;
void main()
{
    u_xlat0 = vs_TEXCOORD1;
#ifdef UNITY_ADRENO_ES3
    u_xlat0 = min(max(u_xlat0, 0.0), 1.0);
#else
    u_xlat0 = clamp(u_xlat0, 0.0, 1.0);
#endif
    u_xlat1.w = texture(_HaloFalloff, vs_TEXCOORD0.xy).w;
    u_xlat10_2.xyz = u_xlat1.www * vs_COLOR0.xyz;
    u_xlat1.xyz = vec3(u_xlat0) * u_xlat10_2.xyz;
    SV_Target0 = u_xlat1;
    return;
}
#endif
"
}
SubProgram "gles " {
Keywords { "FOG_EXP" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_FogParams;
uniform highp vec4 _HaloFalloff_ST;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp float xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = (glstate_matrix_mvp * _glesVertex);
  gl_Position = tmpvar_1;
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _HaloFalloff_ST.xy) + _HaloFalloff_ST.zw);
  xlv_TEXCOORD1 = exp2(-((unity_FogParams.y * tmpvar_1.z)));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _HaloFalloff;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp float xlv_TEXCOORD1;
void main ()
{
  lowp vec4 col_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_HaloFalloff, xlv_TEXCOORD0);
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = (xlv_COLOR.xyz * tmpvar_2.w);
  tmpvar_3.w = tmpvar_2.w;
  col_1.w = tmpvar_3.w;
  highp float tmpvar_4;
  tmpvar_4 = clamp (xlv_TEXCOORD1, 0.0, 1.0);
  col_1.xyz = (tmpvar_3.xyz * vec3(tmpvar_4));
  gl_FragData[0] = col_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "FOG_EXP" }
"#ifdef VERTEX
#version 300 es
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	vec4 unity_FogParams;
uniform 	vec4 _HaloFalloff_ST;
in highp vec4 in_POSITION0;
in lowp vec4 in_COLOR0;
in highp vec2 in_TEXCOORD0;
out lowp vec4 vs_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp float vs_TEXCOORD1;
vec4 u_xlat0;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    gl_Position = u_xlat0;
    u_xlat0.x = u_xlat0.z * unity_FogParams.y;
    vs_TEXCOORD1 = exp2((-u_xlat0.x));
    vs_COLOR0 = in_COLOR0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _HaloFalloff_ST.xy + _HaloFalloff_ST.zw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform lowp sampler2D _HaloFalloff;
in lowp vec4 vs_COLOR0;
in highp vec2 vs_TEXCOORD0;
in highp float vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
float u_xlat0;
vec4 u_xlat1;
lowp vec3 u_xlat10_2;
void main()
{
    u_xlat0 = vs_TEXCOORD1;
#ifdef UNITY_ADRENO_ES3
    u_xlat0 = min(max(u_xlat0, 0.0), 1.0);
#else
    u_xlat0 = clamp(u_xlat0, 0.0, 1.0);
#endif
    u_xlat1.w = texture(_HaloFalloff, vs_TEXCOORD0.xy).w;
    u_xlat10_2.xyz = u_xlat1.www * vs_COLOR0.xyz;
    u_xlat1.xyz = vec3(u_xlat0) * u_xlat10_2.xyz;
    SV_Target0 = u_xlat1;
    return;
}
#endif
"
}
SubProgram "gles " {
Keywords { "FOG_EXP2" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_FogParams;
uniform highp vec4 _HaloFalloff_ST;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp float xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = (glstate_matrix_mvp * _glesVertex);
  highp float tmpvar_2;
  tmpvar_2 = (unity_FogParams.x * tmpvar_1.z);
  gl_Position = tmpvar_1;
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _HaloFalloff_ST.xy) + _HaloFalloff_ST.zw);
  xlv_TEXCOORD1 = exp2((-(tmpvar_2) * tmpvar_2));
}


#endif
#ifdef FRAGMENT
uniform sampler2D _HaloFalloff;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp float xlv_TEXCOORD1;
void main ()
{
  lowp vec4 col_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (_HaloFalloff, xlv_TEXCOORD0);
  lowp vec4 tmpvar_3;
  tmpvar_3.xyz = (xlv_COLOR.xyz * tmpvar_2.w);
  tmpvar_3.w = tmpvar_2.w;
  col_1.w = tmpvar_3.w;
  highp float tmpvar_4;
  tmpvar_4 = clamp (xlv_TEXCOORD1, 0.0, 1.0);
  col_1.xyz = (tmpvar_3.xyz * vec3(tmpvar_4));
  gl_FragData[0] = col_1;
}


#endif
"
}
SubProgram "gles3 " {
Keywords { "FOG_EXP2" }
"#ifdef VERTEX
#version 300 es
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	vec4 unity_FogParams;
uniform 	vec4 _HaloFalloff_ST;
in highp vec4 in_POSITION0;
in lowp vec4 in_COLOR0;
in highp vec2 in_TEXCOORD0;
out lowp vec4 vs_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp float vs_TEXCOORD1;
vec4 u_xlat0;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    gl_Position = u_xlat0;
    u_xlat0.x = u_xlat0.z * unity_FogParams.x;
    u_xlat0.x = u_xlat0.x * (-u_xlat0.x);
    vs_TEXCOORD1 = exp2(u_xlat0.x);
    vs_COLOR0 = in_COLOR0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _HaloFalloff_ST.xy + _HaloFalloff_ST.zw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform lowp sampler2D _HaloFalloff;
in lowp vec4 vs_COLOR0;
in highp vec2 vs_TEXCOORD0;
in highp float vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
float u_xlat0;
vec4 u_xlat1;
lowp vec3 u_xlat10_2;
void main()
{
    u_xlat0 = vs_TEXCOORD1;
#ifdef UNITY_ADRENO_ES3
    u_xlat0 = min(max(u_xlat0, 0.0), 1.0);
#else
    u_xlat0 = clamp(u_xlat0, 0.0, 1.0);
#endif
    u_xlat1.w = texture(_HaloFalloff, vs_TEXCOORD0.xy).w;
    u_xlat10_2.xyz = u_xlat1.www * vs_COLOR0.xyz;
    u_xlat1.xyz = vec3(u_xlat0) * u_xlat10_2.xyz;
    SV_Target0 = u_xlat1;
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
SubProgram "gles " {
Keywords { "FOG_LINEAR" }
""
}
SubProgram "gles3 " {
Keywords { "FOG_LINEAR" }
""
}
SubProgram "gles " {
Keywords { "FOG_EXP" }
""
}
SubProgram "gles3 " {
Keywords { "FOG_EXP" }
""
}
SubProgram "gles " {
Keywords { "FOG_EXP2" }
""
}
SubProgram "gles3 " {
Keywords { "FOG_EXP2" }
""
}
}
 }
}
}