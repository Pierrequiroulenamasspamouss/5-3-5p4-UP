//////////////////////////////////////////
///////////////////////////////////////////
Shader "Hidden/Internal-DeferredShading" {
Properties {
 _LightTexture0 ("", any) = "" { }
 _LightTextureB0 ("", 2D) = "" { }
 _ShadowMapTexture ("", any) = "" { }
 _SrcBlend ("", Float) = 1
 _DstBlend ("", Float) = 1
}
SubShader { 
 Pass {
  Tags { "SHADOWSUPPORT"="true" }
  ZWrite Off
  Blend [_SrcBlend] [_DstBlend]
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  tmpvar_11 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = -(normalize(tmpvar_11));
  lightDir_7 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = (dot (tmpvar_11, tmpvar_11) * _LightPos.w);
  lowp float tmpvar_14;
  tmpvar_14 = texture2D (_LightTextureB0, vec2(tmpvar_13)).w;
  atten_6 = tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_16;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_17;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_18;
  tmpvar_18 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_20;
  viewDir_20 = -(tmpvar_19);
  mediump float specular_21;
  mediump vec3 tmpvar_22;
  mediump vec3 inVec_23;
  inVec_23 = (lightDir_7 + viewDir_20);
  tmpvar_22 = (inVec_23 * inversesqrt(max (0.001, 
    dot (inVec_23, inVec_23)
  )));
  mediump float tmpvar_24;
  tmpvar_24 = max (0.0, dot (lightDir_7, tmpvar_22));
  mediump float tmpvar_25;
  tmpvar_25 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_26;
  tmpvar_26 = max (0.0001, (tmpvar_25 * tmpvar_25));
  mediump float tmpvar_27;
  tmpvar_27 = max (((2.0 / 
    (tmpvar_26 * tmpvar_26)
  ) - 2.0), 0.0001);
  specular_21 = sqrt(max (0.0001, (
    ((tmpvar_27 + 1.0) * pow (max (0.0, dot (tmpvar_18, tmpvar_22)), tmpvar_27))
   / 
    (((8.0 * (
      ((tmpvar_24 * tmpvar_24) * gbuffer1_3.w)
     + 
      (tmpvar_25 * tmpvar_25)
    )) * tmpvar_24) + 0.0001)
  )));
  mediump float tmpvar_28;
  tmpvar_28 = clamp (specular_21, 0.0, 100.0);
  specular_21 = tmpvar_28;
  mediump vec4 tmpvar_29;
  tmpvar_29.w = 1.0;
  tmpvar_29.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_28 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_18, lightDir_7)));
  mediump vec4 tmpvar_30;
  tmpvar_30 = exp2(-(tmpvar_29));
  tmpvar_1 = tmpvar_30;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
  mediump vec3 lightDir_6;
  highp vec2 tmpvar_7;
  tmpvar_7 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_8;
  tmpvar_8.w = 1.0;
  tmpvar_8.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_7).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_9;
  tmpvar_9 = -(_LightDir.xyz);
  lightDir_6 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_4 = tmpvar_10;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_3 = tmpvar_11;
  lowp vec4 tmpvar_12;
  tmpvar_12 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_2 = tmpvar_12;
  tmpvar_5 = _LightColor.xyz;
  mediump vec3 tmpvar_13;
  tmpvar_13 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_14;
  tmpvar_14 = normalize(((_CameraToWorld * tmpvar_8).xyz - _WorldSpaceCameraPos));
  mediump vec3 viewDir_15;
  viewDir_15 = -(tmpvar_14);
  mediump float specular_16;
  mediump vec3 tmpvar_17;
  mediump vec3 inVec_18;
  inVec_18 = (lightDir_6 + viewDir_15);
  tmpvar_17 = (inVec_18 * inversesqrt(max (0.001, 
    dot (inVec_18, inVec_18)
  )));
  mediump float tmpvar_19;
  tmpvar_19 = max (0.0, dot (lightDir_6, tmpvar_17));
  mediump float tmpvar_20;
  tmpvar_20 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_21;
  tmpvar_21 = max (0.0001, (tmpvar_20 * tmpvar_20));
  mediump float tmpvar_22;
  tmpvar_22 = max (((2.0 / 
    (tmpvar_21 * tmpvar_21)
  ) - 2.0), 0.0001);
  specular_16 = sqrt(max (0.0001, (
    ((tmpvar_22 + 1.0) * pow (max (0.0, dot (tmpvar_13, tmpvar_17)), tmpvar_22))
   / 
    (((8.0 * (
      ((tmpvar_19 * tmpvar_19) * gbuffer1_3.w)
     + 
      (tmpvar_20 * tmpvar_20)
    )) * tmpvar_19) + 0.0001)
  )));
  mediump float tmpvar_23;
  tmpvar_23 = clamp (specular_16, 0.0, 100.0);
  specular_16 = tmpvar_23;
  mediump vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_23 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_13, lightDir_6)));
  mediump vec4 tmpvar_25;
  tmpvar_25 = exp2(-(tmpvar_24));
  tmpvar_1 = tmpvar_25;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  tmpvar_11 = (_LightPos.xyz - tmpvar_10);
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize(tmpvar_11);
  lightDir_7 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_10;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_LightMatrix0 * tmpvar_13);
  highp vec4 tmpvar_15;
  tmpvar_15.zw = vec2(0.0, -8.0);
  tmpvar_15.xy = (tmpvar_14.xy / tmpvar_14.w);
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_LightTexture0, tmpvar_15.xy, -8.0);
  highp float tmpvar_17;
  tmpvar_17 = tmpvar_16.w;
  atten_6 = (tmpvar_17 * float((tmpvar_14.w < 0.0)));
  highp float tmpvar_18;
  tmpvar_18 = (dot (tmpvar_11, tmpvar_11) * _LightPos.w);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_LightTextureB0, vec2(tmpvar_18));
  atten_6 = (atten_6 * tmpvar_19.w);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_20;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_22;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_23;
  tmpvar_23 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_24;
  tmpvar_24 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_25;
  viewDir_25 = -(tmpvar_24);
  mediump float specular_26;
  mediump vec3 tmpvar_27;
  mediump vec3 inVec_28;
  inVec_28 = (lightDir_7 + viewDir_25);
  tmpvar_27 = (inVec_28 * inversesqrt(max (0.001, 
    dot (inVec_28, inVec_28)
  )));
  mediump float tmpvar_29;
  tmpvar_29 = max (0.0, dot (lightDir_7, tmpvar_27));
  mediump float tmpvar_30;
  tmpvar_30 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_31;
  tmpvar_31 = max (0.0001, (tmpvar_30 * tmpvar_30));
  mediump float tmpvar_32;
  tmpvar_32 = max (((2.0 / 
    (tmpvar_31 * tmpvar_31)
  ) - 2.0), 0.0001);
  specular_26 = sqrt(max (0.0001, (
    ((tmpvar_32 + 1.0) * pow (max (0.0, dot (tmpvar_23, tmpvar_27)), tmpvar_32))
   / 
    (((8.0 * (
      ((tmpvar_29 * tmpvar_29) * gbuffer1_3.w)
     + 
      (tmpvar_30 * tmpvar_30)
    )) * tmpvar_29) + 0.0001)
  )));
  mediump float tmpvar_33;
  tmpvar_33 = clamp (specular_26, 0.0, 100.0);
  specular_26 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34.w = 1.0;
  tmpvar_34.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_33 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_23, lightDir_7)));
  mediump vec4 tmpvar_35;
  tmpvar_35 = exp2(-(tmpvar_34));
  tmpvar_1 = tmpvar_35;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  tmpvar_11 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = -(normalize(tmpvar_11));
  lightDir_7 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = (dot (tmpvar_11, tmpvar_11) * _LightPos.w);
  lowp float tmpvar_14;
  tmpvar_14 = texture2D (_LightTextureB0, vec2(tmpvar_13)).w;
  atten_6 = tmpvar_14;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_10;
  highp vec4 tmpvar_16;
  tmpvar_16.w = -8.0;
  tmpvar_16.xyz = (_LightMatrix0 * tmpvar_15).xyz;
  lowp vec4 tmpvar_17;
  tmpvar_17 = textureCube (_LightTexture0, tmpvar_16.xyz, -8.0);
  atten_6 = (atten_6 * tmpvar_17.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_20;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_21;
  tmpvar_21 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_23;
  viewDir_23 = -(tmpvar_22);
  mediump float specular_24;
  mediump vec3 tmpvar_25;
  mediump vec3 inVec_26;
  inVec_26 = (lightDir_7 + viewDir_23);
  tmpvar_25 = (inVec_26 * inversesqrt(max (0.001, 
    dot (inVec_26, inVec_26)
  )));
  mediump float tmpvar_27;
  tmpvar_27 = max (0.0, dot (lightDir_7, tmpvar_25));
  mediump float tmpvar_28;
  tmpvar_28 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_29;
  tmpvar_29 = max (0.0001, (tmpvar_28 * tmpvar_28));
  mediump float tmpvar_30;
  tmpvar_30 = max (((2.0 / 
    (tmpvar_29 * tmpvar_29)
  ) - 2.0), 0.0001);
  specular_24 = sqrt(max (0.0001, (
    ((tmpvar_30 + 1.0) * pow (max (0.0, dot (tmpvar_21, tmpvar_25)), tmpvar_30))
   / 
    (((8.0 * (
      ((tmpvar_27 * tmpvar_27) * gbuffer1_3.w)
     + 
      (tmpvar_28 * tmpvar_28)
    )) * tmpvar_27) + 0.0001)
  )));
  mediump float tmpvar_31;
  tmpvar_31 = clamp (specular_24, 0.0, 100.0);
  specular_24 = tmpvar_31;
  mediump vec4 tmpvar_32;
  tmpvar_32.w = 1.0;
  tmpvar_32.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_31 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_21, lightDir_7)));
  mediump vec4 tmpvar_33;
  tmpvar_33 = exp2(-(tmpvar_32));
  tmpvar_1 = tmpvar_33;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  tmpvar_11 = -(_LightDir.xyz);
  lightDir_7 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = tmpvar_10;
  highp vec4 tmpvar_13;
  tmpvar_13.zw = vec2(0.0, -8.0);
  tmpvar_13.xy = (_LightMatrix0 * tmpvar_12).xy;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_LightTexture0, tmpvar_13.xy, -8.0);
  atten_6 = tmpvar_14.w;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_16;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_17;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_18;
  tmpvar_18 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_20;
  viewDir_20 = -(tmpvar_19);
  mediump float specular_21;
  mediump vec3 tmpvar_22;
  mediump vec3 inVec_23;
  inVec_23 = (lightDir_7 + viewDir_20);
  tmpvar_22 = (inVec_23 * inversesqrt(max (0.001, 
    dot (inVec_23, inVec_23)
  )));
  mediump float tmpvar_24;
  tmpvar_24 = max (0.0, dot (lightDir_7, tmpvar_22));
  mediump float tmpvar_25;
  tmpvar_25 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_26;
  tmpvar_26 = max (0.0001, (tmpvar_25 * tmpvar_25));
  mediump float tmpvar_27;
  tmpvar_27 = max (((2.0 / 
    (tmpvar_26 * tmpvar_26)
  ) - 2.0), 0.0001);
  specular_21 = sqrt(max (0.0001, (
    ((tmpvar_27 + 1.0) * pow (max (0.0, dot (tmpvar_18, tmpvar_22)), tmpvar_27))
   / 
    (((8.0 * (
      ((tmpvar_24 * tmpvar_24) * gbuffer1_3.w)
     + 
      (tmpvar_25 * tmpvar_25)
    )) * tmpvar_24) + 0.0001)
  )));
  mediump float tmpvar_28;
  tmpvar_28 = clamp (specular_21, 0.0, 100.0);
  specular_21 = tmpvar_28;
  mediump vec4 tmpvar_29;
  tmpvar_29.w = 1.0;
  tmpvar_29.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_28 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_18, lightDir_7)));
  mediump vec4 tmpvar_30;
  tmpvar_30 = exp2(-(tmpvar_29));
  tmpvar_1 = tmpvar_30;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  mediump float tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = clamp (((
    mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w)
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = tmpvar_10;
  highp vec4 tmpvar_24;
  tmpvar_24 = (unity_World2Shadow[0] * tmpvar_23);
  lowp float tmpvar_25;
  highp vec4 tmpvar_26;
  tmpvar_26 = texture2DProj (_ShadowMapTexture, tmpvar_24);
  mediump float tmpvar_27;
  if ((tmpvar_26.x < (tmpvar_24.z / tmpvar_24.w))) {
    tmpvar_27 = _LightShadowData.x;
  } else {
    tmpvar_27 = 1.0;
  };
  tmpvar_25 = tmpvar_27;
  highp float tmpvar_28;
  tmpvar_28 = clamp ((tmpvar_25 + tmpvar_22), 0.0, 1.0);
  tmpvar_21 = tmpvar_28;
  atten_6 = (atten_6 * tmpvar_21);
  lowp vec4 tmpvar_29;
  tmpvar_29 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_29;
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_30;
  lowp vec4 tmpvar_31;
  tmpvar_31 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_31;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_32;
  tmpvar_32 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_33;
  tmpvar_33 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_34;
  viewDir_34 = -(tmpvar_33);
  mediump float specular_35;
  mediump vec3 tmpvar_36;
  mediump vec3 inVec_37;
  inVec_37 = (lightDir_7 + viewDir_34);
  tmpvar_36 = (inVec_37 * inversesqrt(max (0.001, 
    dot (inVec_37, inVec_37)
  )));
  mediump float tmpvar_38;
  tmpvar_38 = max (0.0, dot (lightDir_7, tmpvar_36));
  mediump float tmpvar_39;
  tmpvar_39 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_40;
  tmpvar_40 = max (0.0001, (tmpvar_39 * tmpvar_39));
  mediump float tmpvar_41;
  tmpvar_41 = max (((2.0 / 
    (tmpvar_40 * tmpvar_40)
  ) - 2.0), 0.0001);
  specular_35 = sqrt(max (0.0001, (
    ((tmpvar_41 + 1.0) * pow (max (0.0, dot (tmpvar_32, tmpvar_36)), tmpvar_41))
   / 
    (((8.0 * (
      ((tmpvar_38 * tmpvar_38) * gbuffer1_3.w)
     + 
      (tmpvar_39 * tmpvar_39)
    )) * tmpvar_38) + 0.0001)
  )));
  mediump float tmpvar_42;
  tmpvar_42 = clamp (specular_35, 0.0, 100.0);
  specular_35 = tmpvar_42;
  mediump vec4 tmpvar_43;
  tmpvar_43.w = 1.0;
  tmpvar_43.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_42 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_32, lightDir_7)));
  mediump vec4 tmpvar_44;
  tmpvar_44 = exp2(-(tmpvar_43));
  tmpvar_1 = tmpvar_44;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  mediump float tmpvar_21;
  highp vec4 tmpvar_22;
  tmpvar_22.w = 1.0;
  tmpvar_22.xyz = tmpvar_10;
  highp vec4 tmpvar_23;
  tmpvar_23 = (unity_World2Shadow[0] * tmpvar_22);
  lowp float tmpvar_24;
  mediump float shadow_25;
  lowp float tmpvar_26;
  tmpvar_26 = shadow2DProjEXT (_ShadowMapTexture, tmpvar_23);
  mediump float tmpvar_27;
  tmpvar_27 = tmpvar_26;
  shadow_25 = (_LightShadowData.x + (tmpvar_27 * (1.0 - _LightShadowData.x)));
  tmpvar_24 = shadow_25;
  highp float tmpvar_28;
  tmpvar_28 = clamp ((tmpvar_24 + clamp (
    ((mix (tmpvar_9.z, sqrt(
      dot (tmpvar_11, tmpvar_11)
    ), unity_ShadowFadeCenterAndType.w) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_21 = tmpvar_28;
  atten_6 = (atten_6 * tmpvar_21);
  lowp vec4 tmpvar_29;
  tmpvar_29 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_29;
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_30;
  lowp vec4 tmpvar_31;
  tmpvar_31 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_31;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_32;
  tmpvar_32 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_33;
  tmpvar_33 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_34;
  viewDir_34 = -(tmpvar_33);
  mediump float specular_35;
  mediump vec3 tmpvar_36;
  mediump vec3 inVec_37;
  inVec_37 = (lightDir_7 + viewDir_34);
  tmpvar_36 = (inVec_37 * inversesqrt(max (0.001, 
    dot (inVec_37, inVec_37)
  )));
  mediump float tmpvar_38;
  tmpvar_38 = max (0.0, dot (lightDir_7, tmpvar_36));
  mediump float tmpvar_39;
  tmpvar_39 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_40;
  tmpvar_40 = max (0.0001, (tmpvar_39 * tmpvar_39));
  mediump float tmpvar_41;
  tmpvar_41 = max (((2.0 / 
    (tmpvar_40 * tmpvar_40)
  ) - 2.0), 0.0001);
  specular_35 = sqrt(max (0.0001, (
    ((tmpvar_41 + 1.0) * pow (max (0.0, dot (tmpvar_32, tmpvar_36)), tmpvar_41))
   / 
    (((8.0 * (
      ((tmpvar_38 * tmpvar_38) * gbuffer1_3.w)
     + 
      (tmpvar_39 * tmpvar_39)
    )) * tmpvar_38) + 0.0001)
  )));
  mediump float tmpvar_42;
  tmpvar_42 = clamp (specular_35, 0.0, 100.0);
  specular_35 = tmpvar_42;
  mediump vec4 tmpvar_43;
  tmpvar_43.w = 1.0;
  tmpvar_43.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_42 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_32, lightDir_7)));
  mediump vec4 tmpvar_44;
  tmpvar_44 = exp2(-(tmpvar_43));
  tmpvar_1 = tmpvar_44;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  mediump float tmpvar_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_15;
  tmpvar_15 = clamp ((tmpvar_14.x + clamp (
    ((mix (tmpvar_9.z, sqrt(
      dot (tmpvar_11, tmpvar_11)
    ), unity_ShadowFadeCenterAndType.w) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_13 = tmpvar_15;
  atten_6 = tmpvar_13;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_16;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_17;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_18;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_19;
  tmpvar_19 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_20;
  tmpvar_20 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_21;
  viewDir_21 = -(tmpvar_20);
  mediump float specular_22;
  mediump vec3 tmpvar_23;
  mediump vec3 inVec_24;
  inVec_24 = (lightDir_7 + viewDir_21);
  tmpvar_23 = (inVec_24 * inversesqrt(max (0.001, 
    dot (inVec_24, inVec_24)
  )));
  mediump float tmpvar_25;
  tmpvar_25 = max (0.0, dot (lightDir_7, tmpvar_23));
  mediump float tmpvar_26;
  tmpvar_26 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_27;
  tmpvar_27 = max (0.0001, (tmpvar_26 * tmpvar_26));
  mediump float tmpvar_28;
  tmpvar_28 = max (((2.0 / 
    (tmpvar_27 * tmpvar_27)
  ) - 2.0), 0.0001);
  specular_22 = sqrt(max (0.0001, (
    ((tmpvar_28 + 1.0) * pow (max (0.0, dot (tmpvar_19, tmpvar_23)), tmpvar_28))
   / 
    (((8.0 * (
      ((tmpvar_25 * tmpvar_25) * gbuffer1_3.w)
     + 
      (tmpvar_26 * tmpvar_26)
    )) * tmpvar_25) + 0.0001)
  )));
  mediump float tmpvar_29;
  tmpvar_29 = clamp (specular_22, 0.0, 100.0);
  specular_22 = tmpvar_29;
  mediump vec4 tmpvar_30;
  tmpvar_30.w = 1.0;
  tmpvar_30.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_29 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_19, lightDir_7)));
  mediump vec4 tmpvar_31;
  tmpvar_31 = exp2(-(tmpvar_30));
  tmpvar_1 = tmpvar_31;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  mediump float tmpvar_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_15;
  tmpvar_15 = clamp ((tmpvar_14.x + clamp (
    ((mix (tmpvar_9.z, sqrt(
      dot (tmpvar_11, tmpvar_11)
    ), unity_ShadowFadeCenterAndType.w) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_13 = tmpvar_15;
  atten_6 = tmpvar_13;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_10;
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (_LightMatrix0 * tmpvar_16).xy;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  atten_6 = (atten_6 * tmpvar_18.w);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_20;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_21;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_22;
  tmpvar_22 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_24;
  viewDir_24 = -(tmpvar_23);
  mediump float specular_25;
  mediump vec3 tmpvar_26;
  mediump vec3 inVec_27;
  inVec_27 = (lightDir_7 + viewDir_24);
  tmpvar_26 = (inVec_27 * inversesqrt(max (0.001, 
    dot (inVec_27, inVec_27)
  )));
  mediump float tmpvar_28;
  tmpvar_28 = max (0.0, dot (lightDir_7, tmpvar_26));
  mediump float tmpvar_29;
  tmpvar_29 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_30;
  tmpvar_30 = max (0.0001, (tmpvar_29 * tmpvar_29));
  mediump float tmpvar_31;
  tmpvar_31 = max (((2.0 / 
    (tmpvar_30 * tmpvar_30)
  ) - 2.0), 0.0001);
  specular_25 = sqrt(max (0.0001, (
    ((tmpvar_31 + 1.0) * pow (max (0.0, dot (tmpvar_22, tmpvar_26)), tmpvar_31))
   / 
    (((8.0 * (
      ((tmpvar_28 * tmpvar_28) * gbuffer1_3.w)
     + 
      (tmpvar_29 * tmpvar_29)
    )) * tmpvar_28) + 0.0001)
  )));
  mediump float tmpvar_32;
  tmpvar_32 = clamp (specular_25, 0.0, 100.0);
  specular_25 = tmpvar_32;
  mediump vec4 tmpvar_33;
  tmpvar_33.w = 1.0;
  tmpvar_33.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_32 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_22, lightDir_7)));
  mediump vec4 tmpvar_34;
  tmpvar_34 = exp2(-(tmpvar_33));
  tmpvar_1 = tmpvar_34;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  tmpvar_11 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = -(normalize(tmpvar_11));
  lightDir_7 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = (dot (tmpvar_11, tmpvar_11) * _LightPos.w);
  lowp float tmpvar_14;
  tmpvar_14 = texture2D (_LightTextureB0, vec2(tmpvar_13)).w;
  atten_6 = tmpvar_14;
  highp float mydist_15;
  mydist_15 = ((sqrt(
    dot (tmpvar_11, tmpvar_11)
  ) * _LightPositionRange.w) * 0.97);
  highp float tmpvar_16;
  tmpvar_16 = dot (textureCube (_ShadowMapTexture, tmpvar_11), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  mediump float tmpvar_17;
  if ((tmpvar_16 < mydist_15)) {
    tmpvar_17 = _LightShadowData.x;
  } else {
    tmpvar_17 = 1.0;
  };
  atten_6 = (atten_6 * tmpvar_17);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_20;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_21;
  tmpvar_21 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_23;
  viewDir_23 = -(tmpvar_22);
  mediump float specular_24;
  mediump vec3 tmpvar_25;
  mediump vec3 inVec_26;
  inVec_26 = (lightDir_7 + viewDir_23);
  tmpvar_25 = (inVec_26 * inversesqrt(max (0.001, 
    dot (inVec_26, inVec_26)
  )));
  mediump float tmpvar_27;
  tmpvar_27 = max (0.0, dot (lightDir_7, tmpvar_25));
  mediump float tmpvar_28;
  tmpvar_28 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_29;
  tmpvar_29 = max (0.0001, (tmpvar_28 * tmpvar_28));
  mediump float tmpvar_30;
  tmpvar_30 = max (((2.0 / 
    (tmpvar_29 * tmpvar_29)
  ) - 2.0), 0.0001);
  specular_24 = sqrt(max (0.0001, (
    ((tmpvar_30 + 1.0) * pow (max (0.0, dot (tmpvar_21, tmpvar_25)), tmpvar_30))
   / 
    (((8.0 * (
      ((tmpvar_27 * tmpvar_27) * gbuffer1_3.w)
     + 
      (tmpvar_28 * tmpvar_28)
    )) * tmpvar_27) + 0.0001)
  )));
  mediump float tmpvar_31;
  tmpvar_31 = clamp (specular_24, 0.0, 100.0);
  specular_24 = tmpvar_31;
  mediump vec4 tmpvar_32;
  tmpvar_32.w = 1.0;
  tmpvar_32.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_31 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_21, lightDir_7)));
  mediump vec4 tmpvar_33;
  tmpvar_33 = exp2(-(tmpvar_32));
  tmpvar_1 = tmpvar_33;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  tmpvar_11 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = -(normalize(tmpvar_11));
  lightDir_7 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = (dot (tmpvar_11, tmpvar_11) * _LightPos.w);
  lowp float tmpvar_14;
  tmpvar_14 = texture2D (_LightTextureB0, vec2(tmpvar_13)).w;
  atten_6 = tmpvar_14;
  highp float mydist_15;
  mydist_15 = ((sqrt(
    dot (tmpvar_11, tmpvar_11)
  ) * _LightPositionRange.w) * 0.97);
  highp float tmpvar_16;
  tmpvar_16 = dot (textureCube (_ShadowMapTexture, tmpvar_11), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  mediump float tmpvar_17;
  if ((tmpvar_16 < mydist_15)) {
    tmpvar_17 = _LightShadowData.x;
  } else {
    tmpvar_17 = 1.0;
  };
  atten_6 = (atten_6 * tmpvar_17);
  highp vec4 tmpvar_18;
  tmpvar_18.w = 1.0;
  tmpvar_18.xyz = tmpvar_10;
  highp vec4 tmpvar_19;
  tmpvar_19.w = -8.0;
  tmpvar_19.xyz = (_LightMatrix0 * tmpvar_18).xyz;
  lowp vec4 tmpvar_20;
  tmpvar_20 = textureCube (_LightTexture0, tmpvar_19.xyz, -8.0);
  atten_6 = (atten_6 * tmpvar_20.w);
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_22;
  lowp vec4 tmpvar_23;
  tmpvar_23 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_23;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_24;
  tmpvar_24 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_25;
  tmpvar_25 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_26;
  viewDir_26 = -(tmpvar_25);
  mediump float specular_27;
  mediump vec3 tmpvar_28;
  mediump vec3 inVec_29;
  inVec_29 = (lightDir_7 + viewDir_26);
  tmpvar_28 = (inVec_29 * inversesqrt(max (0.001, 
    dot (inVec_29, inVec_29)
  )));
  mediump float tmpvar_30;
  tmpvar_30 = max (0.0, dot (lightDir_7, tmpvar_28));
  mediump float tmpvar_31;
  tmpvar_31 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_32;
  tmpvar_32 = max (0.0001, (tmpvar_31 * tmpvar_31));
  mediump float tmpvar_33;
  tmpvar_33 = max (((2.0 / 
    (tmpvar_32 * tmpvar_32)
  ) - 2.0), 0.0001);
  specular_27 = sqrt(max (0.0001, (
    ((tmpvar_33 + 1.0) * pow (max (0.0, dot (tmpvar_24, tmpvar_28)), tmpvar_33))
   / 
    (((8.0 * (
      ((tmpvar_30 * tmpvar_30) * gbuffer1_3.w)
     + 
      (tmpvar_31 * tmpvar_31)
    )) * tmpvar_30) + 0.0001)
  )));
  mediump float tmpvar_34;
  tmpvar_34 = clamp (specular_27, 0.0, 100.0);
  specular_27 = tmpvar_34;
  mediump vec4 tmpvar_35;
  tmpvar_35.w = 1.0;
  tmpvar_35.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_34 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_24, lightDir_7)));
  mediump vec4 tmpvar_36;
  tmpvar_36 = exp2(-(tmpvar_35));
  tmpvar_1 = tmpvar_36;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform highp sampler2D _ShadowMapTexture;
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  mediump float tmpvar_21;
  highp float tmpvar_22;
  tmpvar_22 = clamp (((
    mix (tmpvar_9.z, sqrt(dot (tmpvar_11, tmpvar_11)), unity_ShadowFadeCenterAndType.w)
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = tmpvar_10;
  highp vec4 tmpvar_24;
  tmpvar_24 = (unity_World2Shadow[0] * tmpvar_23);
  lowp float tmpvar_25;
  highp vec4 shadowVals_26;
  highp vec3 tmpvar_27;
  tmpvar_27 = (tmpvar_24.xyz / tmpvar_24.w);
  shadowVals_26.x = texture2D (_ShadowMapTexture, (tmpvar_27.xy + _ShadowOffsets[0].xy)).x;
  shadowVals_26.y = texture2D (_ShadowMapTexture, (tmpvar_27.xy + _ShadowOffsets[1].xy)).x;
  shadowVals_26.z = texture2D (_ShadowMapTexture, (tmpvar_27.xy + _ShadowOffsets[2].xy)).x;
  shadowVals_26.w = texture2D (_ShadowMapTexture, (tmpvar_27.xy + _ShadowOffsets[3].xy)).x;
  bvec4 tmpvar_28;
  tmpvar_28 = lessThan (shadowVals_26, tmpvar_27.zzzz);
  mediump vec4 tmpvar_29;
  tmpvar_29 = _LightShadowData.xxxx;
  mediump float tmpvar_30;
  if (tmpvar_28.x) {
    tmpvar_30 = tmpvar_29.x;
  } else {
    tmpvar_30 = 1.0;
  };
  mediump float tmpvar_31;
  if (tmpvar_28.y) {
    tmpvar_31 = tmpvar_29.y;
  } else {
    tmpvar_31 = 1.0;
  };
  mediump float tmpvar_32;
  if (tmpvar_28.z) {
    tmpvar_32 = tmpvar_29.z;
  } else {
    tmpvar_32 = 1.0;
  };
  mediump float tmpvar_33;
  if (tmpvar_28.w) {
    tmpvar_33 = tmpvar_29.w;
  } else {
    tmpvar_33 = 1.0;
  };
  mediump vec4 tmpvar_34;
  tmpvar_34.x = tmpvar_30;
  tmpvar_34.y = tmpvar_31;
  tmpvar_34.z = tmpvar_32;
  tmpvar_34.w = tmpvar_33;
  mediump float tmpvar_35;
  tmpvar_35 = dot (tmpvar_34, vec4(0.25, 0.25, 0.25, 0.25));
  tmpvar_25 = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = clamp ((tmpvar_25 + tmpvar_22), 0.0, 1.0);
  tmpvar_21 = tmpvar_36;
  atten_6 = (atten_6 * tmpvar_21);
  lowp vec4 tmpvar_37;
  tmpvar_37 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_37;
  lowp vec4 tmpvar_38;
  tmpvar_38 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_38;
  lowp vec4 tmpvar_39;
  tmpvar_39 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_39;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_40;
  tmpvar_40 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_41;
  tmpvar_41 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_42;
  viewDir_42 = -(tmpvar_41);
  mediump float specular_43;
  mediump vec3 tmpvar_44;
  mediump vec3 inVec_45;
  inVec_45 = (lightDir_7 + viewDir_42);
  tmpvar_44 = (inVec_45 * inversesqrt(max (0.001, 
    dot (inVec_45, inVec_45)
  )));
  mediump float tmpvar_46;
  tmpvar_46 = max (0.0, dot (lightDir_7, tmpvar_44));
  mediump float tmpvar_47;
  tmpvar_47 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_48;
  tmpvar_48 = max (0.0001, (tmpvar_47 * tmpvar_47));
  mediump float tmpvar_49;
  tmpvar_49 = max (((2.0 / 
    (tmpvar_48 * tmpvar_48)
  ) - 2.0), 0.0001);
  specular_43 = sqrt(max (0.0001, (
    ((tmpvar_49 + 1.0) * pow (max (0.0, dot (tmpvar_40, tmpvar_44)), tmpvar_49))
   / 
    (((8.0 * (
      ((tmpvar_46 * tmpvar_46) * gbuffer1_3.w)
     + 
      (tmpvar_47 * tmpvar_47)
    )) * tmpvar_46) + 0.0001)
  )));
  mediump float tmpvar_50;
  tmpvar_50 = clamp (specular_43, 0.0, 100.0);
  specular_43 = tmpvar_50;
  mediump vec4 tmpvar_51;
  tmpvar_51.w = 1.0;
  tmpvar_51.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_50 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_40, lightDir_7)));
  mediump vec4 tmpvar_52;
  tmpvar_52 = exp2(-(tmpvar_51));
  tmpvar_1 = tmpvar_52;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  mediump float tmpvar_21;
  highp vec4 tmpvar_22;
  tmpvar_22.w = 1.0;
  tmpvar_22.xyz = tmpvar_10;
  highp vec4 tmpvar_23;
  tmpvar_23 = (unity_World2Shadow[0] * tmpvar_22);
  lowp float tmpvar_24;
  mediump vec4 shadows_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (tmpvar_23.xyz / tmpvar_23.w);
  highp vec3 coord_27;
  coord_27 = (tmpvar_26 + _ShadowOffsets[0].xyz);
  lowp float tmpvar_28;
  tmpvar_28 = shadow2DEXT (_ShadowMapTexture, coord_27);
  shadows_25.x = tmpvar_28;
  highp vec3 coord_29;
  coord_29 = (tmpvar_26 + _ShadowOffsets[1].xyz);
  lowp float tmpvar_30;
  tmpvar_30 = shadow2DEXT (_ShadowMapTexture, coord_29);
  shadows_25.y = tmpvar_30;
  highp vec3 coord_31;
  coord_31 = (tmpvar_26 + _ShadowOffsets[2].xyz);
  lowp float tmpvar_32;
  tmpvar_32 = shadow2DEXT (_ShadowMapTexture, coord_31);
  shadows_25.z = tmpvar_32;
  highp vec3 coord_33;
  coord_33 = (tmpvar_26 + _ShadowOffsets[3].xyz);
  lowp float tmpvar_34;
  tmpvar_34 = shadow2DEXT (_ShadowMapTexture, coord_33);
  shadows_25.w = tmpvar_34;
  shadows_25 = (_LightShadowData.xxxx + (shadows_25 * (1.0 - _LightShadowData.xxxx)));
  mediump float tmpvar_35;
  tmpvar_35 = dot (shadows_25, vec4(0.25, 0.25, 0.25, 0.25));
  tmpvar_24 = tmpvar_35;
  highp float tmpvar_36;
  tmpvar_36 = clamp ((tmpvar_24 + clamp (
    ((mix (tmpvar_9.z, sqrt(
      dot (tmpvar_11, tmpvar_11)
    ), unity_ShadowFadeCenterAndType.w) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_21 = tmpvar_36;
  atten_6 = (atten_6 * tmpvar_21);
  lowp vec4 tmpvar_37;
  tmpvar_37 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_37;
  lowp vec4 tmpvar_38;
  tmpvar_38 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_38;
  lowp vec4 tmpvar_39;
  tmpvar_39 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_39;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_40;
  tmpvar_40 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_41;
  tmpvar_41 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_42;
  viewDir_42 = -(tmpvar_41);
  mediump float specular_43;
  mediump vec3 tmpvar_44;
  mediump vec3 inVec_45;
  inVec_45 = (lightDir_7 + viewDir_42);
  tmpvar_44 = (inVec_45 * inversesqrt(max (0.001, 
    dot (inVec_45, inVec_45)
  )));
  mediump float tmpvar_46;
  tmpvar_46 = max (0.0, dot (lightDir_7, tmpvar_44));
  mediump float tmpvar_47;
  tmpvar_47 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_48;
  tmpvar_48 = max (0.0001, (tmpvar_47 * tmpvar_47));
  mediump float tmpvar_49;
  tmpvar_49 = max (((2.0 / 
    (tmpvar_48 * tmpvar_48)
  ) - 2.0), 0.0001);
  specular_43 = sqrt(max (0.0001, (
    ((tmpvar_49 + 1.0) * pow (max (0.0, dot (tmpvar_40, tmpvar_44)), tmpvar_49))
   / 
    (((8.0 * (
      ((tmpvar_46 * tmpvar_46) * gbuffer1_3.w)
     + 
      (tmpvar_47 * tmpvar_47)
    )) * tmpvar_46) + 0.0001)
  )));
  mediump float tmpvar_50;
  tmpvar_50 = clamp (specular_43, 0.0, 100.0);
  specular_43 = tmpvar_50;
  mediump vec4 tmpvar_51;
  tmpvar_51.w = 1.0;
  tmpvar_51.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_50 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_40, lightDir_7)));
  mediump vec4 tmpvar_52;
  tmpvar_52 = exp2(-(tmpvar_51));
  tmpvar_1 = tmpvar_52;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  tmpvar_11 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = -(normalize(tmpvar_11));
  lightDir_7 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = (dot (tmpvar_11, tmpvar_11) * _LightPos.w);
  lowp float tmpvar_14;
  tmpvar_14 = texture2D (_LightTextureB0, vec2(tmpvar_13)).w;
  atten_6 = tmpvar_14;
  highp vec4 shadowVals_15;
  highp float mydist_16;
  mydist_16 = ((sqrt(
    dot (tmpvar_11, tmpvar_11)
  ) * _LightPositionRange.w) * 0.97);
  shadowVals_15.x = dot (textureCube (_ShadowMapTexture, (tmpvar_11 + vec3(0.0078125, 0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_15.y = dot (textureCube (_ShadowMapTexture, (tmpvar_11 + vec3(-0.0078125, -0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_15.z = dot (textureCube (_ShadowMapTexture, (tmpvar_11 + vec3(-0.0078125, 0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_15.w = dot (textureCube (_ShadowMapTexture, (tmpvar_11 + vec3(0.0078125, -0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  bvec4 tmpvar_17;
  tmpvar_17 = lessThan (shadowVals_15, vec4(mydist_16));
  mediump vec4 tmpvar_18;
  tmpvar_18 = _LightShadowData.xxxx;
  mediump float tmpvar_19;
  if (tmpvar_17.x) {
    tmpvar_19 = tmpvar_18.x;
  } else {
    tmpvar_19 = 1.0;
  };
  mediump float tmpvar_20;
  if (tmpvar_17.y) {
    tmpvar_20 = tmpvar_18.y;
  } else {
    tmpvar_20 = 1.0;
  };
  mediump float tmpvar_21;
  if (tmpvar_17.z) {
    tmpvar_21 = tmpvar_18.z;
  } else {
    tmpvar_21 = 1.0;
  };
  mediump float tmpvar_22;
  if (tmpvar_17.w) {
    tmpvar_22 = tmpvar_18.w;
  } else {
    tmpvar_22 = 1.0;
  };
  mediump vec4 tmpvar_23;
  tmpvar_23.x = tmpvar_19;
  tmpvar_23.y = tmpvar_20;
  tmpvar_23.z = tmpvar_21;
  tmpvar_23.w = tmpvar_22;
  mediump float tmpvar_24;
  tmpvar_24 = dot (tmpvar_23, vec4(0.25, 0.25, 0.25, 0.25));
  atten_6 = (atten_6 * tmpvar_24);
  lowp vec4 tmpvar_25;
  tmpvar_25 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_25;
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_26;
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_27;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_28;
  tmpvar_28 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_29;
  tmpvar_29 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_30;
  viewDir_30 = -(tmpvar_29);
  mediump float specular_31;
  mediump vec3 tmpvar_32;
  mediump vec3 inVec_33;
  inVec_33 = (lightDir_7 + viewDir_30);
  tmpvar_32 = (inVec_33 * inversesqrt(max (0.001, 
    dot (inVec_33, inVec_33)
  )));
  mediump float tmpvar_34;
  tmpvar_34 = max (0.0, dot (lightDir_7, tmpvar_32));
  mediump float tmpvar_35;
  tmpvar_35 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_36;
  tmpvar_36 = max (0.0001, (tmpvar_35 * tmpvar_35));
  mediump float tmpvar_37;
  tmpvar_37 = max (((2.0 / 
    (tmpvar_36 * tmpvar_36)
  ) - 2.0), 0.0001);
  specular_31 = sqrt(max (0.0001, (
    ((tmpvar_37 + 1.0) * pow (max (0.0, dot (tmpvar_28, tmpvar_32)), tmpvar_37))
   / 
    (((8.0 * (
      ((tmpvar_34 * tmpvar_34) * gbuffer1_3.w)
     + 
      (tmpvar_35 * tmpvar_35)
    )) * tmpvar_34) + 0.0001)
  )));
  mediump float tmpvar_38;
  tmpvar_38 = clamp (specular_31, 0.0, 100.0);
  specular_31 = tmpvar_38;
  mediump vec4 tmpvar_39;
  tmpvar_39.w = 1.0;
  tmpvar_39.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_38 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_28, lightDir_7)));
  mediump vec4 tmpvar_40;
  tmpvar_40 = exp2(-(tmpvar_39));
  tmpvar_1 = tmpvar_40;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  tmpvar_11 = (tmpvar_10 - _LightPos.xyz);
  highp vec3 tmpvar_12;
  tmpvar_12 = -(normalize(tmpvar_11));
  lightDir_7 = tmpvar_12;
  highp float tmpvar_13;
  tmpvar_13 = (dot (tmpvar_11, tmpvar_11) * _LightPos.w);
  lowp float tmpvar_14;
  tmpvar_14 = texture2D (_LightTextureB0, vec2(tmpvar_13)).w;
  atten_6 = tmpvar_14;
  highp vec4 shadowVals_15;
  highp float mydist_16;
  mydist_16 = ((sqrt(
    dot (tmpvar_11, tmpvar_11)
  ) * _LightPositionRange.w) * 0.97);
  shadowVals_15.x = dot (textureCube (_ShadowMapTexture, (tmpvar_11 + vec3(0.0078125, 0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_15.y = dot (textureCube (_ShadowMapTexture, (tmpvar_11 + vec3(-0.0078125, -0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_15.z = dot (textureCube (_ShadowMapTexture, (tmpvar_11 + vec3(-0.0078125, 0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_15.w = dot (textureCube (_ShadowMapTexture, (tmpvar_11 + vec3(0.0078125, -0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  bvec4 tmpvar_17;
  tmpvar_17 = lessThan (shadowVals_15, vec4(mydist_16));
  mediump vec4 tmpvar_18;
  tmpvar_18 = _LightShadowData.xxxx;
  mediump float tmpvar_19;
  if (tmpvar_17.x) {
    tmpvar_19 = tmpvar_18.x;
  } else {
    tmpvar_19 = 1.0;
  };
  mediump float tmpvar_20;
  if (tmpvar_17.y) {
    tmpvar_20 = tmpvar_18.y;
  } else {
    tmpvar_20 = 1.0;
  };
  mediump float tmpvar_21;
  if (tmpvar_17.z) {
    tmpvar_21 = tmpvar_18.z;
  } else {
    tmpvar_21 = 1.0;
  };
  mediump float tmpvar_22;
  if (tmpvar_17.w) {
    tmpvar_22 = tmpvar_18.w;
  } else {
    tmpvar_22 = 1.0;
  };
  mediump vec4 tmpvar_23;
  tmpvar_23.x = tmpvar_19;
  tmpvar_23.y = tmpvar_20;
  tmpvar_23.z = tmpvar_21;
  tmpvar_23.w = tmpvar_22;
  mediump float tmpvar_24;
  tmpvar_24 = dot (tmpvar_23, vec4(0.25, 0.25, 0.25, 0.25));
  atten_6 = (atten_6 * tmpvar_24);
  highp vec4 tmpvar_25;
  tmpvar_25.w = 1.0;
  tmpvar_25.xyz = tmpvar_10;
  highp vec4 tmpvar_26;
  tmpvar_26.w = -8.0;
  tmpvar_26.xyz = (_LightMatrix0 * tmpvar_25).xyz;
  lowp vec4 tmpvar_27;
  tmpvar_27 = textureCube (_LightTexture0, tmpvar_26.xyz, -8.0);
  atten_6 = (atten_6 * tmpvar_27.w);
  lowp vec4 tmpvar_28;
  tmpvar_28 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_29 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_29;
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_30;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_31;
  tmpvar_31 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_32;
  tmpvar_32 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_33;
  viewDir_33 = -(tmpvar_32);
  mediump float specular_34;
  mediump vec3 tmpvar_35;
  mediump vec3 inVec_36;
  inVec_36 = (lightDir_7 + viewDir_33);
  tmpvar_35 = (inVec_36 * inversesqrt(max (0.001, 
    dot (inVec_36, inVec_36)
  )));
  mediump float tmpvar_37;
  tmpvar_37 = max (0.0, dot (lightDir_7, tmpvar_35));
  mediump float tmpvar_38;
  tmpvar_38 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_39;
  tmpvar_39 = max (0.0001, (tmpvar_38 * tmpvar_38));
  mediump float tmpvar_40;
  tmpvar_40 = max (((2.0 / 
    (tmpvar_39 * tmpvar_39)
  ) - 2.0), 0.0001);
  specular_34 = sqrt(max (0.0001, (
    ((tmpvar_40 + 1.0) * pow (max (0.0, dot (tmpvar_31, tmpvar_35)), tmpvar_40))
   / 
    (((8.0 * (
      ((tmpvar_37 * tmpvar_37) * gbuffer1_3.w)
     + 
      (tmpvar_38 * tmpvar_38)
    )) * tmpvar_37) + 0.0001)
  )));
  mediump float tmpvar_41;
  tmpvar_41 = clamp (specular_34, 0.0, 100.0);
  specular_34 = tmpvar_41;
  mediump vec4 tmpvar_42;
  tmpvar_42.w = 1.0;
  tmpvar_42.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_41 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_31, lightDir_7)));
  mediump vec4 tmpvar_43;
  tmpvar_43 = exp2(-(tmpvar_42));
  tmpvar_1 = tmpvar_43;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  mediump float tmpvar_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_15;
  tmpvar_15 = clamp ((tmpvar_14.x + clamp (
    ((mix (tmpvar_9.z, sqrt(
      dot (tmpvar_11, tmpvar_11)
    ), unity_ShadowFadeCenterAndType.w) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_13 = tmpvar_15;
  atten_6 = tmpvar_13;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_16;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_17;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_18;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_19;
  tmpvar_19 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_20;
  tmpvar_20 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_21;
  viewDir_21 = -(tmpvar_20);
  mediump float specular_22;
  mediump vec3 tmpvar_23;
  mediump vec3 inVec_24;
  inVec_24 = (lightDir_7 + viewDir_21);
  tmpvar_23 = (inVec_24 * inversesqrt(max (0.001, 
    dot (inVec_24, inVec_24)
  )));
  mediump float tmpvar_25;
  tmpvar_25 = max (0.0, dot (lightDir_7, tmpvar_23));
  mediump float tmpvar_26;
  tmpvar_26 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_27;
  tmpvar_27 = max (0.0001, (tmpvar_26 * tmpvar_26));
  mediump float tmpvar_28;
  tmpvar_28 = max (((2.0 / 
    (tmpvar_27 * tmpvar_27)
  ) - 2.0), 0.0001);
  specular_22 = sqrt(max (0.0001, (
    ((tmpvar_28 + 1.0) * pow (max (0.0, dot (tmpvar_19, tmpvar_23)), tmpvar_28))
   / 
    (((8.0 * (
      ((tmpvar_25 * tmpvar_25) * gbuffer1_3.w)
     + 
      (tmpvar_26 * tmpvar_26)
    )) * tmpvar_25) + 0.0001)
  )));
  mediump float tmpvar_29;
  tmpvar_29 = clamp (specular_22, 0.0, 100.0);
  specular_22 = tmpvar_29;
  mediump vec4 tmpvar_30;
  tmpvar_30.w = 1.0;
  tmpvar_30.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_29 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_19, lightDir_7)));
  mediump vec4 tmpvar_31;
  tmpvar_31 = exp2(-(tmpvar_30));
  tmpvar_1 = tmpvar_31;
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  lowp vec4 tmpvar_1;
  mediump vec4 gbuffer2_2;
  mediump vec4 gbuffer1_3;
  mediump vec4 gbuffer0_4;
  mediump vec3 tmpvar_5;
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
  mediump float tmpvar_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_ShadowMapTexture, tmpvar_8);
  highp float tmpvar_15;
  tmpvar_15 = clamp ((tmpvar_14.x + clamp (
    ((mix (tmpvar_9.z, sqrt(
      dot (tmpvar_11, tmpvar_11)
    ), unity_ShadowFadeCenterAndType.w) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_13 = tmpvar_15;
  atten_6 = tmpvar_13;
  highp vec4 tmpvar_16;
  tmpvar_16.w = 1.0;
  tmpvar_16.xyz = tmpvar_10;
  highp vec4 tmpvar_17;
  tmpvar_17.zw = vec2(0.0, -8.0);
  tmpvar_17.xy = (_LightMatrix0 * tmpvar_16).xy;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTexture0, tmpvar_17.xy, -8.0);
  atten_6 = (atten_6 * tmpvar_18.w);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_CameraGBufferTexture0, tmpvar_8);
  gbuffer0_4 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraGBufferTexture1, tmpvar_8);
  gbuffer1_3 = tmpvar_20;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_CameraGBufferTexture2, tmpvar_8);
  gbuffer2_2 = tmpvar_21;
  tmpvar_5 = (_LightColor.xyz * atten_6);
  mediump vec3 tmpvar_22;
  tmpvar_22 = normalize(((gbuffer2_2.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((tmpvar_10 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_24;
  viewDir_24 = -(tmpvar_23);
  mediump float specular_25;
  mediump vec3 tmpvar_26;
  mediump vec3 inVec_27;
  inVec_27 = (lightDir_7 + viewDir_24);
  tmpvar_26 = (inVec_27 * inversesqrt(max (0.001, 
    dot (inVec_27, inVec_27)
  )));
  mediump float tmpvar_28;
  tmpvar_28 = max (0.0, dot (lightDir_7, tmpvar_26));
  mediump float tmpvar_29;
  tmpvar_29 = (1.0 - gbuffer1_3.w);
  mediump float tmpvar_30;
  tmpvar_30 = max (0.0001, (tmpvar_29 * tmpvar_29));
  mediump float tmpvar_31;
  tmpvar_31 = max (((2.0 / 
    (tmpvar_30 * tmpvar_30)
  ) - 2.0), 0.0001);
  specular_25 = sqrt(max (0.0001, (
    ((tmpvar_31 + 1.0) * pow (max (0.0, dot (tmpvar_22, tmpvar_26)), tmpvar_31))
   / 
    (((8.0 * (
      ((tmpvar_28 * tmpvar_28) * gbuffer1_3.w)
     + 
      (tmpvar_29 * tmpvar_29)
    )) * tmpvar_28) + 0.0001)
  )));
  mediump float tmpvar_32;
  tmpvar_32 = clamp (specular_25, 0.0, 100.0);
  specular_25 = tmpvar_32;
  mediump vec4 tmpvar_33;
  tmpvar_33.w = 1.0;
  tmpvar_33.xyz = (((gbuffer0_4.xyz + 
    (tmpvar_32 * gbuffer1_3.xyz)
  ) * tmpvar_5) * max (0.0, dot (tmpvar_22, lightDir_7)));
  mediump vec4 tmpvar_34;
  tmpvar_34 = exp2(-(tmpvar_33));
  tmpvar_1 = tmpvar_34;
  gl_FragData[0] = tmpvar_1;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_OFF" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  tmpvar_10 = (tmpvar_9 - _LightPos.xyz);
  highp vec3 tmpvar_11;
  tmpvar_11 = -(normalize(tmpvar_10));
  lightDir_6 = tmpvar_11;
  highp float tmpvar_12;
  tmpvar_12 = (dot (tmpvar_10, tmpvar_10) * _LightPos.w);
  lowp float tmpvar_13;
  tmpvar_13 = texture2D (_LightTextureB0, vec2(tmpvar_12)).w;
  atten_5 = tmpvar_13;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_16;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_17;
  tmpvar_17 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_19;
  viewDir_19 = -(tmpvar_18);
  mediump float specular_20;
  mediump vec3 tmpvar_21;
  mediump vec3 inVec_22;
  inVec_22 = (lightDir_6 + viewDir_19);
  tmpvar_21 = (inVec_22 * inversesqrt(max (0.001, 
    dot (inVec_22, inVec_22)
  )));
  mediump float tmpvar_23;
  tmpvar_23 = max (0.0, dot (lightDir_6, tmpvar_21));
  mediump float tmpvar_24;
  tmpvar_24 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_25;
  tmpvar_25 = max (0.0001, (tmpvar_24 * tmpvar_24));
  mediump float tmpvar_26;
  tmpvar_26 = max (((2.0 / 
    (tmpvar_25 * tmpvar_25)
  ) - 2.0), 0.0001);
  specular_20 = sqrt(max (0.0001, (
    ((tmpvar_26 + 1.0) * pow (max (0.0, dot (tmpvar_17, tmpvar_21)), tmpvar_26))
   / 
    (((8.0 * (
      ((tmpvar_23 * tmpvar_23) * gbuffer1_2.w)
     + 
      (tmpvar_24 * tmpvar_24)
    )) * tmpvar_23) + 0.0001)
  )));
  mediump float tmpvar_27;
  tmpvar_27 = clamp (specular_20, 0.0, 100.0);
  specular_20 = tmpvar_27;
  mediump vec4 tmpvar_28;
  tmpvar_28.w = 1.0;
  tmpvar_28.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_27 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_17, lightDir_6)));
  gl_FragData[0] = tmpvar_28;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  mediump vec3 lightDir_5;
  highp vec2 tmpvar_6;
  tmpvar_6 = (xlv_TEXCOORD0.xy / xlv_TEXCOORD0.w);
  highp vec4 tmpvar_7;
  tmpvar_7.w = 1.0;
  tmpvar_7.xyz = ((xlv_TEXCOORD1 * (_ProjectionParams.z / xlv_TEXCOORD1.z)) * (1.0/((
    (_ZBufferParams.x * texture2D (_CameraDepthTexture, tmpvar_6).x)
   + _ZBufferParams.y))));
  highp vec3 tmpvar_8;
  tmpvar_8 = -(_LightDir.xyz);
  lightDir_5 = tmpvar_8;
  lowp vec4 tmpvar_9;
  tmpvar_9 = texture2D (_CameraGBufferTexture0, tmpvar_6);
  gbuffer0_3 = tmpvar_9;
  lowp vec4 tmpvar_10;
  tmpvar_10 = texture2D (_CameraGBufferTexture1, tmpvar_6);
  gbuffer1_2 = tmpvar_10;
  lowp vec4 tmpvar_11;
  tmpvar_11 = texture2D (_CameraGBufferTexture2, tmpvar_6);
  gbuffer2_1 = tmpvar_11;
  tmpvar_4 = _LightColor.xyz;
  mediump vec3 tmpvar_12;
  tmpvar_12 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_13;
  tmpvar_13 = normalize(((_CameraToWorld * tmpvar_7).xyz - _WorldSpaceCameraPos));
  mediump vec3 viewDir_14;
  viewDir_14 = -(tmpvar_13);
  mediump float specular_15;
  mediump vec3 tmpvar_16;
  mediump vec3 inVec_17;
  inVec_17 = (lightDir_5 + viewDir_14);
  tmpvar_16 = (inVec_17 * inversesqrt(max (0.001, 
    dot (inVec_17, inVec_17)
  )));
  mediump float tmpvar_18;
  tmpvar_18 = max (0.0, dot (lightDir_5, tmpvar_16));
  mediump float tmpvar_19;
  tmpvar_19 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_20;
  tmpvar_20 = max (0.0001, (tmpvar_19 * tmpvar_19));
  mediump float tmpvar_21;
  tmpvar_21 = max (((2.0 / 
    (tmpvar_20 * tmpvar_20)
  ) - 2.0), 0.0001);
  specular_15 = sqrt(max (0.0001, (
    ((tmpvar_21 + 1.0) * pow (max (0.0, dot (tmpvar_12, tmpvar_16)), tmpvar_21))
   / 
    (((8.0 * (
      ((tmpvar_18 * tmpvar_18) * gbuffer1_2.w)
     + 
      (tmpvar_19 * tmpvar_19)
    )) * tmpvar_18) + 0.0001)
  )));
  mediump float tmpvar_22;
  tmpvar_22 = clamp (specular_15, 0.0, 100.0);
  specular_15 = tmpvar_22;
  mediump vec4 tmpvar_23;
  tmpvar_23.w = 1.0;
  tmpvar_23.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_22 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_12, lightDir_5)));
  gl_FragData[0] = tmpvar_23;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_OFF" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  tmpvar_10 = (_LightPos.xyz - tmpvar_9);
  highp vec3 tmpvar_11;
  tmpvar_11 = normalize(tmpvar_10);
  lightDir_6 = tmpvar_11;
  highp vec4 tmpvar_12;
  tmpvar_12.w = 1.0;
  tmpvar_12.xyz = tmpvar_9;
  highp vec4 tmpvar_13;
  tmpvar_13 = (_LightMatrix0 * tmpvar_12);
  highp vec4 tmpvar_14;
  tmpvar_14.zw = vec2(0.0, -8.0);
  tmpvar_14.xy = (tmpvar_13.xy / tmpvar_13.w);
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_LightTexture0, tmpvar_14.xy, -8.0);
  highp float tmpvar_16;
  tmpvar_16 = tmpvar_15.w;
  atten_5 = (tmpvar_16 * float((tmpvar_13.w < 0.0)));
  highp float tmpvar_17;
  tmpvar_17 = (dot (tmpvar_10, tmpvar_10) * _LightPos.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_LightTextureB0, vec2(tmpvar_17));
  atten_5 = (atten_5 * tmpvar_18.w);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_20;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_21;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_22;
  tmpvar_22 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_23;
  tmpvar_23 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_24;
  viewDir_24 = -(tmpvar_23);
  mediump float specular_25;
  mediump vec3 tmpvar_26;
  mediump vec3 inVec_27;
  inVec_27 = (lightDir_6 + viewDir_24);
  tmpvar_26 = (inVec_27 * inversesqrt(max (0.001, 
    dot (inVec_27, inVec_27)
  )));
  mediump float tmpvar_28;
  tmpvar_28 = max (0.0, dot (lightDir_6, tmpvar_26));
  mediump float tmpvar_29;
  tmpvar_29 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_30;
  tmpvar_30 = max (0.0001, (tmpvar_29 * tmpvar_29));
  mediump float tmpvar_31;
  tmpvar_31 = max (((2.0 / 
    (tmpvar_30 * tmpvar_30)
  ) - 2.0), 0.0001);
  specular_25 = sqrt(max (0.0001, (
    ((tmpvar_31 + 1.0) * pow (max (0.0, dot (tmpvar_22, tmpvar_26)), tmpvar_31))
   / 
    (((8.0 * (
      ((tmpvar_28 * tmpvar_28) * gbuffer1_2.w)
     + 
      (tmpvar_29 * tmpvar_29)
    )) * tmpvar_28) + 0.0001)
  )));
  mediump float tmpvar_32;
  tmpvar_32 = clamp (specular_25, 0.0, 100.0);
  specular_25 = tmpvar_32;
  mediump vec4 tmpvar_33;
  tmpvar_33.w = 1.0;
  tmpvar_33.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_32 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_22, lightDir_6)));
  gl_FragData[0] = tmpvar_33;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_OFF" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  tmpvar_10 = (tmpvar_9 - _LightPos.xyz);
  highp vec3 tmpvar_11;
  tmpvar_11 = -(normalize(tmpvar_10));
  lightDir_6 = tmpvar_11;
  highp float tmpvar_12;
  tmpvar_12 = (dot (tmpvar_10, tmpvar_10) * _LightPos.w);
  lowp float tmpvar_13;
  tmpvar_13 = texture2D (_LightTextureB0, vec2(tmpvar_12)).w;
  atten_5 = tmpvar_13;
  highp vec4 tmpvar_14;
  tmpvar_14.w = 1.0;
  tmpvar_14.xyz = tmpvar_9;
  highp vec4 tmpvar_15;
  tmpvar_15.w = -8.0;
  tmpvar_15.xyz = (_LightMatrix0 * tmpvar_14).xyz;
  lowp vec4 tmpvar_16;
  tmpvar_16 = textureCube (_LightTexture0, tmpvar_15.xyz, -8.0);
  atten_5 = (atten_5 * tmpvar_16.w);
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_17;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_19;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_20;
  tmpvar_20 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_22;
  viewDir_22 = -(tmpvar_21);
  mediump float specular_23;
  mediump vec3 tmpvar_24;
  mediump vec3 inVec_25;
  inVec_25 = (lightDir_6 + viewDir_22);
  tmpvar_24 = (inVec_25 * inversesqrt(max (0.001, 
    dot (inVec_25, inVec_25)
  )));
  mediump float tmpvar_26;
  tmpvar_26 = max (0.0, dot (lightDir_6, tmpvar_24));
  mediump float tmpvar_27;
  tmpvar_27 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_28;
  tmpvar_28 = max (0.0001, (tmpvar_27 * tmpvar_27));
  mediump float tmpvar_29;
  tmpvar_29 = max (((2.0 / 
    (tmpvar_28 * tmpvar_28)
  ) - 2.0), 0.0001);
  specular_23 = sqrt(max (0.0001, (
    ((tmpvar_29 + 1.0) * pow (max (0.0, dot (tmpvar_20, tmpvar_24)), tmpvar_29))
   / 
    (((8.0 * (
      ((tmpvar_26 * tmpvar_26) * gbuffer1_2.w)
     + 
      (tmpvar_27 * tmpvar_27)
    )) * tmpvar_26) + 0.0001)
  )));
  mediump float tmpvar_30;
  tmpvar_30 = clamp (specular_23, 0.0, 100.0);
  specular_23 = tmpvar_30;
  mediump vec4 tmpvar_31;
  tmpvar_31.w = 1.0;
  tmpvar_31.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_30 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_20, lightDir_6)));
  gl_FragData[0] = tmpvar_31;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_OFF" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  tmpvar_10 = -(_LightDir.xyz);
  lightDir_6 = tmpvar_10;
  highp vec4 tmpvar_11;
  tmpvar_11.w = 1.0;
  tmpvar_11.xyz = tmpvar_9;
  highp vec4 tmpvar_12;
  tmpvar_12.zw = vec2(0.0, -8.0);
  tmpvar_12.xy = (_LightMatrix0 * tmpvar_11).xy;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_LightTexture0, tmpvar_12.xy, -8.0);
  atten_5 = tmpvar_13.w;
  lowp vec4 tmpvar_14;
  tmpvar_14 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_14;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_16;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_17;
  tmpvar_17 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_18;
  tmpvar_18 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_19;
  viewDir_19 = -(tmpvar_18);
  mediump float specular_20;
  mediump vec3 tmpvar_21;
  mediump vec3 inVec_22;
  inVec_22 = (lightDir_6 + viewDir_19);
  tmpvar_21 = (inVec_22 * inversesqrt(max (0.001, 
    dot (inVec_22, inVec_22)
  )));
  mediump float tmpvar_23;
  tmpvar_23 = max (0.0, dot (lightDir_6, tmpvar_21));
  mediump float tmpvar_24;
  tmpvar_24 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_25;
  tmpvar_25 = max (0.0001, (tmpvar_24 * tmpvar_24));
  mediump float tmpvar_26;
  tmpvar_26 = max (((2.0 / 
    (tmpvar_25 * tmpvar_25)
  ) - 2.0), 0.0001);
  specular_20 = sqrt(max (0.0001, (
    ((tmpvar_26 + 1.0) * pow (max (0.0, dot (tmpvar_17, tmpvar_21)), tmpvar_26))
   / 
    (((8.0 * (
      ((tmpvar_23 * tmpvar_23) * gbuffer1_2.w)
     + 
      (tmpvar_24 * tmpvar_24)
    )) * tmpvar_23) + 0.0001)
  )));
  mediump float tmpvar_27;
  tmpvar_27 = clamp (specular_20, 0.0, 100.0);
  specular_20 = tmpvar_27;
  mediump vec4 tmpvar_28;
  tmpvar_28.w = 1.0;
  tmpvar_28.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_27 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_17, lightDir_6)));
  gl_FragData[0] = tmpvar_28;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "UNITY_HDR_ON" "SHADOWS_NONATIVE" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform highp sampler2D _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  tmpvar_11 = (_LightPos.xyz - tmpvar_9);
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize(tmpvar_11);
  lightDir_6 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_9;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_LightMatrix0 * tmpvar_13);
  highp vec4 tmpvar_15;
  tmpvar_15.zw = vec2(0.0, -8.0);
  tmpvar_15.xy = (tmpvar_14.xy / tmpvar_14.w);
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_LightTexture0, tmpvar_15.xy, -8.0);
  highp float tmpvar_17;
  tmpvar_17 = tmpvar_16.w;
  atten_5 = (tmpvar_17 * float((tmpvar_14.w < 0.0)));
  highp float tmpvar_18;
  tmpvar_18 = (dot (tmpvar_11, tmpvar_11) * _LightPos.w);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_LightTextureB0, vec2(tmpvar_18));
  atten_5 = (atten_5 * tmpvar_19.w);
  mediump float tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = clamp (((
    mix (tmpvar_8.z, sqrt(dot (tmpvar_10, tmpvar_10)), unity_ShadowFadeCenterAndType.w)
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_22;
  tmpvar_22.w = 1.0;
  tmpvar_22.xyz = tmpvar_9;
  highp vec4 tmpvar_23;
  tmpvar_23 = (unity_World2Shadow[0] * tmpvar_22);
  lowp float tmpvar_24;
  highp vec4 tmpvar_25;
  tmpvar_25 = texture2DProj (_ShadowMapTexture, tmpvar_23);
  mediump float tmpvar_26;
  if ((tmpvar_25.x < (tmpvar_23.z / tmpvar_23.w))) {
    tmpvar_26 = _LightShadowData.x;
  } else {
    tmpvar_26 = 1.0;
  };
  tmpvar_24 = tmpvar_26;
  highp float tmpvar_27;
  tmpvar_27 = clamp ((tmpvar_24 + tmpvar_21), 0.0, 1.0);
  tmpvar_20 = tmpvar_27;
  atten_5 = (atten_5 * tmpvar_20);
  lowp vec4 tmpvar_28;
  tmpvar_28 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_29 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_29;
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_30;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_31;
  tmpvar_31 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_32;
  tmpvar_32 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_33;
  viewDir_33 = -(tmpvar_32);
  mediump float specular_34;
  mediump vec3 tmpvar_35;
  mediump vec3 inVec_36;
  inVec_36 = (lightDir_6 + viewDir_33);
  tmpvar_35 = (inVec_36 * inversesqrt(max (0.001, 
    dot (inVec_36, inVec_36)
  )));
  mediump float tmpvar_37;
  tmpvar_37 = max (0.0, dot (lightDir_6, tmpvar_35));
  mediump float tmpvar_38;
  tmpvar_38 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_39;
  tmpvar_39 = max (0.0001, (tmpvar_38 * tmpvar_38));
  mediump float tmpvar_40;
  tmpvar_40 = max (((2.0 / 
    (tmpvar_39 * tmpvar_39)
  ) - 2.0), 0.0001);
  specular_34 = sqrt(max (0.0001, (
    ((tmpvar_40 + 1.0) * pow (max (0.0, dot (tmpvar_31, tmpvar_35)), tmpvar_40))
   / 
    (((8.0 * (
      ((tmpvar_37 * tmpvar_37) * gbuffer1_2.w)
     + 
      (tmpvar_38 * tmpvar_38)
    )) * tmpvar_37) + 0.0001)
  )));
  mediump float tmpvar_41;
  tmpvar_41 = clamp (specular_34, 0.0, 100.0);
  specular_34 = tmpvar_41;
  mediump vec4 tmpvar_42;
  tmpvar_42.w = 1.0;
  tmpvar_42.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_41 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_31, lightDir_6)));
  gl_FragData[0] = tmpvar_42;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NATIVE" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  tmpvar_11 = (_LightPos.xyz - tmpvar_9);
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize(tmpvar_11);
  lightDir_6 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_9;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_LightMatrix0 * tmpvar_13);
  highp vec4 tmpvar_15;
  tmpvar_15.zw = vec2(0.0, -8.0);
  tmpvar_15.xy = (tmpvar_14.xy / tmpvar_14.w);
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_LightTexture0, tmpvar_15.xy, -8.0);
  highp float tmpvar_17;
  tmpvar_17 = tmpvar_16.w;
  atten_5 = (tmpvar_17 * float((tmpvar_14.w < 0.0)));
  highp float tmpvar_18;
  tmpvar_18 = (dot (tmpvar_11, tmpvar_11) * _LightPos.w);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_LightTextureB0, vec2(tmpvar_18));
  atten_5 = (atten_5 * tmpvar_19.w);
  mediump float tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = tmpvar_9;
  highp vec4 tmpvar_22;
  tmpvar_22 = (unity_World2Shadow[0] * tmpvar_21);
  lowp float tmpvar_23;
  mediump float shadow_24;
  lowp float tmpvar_25;
  tmpvar_25 = shadow2DProjEXT (_ShadowMapTexture, tmpvar_22);
  mediump float tmpvar_26;
  tmpvar_26 = tmpvar_25;
  shadow_24 = (_LightShadowData.x + (tmpvar_26 * (1.0 - _LightShadowData.x)));
  tmpvar_23 = shadow_24;
  highp float tmpvar_27;
  tmpvar_27 = clamp ((tmpvar_23 + clamp (
    ((mix (tmpvar_8.z, sqrt(
      dot (tmpvar_10, tmpvar_10)
    ), unity_ShadowFadeCenterAndType.w) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_20 = tmpvar_27;
  atten_5 = (atten_5 * tmpvar_20);
  lowp vec4 tmpvar_28;
  tmpvar_28 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_29 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_29;
  lowp vec4 tmpvar_30;
  tmpvar_30 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_30;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_31;
  tmpvar_31 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_32;
  tmpvar_32 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_33;
  viewDir_33 = -(tmpvar_32);
  mediump float specular_34;
  mediump vec3 tmpvar_35;
  mediump vec3 inVec_36;
  inVec_36 = (lightDir_6 + viewDir_33);
  tmpvar_35 = (inVec_36 * inversesqrt(max (0.001, 
    dot (inVec_36, inVec_36)
  )));
  mediump float tmpvar_37;
  tmpvar_37 = max (0.0, dot (lightDir_6, tmpvar_35));
  mediump float tmpvar_38;
  tmpvar_38 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_39;
  tmpvar_39 = max (0.0001, (tmpvar_38 * tmpvar_38));
  mediump float tmpvar_40;
  tmpvar_40 = max (((2.0 / 
    (tmpvar_39 * tmpvar_39)
  ) - 2.0), 0.0001);
  specular_34 = sqrt(max (0.0001, (
    ((tmpvar_40 + 1.0) * pow (max (0.0, dot (tmpvar_31, tmpvar_35)), tmpvar_40))
   / 
    (((8.0 * (
      ((tmpvar_37 * tmpvar_37) * gbuffer1_2.w)
     + 
      (tmpvar_38 * tmpvar_38)
    )) * tmpvar_37) + 0.0001)
  )));
  mediump float tmpvar_41;
  tmpvar_41 = clamp (specular_34, 0.0, 100.0);
  specular_34 = tmpvar_41;
  mediump vec4 tmpvar_42;
  tmpvar_42.w = 1.0;
  tmpvar_42.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_41 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_31, lightDir_6)));
  gl_FragData[0] = tmpvar_42;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  mediump float tmpvar_12;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_ShadowMapTexture, tmpvar_7);
  highp float tmpvar_14;
  tmpvar_14 = clamp ((tmpvar_13.x + clamp (
    ((mix (tmpvar_8.z, sqrt(
      dot (tmpvar_10, tmpvar_10)
    ), unity_ShadowFadeCenterAndType.w) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_12 = tmpvar_14;
  atten_5 = tmpvar_12;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_16;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_17;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_18;
  tmpvar_18 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_20;
  viewDir_20 = -(tmpvar_19);
  mediump float specular_21;
  mediump vec3 tmpvar_22;
  mediump vec3 inVec_23;
  inVec_23 = (lightDir_6 + viewDir_20);
  tmpvar_22 = (inVec_23 * inversesqrt(max (0.001, 
    dot (inVec_23, inVec_23)
  )));
  mediump float tmpvar_24;
  tmpvar_24 = max (0.0, dot (lightDir_6, tmpvar_22));
  mediump float tmpvar_25;
  tmpvar_25 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_26;
  tmpvar_26 = max (0.0001, (tmpvar_25 * tmpvar_25));
  mediump float tmpvar_27;
  tmpvar_27 = max (((2.0 / 
    (tmpvar_26 * tmpvar_26)
  ) - 2.0), 0.0001);
  specular_21 = sqrt(max (0.0001, (
    ((tmpvar_27 + 1.0) * pow (max (0.0, dot (tmpvar_18, tmpvar_22)), tmpvar_27))
   / 
    (((8.0 * (
      ((tmpvar_24 * tmpvar_24) * gbuffer1_2.w)
     + 
      (tmpvar_25 * tmpvar_25)
    )) * tmpvar_24) + 0.0001)
  )));
  mediump float tmpvar_28;
  tmpvar_28 = clamp (specular_21, 0.0, 100.0);
  specular_21 = tmpvar_28;
  mediump vec4 tmpvar_29;
  tmpvar_29.w = 1.0;
  tmpvar_29.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_28 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_18, lightDir_6)));
  gl_FragData[0] = tmpvar_29;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  mediump float tmpvar_12;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_ShadowMapTexture, tmpvar_7);
  highp float tmpvar_14;
  tmpvar_14 = clamp ((tmpvar_13.x + clamp (
    ((mix (tmpvar_8.z, sqrt(
      dot (tmpvar_10, tmpvar_10)
    ), unity_ShadowFadeCenterAndType.w) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_12 = tmpvar_14;
  atten_5 = tmpvar_12;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_9;
  highp vec4 tmpvar_16;
  tmpvar_16.zw = vec2(0.0, -8.0);
  tmpvar_16.xy = (_LightMatrix0 * tmpvar_15).xy;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_LightTexture0, tmpvar_16.xy, -8.0);
  atten_5 = (atten_5 * tmpvar_17.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_20;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_21;
  tmpvar_21 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_23;
  viewDir_23 = -(tmpvar_22);
  mediump float specular_24;
  mediump vec3 tmpvar_25;
  mediump vec3 inVec_26;
  inVec_26 = (lightDir_6 + viewDir_23);
  tmpvar_25 = (inVec_26 * inversesqrt(max (0.001, 
    dot (inVec_26, inVec_26)
  )));
  mediump float tmpvar_27;
  tmpvar_27 = max (0.0, dot (lightDir_6, tmpvar_25));
  mediump float tmpvar_28;
  tmpvar_28 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_29;
  tmpvar_29 = max (0.0001, (tmpvar_28 * tmpvar_28));
  mediump float tmpvar_30;
  tmpvar_30 = max (((2.0 / 
    (tmpvar_29 * tmpvar_29)
  ) - 2.0), 0.0001);
  specular_24 = sqrt(max (0.0001, (
    ((tmpvar_30 + 1.0) * pow (max (0.0, dot (tmpvar_21, tmpvar_25)), tmpvar_30))
   / 
    (((8.0 * (
      ((tmpvar_27 * tmpvar_27) * gbuffer1_2.w)
     + 
      (tmpvar_28 * tmpvar_28)
    )) * tmpvar_27) + 0.0001)
  )));
  mediump float tmpvar_31;
  tmpvar_31 = clamp (specular_24, 0.0, 100.0);
  specular_24 = tmpvar_31;
  mediump vec4 tmpvar_32;
  tmpvar_32.w = 1.0;
  tmpvar_32.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_31 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_21, lightDir_6)));
  gl_FragData[0] = tmpvar_32;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  tmpvar_10 = (tmpvar_9 - _LightPos.xyz);
  highp vec3 tmpvar_11;
  tmpvar_11 = -(normalize(tmpvar_10));
  lightDir_6 = tmpvar_11;
  highp float tmpvar_12;
  tmpvar_12 = (dot (tmpvar_10, tmpvar_10) * _LightPos.w);
  lowp float tmpvar_13;
  tmpvar_13 = texture2D (_LightTextureB0, vec2(tmpvar_12)).w;
  atten_5 = tmpvar_13;
  highp float mydist_14;
  mydist_14 = ((sqrt(
    dot (tmpvar_10, tmpvar_10)
  ) * _LightPositionRange.w) * 0.97);
  highp float tmpvar_15;
  tmpvar_15 = dot (textureCube (_ShadowMapTexture, tmpvar_10), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  mediump float tmpvar_16;
  if ((tmpvar_15 < mydist_14)) {
    tmpvar_16 = _LightShadowData.x;
  } else {
    tmpvar_16 = 1.0;
  };
  atten_5 = (atten_5 * tmpvar_16);
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_17;
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_19;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_20;
  tmpvar_20 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_21;
  tmpvar_21 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_22;
  viewDir_22 = -(tmpvar_21);
  mediump float specular_23;
  mediump vec3 tmpvar_24;
  mediump vec3 inVec_25;
  inVec_25 = (lightDir_6 + viewDir_22);
  tmpvar_24 = (inVec_25 * inversesqrt(max (0.001, 
    dot (inVec_25, inVec_25)
  )));
  mediump float tmpvar_26;
  tmpvar_26 = max (0.0, dot (lightDir_6, tmpvar_24));
  mediump float tmpvar_27;
  tmpvar_27 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_28;
  tmpvar_28 = max (0.0001, (tmpvar_27 * tmpvar_27));
  mediump float tmpvar_29;
  tmpvar_29 = max (((2.0 / 
    (tmpvar_28 * tmpvar_28)
  ) - 2.0), 0.0001);
  specular_23 = sqrt(max (0.0001, (
    ((tmpvar_29 + 1.0) * pow (max (0.0, dot (tmpvar_20, tmpvar_24)), tmpvar_29))
   / 
    (((8.0 * (
      ((tmpvar_26 * tmpvar_26) * gbuffer1_2.w)
     + 
      (tmpvar_27 * tmpvar_27)
    )) * tmpvar_26) + 0.0001)
  )));
  mediump float tmpvar_30;
  tmpvar_30 = clamp (specular_23, 0.0, 100.0);
  specular_23 = tmpvar_30;
  mediump vec4 tmpvar_31;
  tmpvar_31.w = 1.0;
  tmpvar_31.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_30 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_20, lightDir_6)));
  gl_FragData[0] = tmpvar_31;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  tmpvar_10 = (tmpvar_9 - _LightPos.xyz);
  highp vec3 tmpvar_11;
  tmpvar_11 = -(normalize(tmpvar_10));
  lightDir_6 = tmpvar_11;
  highp float tmpvar_12;
  tmpvar_12 = (dot (tmpvar_10, tmpvar_10) * _LightPos.w);
  lowp float tmpvar_13;
  tmpvar_13 = texture2D (_LightTextureB0, vec2(tmpvar_12)).w;
  atten_5 = tmpvar_13;
  highp float mydist_14;
  mydist_14 = ((sqrt(
    dot (tmpvar_10, tmpvar_10)
  ) * _LightPositionRange.w) * 0.97);
  highp float tmpvar_15;
  tmpvar_15 = dot (textureCube (_ShadowMapTexture, tmpvar_10), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  mediump float tmpvar_16;
  if ((tmpvar_15 < mydist_14)) {
    tmpvar_16 = _LightShadowData.x;
  } else {
    tmpvar_16 = 1.0;
  };
  atten_5 = (atten_5 * tmpvar_16);
  highp vec4 tmpvar_17;
  tmpvar_17.w = 1.0;
  tmpvar_17.xyz = tmpvar_9;
  highp vec4 tmpvar_18;
  tmpvar_18.w = -8.0;
  tmpvar_18.xyz = (_LightMatrix0 * tmpvar_17).xyz;
  lowp vec4 tmpvar_19;
  tmpvar_19 = textureCube (_LightTexture0, tmpvar_18.xyz, -8.0);
  atten_5 = (atten_5 * tmpvar_19.w);
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_20;
  lowp vec4 tmpvar_21;
  tmpvar_21 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_21;
  lowp vec4 tmpvar_22;
  tmpvar_22 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_22;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_23;
  tmpvar_23 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_24;
  tmpvar_24 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_25;
  viewDir_25 = -(tmpvar_24);
  mediump float specular_26;
  mediump vec3 tmpvar_27;
  mediump vec3 inVec_28;
  inVec_28 = (lightDir_6 + viewDir_25);
  tmpvar_27 = (inVec_28 * inversesqrt(max (0.001, 
    dot (inVec_28, inVec_28)
  )));
  mediump float tmpvar_29;
  tmpvar_29 = max (0.0, dot (lightDir_6, tmpvar_27));
  mediump float tmpvar_30;
  tmpvar_30 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_31;
  tmpvar_31 = max (0.0001, (tmpvar_30 * tmpvar_30));
  mediump float tmpvar_32;
  tmpvar_32 = max (((2.0 / 
    (tmpvar_31 * tmpvar_31)
  ) - 2.0), 0.0001);
  specular_26 = sqrt(max (0.0001, (
    ((tmpvar_32 + 1.0) * pow (max (0.0, dot (tmpvar_23, tmpvar_27)), tmpvar_32))
   / 
    (((8.0 * (
      ((tmpvar_29 * tmpvar_29) * gbuffer1_2.w)
     + 
      (tmpvar_30 * tmpvar_30)
    )) * tmpvar_29) + 0.0001)
  )));
  mediump float tmpvar_33;
  tmpvar_33 = clamp (specular_26, 0.0, 100.0);
  specular_26 = tmpvar_33;
  mediump vec4 tmpvar_34;
  tmpvar_34.w = 1.0;
  tmpvar_34.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_33 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_23, lightDir_6)));
  gl_FragData[0] = tmpvar_34;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "UNITY_HDR_ON" "SHADOWS_NONATIVE" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform highp sampler2D _ShadowMapTexture;
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  tmpvar_11 = (_LightPos.xyz - tmpvar_9);
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize(tmpvar_11);
  lightDir_6 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_9;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_LightMatrix0 * tmpvar_13);
  highp vec4 tmpvar_15;
  tmpvar_15.zw = vec2(0.0, -8.0);
  tmpvar_15.xy = (tmpvar_14.xy / tmpvar_14.w);
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_LightTexture0, tmpvar_15.xy, -8.0);
  highp float tmpvar_17;
  tmpvar_17 = tmpvar_16.w;
  atten_5 = (tmpvar_17 * float((tmpvar_14.w < 0.0)));
  highp float tmpvar_18;
  tmpvar_18 = (dot (tmpvar_11, tmpvar_11) * _LightPos.w);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_LightTextureB0, vec2(tmpvar_18));
  atten_5 = (atten_5 * tmpvar_19.w);
  mediump float tmpvar_20;
  highp float tmpvar_21;
  tmpvar_21 = clamp (((
    mix (tmpvar_8.z, sqrt(dot (tmpvar_10, tmpvar_10)), unity_ShadowFadeCenterAndType.w)
   * _LightShadowData.z) + _LightShadowData.w), 0.0, 1.0);
  highp vec4 tmpvar_22;
  tmpvar_22.w = 1.0;
  tmpvar_22.xyz = tmpvar_9;
  highp vec4 tmpvar_23;
  tmpvar_23 = (unity_World2Shadow[0] * tmpvar_22);
  lowp float tmpvar_24;
  highp vec4 shadowVals_25;
  highp vec3 tmpvar_26;
  tmpvar_26 = (tmpvar_23.xyz / tmpvar_23.w);
  shadowVals_25.x = texture2D (_ShadowMapTexture, (tmpvar_26.xy + _ShadowOffsets[0].xy)).x;
  shadowVals_25.y = texture2D (_ShadowMapTexture, (tmpvar_26.xy + _ShadowOffsets[1].xy)).x;
  shadowVals_25.z = texture2D (_ShadowMapTexture, (tmpvar_26.xy + _ShadowOffsets[2].xy)).x;
  shadowVals_25.w = texture2D (_ShadowMapTexture, (tmpvar_26.xy + _ShadowOffsets[3].xy)).x;
  bvec4 tmpvar_27;
  tmpvar_27 = lessThan (shadowVals_25, tmpvar_26.zzzz);
  mediump vec4 tmpvar_28;
  tmpvar_28 = _LightShadowData.xxxx;
  mediump float tmpvar_29;
  if (tmpvar_27.x) {
    tmpvar_29 = tmpvar_28.x;
  } else {
    tmpvar_29 = 1.0;
  };
  mediump float tmpvar_30;
  if (tmpvar_27.y) {
    tmpvar_30 = tmpvar_28.y;
  } else {
    tmpvar_30 = 1.0;
  };
  mediump float tmpvar_31;
  if (tmpvar_27.z) {
    tmpvar_31 = tmpvar_28.z;
  } else {
    tmpvar_31 = 1.0;
  };
  mediump float tmpvar_32;
  if (tmpvar_27.w) {
    tmpvar_32 = tmpvar_28.w;
  } else {
    tmpvar_32 = 1.0;
  };
  mediump vec4 tmpvar_33;
  tmpvar_33.x = tmpvar_29;
  tmpvar_33.y = tmpvar_30;
  tmpvar_33.z = tmpvar_31;
  tmpvar_33.w = tmpvar_32;
  mediump float tmpvar_34;
  tmpvar_34 = dot (tmpvar_33, vec4(0.25, 0.25, 0.25, 0.25));
  tmpvar_24 = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = clamp ((tmpvar_24 + tmpvar_21), 0.0, 1.0);
  tmpvar_20 = tmpvar_35;
  atten_5 = (atten_5 * tmpvar_20);
  lowp vec4 tmpvar_36;
  tmpvar_36 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_36;
  lowp vec4 tmpvar_37;
  tmpvar_37 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_37;
  lowp vec4 tmpvar_38;
  tmpvar_38 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_38;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_39;
  tmpvar_39 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_40;
  tmpvar_40 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_41;
  viewDir_41 = -(tmpvar_40);
  mediump float specular_42;
  mediump vec3 tmpvar_43;
  mediump vec3 inVec_44;
  inVec_44 = (lightDir_6 + viewDir_41);
  tmpvar_43 = (inVec_44 * inversesqrt(max (0.001, 
    dot (inVec_44, inVec_44)
  )));
  mediump float tmpvar_45;
  tmpvar_45 = max (0.0, dot (lightDir_6, tmpvar_43));
  mediump float tmpvar_46;
  tmpvar_46 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_47;
  tmpvar_47 = max (0.0001, (tmpvar_46 * tmpvar_46));
  mediump float tmpvar_48;
  tmpvar_48 = max (((2.0 / 
    (tmpvar_47 * tmpvar_47)
  ) - 2.0), 0.0001);
  specular_42 = sqrt(max (0.0001, (
    ((tmpvar_48 + 1.0) * pow (max (0.0, dot (tmpvar_39, tmpvar_43)), tmpvar_48))
   / 
    (((8.0 * (
      ((tmpvar_45 * tmpvar_45) * gbuffer1_2.w)
     + 
      (tmpvar_46 * tmpvar_46)
    )) * tmpvar_45) + 0.0001)
  )));
  mediump float tmpvar_49;
  tmpvar_49 = clamp (specular_42, 0.0, 100.0);
  specular_42 = tmpvar_49;
  mediump vec4 tmpvar_50;
  tmpvar_50.w = 1.0;
  tmpvar_50.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_49 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_39, lightDir_6)));
  gl_FragData[0] = tmpvar_50;
}


#endif
"
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NATIVE" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform sampler2D _LightTexture0;
uniform lowp sampler2DShadow _ShadowMapTexture;
uniform highp vec4 _ShadowOffsets[4];
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  tmpvar_11 = (_LightPos.xyz - tmpvar_9);
  highp vec3 tmpvar_12;
  tmpvar_12 = normalize(tmpvar_11);
  lightDir_6 = tmpvar_12;
  highp vec4 tmpvar_13;
  tmpvar_13.w = 1.0;
  tmpvar_13.xyz = tmpvar_9;
  highp vec4 tmpvar_14;
  tmpvar_14 = (_LightMatrix0 * tmpvar_13);
  highp vec4 tmpvar_15;
  tmpvar_15.zw = vec2(0.0, -8.0);
  tmpvar_15.xy = (tmpvar_14.xy / tmpvar_14.w);
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_LightTexture0, tmpvar_15.xy, -8.0);
  highp float tmpvar_17;
  tmpvar_17 = tmpvar_16.w;
  atten_5 = (tmpvar_17 * float((tmpvar_14.w < 0.0)));
  highp float tmpvar_18;
  tmpvar_18 = (dot (tmpvar_11, tmpvar_11) * _LightPos.w);
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_LightTextureB0, vec2(tmpvar_18));
  atten_5 = (atten_5 * tmpvar_19.w);
  mediump float tmpvar_20;
  highp vec4 tmpvar_21;
  tmpvar_21.w = 1.0;
  tmpvar_21.xyz = tmpvar_9;
  highp vec4 tmpvar_22;
  tmpvar_22 = (unity_World2Shadow[0] * tmpvar_21);
  lowp float tmpvar_23;
  mediump vec4 shadows_24;
  highp vec3 tmpvar_25;
  tmpvar_25 = (tmpvar_22.xyz / tmpvar_22.w);
  highp vec3 coord_26;
  coord_26 = (tmpvar_25 + _ShadowOffsets[0].xyz);
  lowp float tmpvar_27;
  tmpvar_27 = shadow2DEXT (_ShadowMapTexture, coord_26);
  shadows_24.x = tmpvar_27;
  highp vec3 coord_28;
  coord_28 = (tmpvar_25 + _ShadowOffsets[1].xyz);
  lowp float tmpvar_29;
  tmpvar_29 = shadow2DEXT (_ShadowMapTexture, coord_28);
  shadows_24.y = tmpvar_29;
  highp vec3 coord_30;
  coord_30 = (tmpvar_25 + _ShadowOffsets[2].xyz);
  lowp float tmpvar_31;
  tmpvar_31 = shadow2DEXT (_ShadowMapTexture, coord_30);
  shadows_24.z = tmpvar_31;
  highp vec3 coord_32;
  coord_32 = (tmpvar_25 + _ShadowOffsets[3].xyz);
  lowp float tmpvar_33;
  tmpvar_33 = shadow2DEXT (_ShadowMapTexture, coord_32);
  shadows_24.w = tmpvar_33;
  shadows_24 = (_LightShadowData.xxxx + (shadows_24 * (1.0 - _LightShadowData.xxxx)));
  mediump float tmpvar_34;
  tmpvar_34 = dot (shadows_24, vec4(0.25, 0.25, 0.25, 0.25));
  tmpvar_23 = tmpvar_34;
  highp float tmpvar_35;
  tmpvar_35 = clamp ((tmpvar_23 + clamp (
    ((mix (tmpvar_8.z, sqrt(
      dot (tmpvar_10, tmpvar_10)
    ), unity_ShadowFadeCenterAndType.w) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_20 = tmpvar_35;
  atten_5 = (atten_5 * tmpvar_20);
  lowp vec4 tmpvar_36;
  tmpvar_36 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_36;
  lowp vec4 tmpvar_37;
  tmpvar_37 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_37;
  lowp vec4 tmpvar_38;
  tmpvar_38 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_38;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_39;
  tmpvar_39 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_40;
  tmpvar_40 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_41;
  viewDir_41 = -(tmpvar_40);
  mediump float specular_42;
  mediump vec3 tmpvar_43;
  mediump vec3 inVec_44;
  inVec_44 = (lightDir_6 + viewDir_41);
  tmpvar_43 = (inVec_44 * inversesqrt(max (0.001, 
    dot (inVec_44, inVec_44)
  )));
  mediump float tmpvar_45;
  tmpvar_45 = max (0.0, dot (lightDir_6, tmpvar_43));
  mediump float tmpvar_46;
  tmpvar_46 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_47;
  tmpvar_47 = max (0.0001, (tmpvar_46 * tmpvar_46));
  mediump float tmpvar_48;
  tmpvar_48 = max (((2.0 / 
    (tmpvar_47 * tmpvar_47)
  ) - 2.0), 0.0001);
  specular_42 = sqrt(max (0.0001, (
    ((tmpvar_48 + 1.0) * pow (max (0.0, dot (tmpvar_39, tmpvar_43)), tmpvar_48))
   / 
    (((8.0 * (
      ((tmpvar_45 * tmpvar_45) * gbuffer1_2.w)
     + 
      (tmpvar_46 * tmpvar_46)
    )) * tmpvar_45) + 0.0001)
  )));
  mediump float tmpvar_49;
  tmpvar_49 = clamp (specular_42, 0.0, 100.0);
  specular_42 = tmpvar_49;
  mediump vec4 tmpvar_50;
  tmpvar_50.w = 1.0;
  tmpvar_50.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_49 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_39, lightDir_6)));
  gl_FragData[0] = tmpvar_50;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" "SHADOWS_SOFT" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _LightTextureB0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  tmpvar_10 = (tmpvar_9 - _LightPos.xyz);
  highp vec3 tmpvar_11;
  tmpvar_11 = -(normalize(tmpvar_10));
  lightDir_6 = tmpvar_11;
  highp float tmpvar_12;
  tmpvar_12 = (dot (tmpvar_10, tmpvar_10) * _LightPos.w);
  lowp float tmpvar_13;
  tmpvar_13 = texture2D (_LightTextureB0, vec2(tmpvar_12)).w;
  atten_5 = tmpvar_13;
  highp vec4 shadowVals_14;
  highp float mydist_15;
  mydist_15 = ((sqrt(
    dot (tmpvar_10, tmpvar_10)
  ) * _LightPositionRange.w) * 0.97);
  shadowVals_14.x = dot (textureCube (_ShadowMapTexture, (tmpvar_10 + vec3(0.0078125, 0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_14.y = dot (textureCube (_ShadowMapTexture, (tmpvar_10 + vec3(-0.0078125, -0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_14.z = dot (textureCube (_ShadowMapTexture, (tmpvar_10 + vec3(-0.0078125, 0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_14.w = dot (textureCube (_ShadowMapTexture, (tmpvar_10 + vec3(0.0078125, -0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  bvec4 tmpvar_16;
  tmpvar_16 = lessThan (shadowVals_14, vec4(mydist_15));
  mediump vec4 tmpvar_17;
  tmpvar_17 = _LightShadowData.xxxx;
  mediump float tmpvar_18;
  if (tmpvar_16.x) {
    tmpvar_18 = tmpvar_17.x;
  } else {
    tmpvar_18 = 1.0;
  };
  mediump float tmpvar_19;
  if (tmpvar_16.y) {
    tmpvar_19 = tmpvar_17.y;
  } else {
    tmpvar_19 = 1.0;
  };
  mediump float tmpvar_20;
  if (tmpvar_16.z) {
    tmpvar_20 = tmpvar_17.z;
  } else {
    tmpvar_20 = 1.0;
  };
  mediump float tmpvar_21;
  if (tmpvar_16.w) {
    tmpvar_21 = tmpvar_17.w;
  } else {
    tmpvar_21 = 1.0;
  };
  mediump vec4 tmpvar_22;
  tmpvar_22.x = tmpvar_18;
  tmpvar_22.y = tmpvar_19;
  tmpvar_22.z = tmpvar_20;
  tmpvar_22.w = tmpvar_21;
  mediump float tmpvar_23;
  tmpvar_23 = dot (tmpvar_22, vec4(0.25, 0.25, 0.25, 0.25));
  atten_5 = (atten_5 * tmpvar_23);
  lowp vec4 tmpvar_24;
  tmpvar_24 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_24;
  lowp vec4 tmpvar_25;
  tmpvar_25 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_25;
  lowp vec4 tmpvar_26;
  tmpvar_26 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_26;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_27;
  tmpvar_27 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_28;
  tmpvar_28 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_29;
  viewDir_29 = -(tmpvar_28);
  mediump float specular_30;
  mediump vec3 tmpvar_31;
  mediump vec3 inVec_32;
  inVec_32 = (lightDir_6 + viewDir_29);
  tmpvar_31 = (inVec_32 * inversesqrt(max (0.001, 
    dot (inVec_32, inVec_32)
  )));
  mediump float tmpvar_33;
  tmpvar_33 = max (0.0, dot (lightDir_6, tmpvar_31));
  mediump float tmpvar_34;
  tmpvar_34 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_35;
  tmpvar_35 = max (0.0001, (tmpvar_34 * tmpvar_34));
  mediump float tmpvar_36;
  tmpvar_36 = max (((2.0 / 
    (tmpvar_35 * tmpvar_35)
  ) - 2.0), 0.0001);
  specular_30 = sqrt(max (0.0001, (
    ((tmpvar_36 + 1.0) * pow (max (0.0, dot (tmpvar_27, tmpvar_31)), tmpvar_36))
   / 
    (((8.0 * (
      ((tmpvar_33 * tmpvar_33) * gbuffer1_2.w)
     + 
      (tmpvar_34 * tmpvar_34)
    )) * tmpvar_33) + 0.0001)
  )));
  mediump float tmpvar_37;
  tmpvar_37 = clamp (specular_30, 0.0, 100.0);
  specular_30 = tmpvar_37;
  mediump vec4 tmpvar_38;
  tmpvar_38.w = 1.0;
  tmpvar_38.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_37 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_27, lightDir_6)));
  gl_FragData[0] = tmpvar_38;
}


#endif
"
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" "SHADOWS_SOFT" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightPos;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTextureB0;
uniform lowp samplerCube _LightTexture0;
uniform highp samplerCube _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  tmpvar_10 = (tmpvar_9 - _LightPos.xyz);
  highp vec3 tmpvar_11;
  tmpvar_11 = -(normalize(tmpvar_10));
  lightDir_6 = tmpvar_11;
  highp float tmpvar_12;
  tmpvar_12 = (dot (tmpvar_10, tmpvar_10) * _LightPos.w);
  lowp float tmpvar_13;
  tmpvar_13 = texture2D (_LightTextureB0, vec2(tmpvar_12)).w;
  atten_5 = tmpvar_13;
  highp vec4 shadowVals_14;
  highp float mydist_15;
  mydist_15 = ((sqrt(
    dot (tmpvar_10, tmpvar_10)
  ) * _LightPositionRange.w) * 0.97);
  shadowVals_14.x = dot (textureCube (_ShadowMapTexture, (tmpvar_10 + vec3(0.0078125, 0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_14.y = dot (textureCube (_ShadowMapTexture, (tmpvar_10 + vec3(-0.0078125, -0.0078125, 0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_14.z = dot (textureCube (_ShadowMapTexture, (tmpvar_10 + vec3(-0.0078125, 0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  shadowVals_14.w = dot (textureCube (_ShadowMapTexture, (tmpvar_10 + vec3(0.0078125, -0.0078125, -0.0078125))), vec4(1.0, 0.003921569, 1.53787e-05, 6.030863e-08));
  bvec4 tmpvar_16;
  tmpvar_16 = lessThan (shadowVals_14, vec4(mydist_15));
  mediump vec4 tmpvar_17;
  tmpvar_17 = _LightShadowData.xxxx;
  mediump float tmpvar_18;
  if (tmpvar_16.x) {
    tmpvar_18 = tmpvar_17.x;
  } else {
    tmpvar_18 = 1.0;
  };
  mediump float tmpvar_19;
  if (tmpvar_16.y) {
    tmpvar_19 = tmpvar_17.y;
  } else {
    tmpvar_19 = 1.0;
  };
  mediump float tmpvar_20;
  if (tmpvar_16.z) {
    tmpvar_20 = tmpvar_17.z;
  } else {
    tmpvar_20 = 1.0;
  };
  mediump float tmpvar_21;
  if (tmpvar_16.w) {
    tmpvar_21 = tmpvar_17.w;
  } else {
    tmpvar_21 = 1.0;
  };
  mediump vec4 tmpvar_22;
  tmpvar_22.x = tmpvar_18;
  tmpvar_22.y = tmpvar_19;
  tmpvar_22.z = tmpvar_20;
  tmpvar_22.w = tmpvar_21;
  mediump float tmpvar_23;
  tmpvar_23 = dot (tmpvar_22, vec4(0.25, 0.25, 0.25, 0.25));
  atten_5 = (atten_5 * tmpvar_23);
  highp vec4 tmpvar_24;
  tmpvar_24.w = 1.0;
  tmpvar_24.xyz = tmpvar_9;
  highp vec4 tmpvar_25;
  tmpvar_25.w = -8.0;
  tmpvar_25.xyz = (_LightMatrix0 * tmpvar_24).xyz;
  lowp vec4 tmpvar_26;
  tmpvar_26 = textureCube (_LightTexture0, tmpvar_25.xyz, -8.0);
  atten_5 = (atten_5 * tmpvar_26.w);
  lowp vec4 tmpvar_27;
  tmpvar_27 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_27;
  lowp vec4 tmpvar_28;
  tmpvar_28 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_28;
  lowp vec4 tmpvar_29;
  tmpvar_29 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_29;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_30;
  tmpvar_30 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_31;
  tmpvar_31 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_32;
  viewDir_32 = -(tmpvar_31);
  mediump float specular_33;
  mediump vec3 tmpvar_34;
  mediump vec3 inVec_35;
  inVec_35 = (lightDir_6 + viewDir_32);
  tmpvar_34 = (inVec_35 * inversesqrt(max (0.001, 
    dot (inVec_35, inVec_35)
  )));
  mediump float tmpvar_36;
  tmpvar_36 = max (0.0, dot (lightDir_6, tmpvar_34));
  mediump float tmpvar_37;
  tmpvar_37 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_38;
  tmpvar_38 = max (0.0001, (tmpvar_37 * tmpvar_37));
  mediump float tmpvar_39;
  tmpvar_39 = max (((2.0 / 
    (tmpvar_38 * tmpvar_38)
  ) - 2.0), 0.0001);
  specular_33 = sqrt(max (0.0001, (
    ((tmpvar_39 + 1.0) * pow (max (0.0, dot (tmpvar_30, tmpvar_34)), tmpvar_39))
   / 
    (((8.0 * (
      ((tmpvar_36 * tmpvar_36) * gbuffer1_2.w)
     + 
      (tmpvar_37 * tmpvar_37)
    )) * tmpvar_36) + 0.0001)
  )));
  mediump float tmpvar_40;
  tmpvar_40 = clamp (specular_33, 0.0, 100.0);
  specular_33 = tmpvar_40;
  mediump vec4 tmpvar_41;
  tmpvar_41.w = 1.0;
  tmpvar_41.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_40 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_30, lightDir_6)));
  gl_FragData[0] = tmpvar_41;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  mediump float tmpvar_12;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_ShadowMapTexture, tmpvar_7);
  highp float tmpvar_14;
  tmpvar_14 = clamp ((tmpvar_13.x + clamp (
    ((mix (tmpvar_8.z, sqrt(
      dot (tmpvar_10, tmpvar_10)
    ), unity_ShadowFadeCenterAndType.w) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_12 = tmpvar_14;
  atten_5 = tmpvar_12;
  lowp vec4 tmpvar_15;
  tmpvar_15 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_15;
  lowp vec4 tmpvar_16;
  tmpvar_16 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_16;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_17;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_18;
  tmpvar_18 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_19;
  tmpvar_19 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_20;
  viewDir_20 = -(tmpvar_19);
  mediump float specular_21;
  mediump vec3 tmpvar_22;
  mediump vec3 inVec_23;
  inVec_23 = (lightDir_6 + viewDir_20);
  tmpvar_22 = (inVec_23 * inversesqrt(max (0.001, 
    dot (inVec_23, inVec_23)
  )));
  mediump float tmpvar_24;
  tmpvar_24 = max (0.0, dot (lightDir_6, tmpvar_22));
  mediump float tmpvar_25;
  tmpvar_25 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_26;
  tmpvar_26 = max (0.0001, (tmpvar_25 * tmpvar_25));
  mediump float tmpvar_27;
  tmpvar_27 = max (((2.0 / 
    (tmpvar_26 * tmpvar_26)
  ) - 2.0), 0.0001);
  specular_21 = sqrt(max (0.0001, (
    ((tmpvar_27 + 1.0) * pow (max (0.0, dot (tmpvar_18, tmpvar_22)), tmpvar_27))
   / 
    (((8.0 * (
      ((tmpvar_24 * tmpvar_24) * gbuffer1_2.w)
     + 
      (tmpvar_25 * tmpvar_25)
    )) * tmpvar_24) + 0.0001)
  )));
  mediump float tmpvar_28;
  tmpvar_28 = clamp (specular_21, 0.0, 100.0);
  specular_21 = tmpvar_28;
  mediump vec4 tmpvar_29;
  tmpvar_29.w = 1.0;
  tmpvar_29.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_28 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_18, lightDir_6)));
  gl_FragData[0] = tmpvar_29;
}


#endif
"
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "UNITY_HDR_ON" }
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
uniform highp sampler2D _CameraDepthTexture;
uniform highp vec4 _LightDir;
uniform highp vec4 _LightColor;
uniform highp mat4 _CameraToWorld;
uniform highp mat4 _LightMatrix0;
uniform sampler2D _LightTexture0;
uniform sampler2D _ShadowMapTexture;
uniform sampler2D _CameraGBufferTexture0;
uniform sampler2D _CameraGBufferTexture1;
uniform sampler2D _CameraGBufferTexture2;
varying highp vec4 xlv_TEXCOORD0;
varying highp vec3 xlv_TEXCOORD1;
void main ()
{
  mediump vec4 gbuffer2_1;
  mediump vec4 gbuffer1_2;
  mediump vec4 gbuffer0_3;
  mediump vec3 tmpvar_4;
  highp float atten_5;
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
  mediump float tmpvar_12;
  lowp vec4 tmpvar_13;
  tmpvar_13 = texture2D (_ShadowMapTexture, tmpvar_7);
  highp float tmpvar_14;
  tmpvar_14 = clamp ((tmpvar_13.x + clamp (
    ((mix (tmpvar_8.z, sqrt(
      dot (tmpvar_10, tmpvar_10)
    ), unity_ShadowFadeCenterAndType.w) * _LightShadowData.z) + _LightShadowData.w)
  , 0.0, 1.0)), 0.0, 1.0);
  tmpvar_12 = tmpvar_14;
  atten_5 = tmpvar_12;
  highp vec4 tmpvar_15;
  tmpvar_15.w = 1.0;
  tmpvar_15.xyz = tmpvar_9;
  highp vec4 tmpvar_16;
  tmpvar_16.zw = vec2(0.0, -8.0);
  tmpvar_16.xy = (_LightMatrix0 * tmpvar_15).xy;
  lowp vec4 tmpvar_17;
  tmpvar_17 = texture2D (_LightTexture0, tmpvar_16.xy, -8.0);
  atten_5 = (atten_5 * tmpvar_17.w);
  lowp vec4 tmpvar_18;
  tmpvar_18 = texture2D (_CameraGBufferTexture0, tmpvar_7);
  gbuffer0_3 = tmpvar_18;
  lowp vec4 tmpvar_19;
  tmpvar_19 = texture2D (_CameraGBufferTexture1, tmpvar_7);
  gbuffer1_2 = tmpvar_19;
  lowp vec4 tmpvar_20;
  tmpvar_20 = texture2D (_CameraGBufferTexture2, tmpvar_7);
  gbuffer2_1 = tmpvar_20;
  tmpvar_4 = (_LightColor.xyz * atten_5);
  mediump vec3 tmpvar_21;
  tmpvar_21 = normalize(((gbuffer2_1.xyz * 2.0) - 1.0));
  highp vec3 tmpvar_22;
  tmpvar_22 = normalize((tmpvar_9 - _WorldSpaceCameraPos));
  mediump vec3 viewDir_23;
  viewDir_23 = -(tmpvar_22);
  mediump float specular_24;
  mediump vec3 tmpvar_25;
  mediump vec3 inVec_26;
  inVec_26 = (lightDir_6 + viewDir_23);
  tmpvar_25 = (inVec_26 * inversesqrt(max (0.001, 
    dot (inVec_26, inVec_26)
  )));
  mediump float tmpvar_27;
  tmpvar_27 = max (0.0, dot (lightDir_6, tmpvar_25));
  mediump float tmpvar_28;
  tmpvar_28 = (1.0 - gbuffer1_2.w);
  mediump float tmpvar_29;
  tmpvar_29 = max (0.0001, (tmpvar_28 * tmpvar_28));
  mediump float tmpvar_30;
  tmpvar_30 = max (((2.0 / 
    (tmpvar_29 * tmpvar_29)
  ) - 2.0), 0.0001);
  specular_24 = sqrt(max (0.0001, (
    ((tmpvar_30 + 1.0) * pow (max (0.0, dot (tmpvar_21, tmpvar_25)), tmpvar_30))
   / 
    (((8.0 * (
      ((tmpvar_27 * tmpvar_27) * gbuffer1_2.w)
     + 
      (tmpvar_28 * tmpvar_28)
    )) * tmpvar_27) + 0.0001)
  )));
  mediump float tmpvar_31;
  tmpvar_31 = clamp (specular_24, 0.0, 100.0);
  specular_24 = tmpvar_31;
  mediump vec4 tmpvar_32;
  tmpvar_32.w = 1.0;
  tmpvar_32.xyz = (((gbuffer0_3.xyz + 
    (tmpvar_31 * gbuffer1_2.xyz)
  ) * tmpvar_4) * max (0.0, dot (tmpvar_21, lightDir_6)));
  gl_FragData[0] = tmpvar_32;
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
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_OFF" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_OFF" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_OFF" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_OFF" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_OFF" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "UNITY_HDR_ON" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_NATIVE" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "UNITY_HDR_ON" "SHADOWS_NONATIVE" }
""
}
SubProgram "gles " {
Keywords { "SPOT" "SHADOWS_DEPTH" "SHADOWS_SOFT" "SHADOWS_NATIVE" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "POINT" "SHADOWS_CUBE" "SHADOWS_SOFT" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "POINT_COOKIE" "SHADOWS_CUBE" "SHADOWS_SOFT" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "UNITY_HDR_ON" }
""
}
SubProgram "gles " {
Keywords { "DIRECTIONAL_COOKIE" "SHADOWS_SCREEN" "SHADOWS_NATIVE" "UNITY_HDR_ON" }
""
}
}
 }
 Pass {
  ZTest Always
  ZWrite Off
  Cull Off
  Stencil {
   Ref [_StencilNonBackground]
   ReadMask [_StencilNonBackground]
   CompFront Equal
   CompBack Equal
  }
GLSLPROGRAM
#version 100

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
uniform sampler2D _LightBuffer;
varying highp vec2 xlv_TEXCOORD0;
void main ()
{
  lowp vec4 tmpvar_1;
  tmpvar_1 = -(log2(texture2D (_LightBuffer, xlv_TEXCOORD0)));
  gl_FragData[0] = tmpvar_1;
}


#endif

ENDGLSL
 }
}
Fallback Off
}