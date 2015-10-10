Shader "Custom/SimpleColorShader" {

	Properties {
	
		_Color("Outline Color",Color) = (0,0,0,0)
	}
	
	SubShader {
	
		Tags{"RenderType" = "Transparent"}
		
		Pass{
		
			Blend SrcAlpha OneMinusSrcAlpha
		
			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			
			struct appdata{
			
				float4 vertex:POSITION;
			};
			
			struct v2f{
			
				float4 pos:POSITION;
			};

			half4 _Color;
			
			v2f vert(appdata v){
			
				v2f o;
				
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				
				return o;
			}
			
			half4 frag(v2f o):COLOR{
			
				return _Color;
			}
			
			ENDCG
		}
	} 
	
	FallBack "Mobile/Diffuse"
}
