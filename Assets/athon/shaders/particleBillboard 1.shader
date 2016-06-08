Shader "Custom/particleBillboard" {
	Properties {
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_PosTex ("position", 2D) = "white" {}
		_Offset ("offset", 2D) = "white" {}
		_Color ("color",Color) = (1,1,1,1)
		_SpriteTex("sprite", 2D) = "white" {}
		_ShadowTex("shadow", 2D) = "white" {}
		_Tile ("Tiling", Float) = 12
		_ShadowTile ("Tiling", Float) = 12
		_ShadowSpeed ("Tiling", Float) = 12
		_LineWidth ("lineWidth", Float) = 12
		_Speed ("Speed", Float) = 12
		_Saturation ("saturation", Float) = 1
		_Brightness ("brightness", Float) = 1
		_UNPnts ("pointsAmount", Float) = 100
		_HueShift ("hue shift", Float) = 0
//		_ObjMat ("pointsAmount", Float4x4) = 100
	}

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 100
		
		ZWrite Off
		Blend One One//SrcAlpha OneMinusSrcAlpha 
		Cull Off
		
		Pass {  
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float2 texcoord : TEXCOORD0;
					float2 texcoord2 : TEXCOORD1;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					half2 texcoord : TEXCOORD0;
					half2 texcoord2 : TEXCOORD1;
					UNITY_FOG_COORDS(1)
				};

				sampler2D _MainTex;
				sampler2D _Offset;
				float4 _MainTex_ST;
				sampler2D _SpriteTex;
				sampler2D _PosTex;
				sampler2D _ShadowTex;
				float _Tile;
				float _ShadowTile;
				float _HueShift;
				float _ShadowSpeed;
				float _Saturation;
				float _Brightness;
				float _Speed;
				float _LineWidth;
				float4 _Color;
				
				float _UNPnts;
	         	float repeat;
				
				float3 hsv2rgb(float3 c)
				{
				    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
				    float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
				    return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
				}
				float4 place(float f,float b) {
//		            float t = max(0., min(.999, f)) * (_UNPnts-1);
//		            int n = t;//int(t);
					float4 off = tex2Dlod(_Offset,float4(f,b,0,0));
		            return  off;//tex2Dlod (_PosTex, float4(f,b,0,0))+off;
//		            float4 uData2 = tex2Dlod (_PosTex, float4(f+.0001,0,0,0));
//		            float4 uData1 = tex2D(_PosTex, float2(f,0.));
//		            float4 uData2 = tex2D(_PosTex, float2(f+.0001,0.));
//		            return uData1;//float4(lerp(uData1,uData2, t-float(n) ).xyz,1.);
		         }
	     
	         
				v2f vert (appdata_t v)
				{
					v2f o;
					o.vertex = v.vertex;//mul(UNITY_MATRIX_MVP, v.vertex);
//					o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
					float2 uv = v.texcoord;
					float2 uv2 = v.texcoord2;
					
					o.texcoord = v.texcoord;//TRANSFORM_TEX(v.texcoord, _MainTex);
					o.texcoord2 = v.texcoord2;
					
					float4 position = o.vertex;
	            	float4 p0 = mul(UNITY_MATRIX_MVP, place(position.x,position.y ));
//	            	float4 p1 = mul(UNITY_MATRIX_MVP, place(position.x + (1/_UNPnts)));
	            	o.vertex = p0+ float4(uv2.x,uv2.y*1.5,0,0)*_LineWidth;//p1 + normalize(float4(p1.y-p0.y, p0.x-p1.x, 0., 0.)) * position.y * _LineWidth;// tex2Dlod (_PosTex, float4(uv.x,uv.y,0,0)).a * _LineWidth;

					return o;
				}
				
				fixed4 frag (v2f i) : SV_Target
				{
//					fixed4 col2 = tex2D(_SpriteTex, i.texcoord2);
					fixed4 col = tex2D(_MainTex, i.texcoord);
					fixed4 pos = tex2D(_Offset, i.texcoord);
					fixed4 sprite =  tex2D(_SpriteTex, float2(((i.texcoord2.x)*_Tile*col.r)+_Time.z*_Speed,i.texcoord2.y));
//					fixed4 sprite2 = tex2D(_ShadowTex, float2((i.texcoord2.x*_ShadowTile*col.r)+col.g*_Time.z*_ShadowSpeed,i.texcoord2.y));
					float wave = min(1.0,sin(i.texcoord2.x*3.1415)*2.);
//					sprite*=sprite2;
					float4 hue = float4(hsv2rgb(float3(pos.r+col.b+(_HueShift*_Time.x),_Saturation*.5,_Brightness*.2)),1.0);
					sprite.a *= col.a;
//					UNITY_APPLY_FOG(i.fogCoord, col);
					return hue*tex2D(_SpriteTex,i.texcoord2)*tex2D(_SpriteTex,i.texcoord2).a*_Color*_Color.a*9.;//wave*sprite*hue*sprite.a;
				}
			ENDCG
		}
	}

}
