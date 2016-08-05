Shader "Unlit/addEmTogether2"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Noise ("Texture", 2D) = "white" {}
		_Other ("Texture", 2D) = "white" {}
		_Other2 ("Texture", 2D) = "white" {}
		_Mix("Mix",Color) = (1,1,1,1)
		_Pos("pos",vector)=(1,1,1,1)
		_Amount("amount",float)=1
		_VideoLevels("videoLevels",Vector) = (1,1,1,1)

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
			sampler2D _Noise;
			sampler2D _Other;
			sampler2D _Other2;
			float4 _MainTex_ST;
			float4 _Pos;
			float4 _Mix;
			float _Amount;
			float4 _VideoLevels;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 f4(float a){
				return fixed4(a,a,a,a);
			}
			float map(float s, float a1, float a2, float b1, float b2)
			{
				return b1 + (s-a1)*(b2-b1)/(a2-a1);
			}
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 init = tex2D(_MainTex,i.uv);
				fixed4 ns = tex2D(_Noise,init.xy);
				float nsClamp = max(0.0,min(1.0,map(ns.r,0.0,1.0,_VideoLevels.x,_VideoLevels.y))); //min(1.,max(0,(((1.-ns.r)-.3)*10.)));
				fixed4 col = tex2D(_Other, i.uv);
				float d =  distance(col,f4(0.0));
				fixed4 angle = col;// -f4(0.0);
				// apply fog
//				UNITY_APPLY_FOG(i.fogCoord, col);	
				//float force = .0001;
//				fixed4 vel = (normalize(col-_Pos) * force)/(distance(col,_Pos));
				float dist = min(1.0,max(0.,(1-d)+1.));	
//				return col+_Amount*tex2D(_Other,i.uv)*cos(dist*3.14);	
				//if(d>1.1)
//				col+=(tex2D(_Other,i.uv))*_Amount;//*(1.0-max(0.0,d));//tex2D(_Other,i.uv)/d*d*d*d;
				return lerp(lerp(angle,col+_Amount*tex2D(_Other2,i.uv),dist),fixed4(init.rgb-.5,1.0),nsClamp.r);
			}
			ENDCG
		}
	}
}
