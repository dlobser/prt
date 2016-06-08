Shader "Unlit/addEmTogether"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Other ("Texture", 2D) = "white" {}
		_Pos("pos",vector)=(1,1,1,1)
		_Amount("amount",float)=1

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
			sampler2D _Other;
			float4 _MainTex_ST;
			float4 _Pos;
			float _Amount;
			
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
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float d = distance(col,f4(0.0));
				fixed4 angle = col-f4(0.0);
				// apply fog
//				UNITY_APPLY_FOG(i.fogCoord, col);	
				float force = .0001;
//				fixed4 vel = (normalize(col-_Pos) * force)/(distance(col,_Pos));
				float dist = min(1.0,max(0.,(1-d)+1.));	
//				return col+_Amount*tex2D(_Other,i.uv)*cos(dist*3.14);	
				return lerp(angle,col+_Amount*tex2D(_Other,i.uv),dist);
			}
			ENDCG
		}
	}
}
