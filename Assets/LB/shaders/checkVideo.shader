Shader "Unlit/checkVideo"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_VideoLevels("videoLevels",Vector) = (1,1,1)
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
			float4 _MainTex_ST;
			float4 _VideoLevels;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			float map(float s, float a1, float a2, float b1, float b2)
			{
				return b1 + (s-a1)*(b2-b1)/(a2-a1);
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				float nsClamp = max(0.0,min(1.0,map(col.r,0.0,1.0,_VideoLevels.x,_VideoLevels.y))); //min(1.,max(0,(((1.-ns.r)-.3)*10.)));

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return fixed4(nsClamp,nsClamp,nsClamp,1.0);
			}
			ENDCG
		}
	}
}
