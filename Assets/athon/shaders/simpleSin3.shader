 Shader "Unlit/simpleSin3"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Feedback ("feedback", 2D) = "white" {}
		_Speed("speed",float)=1
		_Which("which",float)=1
		_Gravity("gravity",float)=1
		_Freq("freq",float)=1
		_Pos("pos",Vector)=(0,0,0,0)
		_Pos2("pos",Vector) = (0,0,0,0)
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
			sampler2D _Feedback;
			float4 _MainTex_ST;
			float _Speed;
			float4 _Pos;
			float4 _Pos2;
			float _Freq;
			float4 _Speeds;
			float _Which;
			float _SinAdd;
			float _Gravity;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			float3 curl(float3 pos, float time){
			  //Assuming the range of pos is -1 to 1. 
			  //That accounts for the *0.5+0.5 to transform it to the range 0-1
			  float pRangeScale = .05;
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
				fixed4 col2 = tex2D(_MainTex, i.uv);
				fixed4 col = tex2D(_MainTex, i.uv)*_Freq;
				fixed4 vel = tex2D(_Feedback, i.uv);
				// apply fog

				float d = max(distance(col2.xyz, _Pos2.xyz)*10., 1.0);
				fixed4 delt = normalize(_Pos2 - col2);
				float4 gravity = ((delt) / (d*d*d));// (vel*.98) + ((delt) / (d*d*d));
				
				d = max(distance(col2.xyz,_Pos.xyz)*10.,1.0);
				delt = normalize(_Pos-col2);
				float4 cur = pow(10.*(float4(curl(col.xyz/d,_Time.y*_Speed),1.0)/(d*d*d)),1.0);
				float4 superNoise = lerp(float4(0.0,0.0,0.0,0.0),float4(sin(i.uv.x*14385+col.x*314358.3),sin(col.y*13434.5),sin(i.uv.y*438972+col.z*294298.3),0.0),max(0.0,1.-d*.5));

				return ( (vel*.98+ gravity + lerp(float4(0.0,0.0,0.0,0.0),cur,max(0.0,1.-d*.01))+ max(0.0,(((delt)/(d*d)))-.02) + superNoise));
			}
			ENDCG
		}
	}
}
