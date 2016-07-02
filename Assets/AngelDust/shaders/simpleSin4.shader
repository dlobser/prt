 Shader "Unlit/simpleSin4"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Feedback ("feedback", 2D) = "white" {}
		_Speed("speed",float)=1
		_Freq("freq",float)=1
		_Pos("pos",Vector)=(0,0,0,0)
		_Pos2("pos",Vector) = (0,0,0,0)
		_Repel1("repel",float)=1
		_Repel2("repel",float)=1
		_Force1("force",float) = 1
		_Force2("force",float) = 1
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
			#pragma target 3.0
			//#include "noiseSimplex.cginc"
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
			float _Repel1;
			float _Repel2;
			float _Force1;
			float _Force2;


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
				float4 col = tex2D(_MainTex, i.uv);
//				fixed4 col = tex2D(_MainTex, i.uv)*_Freq;
				float4 vel = tex2D(_Feedback, i.uv);
				// apply fog
				
				_Force1 += 1;
			
				_Force2 += 1;
	
				float d = max(distance(col.xyz, _Pos2.xyz)*10., 1.0);
				float4 delt = normalize(_Pos2 - col) * _Repel1;
				float4 superNoise = max(0,(_Repel1*-1))*lerp(float4(0.0,0.0,0.0,0.0),float4(sin(i.uv.x*14385+col.x*314358.3),sin(i.uv.y*98743957+col.y*13434.5),sin(i.uv.y*438972+col.z*294298.3),0.0),max(0.0,1.-d*.75));
				float4 gravity1 = ((delt) / (d*d*d)) + superNoise;// (vel*.98) + ((delt) / (d*d*d));

				d = max(distance(col.xyz,_Pos.xyz)*10.,1.0);
				delt = normalize(_Pos-col) * _Repel2;
				superNoise = max(0,(_Repel2*-1))*lerp(float4(0.0,0.0,0.0,0.0),float4(sin(i.uv.x*14385+col.x*314358.3),sin(i.uv.y * 98743957 + col.y*13434.5),sin(i.uv.y*438972+col.z*294298.3),0.0),max(0.0,1.-d*.75));
				float4 gravity2 = ((delt) / (d*d*d)) + superNoise;// (vel*.98) + ((delt) / (d*d*d));

				return vel*.98 + ((_Force1*gravity1+_Force2*gravity2)*.5);// vel*.98+ ((gravity1+gravity2)*.5);
			}
			ENDCG
		}
	}
}
