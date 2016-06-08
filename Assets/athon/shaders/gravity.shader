Shader "Custom/gravity"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Velocity ("velocity", 2D) = "white" {}
		_Speed("speed",float)=1
		_Which("which",float)=1
		_Freq("freq",float)=1
		_Pos("pos",Vector)=(0,0,0,0)
		_Speeds("speeds",vector)=(0,0,0,0)
		_SinAdd("SinAdd",float)=0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			#include "noiseSimplex.cginc"
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _Velocity;
			float4 _MainTex_ST;
			float _Speed;
			float4 _Pos;
			float _Freq;
			float4 _Speeds;
			float _Which;
			float _SinAdd;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
		
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv)*_Freq;
				fixed4 vel = tex2D(_Velocity, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				
				
				float d = distance(col,_Pos);
				float t = _Time.y*_Speed;//*(1.+d*.1);
//				float sn = snoise(col+_Time.y*_Speed);		
	
//				return float4(
//				sin(.8*snoise(float2(col.x,col.y+_Time.x*_Speeds.x)+t)),
//				sin(snoise(float2(col.z,col.x+_Time.x*_Speeds.y)+t)),
//				sin(1.1*snoise(float2(col.y,col.z+_Time.x*_Speeds.z)+t)),1.0)*.005;
//				
//				return float4(curl(col.xyz*.3,_Time.x*_Speed),1.0);
				fixed4 grav = vel*.01 + (normalize(_Pos-col))/d;
				return col;
//				return lerp(float4(
////				sin(col.x*_Freq*_Speeds.x),
////				sin(_Time.z*_Speed*10.+col.y*_Freq*_Speeds.y),
////				sin(col.z*_Freq*_Speeds.z),0.0),
//				sin(col.x*_Freq*_Speeds.x),
//				sin(_Time.z*_Speed*10.+col.y*_Freq*_Speeds.y),
//				sin(col.z*_Freq*_Speeds.z),0.0),
//				lerp(float4(
//				sin(.8*snoise(float2(col.x,col.y+_Time.x)+t)),
//				sin(snoise(float2(col.z,col.x+_Time.x)+t)),
//				sin(1.1*snoise(float2(col.y,col.z+_Time.x)+t)),1.0),float4(curl(col.xyz,_Time.x*_Speed),1.0),_Which),_SinAdd);
			}
			ENDCG
		}
	}
}

