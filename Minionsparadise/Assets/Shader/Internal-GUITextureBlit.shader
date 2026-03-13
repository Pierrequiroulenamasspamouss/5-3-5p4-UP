//////////////////////////////////////////
///////////////////////////////////////////
Shader "Hidden/Internal-GUITextureBlit" {
Properties {
 _MainTex ("Texture", any) = "white" { }
}
SubShader { 
 Tags { "ForceSupported"="true" }
 Pass {
  Tags { "ForceSupported"="true" }
  ZTest Always
  ZWrite Off
  Cull Off
  Blend SrcAlpha OneMinusSrcAlpha
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp vec4 _MainTex_ST;
uniform highp mat4 _GUIClipTextureMatrix;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_COLOR = _glesColor;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = (_GUIClipTextureMatrix * (glstate_matrix_modelview0 * _glesVertex)).xy;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform sampler2D _GUIClipTexture;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 col_1;
  col_1.xyz = (texture2D (_MainTex, xlv_TEXCOORD0).xyz * xlv_COLOR.xyz);
  col_1.w = (xlv_COLOR.w * texture2D (_GUIClipTexture, xlv_TEXCOORD1).w);
  gl_FragData[0] = col_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 glstate_matrix_modelview0;
uniform 	vec4 _MainTex_ST;
uniform 	mat4x4 _GUIClipTextureMatrix;
in highp vec4 in_POSITION0;
in lowp vec4 in_COLOR0;
in highp vec2 in_TEXCOORD0;
out lowp vec4 vs_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp vec2 vs_TEXCOORD1;
vec4 u_xlat0;
vec2 u_xlat1;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    vs_COLOR0 = in_COLOR0;
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_modelview0[1];
    u_xlat0 = glstate_matrix_modelview0[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_modelview0[2] * in_POSITION0.zzzz + u_xlat0;
    u_xlat0 = glstate_matrix_modelview0[3] * in_POSITION0.wwww + u_xlat0;
    u_xlat1.xy = u_xlat0.yy * _GUIClipTextureMatrix[1].xy;
    u_xlat0.xy = _GUIClipTextureMatrix[0].xy * u_xlat0.xx + u_xlat1.xy;
    u_xlat0.xy = _GUIClipTextureMatrix[2].xy * u_xlat0.zz + u_xlat0.xy;
    vs_TEXCOORD1.xy = _GUIClipTextureMatrix[3].xy * u_xlat0.ww + u_xlat0.xy;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform lowp sampler2D _MainTex;
uniform lowp sampler2D _GUIClipTexture;
in lowp vec4 vs_COLOR0;
in highp vec2 vs_TEXCOORD0;
in highp vec2 vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
lowp vec3 u_xlat10_0;
lowp float u_xlat10_1;
void main()
{
    u_xlat10_0.xyz = texture(_MainTex, vs_TEXCOORD0.xy).xyz;
    u_xlat0.xyz = u_xlat10_0.xyz * vs_COLOR0.xyz;
    u_xlat10_1 = texture(_GUIClipTexture, vs_TEXCOORD1.xy).w;
    u_xlat0.w = u_xlat10_1 * vs_COLOR0.w;
    SV_Target0 = u_xlat0;
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
}