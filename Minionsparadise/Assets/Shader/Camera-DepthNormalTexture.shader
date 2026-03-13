//////////////////////////////////////////
///////////////////////////////////////////
Shader "Hidden/Camera-DepthNormalTexture" {
Properties {
 _MainTex ("", 2D) = "white" { }
 _Cutoff ("", Float) = 0.5
 _Color ("", Color) = (1,1,1,1)
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
varying highp vec4 xlv_TEXCOORD0;
void main ()
{
  highp vec4 tmpvar_1;
  highp mat3 tmpvar_2;
  tmpvar_2[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_2[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_2[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  tmpvar_1.xyz = normalize((tmpvar_2 * _glesNormal));
  tmpvar_1.w = -(((glstate_matrix_modelview0 * _glesVertex).z * _ProjectionParams.w));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
}


#endif
#ifdef FRAGMENT
varying highp vec4 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec4 enc_2;
  highp vec2 enc_3;
  enc_3 = (xlv_TEXCOORD0.xy / (xlv_TEXCOORD0.z + 1.0));
  enc_3 = (enc_3 / 1.7777);
  enc_3 = ((enc_3 * 0.5) + 0.5);
  enc_2.xy = enc_3;
  highp vec2 enc_4;
  highp vec2 tmpvar_5;
  tmpvar_5 = fract((vec2(1.0, 255.0) * xlv_TEXCOORD0.w));
  enc_4.y = tmpvar_5.y;
  enc_4.x = (tmpvar_5.x - (tmpvar_5.y * 0.003921569));
  enc_2.zw = enc_4;
  tmpvar_1 = enc_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 glstate_matrix_modelview0;
uniform 	mat4x4 glstate_matrix_invtrans_modelview0;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
out highp vec4 vs_TEXCOORD0;
vec4 u_xlat0;
float u_xlat3;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    u_xlat0.xyz = in_NORMAL0.yyy * glstate_matrix_invtrans_modelview0[1].xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[0].xyz * in_NORMAL0.xxx + u_xlat0.xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[2].xyz * in_NORMAL0.zzz + u_xlat0.xyz;
    u_xlat3 = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat3 = inversesqrt(u_xlat3);
    vs_TEXCOORD0.xyz = vec3(u_xlat3) * u_xlat0.xyz;
    u_xlat0.x = in_POSITION0.y * glstate_matrix_modelview0[1].z;
    u_xlat0.x = glstate_matrix_modelview0[0].z * in_POSITION0.x + u_xlat0.x;
    u_xlat0.x = glstate_matrix_modelview0[2].z * in_POSITION0.z + u_xlat0.x;
    u_xlat0.x = glstate_matrix_modelview0[3].z * in_POSITION0.w + u_xlat0.x;
    u_xlat0.x = u_xlat0.x * _ProjectionParams.w;
    vs_TEXCOORD0.w = (-u_xlat0.x);
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
in highp vec4 vs_TEXCOORD0;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
vec2 u_xlat1;
void main()
{
    u_xlat0.x = vs_TEXCOORD0.z + 1.0;
    u_xlat0.xy = vs_TEXCOORD0.xy / u_xlat0.xx;
    u_xlat0.xy = u_xlat0.xy * vec2(0.281262308, 0.281262308) + vec2(0.5, 0.5);
    u_xlat1.xy = vs_TEXCOORD0.ww * vec2(1.0, 255.0);
    u_xlat1.xy = fract(u_xlat1.xy);
    u_xlat0.z = (-u_xlat1.y) * 0.00392156886 + u_xlat1.x;
    u_xlat0.w = u_xlat1.y;
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
SubShader { 
 Tags { "RenderType"="TransparentCutout" }
 Pass {
  Tags { "RenderType"="TransparentCutout" }
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp vec4 _MainTex_ST;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  highp mat3 tmpvar_2;
  tmpvar_2[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_2[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_2[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  tmpvar_1.xyz = normalize((tmpvar_2 * _glesNormal));
  tmpvar_1.w = -(((glstate_matrix_modelview0 * _glesVertex).z * _ProjectionParams.w));
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  xlv_TEXCOORD1 = tmpvar_1;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform lowp float _Cutoff;
uniform lowp vec4 _Color;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  lowp float x_2;
  x_2 = ((texture2D (_MainTex, xlv_TEXCOORD0).w * _Color.w) - _Cutoff);
  if ((x_2 < 0.0)) {
    discard;
  };
  highp vec4 enc_3;
  highp vec2 enc_4;
  enc_4 = (xlv_TEXCOORD1.xy / (xlv_TEXCOORD1.z + 1.0));
  enc_4 = (enc_4 / 1.7777);
  enc_4 = ((enc_4 * 0.5) + 0.5);
  enc_3.xy = enc_4;
  highp vec2 enc_5;
  highp vec2 tmpvar_6;
  tmpvar_6 = fract((vec2(1.0, 255.0) * xlv_TEXCOORD1.w));
  enc_5.y = tmpvar_6.y;
  enc_5.x = (tmpvar_6.x - (tmpvar_6.y * 0.003921569));
  enc_3.zw = enc_5;
  tmpvar_1 = enc_3;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 glstate_matrix_modelview0;
uniform 	mat4x4 glstate_matrix_invtrans_modelview0;
uniform 	vec4 _MainTex_ST;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
vec4 u_xlat0;
float u_xlat3;
void main()
{
    u_xlat0 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat0 = glstate_matrix_mvp[0] * in_POSITION0.xxxx + u_xlat0;
    u_xlat0 = glstate_matrix_mvp[2] * in_POSITION0.zzzz + u_xlat0;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat0;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
    u_xlat0.xyz = in_NORMAL0.yyy * glstate_matrix_invtrans_modelview0[1].xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[0].xyz * in_NORMAL0.xxx + u_xlat0.xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[2].xyz * in_NORMAL0.zzz + u_xlat0.xyz;
    u_xlat3 = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat3 = inversesqrt(u_xlat3);
    vs_TEXCOORD1.xyz = vec3(u_xlat3) * u_xlat0.xyz;
    u_xlat0.x = in_POSITION0.y * glstate_matrix_modelview0[1].z;
    u_xlat0.x = glstate_matrix_modelview0[0].z * in_POSITION0.x + u_xlat0.x;
    u_xlat0.x = glstate_matrix_modelview0[2].z * in_POSITION0.z + u_xlat0.x;
    u_xlat0.x = glstate_matrix_modelview0[3].z * in_POSITION0.w + u_xlat0.x;
    u_xlat0.x = u_xlat0.x * _ProjectionParams.w;
    vs_TEXCOORD1.w = (-u_xlat0.x);
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	lowp float _Cutoff;
uniform 	lowp vec4 _Color;
uniform lowp sampler2D _MainTex;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
lowp float u_xlat10_0;
bool u_xlatb0;
lowp float u_xlat10_1;
vec2 u_xlat2;
void main()
{
    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
    u_xlat10_1 = u_xlat10_0 * _Color.w + (-_Cutoff);
#ifdef UNITY_ADRENO_ES3
    u_xlatb0 = !!(u_xlat10_1<0.0);
#else
    u_xlatb0 = u_xlat10_1<0.0;
#endif
    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
    u_xlat0.x = vs_TEXCOORD1.z + 1.0;
    u_xlat0.xy = vs_TEXCOORD1.xy / u_xlat0.xx;
    u_xlat0.xy = u_xlat0.xy * vec2(0.281262308, 0.281262308) + vec2(0.5, 0.5);
    u_xlat2.xy = vs_TEXCOORD1.ww * vec2(1.0, 255.0);
    u_xlat2.xy = fract(u_xlat2.xy);
    u_xlat0.z = (-u_xlat2.y) * 0.00392156886 + u_xlat2.x;
    u_xlat0.w = u_xlat2.y;
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
SubShader { 
 Tags { "RenderType"="TreeBark" }
 Pass {
  Tags { "RenderType"="TreeBark" }
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec4 _TreeInstanceScale;
uniform highp vec4 _SquashPlaneNormal;
uniform highp float _SquashAmount;
uniform highp vec4 _Wind;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.w = _glesVertex.w;
  tmpvar_3.xyz = (_glesVertex.xyz * _TreeInstanceScale.xyz);
  highp vec4 tmpvar_4;
  tmpvar_4.xy = tmpvar_1.xy;
  tmpvar_4.zw = _glesMultiTexCoord1.xy;
  highp vec4 pos_5;
  pos_5.w = tmpvar_3.w;
  highp vec3 bend_6;
  highp vec4 v_7;
  v_7.x = _Object2World[0].w;
  v_7.y = _Object2World[1].w;
  v_7.z = _Object2World[2].w;
  v_7.w = _Object2World[3].w;
  highp float tmpvar_8;
  tmpvar_8 = (dot (v_7.xyz, vec3(1.0, 1.0, 1.0)) + tmpvar_4.x);
  highp vec2 tmpvar_9;
  tmpvar_9.x = dot (tmpvar_3.xyz, vec3((tmpvar_4.y + tmpvar_8)));
  tmpvar_9.y = tmpvar_8;
  highp vec4 tmpvar_10;
  tmpvar_10 = abs(((
    fract((((
      fract(((_Time.yy + tmpvar_9).xxyy * vec4(1.975, 0.793, 0.375, 0.193)))
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  highp vec4 tmpvar_11;
  tmpvar_11 = ((tmpvar_10 * tmpvar_10) * (3.0 - (2.0 * tmpvar_10)));
  highp vec2 tmpvar_12;
  tmpvar_12 = (tmpvar_11.xz + tmpvar_11.yw);
  bend_6.xz = ((tmpvar_4.y * 0.1) * _glesNormal).xz;
  bend_6.y = (_glesMultiTexCoord1.y * 0.3);
  pos_5.xyz = (tmpvar_3.xyz + ((
    (tmpvar_12.xyx * bend_6)
   + 
    ((_Wind.xyz * tmpvar_12.y) * _glesMultiTexCoord1.y)
  ) * _Wind.w));
  pos_5.xyz = (pos_5.xyz + (_glesMultiTexCoord1.x * _Wind.xyz));
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = mix ((pos_5.xyz - (
    (dot (_SquashPlaneNormal.xyz, pos_5.xyz) + _SquashPlaneNormal.w)
   * _SquashPlaneNormal.xyz)), pos_5.xyz, vec3(_SquashAmount));
  tmpvar_3 = tmpvar_13;
  highp mat3 tmpvar_14;
  tmpvar_14[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_14[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_14[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  tmpvar_2.xyz = normalize((tmpvar_14 * normalize(_glesNormal)));
  tmpvar_2.w = -(((glstate_matrix_modelview0 * tmpvar_13).z * _ProjectionParams.w));
  gl_Position = (glstate_matrix_mvp * tmpvar_13);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec4 enc_2;
  highp vec2 enc_3;
  enc_3 = (xlv_TEXCOORD1.xy / (xlv_TEXCOORD1.z + 1.0));
  enc_3 = (enc_3 / 1.7777);
  enc_3 = ((enc_3 * 0.5) + 0.5);
  enc_2.xy = enc_3;
  highp vec2 enc_4;
  highp vec2 tmpvar_5;
  tmpvar_5 = fract((vec2(1.0, 255.0) * xlv_TEXCOORD1.w));
  enc_4.y = tmpvar_5.y;
  enc_4.x = (tmpvar_5.x - (tmpvar_5.y * 0.003921569));
  enc_2.zw = enc_4;
  tmpvar_1 = enc_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _Time;
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 glstate_matrix_modelview0;
uniform 	mat4x4 glstate_matrix_invtrans_modelview0;
uniform 	mat4x4 _Object2World;
uniform 	vec4 _TreeInstanceScale;
uniform 	vec4 _SquashPlaneNormal;
uniform 	float _SquashAmount;
uniform 	vec4 _Wind;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in lowp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
vec4 u_xlat0;
vec4 u_xlat1;
vec4 u_xlat2;
vec3 u_xlat3;
float u_xlat4;
float u_xlat8;
float u_xlat12;
void main()
{
    u_xlat0.x = _Object2World[0].w;
    u_xlat0.y = _Object2World[1].w;
    u_xlat0.z = _Object2World[2].w;
    u_xlat0.x = dot(u_xlat0.xyz, vec3(1.0, 1.0, 1.0));
    u_xlat0.y = u_xlat0.x + in_COLOR0.x;
    u_xlat8 = u_xlat0.y + in_COLOR0.y;
    u_xlat1.xyz = in_POSITION0.xyz * _TreeInstanceScale.xyz;
    u_xlat0.x = dot(u_xlat1.xyz, vec3(u_xlat8));
    u_xlat0 = u_xlat0.xxyy + _Time.yyyy;
    u_xlat0 = u_xlat0 * vec4(1.97500002, 0.792999983, 0.375, 0.193000004);
    u_xlat0 = fract(u_xlat0);
    u_xlat0 = u_xlat0 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    u_xlat0 = fract(u_xlat0);
    u_xlat0 = u_xlat0 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    u_xlat2 = abs(u_xlat0) * abs(u_xlat0);
    u_xlat0 = -abs(u_xlat0) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    u_xlat0 = u_xlat0 * u_xlat2;
    u_xlat0.xy = vec2(u_xlat0.y + u_xlat0.x, u_xlat0.w + u_xlat0.z);
    u_xlat2.xyz = u_xlat0.yyy * _Wind.xyz;
    u_xlat2.xyz = u_xlat2.xyz * in_TEXCOORD1.yyy;
    u_xlat3.y = u_xlat0.y * in_TEXCOORD1.y;
    u_xlat4 = in_COLOR0.y * 0.100000001;
    u_xlat3.xz = vec2(u_xlat4) * in_NORMAL0.xz;
    u_xlat0.z = 0.300000012;
    u_xlat0.xyz = u_xlat0.xzx * u_xlat3.xyz + u_xlat2.xyz;
    u_xlat0.xyz = u_xlat0.xyz * _Wind.www + u_xlat1.xyz;
    u_xlat0.xyz = in_TEXCOORD1.xxx * _Wind.xyz + u_xlat0.xyz;
    u_xlat12 = dot(_SquashPlaneNormal.xyz, u_xlat0.xyz);
    u_xlat12 = u_xlat12 + _SquashPlaneNormal.w;
    u_xlat1.xyz = (-vec3(u_xlat12)) * _SquashPlaneNormal.xyz + u_xlat0.xyz;
    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
    u_xlat0.xyz = vec3(_SquashAmount) * u_xlat0.xyz + u_xlat1.xyz;
    u_xlat1 = u_xlat0.yyyy * glstate_matrix_mvp[1];
    u_xlat1 = glstate_matrix_mvp[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = glstate_matrix_mvp[2] * u_xlat0.zzzz + u_xlat1;
    gl_Position = u_xlat1 + glstate_matrix_mvp[3];
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    u_xlat4 = u_xlat0.y * glstate_matrix_modelview0[1].z;
    u_xlat0.x = glstate_matrix_modelview0[0].z * u_xlat0.x + u_xlat4;
    u_xlat0.x = glstate_matrix_modelview0[2].z * u_xlat0.z + u_xlat0.x;
    u_xlat0.x = u_xlat0.x + glstate_matrix_modelview0[3].z;
    u_xlat0.x = u_xlat0.x * _ProjectionParams.w;
    vs_TEXCOORD1.w = (-u_xlat0.x);
    u_xlat0.x = dot(in_NORMAL0.xyz, in_NORMAL0.xyz);
    u_xlat0.x = inversesqrt(u_xlat0.x);
    u_xlat0.xyz = u_xlat0.xxx * in_NORMAL0.xyz;
    u_xlat1.xyz = u_xlat0.yyy * glstate_matrix_invtrans_modelview0[1].xyz;
    u_xlat0.xyw = glstate_matrix_invtrans_modelview0[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[2].xyz * u_xlat0.zzz + u_xlat0.xyw;
    u_xlat12 = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat12 = inversesqrt(u_xlat12);
    vs_TEXCOORD1.xyz = vec3(u_xlat12) * u_xlat0.xyz;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
in highp vec4 vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
vec2 u_xlat1;
void main()
{
    u_xlat0.x = vs_TEXCOORD1.z + 1.0;
    u_xlat0.xy = vs_TEXCOORD1.xy / u_xlat0.xx;
    u_xlat0.xy = u_xlat0.xy * vec2(0.281262308, 0.281262308) + vec2(0.5, 0.5);
    u_xlat1.xy = vs_TEXCOORD1.ww * vec2(1.0, 255.0);
    u_xlat1.xy = fract(u_xlat1.xy);
    u_xlat0.z = (-u_xlat1.y) * 0.00392156886 + u_xlat1.x;
    u_xlat0.w = u_xlat1.y;
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
SubShader { 
 Tags { "RenderType"="TreeLeaf" }
 Pass {
  Tags { "RenderType"="TreeLeaf" }
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _Time;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp mat4 _Object2World;
uniform highp vec4 _TreeInstanceScale;
uniform highp vec4 _SquashPlaneNormal;
uniform highp float _SquashAmount;
uniform highp vec4 _Wind;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesNormal;
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec4 tmpvar_3;
  highp vec4 tmpvar_4;
  highp vec4 pos_5;
  highp float tmpvar_6;
  tmpvar_6 = (1.0 - abs(_glesTANGENT.w));
  highp vec4 tmpvar_7;
  tmpvar_7.w = 0.0;
  tmpvar_7.xyz = tmpvar_1;
  highp vec4 tmpvar_8;
  tmpvar_8.zw = vec2(0.0, 0.0);
  tmpvar_8.xy = tmpvar_1.xy;
  pos_5 = (_glesVertex + ((tmpvar_8 * glstate_matrix_invtrans_modelview0) * tmpvar_6));
  highp vec3 tmpvar_9;
  tmpvar_9 = mix (_glesNormal, normalize((tmpvar_7 * glstate_matrix_invtrans_modelview0)).xyz, vec3(tmpvar_6));
  tmpvar_4.w = pos_5.w;
  tmpvar_4.xyz = (pos_5.xyz * _TreeInstanceScale.xyz);
  highp vec4 tmpvar_10;
  tmpvar_10.xy = tmpvar_2.xy;
  tmpvar_10.zw = _glesMultiTexCoord1.xy;
  highp vec4 pos_11;
  pos_11.w = tmpvar_4.w;
  highp vec3 bend_12;
  highp vec4 v_13;
  v_13.x = _Object2World[0].w;
  v_13.y = _Object2World[1].w;
  v_13.z = _Object2World[2].w;
  v_13.w = _Object2World[3].w;
  highp float tmpvar_14;
  tmpvar_14 = (dot (v_13.xyz, vec3(1.0, 1.0, 1.0)) + tmpvar_10.x);
  highp vec2 tmpvar_15;
  tmpvar_15.x = dot (tmpvar_4.xyz, vec3((tmpvar_10.y + tmpvar_14)));
  tmpvar_15.y = tmpvar_14;
  highp vec4 tmpvar_16;
  tmpvar_16 = abs(((
    fract((((
      fract(((_Time.yy + tmpvar_15).xxyy * vec4(1.975, 0.793, 0.375, 0.193)))
     * 2.0) - 1.0) + 0.5))
   * 2.0) - 1.0));
  highp vec4 tmpvar_17;
  tmpvar_17 = ((tmpvar_16 * tmpvar_16) * (3.0 - (2.0 * tmpvar_16)));
  highp vec2 tmpvar_18;
  tmpvar_18 = (tmpvar_17.xz + tmpvar_17.yw);
  bend_12.xz = ((tmpvar_10.y * 0.1) * tmpvar_9).xz;
  bend_12.y = (_glesMultiTexCoord1.y * 0.3);
  pos_11.xyz = (tmpvar_4.xyz + ((
    (tmpvar_18.xyx * bend_12)
   + 
    ((_Wind.xyz * tmpvar_18.y) * _glesMultiTexCoord1.y)
  ) * _Wind.w));
  pos_11.xyz = (pos_11.xyz + (_glesMultiTexCoord1.x * _Wind.xyz));
  highp vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = mix ((pos_11.xyz - (
    (dot (_SquashPlaneNormal.xyz, pos_11.xyz) + _SquashPlaneNormal.w)
   * _SquashPlaneNormal.xyz)), pos_11.xyz, vec3(_SquashAmount));
  tmpvar_4 = tmpvar_19;
  highp mat3 tmpvar_20;
  tmpvar_20[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_20[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_20[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  tmpvar_3.xyz = normalize((tmpvar_20 * normalize(tmpvar_9)));
  tmpvar_3.w = -(((glstate_matrix_modelview0 * tmpvar_19).z * _ProjectionParams.w));
  gl_Position = (glstate_matrix_mvp * tmpvar_19);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform lowp float _Cutoff;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump float alpha_2;
  lowp float tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0).w;
  alpha_2 = tmpvar_3;
  mediump float x_4;
  x_4 = (alpha_2 - _Cutoff);
  if ((x_4 < 0.0)) {
    discard;
  };
  highp vec4 enc_5;
  highp vec2 enc_6;
  enc_6 = (xlv_TEXCOORD1.xy / (xlv_TEXCOORD1.z + 1.0));
  enc_6 = (enc_6 / 1.7777);
  enc_6 = ((enc_6 * 0.5) + 0.5);
  enc_5.xy = enc_6;
  highp vec2 enc_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = fract((vec2(1.0, 255.0) * xlv_TEXCOORD1.w));
  enc_7.y = tmpvar_8.y;
  enc_7.x = (tmpvar_8.x - (tmpvar_8.y * 0.003921569));
  enc_5.zw = enc_7;
  tmpvar_1 = enc_5;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _Time;
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 glstate_matrix_modelview0;
uniform 	mat4x4 glstate_matrix_invtrans_modelview0;
uniform 	mat4x4 _Object2World;
uniform 	vec4 _TreeInstanceScale;
uniform 	vec4 _SquashPlaneNormal;
uniform 	float _SquashAmount;
uniform 	vec4 _Wind;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in highp vec4 in_TEXCOORD1;
in lowp vec4 in_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
vec3 u_xlat0;
vec4 u_xlat1;
vec4 u_xlat2;
vec4 u_xlat3;
vec4 u_xlat4;
float u_xlat5;
float u_xlat10;
float u_xlat15;
void main()
{
    u_xlat0.x = _Object2World[0].w;
    u_xlat0.y = _Object2World[1].w;
    u_xlat0.z = _Object2World[2].w;
    u_xlat0.x = dot(u_xlat0.xyz, vec3(1.0, 1.0, 1.0));
    u_xlat0.y = u_xlat0.x + in_COLOR0.x;
    u_xlat10 = u_xlat0.y + in_COLOR0.y;
    u_xlat1.x = dot(in_NORMAL0.xy, glstate_matrix_invtrans_modelview0[0].xy);
    u_xlat1.y = dot(in_NORMAL0.xy, glstate_matrix_invtrans_modelview0[1].xy);
    u_xlat1.z = dot(in_NORMAL0.xy, glstate_matrix_invtrans_modelview0[2].xy);
    u_xlat15 = -abs(in_TANGENT0.w) + 1.0;
    u_xlat1.xyz = u_xlat1.xyz * vec3(u_xlat15) + in_POSITION0.xyz;
    u_xlat1.xyz = u_xlat1.xyz * _TreeInstanceScale.xyz;
    u_xlat0.x = dot(u_xlat1.xyz, vec3(u_xlat10));
    u_xlat2 = u_xlat0.xxyy + _Time.yyyy;
    u_xlat2 = u_xlat2 * vec4(1.97500002, 0.792999983, 0.375, 0.193000004);
    u_xlat2 = fract(u_xlat2);
    u_xlat2 = u_xlat2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-0.5, -0.5, -0.5, -0.5);
    u_xlat2 = fract(u_xlat2);
    u_xlat2 = u_xlat2 * vec4(2.0, 2.0, 2.0, 2.0) + vec4(-1.0, -1.0, -1.0, -1.0);
    u_xlat3 = abs(u_xlat2) * abs(u_xlat2);
    u_xlat2 = -abs(u_xlat2) * vec4(2.0, 2.0, 2.0, 2.0) + vec4(3.0, 3.0, 3.0, 3.0);
    u_xlat2 = u_xlat2 * u_xlat3;
    u_xlat0.xy = vec2(u_xlat2.y + u_xlat2.x, u_xlat2.w + u_xlat2.z);
    u_xlat2.xyz = u_xlat0.yyy * _Wind.xyz;
    u_xlat2.xyz = u_xlat2.xyz * in_TEXCOORD1.yyy;
    u_xlat3.y = u_xlat0.y * in_TEXCOORD1.y;
    u_xlat4.w = dot(in_NORMAL0.xyz, glstate_matrix_invtrans_modelview0[3].xyz);
    u_xlat4.x = dot(in_NORMAL0.xyz, glstate_matrix_invtrans_modelview0[0].xyz);
    u_xlat4.y = dot(in_NORMAL0.xyz, glstate_matrix_invtrans_modelview0[1].xyz);
    u_xlat4.z = dot(in_NORMAL0.xyz, glstate_matrix_invtrans_modelview0[2].xyz);
    u_xlat5 = dot(u_xlat4, u_xlat4);
    u_xlat5 = inversesqrt(u_xlat5);
    u_xlat4.xyz = u_xlat4.xyz * vec3(u_xlat5) + (-in_NORMAL0.xyz);
    u_xlat4.xyz = vec3(u_xlat15) * u_xlat4.xyz + in_NORMAL0.xyz;
    u_xlat5 = in_COLOR0.y * 0.100000001;
    u_xlat3.xz = u_xlat4.xz * vec2(u_xlat5);
    u_xlat0.z = 0.300000012;
    u_xlat0.xyz = u_xlat0.xzx * u_xlat3.xyz + u_xlat2.xyz;
    u_xlat0.xyz = u_xlat0.xyz * _Wind.www + u_xlat1.xyz;
    u_xlat0.xyz = in_TEXCOORD1.xxx * _Wind.xyz + u_xlat0.xyz;
    u_xlat15 = dot(_SquashPlaneNormal.xyz, u_xlat0.xyz);
    u_xlat15 = u_xlat15 + _SquashPlaneNormal.w;
    u_xlat1.xyz = (-vec3(u_xlat15)) * _SquashPlaneNormal.xyz + u_xlat0.xyz;
    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
    u_xlat0.xyz = vec3(_SquashAmount) * u_xlat0.xyz + u_xlat1.xyz;
    u_xlat1 = u_xlat0.yyyy * glstate_matrix_mvp[1];
    u_xlat1 = glstate_matrix_mvp[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = glstate_matrix_mvp[2] * u_xlat0.zzzz + u_xlat1;
    gl_Position = u_xlat1 + glstate_matrix_mvp[3];
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    u_xlat15 = dot(u_xlat4.xyz, u_xlat4.xyz);
    u_xlat15 = inversesqrt(u_xlat15);
    u_xlat1.xyz = vec3(u_xlat15) * u_xlat4.xyz;
    u_xlat2.xyz = u_xlat1.yyy * glstate_matrix_invtrans_modelview0[1].xyz;
    u_xlat1.xyw = glstate_matrix_invtrans_modelview0[0].xyz * u_xlat1.xxx + u_xlat2.xyz;
    u_xlat1.xyz = glstate_matrix_invtrans_modelview0[2].xyz * u_xlat1.zzz + u_xlat1.xyw;
    u_xlat15 = dot(u_xlat1.xyz, u_xlat1.xyz);
    u_xlat15 = inversesqrt(u_xlat15);
    vs_TEXCOORD1.xyz = vec3(u_xlat15) * u_xlat1.xyz;
    u_xlat5 = u_xlat0.y * glstate_matrix_modelview0[1].z;
    u_xlat0.x = glstate_matrix_modelview0[0].z * u_xlat0.x + u_xlat5;
    u_xlat0.x = glstate_matrix_modelview0[2].z * u_xlat0.z + u_xlat0.x;
    u_xlat0.x = u_xlat0.x + glstate_matrix_modelview0[3].z;
    u_xlat0.x = u_xlat0.x * _ProjectionParams.w;
    vs_TEXCOORD1.w = (-u_xlat0.x);
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	lowp float _Cutoff;
uniform lowp sampler2D _MainTex;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
lowp float u_xlat10_0;
bool u_xlatb0;
mediump float u_xlat16_1;
vec2 u_xlat2;
void main()
{
    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
    u_xlat16_1 = u_xlat10_0 + (-_Cutoff);
#ifdef UNITY_ADRENO_ES3
    u_xlatb0 = !!(u_xlat16_1<0.0);
#else
    u_xlatb0 = u_xlat16_1<0.0;
#endif
    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
    u_xlat0.x = vs_TEXCOORD1.z + 1.0;
    u_xlat0.xy = vs_TEXCOORD1.xy / u_xlat0.xx;
    u_xlat0.xy = u_xlat0.xy * vec2(0.281262308, 0.281262308) + vec2(0.5, 0.5);
    u_xlat2.xy = vs_TEXCOORD1.ww * vec2(1.0, 255.0);
    u_xlat2.xy = fract(u_xlat2.xy);
    u_xlat0.z = (-u_xlat2.y) * 0.00392156886 + u_xlat2.x;
    u_xlat0.w = u_xlat2.y;
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
SubShader { 
 Tags { "DisableBatching"="true" "RenderType"="TreeOpaque" }
 Pass {
  Tags { "DisableBatching"="true" "RenderType"="TreeOpaque" }
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp vec4 _TreeInstanceScale;
uniform highp mat4 _TerrainEngineBendTree;
uniform highp vec4 _SquashPlaneNormal;
uniform highp float _SquashAmount;
varying highp vec4 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  highp vec4 pos_3;
  pos_3.w = _glesVertex.w;
  highp float alpha_4;
  alpha_4 = tmpvar_1.w;
  pos_3.xyz = (_glesVertex.xyz * _TreeInstanceScale.xyz);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 0.0;
  tmpvar_5.xyz = pos_3.xyz;
  pos_3.xyz = mix (pos_3.xyz, (_TerrainEngineBendTree * tmpvar_5).xyz, vec3(alpha_4));
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = mix ((pos_3.xyz - (
    (dot (_SquashPlaneNormal.xyz, pos_3.xyz) + _SquashPlaneNormal.w)
   * _SquashPlaneNormal.xyz)), pos_3.xyz, vec3(_SquashAmount));
  pos_3 = tmpvar_6;
  highp mat3 tmpvar_7;
  tmpvar_7[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_7[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_7[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  tmpvar_2.xyz = normalize((tmpvar_7 * _glesNormal));
  tmpvar_2.w = -(((glstate_matrix_modelview0 * tmpvar_6).z * _ProjectionParams.w));
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
varying highp vec4 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  highp vec4 enc_2;
  highp vec2 enc_3;
  enc_3 = (xlv_TEXCOORD0.xy / (xlv_TEXCOORD0.z + 1.0));
  enc_3 = (enc_3 / 1.7777);
  enc_3 = ((enc_3 * 0.5) + 0.5);
  enc_2.xy = enc_3;
  highp vec2 enc_4;
  highp vec2 tmpvar_5;
  tmpvar_5 = fract((vec2(1.0, 255.0) * xlv_TEXCOORD0.w));
  enc_4.y = tmpvar_5.y;
  enc_4.x = (tmpvar_5.x - (tmpvar_5.y * 0.003921569));
  enc_2.zw = enc_4;
  tmpvar_1 = enc_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 glstate_matrix_modelview0;
uniform 	mat4x4 glstate_matrix_invtrans_modelview0;
uniform 	vec4 _TreeInstanceScale;
uniform 	mat4x4 _TerrainEngineBendTree;
uniform 	vec4 _SquashPlaneNormal;
uniform 	float _SquashAmount;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in lowp vec4 in_COLOR0;
out highp vec4 vs_TEXCOORD0;
vec3 u_xlat0;
vec4 u_xlat1;
float u_xlat2;
float u_xlat6;
void main()
{
    u_xlat0.xyz = in_POSITION0.xyz * _TreeInstanceScale.xyz;
    u_xlat1.xyz = u_xlat0.yyy * _TerrainEngineBendTree[1].xyz;
    u_xlat1.xyz = _TerrainEngineBendTree[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
    u_xlat1.xyz = _TerrainEngineBendTree[2].xyz * u_xlat0.zzz + u_xlat1.xyz;
    u_xlat1.xyz = (-in_POSITION0.xyz) * _TreeInstanceScale.xyz + u_xlat1.xyz;
    u_xlat0.xyz = in_COLOR0.www * u_xlat1.xyz + u_xlat0.xyz;
    u_xlat6 = dot(_SquashPlaneNormal.xyz, u_xlat0.xyz);
    u_xlat6 = u_xlat6 + _SquashPlaneNormal.w;
    u_xlat1.xyz = (-vec3(u_xlat6)) * _SquashPlaneNormal.xyz + u_xlat0.xyz;
    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
    u_xlat0.xyz = vec3(_SquashAmount) * u_xlat0.xyz + u_xlat1.xyz;
    u_xlat1 = u_xlat0.yyyy * glstate_matrix_mvp[1];
    u_xlat1 = glstate_matrix_mvp[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = glstate_matrix_mvp[2] * u_xlat0.zzzz + u_xlat1;
    gl_Position = u_xlat1 + glstate_matrix_mvp[3];
    u_xlat2 = u_xlat0.y * glstate_matrix_modelview0[1].z;
    u_xlat0.x = glstate_matrix_modelview0[0].z * u_xlat0.x + u_xlat2;
    u_xlat0.x = glstate_matrix_modelview0[2].z * u_xlat0.z + u_xlat0.x;
    u_xlat0.x = u_xlat0.x + glstate_matrix_modelview0[3].z;
    u_xlat0.x = u_xlat0.x * _ProjectionParams.w;
    vs_TEXCOORD0.w = (-u_xlat0.x);
    u_xlat0.xyz = in_NORMAL0.yyy * glstate_matrix_invtrans_modelview0[1].xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[0].xyz * in_NORMAL0.xxx + u_xlat0.xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[2].xyz * in_NORMAL0.zzz + u_xlat0.xyz;
    u_xlat6 = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat6 = inversesqrt(u_xlat6);
    vs_TEXCOORD0.xyz = vec3(u_xlat6) * u_xlat0.xyz;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
in highp vec4 vs_TEXCOORD0;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
vec2 u_xlat1;
void main()
{
    u_xlat0.x = vs_TEXCOORD0.z + 1.0;
    u_xlat0.xy = vs_TEXCOORD0.xy / u_xlat0.xx;
    u_xlat0.xy = u_xlat0.xy * vec2(0.281262308, 0.281262308) + vec2(0.5, 0.5);
    u_xlat1.xy = vs_TEXCOORD0.ww * vec2(1.0, 255.0);
    u_xlat1.xy = fract(u_xlat1.xy);
    u_xlat0.z = (-u_xlat1.y) * 0.00392156886 + u_xlat1.x;
    u_xlat0.w = u_xlat1.y;
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
SubShader { 
 Tags { "DisableBatching"="true" "RenderType"="TreeTransparentCutout" }
 Pass {
  Tags { "DisableBatching"="true" "RenderType"="TreeTransparentCutout" }
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp vec4 _TreeInstanceScale;
uniform highp mat4 _TerrainEngineBendTree;
uniform highp vec4 _SquashPlaneNormal;
uniform highp float _SquashAmount;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  highp vec4 pos_3;
  pos_3.w = _glesVertex.w;
  highp float alpha_4;
  alpha_4 = tmpvar_1.w;
  pos_3.xyz = (_glesVertex.xyz * _TreeInstanceScale.xyz);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 0.0;
  tmpvar_5.xyz = pos_3.xyz;
  pos_3.xyz = mix (pos_3.xyz, (_TerrainEngineBendTree * tmpvar_5).xyz, vec3(alpha_4));
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = mix ((pos_3.xyz - (
    (dot (_SquashPlaneNormal.xyz, pos_3.xyz) + _SquashPlaneNormal.w)
   * _SquashPlaneNormal.xyz)), pos_3.xyz, vec3(_SquashAmount));
  pos_3 = tmpvar_6;
  highp mat3 tmpvar_7;
  tmpvar_7[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_7[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_7[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  tmpvar_2.xyz = normalize((tmpvar_7 * _glesNormal));
  tmpvar_2.w = -(((glstate_matrix_modelview0 * tmpvar_6).z * _ProjectionParams.w));
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform lowp float _Cutoff;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump float alpha_2;
  lowp float tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0).w;
  alpha_2 = tmpvar_3;
  mediump float x_4;
  x_4 = (alpha_2 - _Cutoff);
  if ((x_4 < 0.0)) {
    discard;
  };
  highp vec4 enc_5;
  highp vec2 enc_6;
  enc_6 = (xlv_TEXCOORD1.xy / (xlv_TEXCOORD1.z + 1.0));
  enc_6 = (enc_6 / 1.7777);
  enc_6 = ((enc_6 * 0.5) + 0.5);
  enc_5.xy = enc_6;
  highp vec2 enc_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = fract((vec2(1.0, 255.0) * xlv_TEXCOORD1.w));
  enc_7.y = tmpvar_8.y;
  enc_7.x = (tmpvar_8.x - (tmpvar_8.y * 0.003921569));
  enc_5.zw = enc_7;
  tmpvar_1 = enc_5;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 glstate_matrix_modelview0;
uniform 	mat4x4 glstate_matrix_invtrans_modelview0;
uniform 	vec4 _TreeInstanceScale;
uniform 	mat4x4 _TerrainEngineBendTree;
uniform 	vec4 _SquashPlaneNormal;
uniform 	float _SquashAmount;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in lowp vec4 in_COLOR0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
vec3 u_xlat0;
vec4 u_xlat1;
float u_xlat2;
float u_xlat6;
void main()
{
    u_xlat0.xyz = in_POSITION0.xyz * _TreeInstanceScale.xyz;
    u_xlat1.xyz = u_xlat0.yyy * _TerrainEngineBendTree[1].xyz;
    u_xlat1.xyz = _TerrainEngineBendTree[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
    u_xlat1.xyz = _TerrainEngineBendTree[2].xyz * u_xlat0.zzz + u_xlat1.xyz;
    u_xlat1.xyz = (-in_POSITION0.xyz) * _TreeInstanceScale.xyz + u_xlat1.xyz;
    u_xlat0.xyz = in_COLOR0.www * u_xlat1.xyz + u_xlat0.xyz;
    u_xlat6 = dot(_SquashPlaneNormal.xyz, u_xlat0.xyz);
    u_xlat6 = u_xlat6 + _SquashPlaneNormal.w;
    u_xlat1.xyz = (-vec3(u_xlat6)) * _SquashPlaneNormal.xyz + u_xlat0.xyz;
    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
    u_xlat0.xyz = vec3(_SquashAmount) * u_xlat0.xyz + u_xlat1.xyz;
    u_xlat1 = u_xlat0.yyyy * glstate_matrix_mvp[1];
    u_xlat1 = glstate_matrix_mvp[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = glstate_matrix_mvp[2] * u_xlat0.zzzz + u_xlat1;
    gl_Position = u_xlat1 + glstate_matrix_mvp[3];
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    u_xlat2 = u_xlat0.y * glstate_matrix_modelview0[1].z;
    u_xlat0.x = glstate_matrix_modelview0[0].z * u_xlat0.x + u_xlat2;
    u_xlat0.x = glstate_matrix_modelview0[2].z * u_xlat0.z + u_xlat0.x;
    u_xlat0.x = u_xlat0.x + glstate_matrix_modelview0[3].z;
    u_xlat0.x = u_xlat0.x * _ProjectionParams.w;
    vs_TEXCOORD1.w = (-u_xlat0.x);
    u_xlat0.xyz = in_NORMAL0.yyy * glstate_matrix_invtrans_modelview0[1].xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[0].xyz * in_NORMAL0.xxx + u_xlat0.xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[2].xyz * in_NORMAL0.zzz + u_xlat0.xyz;
    u_xlat6 = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat6 = inversesqrt(u_xlat6);
    vs_TEXCOORD1.xyz = vec3(u_xlat6) * u_xlat0.xyz;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	lowp float _Cutoff;
uniform lowp sampler2D _MainTex;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
lowp float u_xlat10_0;
bool u_xlatb0;
mediump float u_xlat16_1;
vec2 u_xlat2;
void main()
{
    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
    u_xlat16_1 = u_xlat10_0 + (-_Cutoff);
#ifdef UNITY_ADRENO_ES3
    u_xlatb0 = !!(u_xlat16_1<0.0);
#else
    u_xlatb0 = u_xlat16_1<0.0;
#endif
    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
    u_xlat0.x = vs_TEXCOORD1.z + 1.0;
    u_xlat0.xy = vs_TEXCOORD1.xy / u_xlat0.xx;
    u_xlat0.xy = u_xlat0.xy * vec2(0.281262308, 0.281262308) + vec2(0.5, 0.5);
    u_xlat2.xy = vs_TEXCOORD1.ww * vec2(1.0, 255.0);
    u_xlat2.xy = fract(u_xlat2.xy);
    u_xlat0.z = (-u_xlat2.y) * 0.00392156886 + u_xlat2.x;
    u_xlat0.w = u_xlat2.y;
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
 Pass {
  Tags { "DisableBatching"="true" "RenderType"="TreeTransparentCutout" }
  Cull Front
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform highp vec4 _TreeInstanceScale;
uniform highp mat4 _TerrainEngineBendTree;
uniform highp vec4 _SquashPlaneNormal;
uniform highp float _SquashAmount;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = _glesColor;
  highp vec4 tmpvar_2;
  highp vec4 pos_3;
  pos_3.w = _glesVertex.w;
  highp float alpha_4;
  alpha_4 = tmpvar_1.w;
  pos_3.xyz = (_glesVertex.xyz * _TreeInstanceScale.xyz);
  highp vec4 tmpvar_5;
  tmpvar_5.w = 0.0;
  tmpvar_5.xyz = pos_3.xyz;
  pos_3.xyz = mix (pos_3.xyz, (_TerrainEngineBendTree * tmpvar_5).xyz, vec3(alpha_4));
  highp vec4 tmpvar_6;
  tmpvar_6.w = 1.0;
  tmpvar_6.xyz = mix ((pos_3.xyz - (
    (dot (_SquashPlaneNormal.xyz, pos_3.xyz) + _SquashPlaneNormal.w)
   * _SquashPlaneNormal.xyz)), pos_3.xyz, vec3(_SquashAmount));
  pos_3 = tmpvar_6;
  highp mat3 tmpvar_7;
  tmpvar_7[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_7[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_7[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  tmpvar_2.xyz = -(normalize((tmpvar_7 * _glesNormal)));
  tmpvar_2.w = -(((glstate_matrix_modelview0 * tmpvar_6).z * _ProjectionParams.w));
  gl_Position = (glstate_matrix_mvp * tmpvar_6);
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_2;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform lowp float _Cutoff;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  lowp float x_2;
  x_2 = (texture2D (_MainTex, xlv_TEXCOORD0).w - _Cutoff);
  if ((x_2 < 0.0)) {
    discard;
  };
  highp vec4 enc_3;
  highp vec2 enc_4;
  enc_4 = (xlv_TEXCOORD1.xy / (xlv_TEXCOORD1.z + 1.0));
  enc_4 = (enc_4 / 1.7777);
  enc_4 = ((enc_4 * 0.5) + 0.5);
  enc_3.xy = enc_4;
  highp vec2 enc_5;
  highp vec2 tmpvar_6;
  tmpvar_6 = fract((vec2(1.0, 255.0) * xlv_TEXCOORD1.w));
  enc_5.y = tmpvar_6.y;
  enc_5.x = (tmpvar_6.x - (tmpvar_6.y * 0.003921569));
  enc_3.zw = enc_5;
  tmpvar_1 = enc_3;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 glstate_matrix_modelview0;
uniform 	mat4x4 glstate_matrix_invtrans_modelview0;
uniform 	vec4 _TreeInstanceScale;
uniform 	mat4x4 _TerrainEngineBendTree;
uniform 	vec4 _SquashPlaneNormal;
uniform 	float _SquashAmount;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in lowp vec4 in_COLOR0;
in highp vec4 in_TEXCOORD0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
vec3 u_xlat0;
vec4 u_xlat1;
float u_xlat2;
float u_xlat6;
void main()
{
    u_xlat0.xyz = in_POSITION0.xyz * _TreeInstanceScale.xyz;
    u_xlat1.xyz = u_xlat0.yyy * _TerrainEngineBendTree[1].xyz;
    u_xlat1.xyz = _TerrainEngineBendTree[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
    u_xlat1.xyz = _TerrainEngineBendTree[2].xyz * u_xlat0.zzz + u_xlat1.xyz;
    u_xlat1.xyz = (-in_POSITION0.xyz) * _TreeInstanceScale.xyz + u_xlat1.xyz;
    u_xlat0.xyz = in_COLOR0.www * u_xlat1.xyz + u_xlat0.xyz;
    u_xlat6 = dot(_SquashPlaneNormal.xyz, u_xlat0.xyz);
    u_xlat6 = u_xlat6 + _SquashPlaneNormal.w;
    u_xlat1.xyz = (-vec3(u_xlat6)) * _SquashPlaneNormal.xyz + u_xlat0.xyz;
    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
    u_xlat0.xyz = vec3(_SquashAmount) * u_xlat0.xyz + u_xlat1.xyz;
    u_xlat1 = u_xlat0.yyyy * glstate_matrix_mvp[1];
    u_xlat1 = glstate_matrix_mvp[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = glstate_matrix_mvp[2] * u_xlat0.zzzz + u_xlat1;
    gl_Position = u_xlat1 + glstate_matrix_mvp[3];
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    u_xlat2 = u_xlat0.y * glstate_matrix_modelview0[1].z;
    u_xlat0.x = glstate_matrix_modelview0[0].z * u_xlat0.x + u_xlat2;
    u_xlat0.x = glstate_matrix_modelview0[2].z * u_xlat0.z + u_xlat0.x;
    u_xlat0.x = u_xlat0.x + glstate_matrix_modelview0[3].z;
    u_xlat0.x = u_xlat0.x * _ProjectionParams.w;
    vs_TEXCOORD1.w = (-u_xlat0.x);
    u_xlat0.xyz = in_NORMAL0.yyy * glstate_matrix_invtrans_modelview0[1].xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[0].xyz * in_NORMAL0.xxx + u_xlat0.xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[2].xyz * in_NORMAL0.zzz + u_xlat0.xyz;
    u_xlat6 = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat6 = inversesqrt(u_xlat6);
    u_xlat0.xyz = vec3(u_xlat6) * u_xlat0.xyz;
    vs_TEXCOORD1.xyz = (-u_xlat0.xyz);
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	lowp float _Cutoff;
uniform lowp sampler2D _MainTex;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
lowp float u_xlat10_0;
bool u_xlatb0;
lowp float u_xlat10_1;
vec2 u_xlat2;
void main()
{
    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
    u_xlat10_1 = u_xlat10_0 + (-_Cutoff);
#ifdef UNITY_ADRENO_ES3
    u_xlatb0 = !!(u_xlat10_1<0.0);
#else
    u_xlatb0 = u_xlat10_1<0.0;
#endif
    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
    u_xlat0.x = vs_TEXCOORD1.z + 1.0;
    u_xlat0.xy = vs_TEXCOORD1.xy / u_xlat0.xx;
    u_xlat0.xy = u_xlat0.xy * vec2(0.281262308, 0.281262308) + vec2(0.5, 0.5);
    u_xlat2.xy = vs_TEXCOORD1.ww * vec2(1.0, 255.0);
    u_xlat2.xy = fract(u_xlat2.xy);
    u_xlat0.z = (-u_xlat2.y) * 0.00392156886 + u_xlat2.x;
    u_xlat0.w = u_xlat2.y;
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
SubShader { 
 Tags { "RenderType"="TreeBillboard" }
 Pass {
  Tags { "RenderType"="TreeBillboard" }
  Cull Off
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp vec3 _TreeBillboardCameraRight;
uniform highp vec4 _TreeBillboardCameraUp;
uniform highp vec4 _TreeBillboardCameraFront;
uniform highp vec4 _TreeBillboardCameraPos;
uniform highp vec4 _TreeBillboardDistances;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  highp vec2 tmpvar_2;
  highp vec4 tmpvar_3;
  highp vec4 pos_4;
  pos_4 = _glesVertex;
  highp vec2 offset_5;
  offset_5 = _glesMultiTexCoord1.xy;
  highp float offsetz_6;
  offsetz_6 = tmpvar_1.y;
  highp vec3 tmpvar_7;
  tmpvar_7 = (_glesVertex.xyz - _TreeBillboardCameraPos.xyz);
  highp float tmpvar_8;
  tmpvar_8 = dot (tmpvar_7, tmpvar_7);
  if ((tmpvar_8 > _TreeBillboardDistances.x)) {
    offsetz_6 = 0.0;
    offset_5 = vec2(0.0, 0.0);
  };
  pos_4.xyz = (_glesVertex.xyz + (_TreeBillboardCameraRight * offset_5.x));
  pos_4.xyz = (pos_4.xyz + (_TreeBillboardCameraUp.xyz * mix (offset_5.y, offsetz_6, _TreeBillboardCameraPos.w)));
  pos_4.xyz = (pos_4.xyz + ((_TreeBillboardCameraFront.xyz * 
    abs(offset_5.x)
  ) * _TreeBillboardCameraUp.w));
  tmpvar_2.x = tmpvar_1.x;
  tmpvar_2.y = float((_glesMultiTexCoord0.y > 0.0));
  tmpvar_3.xyz = vec3(0.0, 0.0, 1.0);
  tmpvar_3.w = -(((glstate_matrix_modelview0 * pos_4).z * _ProjectionParams.w));
  gl_Position = (glstate_matrix_mvp * pos_4);
  xlv_TEXCOORD0 = tmpvar_2;
  xlv_TEXCOORD1 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  lowp float x_2;
  x_2 = (texture2D (_MainTex, xlv_TEXCOORD0).w - 0.001);
  if ((x_2 < 0.0)) {
    discard;
  };
  highp vec4 enc_3;
  highp vec2 enc_4;
  enc_4 = (xlv_TEXCOORD1.xy / (xlv_TEXCOORD1.z + 1.0));
  enc_4 = (enc_4 / 1.7777);
  enc_4 = ((enc_4 * 0.5) + 0.5);
  enc_3.xy = enc_4;
  highp vec2 enc_5;
  highp vec2 tmpvar_6;
  tmpvar_6 = fract((vec2(1.0, 255.0) * xlv_TEXCOORD1.w));
  enc_5.y = tmpvar_6.y;
  enc_5.x = (tmpvar_6.x - (tmpvar_6.y * 0.003921569));
  enc_3.zw = enc_5;
  tmpvar_1 = enc_3;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 glstate_matrix_modelview0;
uniform 	vec3 _TreeBillboardCameraRight;
uniform 	vec4 _TreeBillboardCameraUp;
uniform 	vec4 _TreeBillboardCameraFront;
uniform 	vec4 _TreeBillboardCameraPos;
uniform 	vec4 _TreeBillboardDistances;
in highp vec4 in_POSITION0;
in highp vec4 in_TEXCOORD0;
in highp vec2 in_TEXCOORD1;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
vec4 u_xlat0;
bool u_xlatb0;
vec4 u_xlat1;
float u_xlat2;
float u_xlat4;
bool u_xlatb6;
void main()
{
    u_xlat0.xyz = in_POSITION0.xyz + (-_TreeBillboardCameraPos.xyz);
    u_xlat0.x = dot(u_xlat0.xyz, u_xlat0.xyz);
#ifdef UNITY_ADRENO_ES3
    u_xlatb0 = !!(_TreeBillboardDistances.x<u_xlat0.x);
#else
    u_xlatb0 = _TreeBillboardDistances.x<u_xlat0.x;
#endif
    u_xlat1.xy = in_TEXCOORD1.xy;
    u_xlat1.z = in_TEXCOORD0.y;
    u_xlat0.xyz = (bool(u_xlatb0)) ? vec3(0.0, 0.0, 0.0) : u_xlat1.xyz;
    u_xlat4 = (-u_xlat0.y) + u_xlat0.z;
    u_xlat2 = _TreeBillboardCameraPos.w * u_xlat4 + u_xlat0.y;
    u_xlat1.xyz = vec3(_TreeBillboardCameraRight.x, _TreeBillboardCameraRight.y, _TreeBillboardCameraRight.z) * u_xlat0.xxx + in_POSITION0.xyz;
    u_xlat0.xzw = abs(u_xlat0.xxx) * _TreeBillboardCameraFront.xyz;
    u_xlat1.xyz = _TreeBillboardCameraUp.xyz * vec3(u_xlat2) + u_xlat1.xyz;
    u_xlat0.xyz = u_xlat0.xzw * _TreeBillboardCameraUp.www + u_xlat1.xyz;
    u_xlat1 = u_xlat0.yyyy * glstate_matrix_mvp[1];
    u_xlat1 = glstate_matrix_mvp[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = glstate_matrix_mvp[2] * u_xlat0.zzzz + u_xlat1;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat1;
#ifdef UNITY_ADRENO_ES3
    u_xlatb6 = !!(0.0<in_TEXCOORD0.y);
#else
    u_xlatb6 = 0.0<in_TEXCOORD0.y;
#endif
    vs_TEXCOORD0.y = u_xlatb6 ? 1.0 : float(0.0);
    vs_TEXCOORD0.x = in_TEXCOORD0.x;
    u_xlat2 = u_xlat0.y * glstate_matrix_modelview0[1].z;
    u_xlat0.x = glstate_matrix_modelview0[0].z * u_xlat0.x + u_xlat2;
    u_xlat0.x = glstate_matrix_modelview0[2].z * u_xlat0.z + u_xlat0.x;
    u_xlat0.x = glstate_matrix_modelview0[3].z * in_POSITION0.w + u_xlat0.x;
    u_xlat0.x = u_xlat0.x * _ProjectionParams.w;
    vs_TEXCOORD1.w = (-u_xlat0.x);
    vs_TEXCOORD1.xyz = vec3(0.0, 0.0, 1.0);
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform lowp sampler2D _MainTex;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
lowp float u_xlat10_0;
bool u_xlatb0;
lowp float u_xlat10_1;
vec2 u_xlat2;
void main()
{
    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
    u_xlat10_1 = u_xlat10_0 + -0.00100000005;
#ifdef UNITY_ADRENO_ES3
    u_xlatb0 = !!(u_xlat10_1<0.0);
#else
    u_xlatb0 = u_xlat10_1<0.0;
#endif
    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
    u_xlat0.x = vs_TEXCOORD1.z + 1.0;
    u_xlat0.xy = vs_TEXCOORD1.xy / u_xlat0.xx;
    u_xlat0.xy = u_xlat0.xy * vec2(0.281262308, 0.281262308) + vec2(0.5, 0.5);
    u_xlat2.xy = vs_TEXCOORD1.ww * vec2(1.0, 255.0);
    u_xlat2.xy = fract(u_xlat2.xy);
    u_xlat0.z = (-u_xlat2.y) * 0.00392156886 + u_xlat2.x;
    u_xlat0.w = u_xlat2.y;
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
SubShader { 
 Tags { "RenderType"="GrassBillboard" }
 Pass {
  Tags { "RenderType"="GrassBillboard" }
  Cull Off
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesTANGENT;
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform lowp vec4 _WavingTint;
uniform highp vec4 _WaveAndDistance;
uniform highp vec4 _CameraPosition;
uniform highp vec3 _CameraRight;
uniform highp vec3 _CameraUp;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  tmpvar_1 = _glesMultiTexCoord0;
  lowp vec4 tmpvar_2;
  tmpvar_2 = _glesColor;
  highp vec4 tmpvar_3;
  highp vec4 pos_4;
  pos_4 = _glesVertex;
  highp vec2 offset_5;
  offset_5 = _glesTANGENT.xy;
  highp vec3 tmpvar_6;
  tmpvar_6 = (_glesVertex.xyz - _CameraPosition.xyz);
  highp float tmpvar_7;
  tmpvar_7 = dot (tmpvar_6, tmpvar_6);
  if ((tmpvar_7 > _WaveAndDistance.w)) {
    offset_5 = vec2(0.0, 0.0);
  };
  pos_4.xyz = (_glesVertex.xyz + (offset_5.x * _CameraRight));
  pos_4.xyz = (pos_4.xyz + (offset_5.y * _CameraUp));
  highp vec4 vertex_8;
  vertex_8.yw = pos_4.yw;
  lowp vec4 color_9;
  color_9.xyz = tmpvar_2.xyz;
  lowp vec3 waveColor_10;
  highp vec3 waveMove_11;
  highp vec4 s_12;
  highp vec4 waves_13;
  waves_13 = (pos_4.x * (vec4(0.012, 0.02, 0.06, 0.024) * _WaveAndDistance.y));
  waves_13 = (waves_13 + (pos_4.z * (vec4(0.006, 0.02, 0.02, 0.05) * _WaveAndDistance.y)));
  waves_13 = (waves_13 + (_WaveAndDistance.x * vec4(1.2, 2.0, 1.6, 4.8)));
  highp vec4 tmpvar_14;
  tmpvar_14 = fract(waves_13);
  waves_13 = tmpvar_14;
  highp vec4 val_15;
  highp vec4 s_16;
  val_15 = ((tmpvar_14 * 6.408849) - 3.141593);
  highp vec4 tmpvar_17;
  tmpvar_17 = (val_15 * val_15);
  highp vec4 tmpvar_18;
  tmpvar_18 = (tmpvar_17 * val_15);
  highp vec4 tmpvar_19;
  tmpvar_19 = (tmpvar_18 * tmpvar_17);
  s_16 = (((val_15 + 
    (tmpvar_18 * -0.1616162)
  ) + (tmpvar_19 * 0.0083333)) + ((tmpvar_19 * tmpvar_17) * -0.00019841));
  s_12 = (s_16 * s_16);
  s_12 = (s_12 * s_12);
  highp float tmpvar_20;
  tmpvar_20 = (dot (s_12, vec4(0.6741998, 0.6741998, 0.2696799, 0.13484)) * 0.7);
  s_12 = (s_12 * _glesTANGENT.y);
  waveMove_11.y = 0.0;
  waveMove_11.x = dot (s_12, vec4(0.024, 0.04, -0.12, 0.096));
  waveMove_11.z = dot (s_12, vec4(0.006, 0.02, -0.02, 0.1));
  vertex_8.xz = (pos_4.xz - (waveMove_11.xz * _WaveAndDistance.z));
  highp vec3 tmpvar_21;
  tmpvar_21 = mix (vec3(0.5, 0.5, 0.5), _WavingTint.xyz, vec3(tmpvar_20));
  waveColor_10 = tmpvar_21;
  highp vec3 tmpvar_22;
  tmpvar_22 = (vertex_8.xyz - _CameraPosition.xyz);
  highp float tmpvar_23;
  tmpvar_23 = clamp (((2.0 * 
    (_WaveAndDistance.w - dot (tmpvar_22, tmpvar_22))
  ) * _CameraPosition.w), 0.0, 1.0);
  color_9.w = tmpvar_23;
  lowp vec4 tmpvar_24;
  tmpvar_24.xyz = ((2.0 * waveColor_10) * _glesColor.xyz);
  tmpvar_24.w = color_9.w;
  highp mat3 tmpvar_25;
  tmpvar_25[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_25[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_25[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  tmpvar_3.xyz = normalize((tmpvar_25 * _glesNormal));
  tmpvar_3.w = -(((glstate_matrix_modelview0 * vertex_8).z * _ProjectionParams.w));
  gl_Position = (glstate_matrix_mvp * vertex_8);
  xlv_COLOR = tmpvar_24;
  xlv_TEXCOORD0 = tmpvar_1.xy;
  xlv_TEXCOORD1 = tmpvar_3;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform lowp float _Cutoff;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  lowp float x_2;
  x_2 = ((texture2D (_MainTex, xlv_TEXCOORD0).w * xlv_COLOR.w) - _Cutoff);
  if ((x_2 < 0.0)) {
    discard;
  };
  highp vec4 enc_3;
  highp vec2 enc_4;
  enc_4 = (xlv_TEXCOORD1.xy / (xlv_TEXCOORD1.z + 1.0));
  enc_4 = (enc_4 / 1.7777);
  enc_4 = ((enc_4 * 0.5) + 0.5);
  enc_3.xy = enc_4;
  highp vec2 enc_5;
  highp vec2 tmpvar_6;
  tmpvar_6 = fract((vec2(1.0, 255.0) * xlv_TEXCOORD1.w));
  enc_5.y = tmpvar_6.y;
  enc_5.x = (tmpvar_6.x - (tmpvar_6.y * 0.003921569));
  enc_3.zw = enc_5;
  tmpvar_1 = enc_3;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 glstate_matrix_modelview0;
uniform 	mat4x4 glstate_matrix_invtrans_modelview0;
uniform 	lowp vec4 _WavingTint;
uniform 	vec4 _WaveAndDistance;
uniform 	vec4 _CameraPosition;
uniform 	vec3 _CameraRight;
uniform 	vec3 _CameraUp;
in highp vec4 in_POSITION0;
in highp vec4 in_TANGENT0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in lowp vec4 in_COLOR0;
out lowp vec4 vs_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
vec4 u_xlat0;
bool u_xlatb0;
vec4 u_xlat1;
vec4 u_xlat2;
vec4 u_xlat3;
lowp vec3 u_xlat10_4;
float u_xlat15;
void main()
{
    u_xlat0.xyz = in_POSITION0.xyz + (-_CameraPosition.xyz);
    u_xlat0.x = dot(u_xlat0.xyz, u_xlat0.xyz);
#ifdef UNITY_ADRENO_ES3
    u_xlatb0 = !!(_WaveAndDistance.w<u_xlat0.x);
#else
    u_xlatb0 = _WaveAndDistance.w<u_xlat0.x;
#endif
    u_xlat0.xy = (bool(u_xlatb0)) ? vec2(0.0, 0.0) : in_TANGENT0.xy;
    u_xlat0.xzw = u_xlat0.xxx * _CameraRight.xyz + in_POSITION0.xyz;
    u_xlat0.xyz = u_xlat0.yyy * _CameraUp.xyz + u_xlat0.xzw;
    u_xlat1.xy = u_xlat0.xz * _WaveAndDistance.yy;
    u_xlat2 = u_xlat1.yyyy * vec4(0.00600000005, 0.0199999996, 0.0199999996, 0.0500000007);
    u_xlat1 = u_xlat1.xxxx * vec4(0.0120000001, 0.0199999996, 0.0599999987, 0.0240000002) + u_xlat2;
    u_xlat1 = _WaveAndDistance.xxxx * vec4(1.20000005, 2.0, 1.60000002, 4.80000019) + u_xlat1;
    u_xlat1 = fract(u_xlat1);
    u_xlat1 = u_xlat1 * vec4(6.40884876, 6.40884876, 6.40884876, 6.40884876) + vec4(-3.14159274, -3.14159274, -3.14159274, -3.14159274);
    u_xlat2 = u_xlat1 * u_xlat1;
    u_xlat3 = u_xlat1 * u_xlat2;
    u_xlat1 = u_xlat3 * vec4(-0.161616161, -0.161616161, -0.161616161, -0.161616161) + u_xlat1;
    u_xlat3 = u_xlat2 * u_xlat3;
    u_xlat2 = u_xlat2 * u_xlat3;
    u_xlat1 = u_xlat3 * vec4(0.00833330024, 0.00833330024, 0.00833330024, 0.00833330024) + u_xlat1;
    u_xlat1 = u_xlat2 * vec4(-0.000198409994, -0.000198409994, -0.000198409994, -0.000198409994) + u_xlat1;
    u_xlat1 = u_xlat1 * u_xlat1;
    u_xlat1 = u_xlat1 * u_xlat1;
    u_xlat2 = u_xlat1 * in_TANGENT0.yyyy;
    u_xlat15 = dot(u_xlat1, vec4(0.674199879, 0.674199879, 0.269679934, 0.134839967));
    u_xlat15 = u_xlat15 * 0.699999988;
    u_xlat1.x = dot(u_xlat2, vec4(0.0240000002, 0.0399999991, -0.119999997, 0.0960000008));
    u_xlat1.z = dot(u_xlat2, vec4(0.00600000005, 0.0199999996, -0.0199999996, 0.100000001));
    u_xlat0.xz = (-u_xlat1.xz) * _WaveAndDistance.zz + u_xlat0.xz;
    u_xlat1 = u_xlat0.yyyy * glstate_matrix_mvp[1];
    u_xlat1 = glstate_matrix_mvp[0] * u_xlat0.xxxx + u_xlat1;
    u_xlat1 = glstate_matrix_mvp[2] * u_xlat0.zzzz + u_xlat1;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat1;
    u_xlat1.x = u_xlat0.y * glstate_matrix_modelview0[1].z;
    u_xlat1.x = glstate_matrix_modelview0[0].z * u_xlat0.x + u_xlat1.x;
    u_xlat1.x = glstate_matrix_modelview0[2].z * u_xlat0.z + u_xlat1.x;
    u_xlat0.xyz = u_xlat0.xyz + (-_CameraPosition.xyz);
    u_xlat0.x = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat0.x = (-u_xlat0.x) + _WaveAndDistance.w;
    u_xlat0.x = dot(_CameraPosition.ww, u_xlat0.xx);
#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    vs_COLOR0.w = u_xlat0.x;
    u_xlat0.x = glstate_matrix_modelview0[3].z * in_POSITION0.w + u_xlat1.x;
    u_xlat0.x = u_xlat0.x * _ProjectionParams.w;
    vs_TEXCOORD1.w = (-u_xlat0.x);
    u_xlat10_4.xyz = _WavingTint.xyz + vec3(-0.5, -0.5, -0.5);
    u_xlat10_4.xyz = vec3(u_xlat15) * u_xlat10_4.xyz + vec3(0.5, 0.5, 0.5);
    u_xlat10_4.xyz = u_xlat10_4.xyz * in_COLOR0.xyz;
    vs_COLOR0.xyz = u_xlat10_4.xyz * vec3(2.0, 2.0, 2.0);
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    u_xlat0.xyz = in_NORMAL0.yyy * glstate_matrix_invtrans_modelview0[1].xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[0].xyz * in_NORMAL0.xxx + u_xlat0.xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[2].xyz * in_NORMAL0.zzz + u_xlat0.xyz;
    u_xlat15 = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat15 = inversesqrt(u_xlat15);
    vs_TEXCOORD1.xyz = vec3(u_xlat15) * u_xlat0.xyz;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	lowp float _Cutoff;
uniform lowp sampler2D _MainTex;
in lowp vec4 vs_COLOR0;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
lowp float u_xlat10_0;
bool u_xlatb0;
lowp float u_xlat10_1;
vec2 u_xlat2;
void main()
{
    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
    u_xlat10_1 = u_xlat10_0 * vs_COLOR0.w + (-_Cutoff);
#ifdef UNITY_ADRENO_ES3
    u_xlatb0 = !!(u_xlat10_1<0.0);
#else
    u_xlatb0 = u_xlat10_1<0.0;
#endif
    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
    u_xlat0.x = vs_TEXCOORD1.z + 1.0;
    u_xlat0.xy = vs_TEXCOORD1.xy / u_xlat0.xx;
    u_xlat0.xy = u_xlat0.xy * vec2(0.281262308, 0.281262308) + vec2(0.5, 0.5);
    u_xlat2.xy = vs_TEXCOORD1.ww * vec2(1.0, 255.0);
    u_xlat2.xy = fract(u_xlat2.xy);
    u_xlat0.z = (-u_xlat2.y) * 0.00392156886 + u_xlat2.x;
    u_xlat0.w = u_xlat2.y;
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
SubShader { 
 Tags { "RenderType"="Grass" }
 Pass {
  Tags { "RenderType"="Grass" }
  Cull Off
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform highp vec4 _ProjectionParams;
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform lowp vec4 _WavingTint;
uniform highp vec4 _WaveAndDistance;
uniform highp vec4 _CameraPosition;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  highp vec4 tmpvar_1;
  highp vec4 vertex_2;
  vertex_2.yw = _glesVertex.yw;
  lowp vec4 color_3;
  color_3.xyz = _glesColor.xyz;
  lowp vec3 waveColor_4;
  highp vec3 waveMove_5;
  highp vec4 s_6;
  highp vec4 waves_7;
  waves_7 = (_glesVertex.x * (vec4(0.012, 0.02, 0.06, 0.024) * _WaveAndDistance.y));
  waves_7 = (waves_7 + (_glesVertex.z * (vec4(0.006, 0.02, 0.02, 0.05) * _WaveAndDistance.y)));
  waves_7 = (waves_7 + (_WaveAndDistance.x * vec4(1.2, 2.0, 1.6, 4.8)));
  highp vec4 tmpvar_8;
  tmpvar_8 = fract(waves_7);
  waves_7 = tmpvar_8;
  highp vec4 val_9;
  highp vec4 s_10;
  val_9 = ((tmpvar_8 * 6.408849) - 3.141593);
  highp vec4 tmpvar_11;
  tmpvar_11 = (val_9 * val_9);
  highp vec4 tmpvar_12;
  tmpvar_12 = (tmpvar_11 * val_9);
  highp vec4 tmpvar_13;
  tmpvar_13 = (tmpvar_12 * tmpvar_11);
  s_10 = (((val_9 + 
    (tmpvar_12 * -0.1616162)
  ) + (tmpvar_13 * 0.0083333)) + ((tmpvar_13 * tmpvar_11) * -0.00019841));
  s_6 = (s_10 * s_10);
  s_6 = (s_6 * s_6);
  highp float tmpvar_14;
  tmpvar_14 = (dot (s_6, vec4(0.6741998, 0.6741998, 0.2696799, 0.13484)) * 0.7);
  s_6 = (s_6 * (_glesColor.w * _WaveAndDistance.z));
  waveMove_5.y = 0.0;
  waveMove_5.x = dot (s_6, vec4(0.024, 0.04, -0.12, 0.096));
  waveMove_5.z = dot (s_6, vec4(0.006, 0.02, -0.02, 0.1));
  vertex_2.xz = (_glesVertex.xz - (waveMove_5.xz * _WaveAndDistance.z));
  highp vec3 tmpvar_15;
  tmpvar_15 = mix (vec3(0.5, 0.5, 0.5), _WavingTint.xyz, vec3(tmpvar_14));
  waveColor_4 = tmpvar_15;
  highp vec3 tmpvar_16;
  tmpvar_16 = (vertex_2.xyz - _CameraPosition.xyz);
  highp float tmpvar_17;
  tmpvar_17 = clamp (((2.0 * 
    (_WaveAndDistance.w - dot (tmpvar_16, tmpvar_16))
  ) * _CameraPosition.w), 0.0, 1.0);
  color_3.w = tmpvar_17;
  lowp vec4 tmpvar_18;
  tmpvar_18.xyz = ((2.0 * waveColor_4) * _glesColor.xyz);
  tmpvar_18.w = color_3.w;
  highp mat3 tmpvar_19;
  tmpvar_19[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_19[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_19[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  tmpvar_1.xyz = normalize((tmpvar_19 * _glesNormal));
  tmpvar_1.w = -(((glstate_matrix_modelview0 * vertex_2).z * _ProjectionParams.w));
  gl_Position = (glstate_matrix_mvp * vertex_2);
  xlv_COLOR = tmpvar_18;
  xlv_TEXCOORD0 = _glesMultiTexCoord0.xy;
  xlv_TEXCOORD1 = tmpvar_1;
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
uniform lowp float _Cutoff;
varying lowp vec4 xlv_COLOR;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  lowp float x_2;
  x_2 = ((texture2D (_MainTex, xlv_TEXCOORD0).w * xlv_COLOR.w) - _Cutoff);
  if ((x_2 < 0.0)) {
    discard;
  };
  highp vec4 enc_3;
  highp vec2 enc_4;
  enc_4 = (xlv_TEXCOORD1.xy / (xlv_TEXCOORD1.z + 1.0));
  enc_4 = (enc_4 / 1.7777);
  enc_4 = ((enc_4 * 0.5) + 0.5);
  enc_3.xy = enc_4;
  highp vec2 enc_5;
  highp vec2 tmpvar_6;
  tmpvar_6 = fract((vec2(1.0, 255.0) * xlv_TEXCOORD1.w));
  enc_5.y = tmpvar_6.y;
  enc_5.x = (tmpvar_6.x - (tmpvar_6.y * 0.003921569));
  enc_3.zw = enc_5;
  tmpvar_1 = enc_3;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles3 " {
"#ifdef VERTEX
#version 300 es
uniform 	vec4 _ProjectionParams;
uniform 	mat4x4 glstate_matrix_mvp;
uniform 	mat4x4 glstate_matrix_modelview0;
uniform 	mat4x4 glstate_matrix_invtrans_modelview0;
uniform 	lowp vec4 _WavingTint;
uniform 	vec4 _WaveAndDistance;
uniform 	vec4 _CameraPosition;
in highp vec4 in_POSITION0;
in highp vec3 in_NORMAL0;
in highp vec4 in_TEXCOORD0;
in lowp vec4 in_COLOR0;
out lowp vec4 vs_COLOR0;
out highp vec2 vs_TEXCOORD0;
out highp vec4 vs_TEXCOORD1;
vec4 u_xlat0;
vec4 u_xlat1;
vec4 u_xlat2;
lowp vec3 u_xlat10_3;
float u_xlat12;
void main()
{
    u_xlat0.xy = in_POSITION0.xz * _WaveAndDistance.yy;
    u_xlat1 = u_xlat0.yyyy * vec4(0.00600000005, 0.0199999996, 0.0199999996, 0.0500000007);
    u_xlat0 = u_xlat0.xxxx * vec4(0.0120000001, 0.0199999996, 0.0599999987, 0.0240000002) + u_xlat1;
    u_xlat0 = _WaveAndDistance.xxxx * vec4(1.20000005, 2.0, 1.60000002, 4.80000019) + u_xlat0;
    u_xlat0 = fract(u_xlat0);
    u_xlat0 = u_xlat0 * vec4(6.40884876, 6.40884876, 6.40884876, 6.40884876) + vec4(-3.14159274, -3.14159274, -3.14159274, -3.14159274);
    u_xlat1 = u_xlat0 * u_xlat0;
    u_xlat2 = u_xlat0 * u_xlat1;
    u_xlat0 = u_xlat2 * vec4(-0.161616161, -0.161616161, -0.161616161, -0.161616161) + u_xlat0;
    u_xlat2 = u_xlat1 * u_xlat2;
    u_xlat1 = u_xlat1 * u_xlat2;
    u_xlat0 = u_xlat2 * vec4(0.00833330024, 0.00833330024, 0.00833330024, 0.00833330024) + u_xlat0;
    u_xlat0 = u_xlat1 * vec4(-0.000198409994, -0.000198409994, -0.000198409994, -0.000198409994) + u_xlat0;
    u_xlat0 = u_xlat0 * u_xlat0;
    u_xlat0 = u_xlat0 * u_xlat0;
    u_xlat1.x = in_COLOR0.w * _WaveAndDistance.z;
    u_xlat1 = u_xlat0 * u_xlat1.xxxx;
    u_xlat0.x = dot(u_xlat0, vec4(0.674199879, 0.674199879, 0.269679934, 0.134839967));
    u_xlat0.x = u_xlat0.x * 0.699999988;
    u_xlat2.x = dot(u_xlat1, vec4(0.0240000002, 0.0399999991, -0.119999997, 0.0960000008));
    u_xlat2.z = dot(u_xlat1, vec4(0.00600000005, 0.0199999996, -0.0199999996, 0.100000001));
    u_xlat1.xz = (-u_xlat2.xz) * _WaveAndDistance.zz + in_POSITION0.xz;
    u_xlat2 = in_POSITION0.yyyy * glstate_matrix_mvp[1];
    u_xlat2 = glstate_matrix_mvp[0] * u_xlat1.xxxx + u_xlat2;
    u_xlat2 = glstate_matrix_mvp[2] * u_xlat1.zzzz + u_xlat2;
    gl_Position = glstate_matrix_mvp[3] * in_POSITION0.wwww + u_xlat2;
    u_xlat10_3.xyz = _WavingTint.xyz + vec3(-0.5, -0.5, -0.5);
    u_xlat10_3.xyz = u_xlat0.xxx * u_xlat10_3.xyz + vec3(0.5, 0.5, 0.5);
    u_xlat10_3.xyz = u_xlat10_3.xyz * in_COLOR0.xyz;
    vs_COLOR0.xyz = u_xlat10_3.xyz * vec3(2.0, 2.0, 2.0);
    u_xlat1.y = in_POSITION0.y;
    u_xlat0.xyz = u_xlat1.xyz + (-_CameraPosition.xyz);
    u_xlat0.x = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat0.x = (-u_xlat0.x) + _WaveAndDistance.w;
    u_xlat0.x = dot(_CameraPosition.ww, u_xlat0.xx);
#ifdef UNITY_ADRENO_ES3
    u_xlat0.x = min(max(u_xlat0.x, 0.0), 1.0);
#else
    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
#endif
    vs_COLOR0.w = u_xlat0.x;
    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
    u_xlat0.x = in_POSITION0.y * glstate_matrix_modelview0[1].z;
    u_xlat0.x = glstate_matrix_modelview0[0].z * u_xlat1.x + u_xlat0.x;
    u_xlat0.x = glstate_matrix_modelview0[2].z * u_xlat1.z + u_xlat0.x;
    u_xlat0.x = glstate_matrix_modelview0[3].z * in_POSITION0.w + u_xlat0.x;
    u_xlat0.x = u_xlat0.x * _ProjectionParams.w;
    vs_TEXCOORD1.w = (-u_xlat0.x);
    u_xlat0.xyz = in_NORMAL0.yyy * glstate_matrix_invtrans_modelview0[1].xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[0].xyz * in_NORMAL0.xxx + u_xlat0.xyz;
    u_xlat0.xyz = glstate_matrix_invtrans_modelview0[2].xyz * in_NORMAL0.zzz + u_xlat0.xyz;
    u_xlat12 = dot(u_xlat0.xyz, u_xlat0.xyz);
    u_xlat12 = inversesqrt(u_xlat12);
    vs_TEXCOORD1.xyz = vec3(u_xlat12) * u_xlat0.xyz;
    return;
}
#endif
#ifdef FRAGMENT
#version 300 es
precision highp int;
uniform 	lowp float _Cutoff;
uniform lowp sampler2D _MainTex;
in lowp vec4 vs_COLOR0;
in highp vec2 vs_TEXCOORD0;
in highp vec4 vs_TEXCOORD1;
layout(location = 0) out lowp vec4 SV_Target0;
vec4 u_xlat0;
lowp float u_xlat10_0;
bool u_xlatb0;
lowp float u_xlat10_1;
vec2 u_xlat2;
void main()
{
    u_xlat10_0 = texture(_MainTex, vs_TEXCOORD0.xy).w;
    u_xlat10_1 = u_xlat10_0 * vs_COLOR0.w + (-_Cutoff);
#ifdef UNITY_ADRENO_ES3
    u_xlatb0 = !!(u_xlat10_1<0.0);
#else
    u_xlatb0 = u_xlat10_1<0.0;
#endif
    if((int(u_xlatb0) * int(0xffffffffu))!=0){discard;}
    u_xlat0.x = vs_TEXCOORD1.z + 1.0;
    u_xlat0.xy = vs_TEXCOORD1.xy / u_xlat0.xx;
    u_xlat0.xy = u_xlat0.xy * vec2(0.281262308, 0.281262308) + vec2(0.5, 0.5);
    u_xlat2.xy = vs_TEXCOORD1.ww * vec2(1.0, 255.0);
    u_xlat2.xy = fract(u_xlat2.xy);
    u_xlat0.z = (-u_xlat2.y) * 0.00392156886 + u_xlat2.x;
    u_xlat0.w = u_xlat2.y;
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
Fallback Off
}