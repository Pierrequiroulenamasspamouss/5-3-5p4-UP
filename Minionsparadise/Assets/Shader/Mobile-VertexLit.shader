//////////////////////////////////////////
///////////////////////////////////////////
Shader "Mobile/VertexLit" {
Properties {
 _MainTex ("Base (RGB)", 2D) = "white" { }
}
SubShader { 
 LOD 80
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "LIGHTMODE"="Vertex" "RenderType"="Opaque" }
GLSLPROGRAM
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform mediump vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform lowp vec4 glstate_lightmodel_ambient;
uniform highp vec4 _MainTex_ST;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec3 lcolor_1;
  mediump vec3 eyeNormal_2;
  mediump vec4 color_3;
  highp mat3 tmpvar_4;
  tmpvar_4[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_4[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_4[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  highp vec3 tmpvar_5;
  tmpvar_5 = normalize((tmpvar_4 * _glesNormal));
  eyeNormal_2 = tmpvar_5;
  lowp vec3 tmpvar_6;
  tmpvar_6 = glstate_lightmodel_ambient.xyz;
  lcolor_1 = tmpvar_6;
  highp vec3 tmpvar_7;
  tmpvar_7 = unity_LightPosition[0].xyz;
  mediump vec3 dirToLight_8;
  dirToLight_8 = tmpvar_7;
  lcolor_1 = (lcolor_1 + min ((
    (vec3(max (dot (eyeNormal_2, dirToLight_8), 0.0)) * unity_LightColor[0].xyz)
   * 0.5), vec3(1.0, 1.0, 1.0)));
  highp vec3 tmpvar_9;
  tmpvar_9 = unity_LightPosition[1].xyz;
  mediump vec3 dirToLight_10;
  dirToLight_10 = tmpvar_9;
  lcolor_1 = (lcolor_1 + min ((
    (vec3(max (dot (eyeNormal_2, dirToLight_10), 0.0)) * unity_LightColor[1].xyz)
   * 0.5), vec3(1.0, 1.0, 1.0)));
  highp vec3 tmpvar_11;
  tmpvar_11 = unity_LightPosition[2].xyz;
  mediump vec3 dirToLight_12;
  dirToLight_12 = tmpvar_11;
  lcolor_1 = (lcolor_1 + min ((
    (vec3(max (dot (eyeNormal_2, dirToLight_12), 0.0)) * unity_LightColor[2].xyz)
   * 0.5), vec3(1.0, 1.0, 1.0)));
  highp vec3 tmpvar_13;
  tmpvar_13 = unity_LightPosition[3].xyz;
  mediump vec3 dirToLight_14;
  dirToLight_14 = tmpvar_13;
  lcolor_1 = (lcolor_1 + min ((
    (vec3(max (dot (eyeNormal_2, dirToLight_14), 0.0)) * unity_LightColor[3].xyz)
   * 0.5), vec3(1.0, 1.0, 1.0)));
  highp vec3 tmpvar_15;
  tmpvar_15 = unity_LightPosition[4].xyz;
  mediump vec3 dirToLight_16;
  dirToLight_16 = tmpvar_15;
  lcolor_1 = (lcolor_1 + min ((
    (vec3(max (dot (eyeNormal_2, dirToLight_16), 0.0)) * unity_LightColor[4].xyz)
   * 0.5), vec3(1.0, 1.0, 1.0)));
  highp vec3 tmpvar_17;
  tmpvar_17 = unity_LightPosition[5].xyz;
  mediump vec3 dirToLight_18;
  dirToLight_18 = tmpvar_17;
  lcolor_1 = (lcolor_1 + min ((
    (vec3(max (dot (eyeNormal_2, dirToLight_18), 0.0)) * unity_LightColor[5].xyz)
   * 0.5), vec3(1.0, 1.0, 1.0)));
  highp vec3 tmpvar_19;
  tmpvar_19 = unity_LightPosition[6].xyz;
  mediump vec3 dirToLight_20;
  dirToLight_20 = tmpvar_19;
  lcolor_1 = (lcolor_1 + min ((
    (vec3(max (dot (eyeNormal_2, dirToLight_20), 0.0)) * unity_LightColor[6].xyz)
   * 0.5), vec3(1.0, 1.0, 1.0)));
  highp vec3 tmpvar_21;
  tmpvar_21 = unity_LightPosition[7].xyz;
  mediump vec3 dirToLight_22;
  dirToLight_22 = tmpvar_21;
  lcolor_1 = (lcolor_1 + min ((
    (vec3(max (dot (eyeNormal_2, dirToLight_22), 0.0)) * unity_LightColor[7].xyz)
   * 0.5), vec3(1.0, 1.0, 1.0)));
  color_3.xyz = lcolor_1;
  color_3.w = 1.0;
  lowp vec4 tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24 = clamp (color_3, 0.0, 1.0);
  tmpvar_23 = tmpvar_24;
  highp vec4 tmpvar_25;
  tmpvar_25.w = 1.0;
  tmpvar_25.xyz = _glesVertex.xyz;
  xlv_COLOR0 = tmpvar_23;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  gl_Position = (glstate_matrix_mvp * tmpvar_25);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 col_1;
  col_1.xyz = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_COLOR0).xyz;
  col_1.xyz = (col_1 * 2.0).xyz;
  col_1.w = 1.0;
  gl_FragData[0] = col_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform mediump vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform mediump vec4 unity_LightAtten[8];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform lowp vec4 glstate_lightmodel_ambient;
uniform highp vec4 _MainTex_ST;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesVertex.xyz;
  mediump vec3 lcolor_3;
  mediump vec3 eyeNormal_4;
  highp vec3 eyePos_5;
  mediump vec4 color_6;
  color_6 = vec4(0.0, 0.0, 0.0, 1.1);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = tmpvar_1;
  eyePos_5 = (glstate_matrix_modelview0 * tmpvar_7).xyz;
  highp mat3 tmpvar_8;
  tmpvar_8[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_8[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_8[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((tmpvar_8 * _glesNormal));
  eyeNormal_4 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = glstate_lightmodel_ambient.xyz;
  lcolor_3 = tmpvar_10;
  for (highp int il_2 = 0; il_2 < 8; il_2++) {
    mediump float att_11;
    highp vec3 dirToLight_12;
    dirToLight_12 = (unity_LightPosition[il_2].xyz - (eyePos_5 * unity_LightPosition[il_2].w));
    highp float tmpvar_13;
    tmpvar_13 = dot (dirToLight_12, dirToLight_12);
    att_11 = (1.0/((1.0 + (unity_LightAtten[il_2].z * tmpvar_13))));
    if (((unity_LightPosition[il_2].w != 0.0) && (tmpvar_13 > unity_LightAtten[il_2].w))) {
      att_11 = 0.0;
    };
    dirToLight_12 = (dirToLight_12 * inversesqrt(tmpvar_13));
    att_11 = (att_11 * 0.5);
    mediump vec3 dirToLight_14;
    dirToLight_14 = dirToLight_12;
    lcolor_3 = (lcolor_3 + min ((
      (vec3(max (dot (eyeNormal_4, dirToLight_14), 0.0)) * unity_LightColor[il_2].xyz)
     * att_11), vec3(1.0, 1.0, 1.0)));
  };
  color_6.xyz = lcolor_3;
  color_6.w = 1.0;
  lowp vec4 tmpvar_15;
  mediump vec4 tmpvar_16;
  tmpvar_16 = clamp (color_6, 0.0, 1.0);
  tmpvar_15 = tmpvar_16;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_1;
  xlv_COLOR0 = tmpvar_15;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  gl_Position = (glstate_matrix_mvp * tmpvar_17);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 col_1;
  col_1.xyz = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_COLOR0).xyz;
  col_1.xyz = (col_1 * 2.0).xyz;
  col_1.w = 1.0;
  gl_FragData[0] = col_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" }
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec3 _glesNormal;
attribute vec4 _glesMultiTexCoord0;
uniform mediump vec4 unity_LightColor[8];
uniform highp vec4 unity_LightPosition[8];
uniform mediump vec4 unity_LightAtten[8];
uniform highp vec4 unity_SpotDirection[8];
uniform highp mat4 glstate_matrix_mvp;
uniform highp mat4 glstate_matrix_modelview0;
uniform highp mat4 glstate_matrix_invtrans_modelview0;
uniform lowp vec4 glstate_lightmodel_ambient;
uniform highp vec4 _MainTex_ST;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  highp vec3 tmpvar_1;
  tmpvar_1 = _glesVertex.xyz;
  mediump vec3 lcolor_3;
  mediump vec3 eyeNormal_4;
  highp vec3 eyePos_5;
  mediump vec4 color_6;
  color_6 = vec4(0.0, 0.0, 0.0, 1.1);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = tmpvar_1;
  eyePos_5 = (glstate_matrix_modelview0 * tmpvar_7).xyz;
  highp mat3 tmpvar_8;
  tmpvar_8[0] = glstate_matrix_invtrans_modelview0[0].xyz;
  tmpvar_8[1] = glstate_matrix_invtrans_modelview0[1].xyz;
  tmpvar_8[2] = glstate_matrix_invtrans_modelview0[2].xyz;
  highp vec3 tmpvar_9;
  tmpvar_9 = normalize((tmpvar_8 * _glesNormal));
  eyeNormal_4 = tmpvar_9;
  lowp vec3 tmpvar_10;
  tmpvar_10 = glstate_lightmodel_ambient.xyz;
  lcolor_3 = tmpvar_10;
  for (highp int il_2 = 0; il_2 < 8; il_2++) {
    mediump float rho_11;
    mediump float att_12;
    highp vec3 dirToLight_13;
    dirToLight_13 = (unity_LightPosition[il_2].xyz - (eyePos_5 * unity_LightPosition[il_2].w));
    highp float tmpvar_14;
    tmpvar_14 = dot (dirToLight_13, dirToLight_13);
    att_12 = (1.0/((1.0 + (unity_LightAtten[il_2].z * tmpvar_14))));
    if (((unity_LightPosition[il_2].w != 0.0) && (tmpvar_14 > unity_LightAtten[il_2].w))) {
      att_12 = 0.0;
    };
    dirToLight_13 = (dirToLight_13 * inversesqrt(tmpvar_14));
    highp float tmpvar_15;
    tmpvar_15 = max (dot (dirToLight_13, unity_SpotDirection[il_2].xyz), 0.0);
    rho_11 = tmpvar_15;
    att_12 = (att_12 * clamp ((
      (rho_11 - unity_LightAtten[il_2].x)
     * unity_LightAtten[il_2].y), 0.0, 1.0));
    att_12 = (att_12 * 0.5);
    mediump vec3 dirToLight_16;
    dirToLight_16 = dirToLight_13;
    lcolor_3 = (lcolor_3 + min ((
      (vec3(max (dot (eyeNormal_4, dirToLight_16), 0.0)) * unity_LightColor[il_2].xyz)
     * att_12), vec3(1.0, 1.0, 1.0)));
  };
  color_6.xyz = lcolor_3;
  color_6.w = 1.0;
  lowp vec4 tmpvar_17;
  mediump vec4 tmpvar_18;
  tmpvar_18 = clamp (color_6, 0.0, 1.0);
  tmpvar_17 = tmpvar_18;
  highp vec4 tmpvar_19;
  tmpvar_19.w = 1.0;
  tmpvar_19.xyz = tmpvar_1;
  xlv_COLOR0 = tmpvar_17;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  gl_Position = (glstate_matrix_mvp * tmpvar_19);
}


#endif
#ifdef FRAGMENT
uniform sampler2D _MainTex;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 col_1;
  col_1.xyz = (texture2D (_MainTex, xlv_TEXCOORD0) * xlv_COLOR0).xyz;
  col_1.xyz = (col_1 * 2.0).xyz;
  col_1.w = 1.0;
  gl_FragData[0] = col_1;
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
Keywords { "POINT" }
""
}
SubProgram "gles " {
Keywords { "SPOT" }
""
}
}
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLM" "RenderType"="Opaque" }
Program "vp" {
SubProgram "gles " {
"#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 tmpvar_2;
  tmpvar_2 = clamp (vec4(0.0, 0.0, 0.0, 1.1), 0.0, 1.0);
  tmpvar_1 = tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = _glesVertex.xyz;
  xlv_COLOR0 = tmpvar_1;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  gl_Position = (glstate_matrix_mvp * tmpvar_3);
}


#endif
#ifdef FRAGMENT
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 col_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (unity_Lightmap, xlv_TEXCOORD0);
  col_1.w = tmpvar_2.w;
  col_1.xyz = (texture2D (_MainTex, xlv_TEXCOORD1) * tmpvar_2).xyz;
  col_1.xyz = (col_1 * 2.0).xyz;
  col_1.w = 1.0;
  gl_FragData[0] = col_1;
}


#endif

ENDGLSL
 }
 Pass {
  Tags { "LIGHTMODE"="VertexLMRGBM" "RenderType"="Opaque" }
GLSLPROGRAM
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesMultiTexCoord0;
attribute vec4 _glesMultiTexCoord1;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 unity_LightmapST;
uniform highp vec4 _MainTex_ST;
varying lowp vec4 xlv_COLOR0;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 tmpvar_2;
  tmpvar_2 = clamp (vec4(0.0, 0.0, 0.0, 1.1), 0.0, 1.0);
  tmpvar_1 = tmpvar_2;
  highp vec4 tmpvar_3;
  tmpvar_3.w = 1.0;
  tmpvar_3.xyz = _glesVertex.xyz;
  xlv_COLOR0 = tmpvar_1;
  xlv_TEXCOORD0 = ((_glesMultiTexCoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
  xlv_TEXCOORD1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  gl_Position = (glstate_matrix_mvp * tmpvar_3);
}


#endif
#ifdef FRAGMENT
uniform sampler2D unity_Lightmap;
uniform sampler2D _MainTex;
varying highp vec2 xlv_TEXCOORD0;
varying highp vec2 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 col_1;
  lowp vec4 tmpvar_2;
  tmpvar_2 = texture2D (unity_Lightmap, xlv_TEXCOORD0);
  col_1 = (tmpvar_2 * tmpvar_2.w);
  col_1 = (col_1 * 2.0);
  col_1.xyz = (texture2D (_MainTex, xlv_TEXCOORD1) * col_1).xyz;
  col_1.xyz = (col_1 * 4.0).xyz;
  col_1.w = 1.0;
  gl_FragData[0] = col_1;
}


#endif

ENDGLSL
 }
 Pass {
  Name "SHADOWCASTER"
  Tags { "LIGHTMODE"="SHADOWCASTER" "SHADOWSUPPORT"="true" "RenderType"="Opaque" }
  Cull Off
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