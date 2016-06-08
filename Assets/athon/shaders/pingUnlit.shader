Shader "Unlit/pingUnlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Speed("speed",float)=1
		_Which("which",float)=1
		_Freq("freq",float)=1
		_Pos("pos",Vector)=(0,0,0,0)
		_Speeds("speeds",vector)=(0,0,0,0)
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
			float4 _MainTex_ST;
			float _Speed;
			float4 _Pos;
			float _Freq;
			float4 _Speeds;
			float _Which;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
//			float3 curl(float3 p)
//			{
//
//			    float e = 1e-4f;
//			    float3 dx = float3(e, 0, 0);
//			    float3 dy = float3(0, e, 0);
//			    float3 dz = float3(0, 0, e);
//			    float x = snoise(p + dy).z - snoise(p - dy).z
//			            - snoise(p + dz).y + snoise(p - dz).y;
//			    float y = snoise(p + dz).x - snoise(p - dz).x
//			            - snoise(p + dx).z + snoise(p - dx).z;
//			    float z = snoise(p + dx).y - snoise(p - dx).y
//			            - snoise(p + dy).x + snoise(p - dy).x;
//			    return float3(x, y, z) / (2*e);
//			}

			float3 curl(float3 pos, float time){
			  //Assuming the range of pos is -1 to 1. 
			  //That accounts for the *0.5+0.5 to transform it to the range 0-1
			  float pRangeScale = .5;
			  float pDomainScale = 4.0;
			  
			  float origin = pRangeScale * abs(
			      snoise(float4(pDomainScale*pos.x*0.5+0.5,pDomainScale*pos.y*0.5+0.5,pDomainScale*pos.z*0.5+0.5,time))
			      );

							//Calculate the gradient to orient the element
			  float dy = pRangeScale * abs(
			      snoise(float4(pDomainScale*pos.x*0.5+0.5,pDomainScale*(pos.y + 0.001)*0.5+0.5,pDomainScale*pos.z*0.5+0.5,time))
			      );
			  float dx = pRangeScale * abs(
			      snoise(float4(pDomainScale*(pos.x+ 0.001)*0.5+0.5,pDomainScale*pos.y*0.5+0.5,pDomainScale*pos.z*0.5+0.5,time))
			      );
			  float dz = pRangeScale * abs(
			      snoise(float4(pDomainScale*pos.x*0.5+0.5,pDomainScale*pos.y*0.5+0.5,pDomainScale*(pos.z + 0.001)*0.5+0.5,time))
			      );
			  
			  float3 baseGradient = float3(dx - origin, dy - origin, dz-origin)*1000.;
//			  baseGradient.scale(1000.0);
//			  var gradient = baseGradient.clone();
			  
			  
			  float3 lateralToGradient = float3(-baseGradient.y, -baseGradient.z, baseGradient.x);
			  float3 normal =  normalize(cross(baseGradient,lateralToGradient));// lateralToGradient.cross(gradient);
//			  normal.normalize();
			  float3 velocity = cross(normal,baseGradient);//normal.cross(gradient);
			  
			  return velocity*1.0;
			};



			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv)*_Freq;
				
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				float t = _Time.y*_Speed;
				
//				float d = distance(col,_Pos);
//				float sn = snoise(col+_Time.y*_Speed);		
	
//				return float4(
//				sin(.8*snoise(float2(col.x,col.y+_Time.x*_Speeds.x)+t)),
//				sin(snoise(float2(col.z,col.x+_Time.x*_Speeds.y)+t)),
//				sin(1.1*snoise(float2(col.y,col.z+_Time.x*_Speeds.z)+t)),1.0)*.005;
//				
//				return float4(curl(col.xyz*.3,_Time.x*_Speed),1.0);

				return lerp(float4(
				sin(.8*snoise(float2(col.x,col.y+_Time.x*_Speeds.x)+t)),
				sin(snoise(float2(col.z,col.x+_Time.x*_Speeds.y)+t)),
				sin(1.1*snoise(float2(col.y,col.z+_Time.x*_Speeds.z)+t)),1.0)*.005,float4(curl(col.xyz,_Time.x*_Speed),1.0),_Which);
			}
			ENDCG
		}
	}
}
