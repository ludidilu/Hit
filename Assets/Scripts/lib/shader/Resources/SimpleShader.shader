Shader "Custom/SimpleShader" {

	Properties {
	
		_MainTex("Main Texture", 2D) = ""{}
	}
	
	SubShader {
	
		Tags{"RenderType" = "Opaque"}
		
		Pass{
		
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct appdata{
			
				float4 vertex:POSITION;
				float2 uv:TEXCOORD;
			};
			
			struct v2f{
			
				float4 pos:POSITION;
				float2 uv:TEXCOORD;
			};

			sampler2D _MainTex;
			
			v2f vert(appdata v){
			
				v2f o;
			
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				
				o.uv = v.uv;
				
				return o;
			}
			
			half4 frag(v2f o):COLOR{
			
				return tex2D(_MainTex,o.uv);
			}
			
			ENDCG
		
		}
	} 
	
	FallBack "Mobile/Diffuse"
}
