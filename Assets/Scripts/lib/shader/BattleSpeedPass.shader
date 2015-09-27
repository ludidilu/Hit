Shader "Custom/BattleSpeedPass" {
	Properties {
	
		_MainTex("Main Texture", 2D) = ""{}
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
				float4 tangent:TANGENT;
				float2 uv:TEXCOORD;
			};
			
			struct v2f{
			
				float4 pos:POSITION;
				float2 uv:TEXCOORD;
			};

			sampler2D _MainTex;
			
			float index;
			
			float4 fix[20];
			
			v2f vert(appdata v){
			
				v2f o;
				float4 vt;
				
				index = v.tangent.x;
				
				vt.x = v.uv.x + fix[index].x;
				vt.y = v.uv.y + fix[index].y;
				o.uv = vt.xy;
				
				
               	o.pos = v.vertex;
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
