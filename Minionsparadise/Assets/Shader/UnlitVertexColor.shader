//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Standard/Vertex Color" {
Properties {
 _MainTex ("MainTex", 2D) = "white" { }
 _boost ("Boost", Float) = 0
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Name "FORWARDBASE"
  Tags { "LIGHTMODE"="ForwardBase" "RenderType"="Opaque" }
GLSLPROGRAM
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
attribute vec4 _glesColor;
attribute vec4 _glesMultiTexCoord0;
uniform highp mat4 glstate_matrix_mvp;
uniform highp vec4 _MainTex_ST;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_COLOR;
void main ()
{
  mediump vec2 tmpvar_1;
  tmpvar_1 = ((_glesMultiTexCoord0.xy * _MainTex_ST.xy) + _MainTex_ST.zw);
  gl_Position = (glstate_matrix_mvp * _glesVertex);
  xlv_TEXCOORD0 = tmpvar_1;
  xlv_COLOR = _glesColor;
}


#endif
#ifdef FRAGMENT
uniform lowp vec4 glstate_lightmodel_ambient;
uniform sampler2D _MainTex;
uniform highp float _boost;
varying mediump vec2 xlv_TEXCOORD0;
varying highp vec4 xlv_COLOR;
void main ()
{
  lowp vec4 c_1;
  lowp float diff_2;
  lowp vec4 tmpvar_3;
  tmpvar_3 = texture2D (_MainTex, xlv_TEXCOORD0);
  highp float tmpvar_4;
  tmpvar_4 = (vec3(1.0, 1.0, 1.0) + (glstate_lightmodel_ambient * 2.0).xyz).x;
  diff_2 = tmpvar_4;
  highp vec3 tmpvar_5;
  tmpvar_5 = (xlv_COLOR * -(_boost)).xyz;
  c_1.xyz = (diff_2 * ((tmpvar_3.xyz - tmpvar_5) * tmpvar_3.xyz));
  c_1.w = 1.0;
  gl_FragData[0] = c_1;
}


#endif

ENDGLSL
 }
}
Fallback "Mobile/Diffuse"
}