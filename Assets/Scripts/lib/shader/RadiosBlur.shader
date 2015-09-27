Shader "Custom/RadiosBlur"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Center ("Center", Vector) = (0.5,0.5,0,0)
		_Times("Times",Float) = 10
		_Fix("Fix", Range(0,0.02)) = 0.01
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			zTest OFF
		
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

			sampler2D _MainTex;
			float2 _Center;
			float _Fix;
			float _Times;
			
			v2f vert (appdata v)
			{
				v2f o;
				
				o.uv = v.uv;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				return o;
			}
			
			fixed4 frag (v2f input) : SV_Target
			{
				fixed4 col = fixed4(0,0,0,0);
				
				for(int i = 0 ; i < _Times ; i++){
				
					float2 vv = lerp(input.uv,_Center,_Fix * i);
				
					col = col + tex2D(_MainTex,vv);
				}
				
				col = col / _Times;
				
				return col;
			}
			ENDCG
		}
	}
}
