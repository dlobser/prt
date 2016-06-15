Shader "Custom/DifferenceShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BufferTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
//       Tags {"Queue"="Transparent" "RenderType"="Transparent"}
//       Blend SrcAlpha OneMinusSrcAlpha
		ZWrite On

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _BufferTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 cola = tex2D(_MainTex, i.uv);
				fixed4 colb = tex2D(_BufferTex, i.uv);
				fixed4 colc = cola/colb;
				float a = max(0.0,colc.x-2.)*2.;
				return float4(a,a,a,a);
			}
			ENDCG
		}
	}
}
