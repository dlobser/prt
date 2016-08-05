Shader "Custom/unlitTransHalfNoiseLB"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Noise", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Speed ("speed", Float) = 0
				_Tile ("Tile", Float) = 0

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "Queue"="Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
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
				float2 uv2 : TEXCOORD1;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
//				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _NoiseTex;
			float4 _NoiseTex_ST;
			float4 _Color;
			float _Speed;
						float _Tile;

			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv, _NoiseTex);
//				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 off = tex2D(_MainTex, i.uv);
				fixed4 noisy = tex2D(_NoiseTex,off.xy);// float2(_Tile*off.x+_Time.x*_Speed,_Tile*off.y));
				fixed4 col = off;//fixed4(pow(off.x,.4554),pow(off.y,.4554),pow(off.z,.4554),off.w);
				float noiseMix = lerp(_Color.r,_Color.g,min(1.,max(0,(((1.-noisy.r)-.5)*10.))));
				float alph = min(1.0,noiseMix)*_Color.a;
				float cAlph = (cos(alph*6.28)*.3)+.7;
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);				
				return fixed4(float2(col.rg-.5),alph,1.0);
			}
			ENDCG
		}
	}
}
