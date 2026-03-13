//////////////////////////////////////////
///////////////////////////////////////////
Shader "Kampai/Standard/Hidden" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("Base (RGB)", 2D) = "white" { }
 _Boost ("Boost", Float) = 1
 _UVScroll ("UV Scroll, Base (XY) Unused (ZW)", Vector) = (0,0,0,0)
 _BlendedColor ("Blended Color", Color) = (0,0,0,0)
[Enum(Kampai.Util.Graphics.CompareFunction)]  _ZTest ("ZTest", Float) = 4
}
SubShader { 
 Pass {
  ZTest [_ZTest]
GLSLPROGRAM
#version 100

#ifdef VERTEX
attribute vec4 _glesVertex;
varying mediump vec2 xlv_TEXCOORD0;
void main ()
{
  mediump vec2 tmpvar_1;
  gl_Position = (_glesVertex - vec4(0.0, 0.0, 1000000.0, 0.0));
  xlv_TEXCOORD0 = tmpvar_1;
}


#endif
#ifdef FRAGMENT
void main ()
{
  gl_FragData[0] = vec4(0.0, 0.0, 0.0, 0.0);
}


#endif

ENDGLSL
 }
}
}