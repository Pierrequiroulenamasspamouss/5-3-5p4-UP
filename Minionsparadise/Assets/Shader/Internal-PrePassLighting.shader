//////////////////////////////////////////
///////////////////////////////////////////
Shader "Hidden/Internal-PrePassLighting" {
Properties {
 _LightTexture0 ("", any) = "" { }
 _LightTextureB0 ("", 2D) = "" { }
 _ShadowMapTexture ("", any) = "" { }
}
SubShader { 
 Pass {
  Tags { "SHADOWSUPPORT"="true" }
  ZWrite Off
  Blend DstColor Zero
Program "vp" {
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(normalize(tmpvar_12));
  lightDir_7 = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = (dot (tmpvar_12, tmpvar_12) * _LightPos.w);
  lowp float tmpvar_15;
  tmpvar_15 = texture2D (_LightTextureB0, vec2(tmpvar_14)).w;
  atten_6 = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = pow (max (0.0, dot (h_4, tmpvar_17)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_19;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_17)
  ) * atten_6));
  mediump vec3 c_20;
  c_20 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_20, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_21;
  tmpvar_21 = clamp ((1.0 - (
    (mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_21);
  mediump vec4 tmpvar_22;
  tmpvar_22 = exp2(-(res_2));
  tmpvar_1 = tmpvar_22;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  mediump vec3 lightDir_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_7).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_9;
  tmpvar_9 = (_CameraToWorld * tmpvar_8).xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_11;
  tmpvar_11 = -(_LightDir.xyz);
  lightDir_6 = tmpvar_11;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_CameraNormalsTexture, tmpvar_7);
  nspec_5 = tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((lightDir_6 - normalize(
    (tmpvar_9 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_14;
  mediump float tmpvar_15;
  tmpvar_15 = pow (max (0.0, dot (h_4, tmpvar_13)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_15;
  spec_3 = (spec_3 * clamp (1.0, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * max (0.0, dot (lightDir_6, tmpvar_13)));
  mediump vec3 c_16;
  c_16 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_16, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_17;
  tmpvar_17 = clamp ((1.0 - (
    (mix (tmpvar_8.z, sqrt(dot (tmpvar_10, tmpvar_10)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_17);
  mediump vec4 tmpvar_18;
  tmpvar_18 = exp2(-(res_2));
  tmpvar_1 = tmpvar_18;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_12);
  lightDir_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_10;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_LightMatrix0 * tmpvar_14);
  highp vec4 tmpvar_16;
  tmpvar_16.zw = vec2(0.0, -8.0);
  tmpvar_16.xy = (tmpvar_15.xy / tmpvar_15.w);
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_LightTexture0, tmpvar_16.xy, -8.0);
  highp float tmpvar_18;
  tmpvar_18 = tmpvar_17.w;
  atten_6 = (tmpvar_18 * float((tmpvar_15.w < 0.0)));
  highp float tmpvar_19;
  tmpvar_19 = (dot (tmpvar_12, tmpvar_12) * _LightPos.w);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_LightTextureB0, vec2(tmpvar_19));
  atten_6 = (atten_6 * tmpvar_20.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_21;
  mediump vec3 tmpvar_22;
  tmpvar_22 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_23;
  mediump float tmpvar_24;
  tmpvar_24 = pow (max (0.0, dot (h_4, tmpvar_22)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_24;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_22)
  ) * atten_6));
  mediump vec3 c_25;
  c_25 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_25, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_26;
  tmpvar_26 = clamp ((1.0 - (
    (mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_26);
  mediump vec4 tmpvar_27;
  tmpvar_27 = exp2(-(res_2));
  tmpvar_1 = tmpvar_27;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(normalize(tmpvar_12));
  lightDir_7 = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = (dot (tmpvar_12, tmpvar_12) * _LightPos.w);
  lowp float tmpvar_15;
  tmpvar_15 = texture2D (_LightTextureB0, vec2(tmpvar_14)).w;
  atten_6 = tmpvar_15;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_10;
  highp vec4 tmpvar_17;
  tmpvar_17.w = -8.0;
  tmpvar_17.xyz = (_LightMatrix0 * tmpvar_16).xyz;
  lowp vec4 tmpvar_18;
  tmpvar_18 = textureCube (_LightTexture0, tmpvar_17.xyz, -8.0);
  atten_6 = (atten_6 * tmpvar_18.w);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_19;
  mediump vec3 tmpvar_20;
  tmpvar_20 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_21;
  mediump float tmpvar_22;
  tmpvar_22 = pow (max (0.0, dot (h_4, tmpvar_20)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_22;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_20)
  ) * atten_6));
  mediump vec3 c_23;
  c_23 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_23, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_24;
  tmpvar_24 = clamp ((1.0 - (
    (mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_24);
  mediump vec4 tmpvar_25;
  tmpvar_25 = exp2(-(res_2));
  tmpvar_1 = tmpvar_25;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_10;
  highp vec4 tmpvar_14;
  tmpvar_14.zw = vec2(0.0, -8.0);
  tmpvar_14.xy = (_LightMatrix0 * tmpvar_13).xy;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_LightTexture0, tmpvar_14.xy, -8.0);
  atten_6 = tmpvar_15.w;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = pow (max (0.0, dot (h_4, tmpvar_17)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_19;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_17)
  ) * atten_6));
  mediump vec3 c_20;
  c_20 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_20, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_21;
  tmpvar_21 = clamp ((1.0 - (
    (mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_21);
  mediump vec4 tmpvar_22;
  tmpvar_22 = exp2(-(res_2));
  tmpvar_1 = tmpvar_22;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NONATIVE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_13);
  lightDir_7 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (tmpvar_16.xy / tmpvar_16.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  highp float tmpvar_19;
  tmpvar_19 = tmpvar_18.w;
  atten_6 = (tmpvar_19 * float((tmpvar_16.w < 0.0)));
  highp float tmpvar_20;
  tmpvar_20 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
  atten_6 = (atten_6 * tmpvar_21.w);
  mediump float tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = clamp (((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = tmpvar_10;
  highp vec4 tmpvar_25;
  tmpvar_25 = (unity_World2Shadow[0] * tmpvar_24);
  lowp float tmpvar_26;
  highp vec4 tmpvar_27;
  tmpvar_27 = texture2DProj (_ShadowMapTexture, tmpvar_25);
  mediump float tmpvar_28;
  if ((tmpvar_27.x < (tmpvar_25.z / tmpvar_25.w))) {
    tmpvar_28 = _LightShadowData.x;
  } else {
    tmpvar_28 = 1.0;
  };
  tmpvar_26 = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = clamp ((tmpvar_26 + tmpvar_23), 0.0, 1.0);
  tmpvar_22 = tmpvar_29;
  atten_6 = (atten_6 * tmpvar_22);
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_30;
  mediump vec3 tmpvar_31;
  tmpvar_31 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_32;
  tmpvar_32 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_32;
  mediump float tmpvar_33;
  tmpvar_33 = pow (max (0.0, dot (h_4, tmpvar_31)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_33;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_31)
  ) * atten_6));
  mediump vec3 c_34;
  c_34 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_34, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_35;
  tmpvar_35 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_35);
  mediump vec4 tmpvar_36;
  tmpvar_36 = exp2(-(res_2));
  tmpvar_1 = tmpvar_36;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NATIVE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
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
#extension GL_EXT_shadow_samplers : enable
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_13);
  lightDir_7 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (tmpvar_16.xy / tmpvar_16.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  highp float tmpvar_19;
  tmpvar_19 = tmpvar_18.w;
  atten_6 = (tmpvar_19 * float((tmpvar_16.w < 0.0)));
  highp float tmpvar_20;
  tmpvar_20 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
  atten_6 = (atten_6 * tmpvar_21.w);
  mediump float tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = tmpvar_10;
  highp vec4 tmpvar_24;
  tmpvar_24 = (unity_World2Shadow[0] * tmpvar_23);
  lowp float tmpvar_25;
  mediump float shadow_26;
  lowp float tmpvar_27;
  tmpvar_27 = shadow2DProjEXT (_ShadowMapTexture, tmpvar_24);
  mediump float tmpvar_28;
  tmpvar_28 = tmpvar_27;
  shadow_26 = (_LightShadowData.x + (tmpvar_28 * (1.0 - _LightShadowData.x)));
  tmpvar_25 = shadow_26;
  highp float tmpvar_29;
  tmpvar_29 = clamp ((tmpvar_25 + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_22 = tmpvar_29;
  atten_6 = (atten_6 * tmpvar_22);
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_30;
  mediump vec3 tmpvar_31;
  tmpvar_31 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_32;
  tmpvar_32 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_32;
  mediump float tmpvar_33;
  tmpvar_33 = pow (max (0.0, dot (h_4, tmpvar_31)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_33;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_31)
  ) * atten_6));
  mediump vec3 c_34;
  c_34 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_34, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_35;
  tmpvar_35 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_35);
  mediump vec4 tmpvar_36;
  tmpvar_36 = exp2(-(res_2));
  tmpvar_1 = tmpvar_36;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_13;
  mediump float tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_16;
  tmpvar_16 = clamp ((tmpvar_15.x + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_14 = tmpvar_16;
  atten_6 = tmpvar_14;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_17;
  mediump vec3 tmpvar_18;
  tmpvar_18 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = pow (max (0.0, dot (h_4, tmpvar_18)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_20;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_18)
  ) * atten_6));
  mediump vec3 c_21;
  c_21 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_21, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_22;
  tmpvar_22 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_22);
  mediump vec4 tmpvar_23;
  tmpvar_23 = exp2(-(res_2));
  tmpvar_1 = tmpvar_23;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_13;
  mediump float tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_16;
  tmpvar_16 = clamp ((tmpvar_15.x + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_14 = tmpvar_16;
  atten_6 = tmpvar_14;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  highp vec4 tmpvar_18;
  tmpvar_18.zw = vec2(0.0, -8.0);
  tmpvar_18.xy = (_LightMatrix0 * tmpvar_17).xy;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_LightTexture0, tmpvar_18.xy, -8.0);
  atten_6 = (atten_6 * tmpvar_19.w);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_20;
  mediump vec3 tmpvar_21;
  tmpvar_21 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_22;
  mediump float tmpvar_23;
  tmpvar_23 = pow (max (0.0, dot (h_4, tmpvar_21)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_23;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_21)
  ) * atten_6));
  mediump vec3 c_24;
  c_24 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_24, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_25);
  mediump vec4 tmpvar_26;
  tmpvar_26 = exp2(-(res_2));
  tmpvar_1 = tmpvar_26;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 _LightPositionRange;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = -(normalize(tmpvar_13));
  lightDir_7 = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp float tmpvar_16;
  tmpvar_16 = texture2D (_LightTextureB0, vec2(tmpvar_15)).w;
  atten_6 = tmpvar_16;
  highp float mydist_17;
  mydist_17 = ((sqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * _LightPositionRange.w) * 0.97);
  highp float tmpvar_18;
  tmpvar_18 = dot (textureCube (_ShadowMapTexture, tmpvar_13), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  mediump float tmpvar_19;
  if ((tmpvar_18 < mydist_17)) {
    tmpvar_19 = _LightShadowData.x;
  } else {
    tmpvar_19 = 1.0;
  };
  atten_6 = (atten_6 * tmpvar_19);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_20;
  mediump vec3 tmpvar_21;
  tmpvar_21 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_22;
  mediump float tmpvar_23;
  tmpvar_23 = pow (max (0.0, dot (h_4, tmpvar_21)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_23;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_21)
  ) * atten_6));
  mediump vec3 c_24;
  c_24 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_24, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_25);
  mediump vec4 tmpvar_26;
  tmpvar_26 = exp2(-(res_2));
  tmpvar_1 = tmpvar_26;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 _LightPositionRange;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = -(normalize(tmpvar_13));
  lightDir_7 = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp float tmpvar_16;
  tmpvar_16 = texture2D (_LightTextureB0, vec2(tmpvar_15)).w;
  atten_6 = tmpvar_16;
  highp float mydist_17;
  mydist_17 = ((sqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * _LightPositionRange.w) * 0.97);
  highp float tmpvar_18;
  tmpvar_18 = dot (textureCube (_ShadowMapTexture, tmpvar_13), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  mediump float tmpvar_19;
  if ((tmpvar_18 < mydist_17)) {
    tmpvar_19 = _LightShadowData.x;
  } else {
    tmpvar_19 = 1.0;
  };
  atten_6 = (atten_6 * tmpvar_19);
  highp vec4 tmpvar_20;
  tmpvar_20.w = 1.0;
  tmpvar_20.xyz = tmpvar_10;
  highp vec4 tmpvar_21;
  tmpvar_21.w = -8.0;
  tmpvar_21.xyz = (_LightMatrix0 * tmpvar_20).xyz;
  lowp vec4 tmpvar_22;
  tmpvar_22 = textureCube (_LightTexture0, tmpvar_21.xyz, -8.0);
  atten_6 = (atten_6 * tmpvar_22.w);
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_23;
  mediump vec3 tmpvar_24;
  tmpvar_24 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_25;
  tmpvar_25 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = pow (max (0.0, dot (h_4, tmpvar_24)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_26;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_24)
  ) * atten_6));
  mediump vec3 c_27;
  c_27 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_27, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_28;
  tmpvar_28 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_28);
  mediump vec4 tmpvar_29;
  tmpvar_29 = exp2(-(res_2));
  tmpvar_1 = tmpvar_29;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NONATIVE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform highp sampler2D _ShadowMapTexture;
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_13);
  lightDir_7 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (tmpvar_16.xy / tmpvar_16.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  highp float tmpvar_19;
  tmpvar_19 = tmpvar_18.w;
  atten_6 = (tmpvar_19 * float((tmpvar_16.w < 0.0)));
  highp float tmpvar_20;
  tmpvar_20 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
  atten_6 = (atten_6 * tmpvar_21.w);
  mediump float tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = clamp (((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = tmpvar_10;
  highp vec4 tmpvar_25;
  tmpvar_25 = (unity_World2Shadow[0] * tmpvar_24);
  lowp float tmpvar_26;
  highp vec4 shadowVals_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_25.xyz / tmpvar_25.w);
  shadowVals_27.x = texture2D (_ShadowMapTexture, (tmpvar_28.xy + _ShadowOffsets[0].xy)).x;
  shadowVals_27.y = texture2D (_ShadowMapTexture, (tmpvar_28.xy + _ShadowOffsets[1].xy)).x;
  shadowVals_27.z = texture2D (_ShadowMapTexture, (tmpvar_28.xy + _ShadowOffsets[2].xy)).x;
  shadowVals_27.w = texture2D (_ShadowMapTexture, (tmpvar_28.xy + _ShadowOffsets[3].xy)).x;
  bvec4 tmpvar_29;
  tmpvar_29 = lessThan (shadowVals_27, tmpvar_28.zzzz);
  mediump vec4 tmpvar_30;
  tmpvar_30 = _LightShadowData.xxxx;
  mediump float tmpvar_31;
  if (tmpvar_29.x) {
    tmpvar_31 = tmpvar_30.x;
  } else {
    tmpvar_31 = 1.0;
  };
  mediump float tmpvar_32;
  if (tmpvar_29.y) {
    tmpvar_32 = tmpvar_30.y;
  } else {
    tmpvar_32 = 1.0;
  };
  mediump float tmpvar_33;
  if (tmpvar_29.z) {
    tmpvar_33 = tmpvar_30.z;
  } else {
    tmpvar_33 = 1.0;
  };
  mediump float tmpvar_34;
  if (tmpvar_29.w) {
    tmpvar_34 = tmpvar_30.w;
  } else {
    tmpvar_34 = 1.0;
  };
  mediump vec4 tmpvar_35;
  tmpvar_35.x = tmpvar_31;
  tmpvar_35.y = tmpvar_32;
  tmpvar_35.z = tmpvar_33;
  tmpvar_35.w = tmpvar_34;
  mediump float tmpvar_36;
  tmpvar_36 = dot (tmpvar_35, vec4(0.25, 0.25, 0.25, 0.25));
  tmpvar_26 = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = clamp ((tmpvar_26 + tmpvar_23), 0.0, 1.0);
  tmpvar_22 = tmpvar_37;
  atten_6 = (atten_6 * tmpvar_22);
  lowp vec4 tmpvar_38;
  tmpvar_38 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_38;
  mediump vec3 tmpvar_39;
  tmpvar_39 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_40;
  tmpvar_40 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_40;
  mediump float tmpvar_41;
  tmpvar_41 = pow (max (0.0, dot (h_4, tmpvar_39)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_41;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_39)
  ) * atten_6));
  mediump vec3 c_42;
  c_42 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_42, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_43;
  tmpvar_43 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_43);
  mediump vec4 tmpvar_44;
  tmpvar_44 = exp2(-(res_2));
  tmpvar_1 = tmpvar_44;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NATIVE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
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
#extension GL_EXT_shadow_samplers : enable
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_13);
  lightDir_7 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (tmpvar_16.xy / tmpvar_16.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  highp float tmpvar_19;
  tmpvar_19 = tmpvar_18.w;
  atten_6 = (tmpvar_19 * float((tmpvar_16.w < 0.0)));
  highp float tmpvar_20;
  tmpvar_20 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
  atten_6 = (atten_6 * tmpvar_21.w);
  mediump float tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = tmpvar_10;
  highp vec4 tmpvar_24;
  tmpvar_24 = (unity_World2Shadow[0] * tmpvar_23);
  lowp float tmpvar_25;
  mediump vec4 shadows_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (tmpvar_24.xyz / tmpvar_24.w);
  highp vec3 coord_28;
  coord_28 = (tmpvar_27 + _ShadowOffsets[0].xyz);
  lowp float tmpvar_29;
  tmpvar_29 = shadow2DEXT (_ShadowMapTexture, coord_28);
  shadows_26.x = tmpvar_29;
  highp vec3 coord_30;
  coord_30 = (tmpvar_27 + _ShadowOffsets[1].xyz);
  lowp float tmpvar_31;
  tmpvar_31 = shadow2DEXT (_ShadowMapTexture, coord_30);
  shadows_26.y = tmpvar_31;
  highp vec3 coord_32;
  coord_32 = (tmpvar_27 + _ShadowOffsets[2].xyz);
  lowp float tmpvar_33;
  tmpvar_33 = shadow2DEXT (_ShadowMapTexture, coord_32);
  shadows_26.z = tmpvar_33;
  highp vec3 coord_34;
  coord_34 = (tmpvar_27 + _ShadowOffsets[3].xyz);
  lowp float tmpvar_35;
  tmpvar_35 = shadow2DEXT (_ShadowMapTexture, coord_34);
  shadows_26.w = tmpvar_35;
  shadows_26 = (_LightShadowData.xxxx + (shadows_26 * (1.0 - _LightShadowData.xxxx)));
  mediump float tmpvar_36;
  tmpvar_36 = dot (shadows_26, vec4(0.25, 0.25, 0.25, 0.25));
  tmpvar_25 = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = clamp ((tmpvar_25 + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_22 = tmpvar_37;
  atten_6 = (atten_6 * tmpvar_22);
  lowp vec4 tmpvar_38;
  tmpvar_38 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_38;
  mediump vec3 tmpvar_39;
  tmpvar_39 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_40;
  tmpvar_40 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_40;
  mediump float tmpvar_41;
  tmpvar_41 = pow (max (0.0, dot (h_4, tmpvar_39)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_41;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_39)
  ) * atten_6));
  mediump vec3 c_42;
  c_42 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_42, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_43;
  tmpvar_43 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_43);
  mediump vec4 tmpvar_44;
  tmpvar_44 = exp2(-(res_2));
  tmpvar_1 = tmpvar_44;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" "SHADOWS_SOFT" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 _LightPositionRange;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = -(normalize(tmpvar_13));
  lightDir_7 = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp float tmpvar_16;
  tmpvar_16 = texture2D (_LightTextureB0, vec2(tmpvar_15)).w;
  atten_6 = tmpvar_16;
  highp vec4 shadowVals_17;
  highp float mydist_18;
  mydist_18 = ((sqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * _LightPositionRange.w) * 0.97);
  shadowVals_17.x = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(0.0078125, 0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.y = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(-0.0078125, -0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.z = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(-0.0078125, 0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.w = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(0.0078125, -0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  bvec4 tmpvar_19;
  tmpvar_19 = lessThan (shadowVals_17, vec4(mydist_18));
  mediump vec4 tmpvar_20;
  tmpvar_20 = _LightShadowData.xxxx;
  mediump float tmpvar_21;
  if (tmpvar_19.x) {
    tmpvar_21 = tmpvar_20.x;
  } else {
    tmpvar_21 = 1.0;
  };
  mediump float tmpvar_22;
  if (tmpvar_19.y) {
    tmpvar_22 = tmpvar_20.y;
  } else {
    tmpvar_22 = 1.0;
  };
  mediump float tmpvar_23;
  if (tmpvar_19.z) {
    tmpvar_23 = tmpvar_20.z;
  } else {
    tmpvar_23 = 1.0;
  };
  mediump float tmpvar_24;
  if (tmpvar_19.w) {
    tmpvar_24 = tmpvar_20.w;
  } else {
    tmpvar_24 = 1.0;
  };
  mediump vec4 tmpvar_25;
  tmpvar_25.x = tmpvar_21;
  tmpvar_25.y = tmpvar_22;
  tmpvar_25.z = tmpvar_23;
  tmpvar_25.w = tmpvar_24;
  mediump float tmpvar_26;
  tmpvar_26 = dot (tmpvar_25, vec4(0.25, 0.25, 0.25, 0.25));
  atten_6 = (atten_6 * tmpvar_26);
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_27;
  mediump vec3 tmpvar_28;
  tmpvar_28 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_29;
  tmpvar_29 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_29;
  mediump float tmpvar_30;
  tmpvar_30 = pow (max (0.0, dot (h_4, tmpvar_28)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_30;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_28)
  ) * atten_6));
  mediump vec3 c_31;
  c_31 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_31, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_32;
  tmpvar_32 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_32);
  mediump vec4 tmpvar_33;
  tmpvar_33 = exp2(-(res_2));
  tmpvar_1 = tmpvar_33;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" "SHADOWS_SOFT" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 _LightPositionRange;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = -(normalize(tmpvar_13));
  lightDir_7 = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp float tmpvar_16;
  tmpvar_16 = texture2D (_LightTextureB0, vec2(tmpvar_15)).w;
  atten_6 = tmpvar_16;
  highp vec4 shadowVals_17;
  highp float mydist_18;
  mydist_18 = ((sqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * _LightPositionRange.w) * 0.97);
  shadowVals_17.x = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(0.0078125, 0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.y = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(-0.0078125, -0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.z = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(-0.0078125, 0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.w = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(0.0078125, -0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  bvec4 tmpvar_19;
  tmpvar_19 = lessThan (shadowVals_17, vec4(mydist_18));
  mediump vec4 tmpvar_20;
  tmpvar_20 = _LightShadowData.xxxx;
  mediump float tmpvar_21;
  if (tmpvar_19.x) {
    tmpvar_21 = tmpvar_20.x;
  } else {
    tmpvar_21 = 1.0;
  };
  mediump float tmpvar_22;
  if (tmpvar_19.y) {
    tmpvar_22 = tmpvar_20.y;
  } else {
    tmpvar_22 = 1.0;
  };
  mediump float tmpvar_23;
  if (tmpvar_19.z) {
    tmpvar_23 = tmpvar_20.z;
  } else {
    tmpvar_23 = 1.0;
  };
  mediump float tmpvar_24;
  if (tmpvar_19.w) {
    tmpvar_24 = tmpvar_20.w;
  } else {
    tmpvar_24 = 1.0;
  };
  mediump vec4 tmpvar_25;
  tmpvar_25.x = tmpvar_21;
  tmpvar_25.y = tmpvar_22;
  tmpvar_25.z = tmpvar_23;
  tmpvar_25.w = tmpvar_24;
  mediump float tmpvar_26;
  tmpvar_26 = dot (tmpvar_25, vec4(0.25, 0.25, 0.25, 0.25));
  atten_6 = (atten_6 * tmpvar_26);
  highp vec4 tmpvar_27;
  tmpvar_27.w = 1.0;
  tmpvar_27.xyz = tmpvar_10;
  highp vec4 tmpvar_28;
  tmpvar_28.w = -8.0;
  tmpvar_28.xyz = (_LightMatrix0 * tmpvar_27).xyz;
  lowp vec4 tmpvar_29;
  tmpvar_29 = textureCube (_LightTexture0, tmpvar_28.xyz, -8.0);
  atten_6 = (atten_6 * tmpvar_29.w);
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_30;
  mediump vec3 tmpvar_31;
  tmpvar_31 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_32;
  tmpvar_32 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_32;
  mediump float tmpvar_33;
  tmpvar_33 = pow (max (0.0, dot (h_4, tmpvar_31)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_33;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_31)
  ) * atten_6));
  mediump vec3 c_34;
  c_34 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_34, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_35;
  tmpvar_35 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_35);
  mediump vec4 tmpvar_36;
  tmpvar_36 = exp2(-(res_2));
  tmpvar_1 = tmpvar_36;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_13;
  mediump float tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_16;
  tmpvar_16 = clamp ((tmpvar_15.x + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_14 = tmpvar_16;
  atten_6 = tmpvar_14;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_17;
  mediump vec3 tmpvar_18;
  tmpvar_18 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = pow (max (0.0, dot (h_4, tmpvar_18)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_20;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_18)
  ) * atten_6));
  mediump vec3 c_21;
  c_21 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_21, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_22;
  tmpvar_22 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_22);
  mediump vec4 tmpvar_23;
  tmpvar_23 = exp2(-(res_2));
  tmpvar_1 = tmpvar_23;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_13;
  mediump float tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_16;
  tmpvar_16 = clamp ((tmpvar_15.x + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_14 = tmpvar_16;
  atten_6 = tmpvar_14;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  highp vec4 tmpvar_18;
  tmpvar_18.zw = vec2(0.0, -8.0);
  tmpvar_18.xy = (_LightMatrix0 * tmpvar_17).xy;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_LightTexture0, tmpvar_18.xy, -8.0);
  atten_6 = (atten_6 * tmpvar_19.w);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_20;
  mediump vec3 tmpvar_21;
  tmpvar_21 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_22;
  mediump float tmpvar_23;
  tmpvar_23 = pow (max (0.0, dot (h_4, tmpvar_21)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_23;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_21)
  ) * atten_6));
  mediump vec3 c_24;
  c_24 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_24, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_25);
  mediump vec4 tmpvar_26;
  tmpvar_26 = exp2(-(res_2));
  tmpvar_1 = tmpvar_26;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NATIVE" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" }
""
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" }
""
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NATIVE" }
""
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" "SHADOWS_SOFT" }
""
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" "SHADOWS_SOFT" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
""
}
}
 }
 Pass {
  Tags { "SHADOWSUPPORT"="true" }
  ZWrite Off
  Blend One One
Program "vp" {
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(normalize(tmpvar_12));
  lightDir_7 = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = (dot (tmpvar_12, tmpvar_12) * _LightPos.w);
  lowp float tmpvar_15;
  tmpvar_15 = texture2D (_LightTextureB0, vec2(tmpvar_14)).w;
  atten_6 = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = pow (max (0.0, dot (h_4, tmpvar_17)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_19;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_17)
  ) * atten_6));
  mediump vec3 c_20;
  c_20 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_20, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_21;
  tmpvar_21 = clamp ((1.0 - (
    (mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_21);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  mediump vec3 lightDir_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_7).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_9;
  tmpvar_9 = (_CameraToWorld * tmpvar_8).xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_11;
  tmpvar_11 = -(_LightDir.xyz);
  lightDir_6 = tmpvar_11;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_CameraNormalsTexture, tmpvar_7);
  nspec_5 = tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((lightDir_6 - normalize(
    (tmpvar_9 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_14;
  mediump float tmpvar_15;
  tmpvar_15 = pow (max (0.0, dot (h_4, tmpvar_13)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_15;
  spec_3 = (spec_3 * clamp (1.0, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * max (0.0, dot (lightDir_6, tmpvar_13)));
  mediump vec3 c_16;
  c_16 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_16, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_17;
  tmpvar_17 = clamp ((1.0 - (
    (mix (tmpvar_8.z, sqrt(dot (tmpvar_10, tmpvar_10)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_17);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_12);
  lightDir_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_10;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_LightMatrix0 * tmpvar_14);
  highp vec4 tmpvar_16;
  tmpvar_16.zw = vec2(0.0, -8.0);
  tmpvar_16.xy = (tmpvar_15.xy / tmpvar_15.w);
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_LightTexture0, tmpvar_16.xy, -8.0);
  highp float tmpvar_18;
  tmpvar_18 = tmpvar_17.w;
  atten_6 = (tmpvar_18 * float((tmpvar_15.w < 0.0)));
  highp float tmpvar_19;
  tmpvar_19 = (dot (tmpvar_12, tmpvar_12) * _LightPos.w);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_LightTextureB0, vec2(tmpvar_19));
  atten_6 = (atten_6 * tmpvar_20.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_21;
  mediump vec3 tmpvar_22;
  tmpvar_22 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_23;
  mediump float tmpvar_24;
  tmpvar_24 = pow (max (0.0, dot (h_4, tmpvar_22)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_24;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_22)
  ) * atten_6));
  mediump vec3 c_25;
  c_25 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_25, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_26;
  tmpvar_26 = clamp ((1.0 - (
    (mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_26);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(normalize(tmpvar_12));
  lightDir_7 = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = (dot (tmpvar_12, tmpvar_12) * _LightPos.w);
  lowp float tmpvar_15;
  tmpvar_15 = texture2D (_LightTextureB0, vec2(tmpvar_14)).w;
  atten_6 = tmpvar_15;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_10;
  highp vec4 tmpvar_17;
  tmpvar_17.w = -8.0;
  tmpvar_17.xyz = (_LightMatrix0 * tmpvar_16).xyz;
  lowp vec4 tmpvar_18;
  tmpvar_18 = textureCube (_LightTexture0, tmpvar_17.xyz, -8.0);
  atten_6 = (atten_6 * tmpvar_18.w);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_19;
  mediump vec3 tmpvar_20;
  tmpvar_20 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_21;
  mediump float tmpvar_22;
  tmpvar_22 = pow (max (0.0, dot (h_4, tmpvar_20)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_22;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_20)
  ) * atten_6));
  mediump vec3 c_23;
  c_23 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_23, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_24;
  tmpvar_24 = clamp ((1.0 - (
    (mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_24);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_10;
  highp vec4 tmpvar_14;
  tmpvar_14.zw = vec2(0.0, -8.0);
  tmpvar_14.xy = (_LightMatrix0 * tmpvar_13).xy;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_LightTexture0, tmpvar_14.xy, -8.0);
  atten_6 = tmpvar_15.w;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = pow (max (0.0, dot (h_4, tmpvar_17)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_19;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_17)
  ) * atten_6));
  mediump vec3 c_20;
  c_20 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_20, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_21;
  tmpvar_21 = clamp ((1.0 - (
    (mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_21);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NONATIVE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_13);
  lightDir_7 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (tmpvar_16.xy / tmpvar_16.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  highp float tmpvar_19;
  tmpvar_19 = tmpvar_18.w;
  atten_6 = (tmpvar_19 * float((tmpvar_16.w < 0.0)));
  highp float tmpvar_20;
  tmpvar_20 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
  atten_6 = (atten_6 * tmpvar_21.w);
  mediump float tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = clamp (((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = tmpvar_10;
  highp vec4 tmpvar_25;
  tmpvar_25 = (unity_World2Shadow[0] * tmpvar_24);
  lowp float tmpvar_26;
  highp vec4 tmpvar_27;
  tmpvar_27 = texture2DProj (_ShadowMapTexture, tmpvar_25);
  mediump float tmpvar_28;
  if ((tmpvar_27.x < (tmpvar_25.z / tmpvar_25.w))) {
    tmpvar_28 = _LightShadowData.x;
  } else {
    tmpvar_28 = 1.0;
  };
  tmpvar_26 = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = clamp ((tmpvar_26 + tmpvar_23), 0.0, 1.0);
  tmpvar_22 = tmpvar_29;
  atten_6 = (atten_6 * tmpvar_22);
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_30;
  mediump vec3 tmpvar_31;
  tmpvar_31 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_32;
  tmpvar_32 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_32;
  mediump float tmpvar_33;
  tmpvar_33 = pow (max (0.0, dot (h_4, tmpvar_31)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_33;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_31)
  ) * atten_6));
  mediump vec3 c_34;
  c_34 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_34, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_35;
  tmpvar_35 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_35);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NATIVE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
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
#extension GL_EXT_shadow_samplers : enable
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_13);
  lightDir_7 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (tmpvar_16.xy / tmpvar_16.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  highp float tmpvar_19;
  tmpvar_19 = tmpvar_18.w;
  atten_6 = (tmpvar_19 * float((tmpvar_16.w < 0.0)));
  highp float tmpvar_20;
  tmpvar_20 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
  atten_6 = (atten_6 * tmpvar_21.w);
  mediump float tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = tmpvar_10;
  highp vec4 tmpvar_24;
  tmpvar_24 = (unity_World2Shadow[0] * tmpvar_23);
  lowp float tmpvar_25;
  mediump float shadow_26;
  lowp float tmpvar_27;
  tmpvar_27 = shadow2DProjEXT (_ShadowMapTexture, tmpvar_24);
  mediump float tmpvar_28;
  tmpvar_28 = tmpvar_27;
  shadow_26 = (_LightShadowData.x + (tmpvar_28 * (1.0 - _LightShadowData.x)));
  tmpvar_25 = shadow_26;
  highp float tmpvar_29;
  tmpvar_29 = clamp ((tmpvar_25 + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_22 = tmpvar_29;
  atten_6 = (atten_6 * tmpvar_22);
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_30;
  mediump vec3 tmpvar_31;
  tmpvar_31 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_32;
  tmpvar_32 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_32;
  mediump float tmpvar_33;
  tmpvar_33 = pow (max (0.0, dot (h_4, tmpvar_31)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_33;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_31)
  ) * atten_6));
  mediump vec3 c_34;
  c_34 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_34, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_35;
  tmpvar_35 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_35);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_13;
  mediump float tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_16;
  tmpvar_16 = clamp ((tmpvar_15.x + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_14 = tmpvar_16;
  atten_6 = tmpvar_14;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_17;
  mediump vec3 tmpvar_18;
  tmpvar_18 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = pow (max (0.0, dot (h_4, tmpvar_18)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_20;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_18)
  ) * atten_6));
  mediump vec3 c_21;
  c_21 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_21, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_22;
  tmpvar_22 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_22);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_13;
  mediump float tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_16;
  tmpvar_16 = clamp ((tmpvar_15.x + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_14 = tmpvar_16;
  atten_6 = tmpvar_14;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  highp vec4 tmpvar_18;
  tmpvar_18.zw = vec2(0.0, -8.0);
  tmpvar_18.xy = (_LightMatrix0 * tmpvar_17).xy;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_LightTexture0, tmpvar_18.xy, -8.0);
  atten_6 = (atten_6 * tmpvar_19.w);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_20;
  mediump vec3 tmpvar_21;
  tmpvar_21 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_22;
  mediump float tmpvar_23;
  tmpvar_23 = pow (max (0.0, dot (h_4, tmpvar_21)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_23;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_21)
  ) * atten_6));
  mediump vec3 c_24;
  c_24 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_24, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_25);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 _LightPositionRange;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = -(normalize(tmpvar_13));
  lightDir_7 = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp float tmpvar_16;
  tmpvar_16 = texture2D (_LightTextureB0, vec2(tmpvar_15)).w;
  atten_6 = tmpvar_16;
  highp float mydist_17;
  mydist_17 = ((sqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * _LightPositionRange.w) * 0.97);
  highp float tmpvar_18;
  tmpvar_18 = dot (textureCube (_ShadowMapTexture, tmpvar_13), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  mediump float tmpvar_19;
  if ((tmpvar_18 < mydist_17)) {
    tmpvar_19 = _LightShadowData.x;
  } else {
    tmpvar_19 = 1.0;
  };
  atten_6 = (atten_6 * tmpvar_19);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_20;
  mediump vec3 tmpvar_21;
  tmpvar_21 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_22;
  mediump float tmpvar_23;
  tmpvar_23 = pow (max (0.0, dot (h_4, tmpvar_21)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_23;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_21)
  ) * atten_6));
  mediump vec3 c_24;
  c_24 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_24, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_25);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 _LightPositionRange;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = -(normalize(tmpvar_13));
  lightDir_7 = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp float tmpvar_16;
  tmpvar_16 = texture2D (_LightTextureB0, vec2(tmpvar_15)).w;
  atten_6 = tmpvar_16;
  highp float mydist_17;
  mydist_17 = ((sqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * _LightPositionRange.w) * 0.97);
  highp float tmpvar_18;
  tmpvar_18 = dot (textureCube (_ShadowMapTexture, tmpvar_13), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  mediump float tmpvar_19;
  if ((tmpvar_18 < mydist_17)) {
    tmpvar_19 = _LightShadowData.x;
  } else {
    tmpvar_19 = 1.0;
  };
  atten_6 = (atten_6 * tmpvar_19);
  highp vec4 tmpvar_20;
  tmpvar_20.w = 1.0;
  tmpvar_20.xyz = tmpvar_10;
  highp vec4 tmpvar_21;
  tmpvar_21.w = -8.0;
  tmpvar_21.xyz = (_LightMatrix0 * tmpvar_20).xyz;
  lowp vec4 tmpvar_22;
  tmpvar_22 = textureCube (_LightTexture0, tmpvar_21.xyz, -8.0);
  atten_6 = (atten_6 * tmpvar_22.w);
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_23;
  mediump vec3 tmpvar_24;
  tmpvar_24 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_25;
  tmpvar_25 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = pow (max (0.0, dot (h_4, tmpvar_24)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_26;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_24)
  ) * atten_6));
  mediump vec3 c_27;
  c_27 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_27, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_28;
  tmpvar_28 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_28);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NONATIVE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform highp sampler2D _ShadowMapTexture;
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_13);
  lightDir_7 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (tmpvar_16.xy / tmpvar_16.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  highp float tmpvar_19;
  tmpvar_19 = tmpvar_18.w;
  atten_6 = (tmpvar_19 * float((tmpvar_16.w < 0.0)));
  highp float tmpvar_20;
  tmpvar_20 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
  atten_6 = (atten_6 * tmpvar_21.w);
  mediump float tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = clamp (((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = tmpvar_10;
  highp vec4 tmpvar_25;
  tmpvar_25 = (unity_World2Shadow[0] * tmpvar_24);
  lowp float tmpvar_26;
  highp vec4 shadowVals_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_25.xyz / tmpvar_25.w);
  shadowVals_27.x = texture2D (_ShadowMapTexture, (tmpvar_28.xy + _ShadowOffsets[0].xy)).x;
  shadowVals_27.y = texture2D (_ShadowMapTexture, (tmpvar_28.xy + _ShadowOffsets[1].xy)).x;
  shadowVals_27.z = texture2D (_ShadowMapTexture, (tmpvar_28.xy + _ShadowOffsets[2].xy)).x;
  shadowVals_27.w = texture2D (_ShadowMapTexture, (tmpvar_28.xy + _ShadowOffsets[3].xy)).x;
  bvec4 tmpvar_29;
  tmpvar_29 = lessThan (shadowVals_27, tmpvar_28.zzzz);
  mediump vec4 tmpvar_30;
  tmpvar_30 = _LightShadowData.xxxx;
  mediump float tmpvar_31;
  if (tmpvar_29.x) {
    tmpvar_31 = tmpvar_30.x;
  } else {
    tmpvar_31 = 1.0;
  };
  mediump float tmpvar_32;
  if (tmpvar_29.y) {
    tmpvar_32 = tmpvar_30.y;
  } else {
    tmpvar_32 = 1.0;
  };
  mediump float tmpvar_33;
  if (tmpvar_29.z) {
    tmpvar_33 = tmpvar_30.z;
  } else {
    tmpvar_33 = 1.0;
  };
  mediump float tmpvar_34;
  if (tmpvar_29.w) {
    tmpvar_34 = tmpvar_30.w;
  } else {
    tmpvar_34 = 1.0;
  };
  mediump vec4 tmpvar_35;
  tmpvar_35.x = tmpvar_31;
  tmpvar_35.y = tmpvar_32;
  tmpvar_35.z = tmpvar_33;
  tmpvar_35.w = tmpvar_34;
  mediump float tmpvar_36;
  tmpvar_36 = dot (tmpvar_35, vec4(0.25, 0.25, 0.25, 0.25));
  tmpvar_26 = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = clamp ((tmpvar_26 + tmpvar_23), 0.0, 1.0);
  tmpvar_22 = tmpvar_37;
  atten_6 = (atten_6 * tmpvar_22);
  lowp vec4 tmpvar_38;
  tmpvar_38 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_38;
  mediump vec3 tmpvar_39;
  tmpvar_39 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_40;
  tmpvar_40 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_40;
  mediump float tmpvar_41;
  tmpvar_41 = pow (max (0.0, dot (h_4, tmpvar_39)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_41;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_39)
  ) * atten_6));
  mediump vec3 c_42;
  c_42 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_42, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_43;
  tmpvar_43 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_43);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NATIVE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
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
#extension GL_EXT_shadow_samplers : enable
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_13);
  lightDir_7 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (tmpvar_16.xy / tmpvar_16.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  highp float tmpvar_19;
  tmpvar_19 = tmpvar_18.w;
  atten_6 = (tmpvar_19 * float((tmpvar_16.w < 0.0)));
  highp float tmpvar_20;
  tmpvar_20 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
  atten_6 = (atten_6 * tmpvar_21.w);
  mediump float tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = tmpvar_10;
  highp vec4 tmpvar_24;
  tmpvar_24 = (unity_World2Shadow[0] * tmpvar_23);
  lowp float tmpvar_25;
  mediump vec4 shadows_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (tmpvar_24.xyz / tmpvar_24.w);
  highp vec3 coord_28;
  coord_28 = (tmpvar_27 + _ShadowOffsets[0].xyz);
  lowp float tmpvar_29;
  tmpvar_29 = shadow2DEXT (_ShadowMapTexture, coord_28);
  shadows_26.x = tmpvar_29;
  highp vec3 coord_30;
  coord_30 = (tmpvar_27 + _ShadowOffsets[1].xyz);
  lowp float tmpvar_31;
  tmpvar_31 = shadow2DEXT (_ShadowMapTexture, coord_30);
  shadows_26.y = tmpvar_31;
  highp vec3 coord_32;
  coord_32 = (tmpvar_27 + _ShadowOffsets[2].xyz);
  lowp float tmpvar_33;
  tmpvar_33 = shadow2DEXT (_ShadowMapTexture, coord_32);
  shadows_26.z = tmpvar_33;
  highp vec3 coord_34;
  coord_34 = (tmpvar_27 + _ShadowOffsets[3].xyz);
  lowp float tmpvar_35;
  tmpvar_35 = shadow2DEXT (_ShadowMapTexture, coord_34);
  shadows_26.w = tmpvar_35;
  shadows_26 = (_LightShadowData.xxxx + (shadows_26 * (1.0 - _LightShadowData.xxxx)));
  mediump float tmpvar_36;
  tmpvar_36 = dot (shadows_26, vec4(0.25, 0.25, 0.25, 0.25));
  tmpvar_25 = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = clamp ((tmpvar_25 + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_22 = tmpvar_37;
  atten_6 = (atten_6 * tmpvar_22);
  lowp vec4 tmpvar_38;
  tmpvar_38 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_38;
  mediump vec3 tmpvar_39;
  tmpvar_39 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_40;
  tmpvar_40 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_40;
  mediump float tmpvar_41;
  tmpvar_41 = pow (max (0.0, dot (h_4, tmpvar_39)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_41;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_39)
  ) * atten_6));
  mediump vec3 c_42;
  c_42 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_42, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_43;
  tmpvar_43 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_43);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" "SHADOWS_SOFT" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 _LightPositionRange;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = -(normalize(tmpvar_13));
  lightDir_7 = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp float tmpvar_16;
  tmpvar_16 = texture2D (_LightTextureB0, vec2(tmpvar_15)).w;
  atten_6 = tmpvar_16;
  highp vec4 shadowVals_17;
  highp float mydist_18;
  mydist_18 = ((sqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * _LightPositionRange.w) * 0.97);
  shadowVals_17.x = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(0.0078125, 0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.y = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(-0.0078125, -0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.z = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(-0.0078125, 0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.w = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(0.0078125, -0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  bvec4 tmpvar_19;
  tmpvar_19 = lessThan (shadowVals_17, vec4(mydist_18));
  mediump vec4 tmpvar_20;
  tmpvar_20 = _LightShadowData.xxxx;
  mediump float tmpvar_21;
  if (tmpvar_19.x) {
    tmpvar_21 = tmpvar_20.x;
  } else {
    tmpvar_21 = 1.0;
  };
  mediump float tmpvar_22;
  if (tmpvar_19.y) {
    tmpvar_22 = tmpvar_20.y;
  } else {
    tmpvar_22 = 1.0;
  };
  mediump float tmpvar_23;
  if (tmpvar_19.z) {
    tmpvar_23 = tmpvar_20.z;
  } else {
    tmpvar_23 = 1.0;
  };
  mediump float tmpvar_24;
  if (tmpvar_19.w) {
    tmpvar_24 = tmpvar_20.w;
  } else {
    tmpvar_24 = 1.0;
  };
  mediump vec4 tmpvar_25;
  tmpvar_25.x = tmpvar_21;
  tmpvar_25.y = tmpvar_22;
  tmpvar_25.z = tmpvar_23;
  tmpvar_25.w = tmpvar_24;
  mediump float tmpvar_26;
  tmpvar_26 = dot (tmpvar_25, vec4(0.25, 0.25, 0.25, 0.25));
  atten_6 = (atten_6 * tmpvar_26);
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_27;
  mediump vec3 tmpvar_28;
  tmpvar_28 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_29;
  tmpvar_29 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_29;
  mediump float tmpvar_30;
  tmpvar_30 = pow (max (0.0, dot (h_4, tmpvar_28)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_30;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_28)
  ) * atten_6));
  mediump vec3 c_31;
  c_31 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_31, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_32;
  tmpvar_32 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_32);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" "SHADOWS_SOFT" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 _LightPositionRange;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = -(normalize(tmpvar_13));
  lightDir_7 = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp float tmpvar_16;
  tmpvar_16 = texture2D (_LightTextureB0, vec2(tmpvar_15)).w;
  atten_6 = tmpvar_16;
  highp vec4 shadowVals_17;
  highp float mydist_18;
  mydist_18 = ((sqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * _LightPositionRange.w) * 0.97);
  shadowVals_17.x = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(0.0078125, 0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.y = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(-0.0078125, -0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.z = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(-0.0078125, 0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.w = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(0.0078125, -0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  bvec4 tmpvar_19;
  tmpvar_19 = lessThan (shadowVals_17, vec4(mydist_18));
  mediump vec4 tmpvar_20;
  tmpvar_20 = _LightShadowData.xxxx;
  mediump float tmpvar_21;
  if (tmpvar_19.x) {
    tmpvar_21 = tmpvar_20.x;
  } else {
    tmpvar_21 = 1.0;
  };
  mediump float tmpvar_22;
  if (tmpvar_19.y) {
    tmpvar_22 = tmpvar_20.y;
  } else {
    tmpvar_22 = 1.0;
  };
  mediump float tmpvar_23;
  if (tmpvar_19.z) {
    tmpvar_23 = tmpvar_20.z;
  } else {
    tmpvar_23 = 1.0;
  };
  mediump float tmpvar_24;
  if (tmpvar_19.w) {
    tmpvar_24 = tmpvar_20.w;
  } else {
    tmpvar_24 = 1.0;
  };
  mediump vec4 tmpvar_25;
  tmpvar_25.x = tmpvar_21;
  tmpvar_25.y = tmpvar_22;
  tmpvar_25.z = tmpvar_23;
  tmpvar_25.w = tmpvar_24;
  mediump float tmpvar_26;
  tmpvar_26 = dot (tmpvar_25, vec4(0.25, 0.25, 0.25, 0.25));
  atten_6 = (atten_6 * tmpvar_26);
  highp vec4 tmpvar_27;
  tmpvar_27.w = 1.0;
  tmpvar_27.xyz = tmpvar_10;
  highp vec4 tmpvar_28;
  tmpvar_28.w = -8.0;
  tmpvar_28.xyz = (_LightMatrix0 * tmpvar_27).xyz;
  lowp vec4 tmpvar_29;
  tmpvar_29 = textureCube (_LightTexture0, tmpvar_28.xyz, -8.0);
  atten_6 = (atten_6 * tmpvar_29.w);
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_30;
  mediump vec3 tmpvar_31;
  tmpvar_31 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_32;
  tmpvar_32 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_32;
  mediump float tmpvar_33;
  tmpvar_33 = pow (max (0.0, dot (h_4, tmpvar_31)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_33;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_31)
  ) * atten_6));
  mediump vec3 c_34;
  c_34 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_34, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_35;
  tmpvar_35 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_35);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_13;
  mediump float tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_16;
  tmpvar_16 = clamp ((tmpvar_15.x + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_14 = tmpvar_16;
  atten_6 = tmpvar_14;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_17;
  mediump vec3 tmpvar_18;
  tmpvar_18 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = pow (max (0.0, dot (h_4, tmpvar_18)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_20;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_18)
  ) * atten_6));
  mediump vec3 c_21;
  c_21 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_21, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_22;
  tmpvar_22 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_22);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_13;
  mediump float tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_16;
  tmpvar_16 = clamp ((tmpvar_15.x + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_14 = tmpvar_16;
  atten_6 = tmpvar_14;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  highp vec4 tmpvar_18;
  tmpvar_18.zw = vec2(0.0, -8.0);
  tmpvar_18.xy = (_LightMatrix0 * tmpvar_17).xy;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_LightTexture0, tmpvar_18.xy, -8.0);
  atten_6 = (atten_6 * tmpvar_19.w);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_20;
  mediump vec3 tmpvar_21;
  tmpvar_21 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_22;
  mediump float tmpvar_23;
  tmpvar_23 = pow (max (0.0, dot (h_4, tmpvar_21)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_23;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_21)
  ) * atten_6));
  mediump vec3 c_24;
  c_24 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_24, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_25);
  tmpvar_1 = res_2;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NATIVE" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" }
""
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" }
""
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NATIVE" }
""
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" "SHADOWS_SOFT" }
""
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" "SHADOWS_SOFT" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
""
}
}
 }
 Pass {
  Tags { "SHADOWSUPPORT"="true" }
  ZWrite Off
  Blend One One
Program "vp" {
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(normalize(tmpvar_12));
  lightDir_7 = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = (dot (tmpvar_12, tmpvar_12) * _LightPos.w);
  lowp float tmpvar_15;
  tmpvar_15 = texture2D (_LightTextureB0, vec2(tmpvar_14)).w;
  atten_6 = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = pow (max (0.0, dot (h_4, tmpvar_17)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_19;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_17)
  ) * atten_6));
  mediump vec3 c_20;
  c_20 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_20, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_21;
  tmpvar_21 = clamp ((1.0 - (
    (mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_21);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  mediump vec3 lightDir_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_7).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_9;
  tmpvar_9 = (_CameraToWorld * tmpvar_8).xyz;
  highp vec3 tmpvar_10;
  tmpvar_10 = (tmpvar_9 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_11;
  tmpvar_11 = -(_LightDir.xyz);
  lightDir_6 = tmpvar_11;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_CameraNormalsTexture, tmpvar_7);
  nspec_5 = tmpvar_12;
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize((lightDir_6 - normalize(
    (tmpvar_9 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_14;
  mediump float tmpvar_15;
  tmpvar_15 = pow (max (0.0, dot (h_4, tmpvar_13)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_15;
  spec_3 = (spec_3 * clamp (1.0, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * max (0.0, dot (lightDir_6, tmpvar_13)));
  mediump vec3 c_16;
  c_16 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_16, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_17;
  tmpvar_17 = clamp ((1.0 - (
    (mix (tmpvar_8.z, sqrt(dot (tmpvar_10, tmpvar_10)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_17);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize(tmpvar_12);
  lightDir_7 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_10;
  highp vec4 tmpvar_15;
  tmpvar_15 = (_LightMatrix0 * tmpvar_14);
  highp vec4 tmpvar_16;
  tmpvar_16.zw = vec2(0.0, -8.0);
  tmpvar_16.xy = (tmpvar_15.xy / tmpvar_15.w);
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_LightTexture0, tmpvar_16.xy, -8.0);
  highp float tmpvar_18;
  tmpvar_18 = tmpvar_17.w;
  atten_6 = (tmpvar_18 * float((tmpvar_15.w < 0.0)));
  highp float tmpvar_19;
  tmpvar_19 = (dot (tmpvar_12, tmpvar_12) * _LightPos.w);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_LightTextureB0, vec2(tmpvar_19));
  atten_6 = (atten_6 * tmpvar_20.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_21;
  mediump vec3 tmpvar_22;
  tmpvar_22 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_23;
  mediump float tmpvar_24;
  tmpvar_24 = pow (max (0.0, dot (h_4, tmpvar_22)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_24;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_22)
  ) * atten_6));
  mediump vec3 c_25;
  c_25 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_25, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_26;
  tmpvar_26 = clamp ((1.0 - (
    (mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_26);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(normalize(tmpvar_12));
  lightDir_7 = tmpvar_13;
  highp float tmpvar_14;
  tmpvar_14 = (dot (tmpvar_12, tmpvar_12) * _LightPos.w);
  lowp float tmpvar_15;
  tmpvar_15 = texture2D (_LightTextureB0, vec2(tmpvar_14)).w;
  atten_6 = tmpvar_15;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_10;
  highp vec4 tmpvar_17;
  tmpvar_17.w = -8.0;
  tmpvar_17.xyz = (_LightMatrix0 * tmpvar_16).xyz;
  lowp vec4 tmpvar_18;
  tmpvar_18 = textureCube (_LightTexture0, tmpvar_17.xyz, -8.0);
  atten_6 = (atten_6 * tmpvar_18.w);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_19;
  mediump vec3 tmpvar_20;
  tmpvar_20 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_21;
  mediump float tmpvar_22;
  tmpvar_22 = pow (max (0.0, dot (h_4, tmpvar_20)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_22;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_20)
  ) * atten_6));
  mediump vec3 c_23;
  c_23 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_23, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_24;
  tmpvar_24 = clamp ((1.0 - (
    (mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_24);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_OFF" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_10;
  highp vec4 tmpvar_14;
  tmpvar_14.zw = vec2(0.0, -8.0);
  tmpvar_14.xy = (_LightMatrix0 * tmpvar_13).xy;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_LightTexture0, tmpvar_14.xy, -8.0);
  atten_6 = tmpvar_15.w;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_16;
  mediump vec3 tmpvar_17;
  tmpvar_17 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_18;
  mediump float tmpvar_19;
  tmpvar_19 = pow (max (0.0, dot (h_4, tmpvar_17)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_19;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_17)
  ) * atten_6));
  mediump vec3 c_20;
  c_20 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_20, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_21;
  tmpvar_21 = clamp ((1.0 - (
    (mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w) * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_21);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NONATIVE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_13);
  lightDir_7 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (tmpvar_16.xy / tmpvar_16.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  highp float tmpvar_19;
  tmpvar_19 = tmpvar_18.w;
  atten_6 = (tmpvar_19 * float((tmpvar_16.w < 0.0)));
  highp float tmpvar_20;
  tmpvar_20 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
  atten_6 = (atten_6 * tmpvar_21.w);
  mediump float tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = clamp (((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = tmpvar_10;
  highp vec4 tmpvar_25;
  tmpvar_25 = (unity_World2Shadow[0] * tmpvar_24);
  lowp float tmpvar_26;
  highp vec4 tmpvar_27;
  tmpvar_27 = texture2DProj (_ShadowMapTexture, tmpvar_25);
  mediump float tmpvar_28;
  if ((tmpvar_27.x < (tmpvar_25.z / tmpvar_25.w))) {
    tmpvar_28 = _LightShadowData.x;
  } else {
    tmpvar_28 = 1.0;
  };
  tmpvar_26 = tmpvar_28;
  highp float tmpvar_29;
  tmpvar_29 = clamp ((tmpvar_26 + tmpvar_23), 0.0, 1.0);
  tmpvar_22 = tmpvar_29;
  atten_6 = (atten_6 * tmpvar_22);
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_30;
  mediump vec3 tmpvar_31;
  tmpvar_31 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_32;
  tmpvar_32 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_32;
  mediump float tmpvar_33;
  tmpvar_33 = pow (max (0.0, dot (h_4, tmpvar_31)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_33;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_31)
  ) * atten_6));
  mediump vec3 c_34;
  c_34 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_34, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_35;
  tmpvar_35 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_35);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NATIVE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
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
#extension GL_EXT_shadow_samplers : enable
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_13);
  lightDir_7 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (tmpvar_16.xy / tmpvar_16.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  highp float tmpvar_19;
  tmpvar_19 = tmpvar_18.w;
  atten_6 = (tmpvar_19 * float((tmpvar_16.w < 0.0)));
  highp float tmpvar_20;
  tmpvar_20 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
  atten_6 = (atten_6 * tmpvar_21.w);
  mediump float tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = tmpvar_10;
  highp vec4 tmpvar_24;
  tmpvar_24 = (unity_World2Shadow[0] * tmpvar_23);
  lowp float tmpvar_25;
  mediump float shadow_26;
  lowp float tmpvar_27;
  tmpvar_27 = shadow2DProjEXT (_ShadowMapTexture, tmpvar_24);
  mediump float tmpvar_28;
  tmpvar_28 = tmpvar_27;
  shadow_26 = (_LightShadowData.x + (tmpvar_28 * (1.0 - _LightShadowData.x)));
  tmpvar_25 = shadow_26;
  highp float tmpvar_29;
  tmpvar_29 = clamp ((tmpvar_25 + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_22 = tmpvar_29;
  atten_6 = (atten_6 * tmpvar_22);
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_30;
  mediump vec3 tmpvar_31;
  tmpvar_31 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_32;
  tmpvar_32 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_32;
  mediump float tmpvar_33;
  tmpvar_33 = pow (max (0.0, dot (h_4, tmpvar_31)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_33;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_31)
  ) * atten_6));
  mediump vec3 c_34;
  c_34 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_34, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_35;
  tmpvar_35 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_35);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_13;
  mediump float tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_16;
  tmpvar_16 = clamp ((tmpvar_15.x + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_14 = tmpvar_16;
  atten_6 = tmpvar_14;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_17;
  mediump vec3 tmpvar_18;
  tmpvar_18 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = pow (max (0.0, dot (h_4, tmpvar_18)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_20;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_18)
  ) * atten_6));
  mediump vec3 c_21;
  c_21 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_21, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_22;
  tmpvar_22 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_22);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_13;
  mediump float tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_16;
  tmpvar_16 = clamp ((tmpvar_15.x + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_14 = tmpvar_16;
  atten_6 = tmpvar_14;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  highp vec4 tmpvar_18;
  tmpvar_18.zw = vec2(0.0, -8.0);
  tmpvar_18.xy = (_LightMatrix0 * tmpvar_17).xy;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_LightTexture0, tmpvar_18.xy, -8.0);
  atten_6 = (atten_6 * tmpvar_19.w);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_20;
  mediump vec3 tmpvar_21;
  tmpvar_21 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_22;
  mediump float tmpvar_23;
  tmpvar_23 = pow (max (0.0, dot (h_4, tmpvar_21)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_23;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_21)
  ) * atten_6));
  mediump vec3 c_24;
  c_24 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_24, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_25);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 _LightPositionRange;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = -(normalize(tmpvar_13));
  lightDir_7 = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp float tmpvar_16;
  tmpvar_16 = texture2D (_LightTextureB0, vec2(tmpvar_15)).w;
  atten_6 = tmpvar_16;
  highp float mydist_17;
  mydist_17 = ((sqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * _LightPositionRange.w) * 0.97);
  highp float tmpvar_18;
  tmpvar_18 = dot (textureCube (_ShadowMapTexture, tmpvar_13), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  mediump float tmpvar_19;
  if ((tmpvar_18 < mydist_17)) {
    tmpvar_19 = _LightShadowData.x;
  } else {
    tmpvar_19 = 1.0;
  };
  atten_6 = (atten_6 * tmpvar_19);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_20;
  mediump vec3 tmpvar_21;
  tmpvar_21 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_22;
  mediump float tmpvar_23;
  tmpvar_23 = pow (max (0.0, dot (h_4, tmpvar_21)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_23;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_21)
  ) * atten_6));
  mediump vec3 c_24;
  c_24 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_24, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_25);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 _LightPositionRange;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = -(normalize(tmpvar_13));
  lightDir_7 = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp float tmpvar_16;
  tmpvar_16 = texture2D (_LightTextureB0, vec2(tmpvar_15)).w;
  atten_6 = tmpvar_16;
  highp float mydist_17;
  mydist_17 = ((sqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * _LightPositionRange.w) * 0.97);
  highp float tmpvar_18;
  tmpvar_18 = dot (textureCube (_ShadowMapTexture, tmpvar_13), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  mediump float tmpvar_19;
  if ((tmpvar_18 < mydist_17)) {
    tmpvar_19 = _LightShadowData.x;
  } else {
    tmpvar_19 = 1.0;
  };
  atten_6 = (atten_6 * tmpvar_19);
  highp vec4 tmpvar_20;
  tmpvar_20.w = 1.0;
  tmpvar_20.xyz = tmpvar_10;
  highp vec4 tmpvar_21;
  tmpvar_21.w = -8.0;
  tmpvar_21.xyz = (_LightMatrix0 * tmpvar_20).xyz;
  lowp vec4 tmpvar_22;
  tmpvar_22 = textureCube (_LightTexture0, tmpvar_21.xyz, -8.0);
  atten_6 = (atten_6 * tmpvar_22.w);
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_23;
  mediump vec3 tmpvar_24;
  tmpvar_24 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_25;
  tmpvar_25 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_25;
  mediump float tmpvar_26;
  tmpvar_26 = pow (max (0.0, dot (h_4, tmpvar_24)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_26;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_24)
  ) * atten_6));
  mediump vec3 c_27;
  c_27 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_27, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_28;
  tmpvar_28 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_28);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NONATIVE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform highp sampler2D _ShadowMapTexture;
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_13);
  lightDir_7 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (tmpvar_16.xy / tmpvar_16.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  highp float tmpvar_19;
  tmpvar_19 = tmpvar_18.w;
  atten_6 = (tmpvar_19 * float((tmpvar_16.w < 0.0)));
  highp float tmpvar_20;
  tmpvar_20 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
  atten_6 = (atten_6 * tmpvar_21.w);
  mediump float tmpvar_22;
  highp float tmpvar_23;
  tmpvar_23 = clamp (((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = tmpvar_10;
  highp vec4 tmpvar_25;
  tmpvar_25 = (unity_World2Shadow[0] * tmpvar_24);
  lowp float tmpvar_26;
  highp vec4 shadowVals_27;
  highp vec3 tmpvar_28;
  tmpvar_28 = (tmpvar_25.xyz / tmpvar_25.w);
  shadowVals_27.x = texture2D (_ShadowMapTexture, (tmpvar_28.xy + _ShadowOffsets[0].xy)).x;
  shadowVals_27.y = texture2D (_ShadowMapTexture, (tmpvar_28.xy + _ShadowOffsets[1].xy)).x;
  shadowVals_27.z = texture2D (_ShadowMapTexture, (tmpvar_28.xy + _ShadowOffsets[2].xy)).x;
  shadowVals_27.w = texture2D (_ShadowMapTexture, (tmpvar_28.xy + _ShadowOffsets[3].xy)).x;
  bvec4 tmpvar_29;
  tmpvar_29 = lessThan (shadowVals_27, tmpvar_28.zzzz);
  mediump vec4 tmpvar_30;
  tmpvar_30 = _LightShadowData.xxxx;
  mediump float tmpvar_31;
  if (tmpvar_29.x) {
    tmpvar_31 = tmpvar_30.x;
  } else {
    tmpvar_31 = 1.0;
  };
  mediump float tmpvar_32;
  if (tmpvar_29.y) {
    tmpvar_32 = tmpvar_30.y;
  } else {
    tmpvar_32 = 1.0;
  };
  mediump float tmpvar_33;
  if (tmpvar_29.z) {
    tmpvar_33 = tmpvar_30.z;
  } else {
    tmpvar_33 = 1.0;
  };
  mediump float tmpvar_34;
  if (tmpvar_29.w) {
    tmpvar_34 = tmpvar_30.w;
  } else {
    tmpvar_34 = 1.0;
  };
  mediump vec4 tmpvar_35;
  tmpvar_35.x = tmpvar_31;
  tmpvar_35.y = tmpvar_32;
  tmpvar_35.z = tmpvar_33;
  tmpvar_35.w = tmpvar_34;
  mediump float tmpvar_36;
  tmpvar_36 = dot (tmpvar_35, vec4(0.25, 0.25, 0.25, 0.25));
  tmpvar_26 = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = clamp ((tmpvar_26 + tmpvar_23), 0.0, 1.0);
  tmpvar_22 = tmpvar_37;
  atten_6 = (atten_6 * tmpvar_22);
  lowp vec4 tmpvar_38;
  tmpvar_38 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_38;
  mediump vec3 tmpvar_39;
  tmpvar_39 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_40;
  tmpvar_40 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_40;
  mediump float tmpvar_41;
  tmpvar_41 = pow (max (0.0, dot (h_4, tmpvar_39)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_41;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_39)
  ) * atten_6));
  mediump vec3 c_42;
  c_42 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_42, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_43;
  tmpvar_43 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_43);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NATIVE" }
"#version 100

#ifdef VERTEX
#extension GL_EXT_shadow_samplers : enable
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
#extension GL_EXT_shadow_samplers : enable
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp mat4 unity_World2Shadow[4];
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(tmpvar_13);
  lightDir_7 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  highp vec4 tmpvar_16;
  tmpvar_16 = (_LightMatrix0 * tmpvar_15);
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (tmpvar_16.xy / tmpvar_16.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  highp float tmpvar_19;
  tmpvar_19 = tmpvar_18.w;
  atten_6 = (tmpvar_19 * float((tmpvar_16.w < 0.0)));
  highp float tmpvar_20;
  tmpvar_20 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_LightTextureB0, vec2(tmpvar_20));
  atten_6 = (atten_6 * tmpvar_21.w);
  mediump float tmpvar_22;
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = tmpvar_10;
  highp vec4 tmpvar_24;
  tmpvar_24 = (unity_World2Shadow[0] * tmpvar_23);
  lowp float tmpvar_25;
  mediump vec4 shadows_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (tmpvar_24.xyz / tmpvar_24.w);
  highp vec3 coord_28;
  coord_28 = (tmpvar_27 + _ShadowOffsets[0].xyz);
  lowp float tmpvar_29;
  tmpvar_29 = shadow2DEXT (_ShadowMapTexture, coord_28);
  shadows_26.x = tmpvar_29;
  highp vec3 coord_30;
  coord_30 = (tmpvar_27 + _ShadowOffsets[1].xyz);
  lowp float tmpvar_31;
  tmpvar_31 = shadow2DEXT (_ShadowMapTexture, coord_30);
  shadows_26.y = tmpvar_31;
  highp vec3 coord_32;
  coord_32 = (tmpvar_27 + _ShadowOffsets[2].xyz);
  lowp float tmpvar_33;
  tmpvar_33 = shadow2DEXT (_ShadowMapTexture, coord_32);
  shadows_26.z = tmpvar_33;
  highp vec3 coord_34;
  coord_34 = (tmpvar_27 + _ShadowOffsets[3].xyz);
  lowp float tmpvar_35;
  tmpvar_35 = shadow2DEXT (_ShadowMapTexture, coord_34);
  shadows_26.w = tmpvar_35;
  shadows_26 = (_LightShadowData.xxxx + (shadows_26 * (1.0 - _LightShadowData.xxxx)));
  mediump float tmpvar_36;
  tmpvar_36 = dot (shadows_26, vec4(0.25, 0.25, 0.25, 0.25));
  tmpvar_25 = tmpvar_36;
  highp float tmpvar_37;
  tmpvar_37 = clamp ((tmpvar_25 + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_22 = tmpvar_37;
  atten_6 = (atten_6 * tmpvar_22);
  lowp vec4 tmpvar_38;
  tmpvar_38 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_38;
  mediump vec3 tmpvar_39;
  tmpvar_39 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_40;
  tmpvar_40 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_40;
  mediump float tmpvar_41;
  tmpvar_41 = pow (max (0.0, dot (h_4, tmpvar_39)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_41;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_39)
  ) * atten_6));
  mediump vec3 c_42;
  c_42 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_42, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_43;
  tmpvar_43 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_43);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" "SHADOWS_SOFT" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 _LightPositionRange;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = -(normalize(tmpvar_13));
  lightDir_7 = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp float tmpvar_16;
  tmpvar_16 = texture2D (_LightTextureB0, vec2(tmpvar_15)).w;
  atten_6 = tmpvar_16;
  highp vec4 shadowVals_17;
  highp float mydist_18;
  mydist_18 = ((sqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * _LightPositionRange.w) * 0.97);
  shadowVals_17.x = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(0.0078125, 0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.y = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(-0.0078125, -0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.z = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(-0.0078125, 0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.w = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(0.0078125, -0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  bvec4 tmpvar_19;
  tmpvar_19 = lessThan (shadowVals_17, vec4(mydist_18));
  mediump vec4 tmpvar_20;
  tmpvar_20 = _LightShadowData.xxxx;
  mediump float tmpvar_21;
  if (tmpvar_19.x) {
    tmpvar_21 = tmpvar_20.x;
  } else {
    tmpvar_21 = 1.0;
  };
  mediump float tmpvar_22;
  if (tmpvar_19.y) {
    tmpvar_22 = tmpvar_20.y;
  } else {
    tmpvar_22 = 1.0;
  };
  mediump float tmpvar_23;
  if (tmpvar_19.z) {
    tmpvar_23 = tmpvar_20.z;
  } else {
    tmpvar_23 = 1.0;
  };
  mediump float tmpvar_24;
  if (tmpvar_19.w) {
    tmpvar_24 = tmpvar_20.w;
  } else {
    tmpvar_24 = 1.0;
  };
  mediump vec4 tmpvar_25;
  tmpvar_25.x = tmpvar_21;
  tmpvar_25.y = tmpvar_22;
  tmpvar_25.z = tmpvar_23;
  tmpvar_25.w = tmpvar_24;
  mediump float tmpvar_26;
  tmpvar_26 = dot (tmpvar_25, vec4(0.25, 0.25, 0.25, 0.25));
  atten_6 = (atten_6 * tmpvar_26);
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_27;
  mediump vec3 tmpvar_28;
  tmpvar_28 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_29;
  tmpvar_29 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_29;
  mediump float tmpvar_30;
  tmpvar_30 = pow (max (0.0, dot (h_4, tmpvar_28)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_30;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_28)
  ) * atten_6));
  mediump vec3 c_31;
  c_31 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_31, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_32;
  tmpvar_32 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_32);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" "SHADOWS_SOFT" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform highp vec4 _LightPositionRange;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_14;
  tmpvar_14 = -(normalize(tmpvar_13));
  lightDir_7 = tmpvar_14;
  highp float tmpvar_15;
  tmpvar_15 = (dot (tmpvar_13, tmpvar_13) * _LightPos.w);
  lowp float tmpvar_16;
  tmpvar_16 = texture2D (_LightTextureB0, vec2(tmpvar_15)).w;
  atten_6 = tmpvar_16;
  highp vec4 shadowVals_17;
  highp float mydist_18;
  mydist_18 = ((sqrt(
    dot (tmpvar_13, tmpvar_13)
  ) * _LightPositionRange.w) * 0.97);
  shadowVals_17.x = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(0.0078125, 0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.y = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(-0.0078125, -0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.z = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(-0.0078125, 0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_17.w = dot (textureCube (_ShadowMapTexture, (tmpvar_13 + vec3(0.0078125, -0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  bvec4 tmpvar_19;
  tmpvar_19 = lessThan (shadowVals_17, vec4(mydist_18));
  mediump vec4 tmpvar_20;
  tmpvar_20 = _LightShadowData.xxxx;
  mediump float tmpvar_21;
  if (tmpvar_19.x) {
    tmpvar_21 = tmpvar_20.x;
  } else {
    tmpvar_21 = 1.0;
  };
  mediump float tmpvar_22;
  if (tmpvar_19.y) {
    tmpvar_22 = tmpvar_20.y;
  } else {
    tmpvar_22 = 1.0;
  };
  mediump float tmpvar_23;
  if (tmpvar_19.z) {
    tmpvar_23 = tmpvar_20.z;
  } else {
    tmpvar_23 = 1.0;
  };
  mediump float tmpvar_24;
  if (tmpvar_19.w) {
    tmpvar_24 = tmpvar_20.w;
  } else {
    tmpvar_24 = 1.0;
  };
  mediump vec4 tmpvar_25;
  tmpvar_25.x = tmpvar_21;
  tmpvar_25.y = tmpvar_22;
  tmpvar_25.z = tmpvar_23;
  tmpvar_25.w = tmpvar_24;
  mediump float tmpvar_26;
  tmpvar_26 = dot (tmpvar_25, vec4(0.25, 0.25, 0.25, 0.25));
  atten_6 = (atten_6 * tmpvar_26);
  highp vec4 tmpvar_27;
  tmpvar_27.w = 1.0;
  tmpvar_27.xyz = tmpvar_10;
  highp vec4 tmpvar_28;
  tmpvar_28.w = -8.0;
  tmpvar_28.xyz = (_LightMatrix0 * tmpvar_27).xyz;
  lowp vec4 tmpvar_29;
  tmpvar_29 = textureCube (_LightTexture0, tmpvar_28.xyz, -8.0);
  atten_6 = (atten_6 * tmpvar_29.w);
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_30;
  mediump vec3 tmpvar_31;
  tmpvar_31 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_32;
  tmpvar_32 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_32;
  mediump float tmpvar_33;
  tmpvar_33 = pow (max (0.0, dot (h_4, tmpvar_31)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_33;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_31)
  ) * atten_6));
  mediump vec3 c_34;
  c_34 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_34, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_35;
  tmpvar_35 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_35);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_13;
  mediump float tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_16;
  tmpvar_16 = clamp ((tmpvar_15.x + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_14 = tmpvar_16;
  atten_6 = tmpvar_14;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_17;
  mediump vec3 tmpvar_18;
  tmpvar_18 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_19;
  mediump float tmpvar_20;
  tmpvar_20 = pow (max (0.0, dot (h_4, tmpvar_18)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_20;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_18)
  ) * atten_6));
  mediump vec3 c_21;
  c_21 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_21, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_22;
  tmpvar_22 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_22);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
"#version 100

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
uniform highp vec3 _WorldSpaceCameraPos;
uniform highp vec4 _ProjectionParams;
uniform highp vec4 _ZBufferParams;
uniform mediump vec4 _LightShadowData;
uniform highp vec4 unity_ShadowFadeCenterAndType;
uniform mediump vec4 unity_ColorSpaceLuminance;
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp vec4 unity_LightmapFade;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraNormalsTexture;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 res_2;
  highp float spec_3;
  mediump vec3 h_4;
  mediump vec4 nspec_5;
  highp float atten_6;
  mediump vec3 lightDir_7;
  highp vec2 tmpvar_8;
  tmpvar_8 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_9;
  tmpvar_9.w = 1.0;
  tmpvar_9.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_8).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_10;
  tmpvar_10 = (_CameraToWorld * tmpvar_9).xyz;
  highp vec3 tmpvar_11;
  tmpvar_11 = (tmpvar_10 - unity_ShadowFadeCenterAndType.xyz);
  highp float tmpvar_12;
  tmpvar_12 = mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w);
  highp vec3 tmpvar_13;
  tmpvar_13 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_13;
  mediump float tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_16;
  tmpvar_16 = clamp ((tmpvar_15.x + clamp (
    ((tmpvar_12 * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_14 = tmpvar_16;
  atten_6 = tmpvar_14;
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_10;
  highp vec4 tmpvar_18;
  tmpvar_18.zw = vec2(0.0, -8.0);
  tmpvar_18.xy = (_LightMatrix0 * tmpvar_17).xy;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_LightTexture0, tmpvar_18.xy, -8.0);
  atten_6 = (atten_6 * tmpvar_19.w);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraNormalsTexture, tmpvar_8);
  nspec_5 = tmpvar_20;
  mediump vec3 tmpvar_21;
  tmpvar_21 = normalize(((nspec_5.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize((lightDir_7 - normalize(
    (tmpvar_10 - _WorldSpaceCameraPos)
  )));
  h_4 = tmpvar_22;
  mediump float tmpvar_23;
  tmpvar_23 = pow (max (0.0, dot (h_4, tmpvar_21)), (nspec_5.w * 128.0));
  spec_3 = tmpvar_23;
  spec_3 = (spec_3 * clamp (atten_6, 0.0, 1.0));
  res_2.xyz = (_LightColor.xyz * (max (0.0, 
    dot (lightDir_7, tmpvar_21)
  ) * atten_6));
  mediump vec3 c_24;
  c_24 = _LightColor.xyz;
  res_2.w = (spec_3 * dot (c_24, unity_ColorSpaceLuminance.xyz));
  highp float tmpvar_25;
  tmpvar_25 = clamp ((1.0 - (
    (tmpvar_12 * unity_LightmapFade.z)
   + unity_LightmapFade.w)), 0.0, 1.0);
  res_2 = (res_2 * tmpvar_25);
  tmpvar_1 = res_2.wxyz;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
}
Program "fp" {
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_OFF" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NATIVE" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" }
""
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" }
""
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NATIVE" }
""
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" "SHADOWS_SOFT" }
""
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" "SHADOWS_SOFT" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" "SHADOWS_NATIVE" }
""
}
}
 }
}
Fallback Off
}