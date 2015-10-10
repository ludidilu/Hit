Shader "Custom/BattleHeroShadowPass" {
	Properties {
	
		_MainTex("Main Texture", 2D) = ""{}
	}
	
	SubShader {
	
		Tags{"RenderType" = "Transparent"}
		
		Pass{
		
			Blend SrcAlpha OneMinusSrcAlpha
			//ZTest Always//Less | Greater | LEqual | GEqual | Equal | NotEqual | Always
            ZWrite Off 
		
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
				float3 color : COLOR0;
			};


			sampler2D _MainTex;
			
			float4 stateInfo[40];
			float4x4 myMatrix[40];
			
			v2f vert(appdata v){
			
				v2f o;
				
				float index = v.tangent.x;
				float4 vt;
				
				o.uv = v.uv;
				
				
				vt = v.vertex;
				
				vt = vt * stateInfo[index].y;
				vt = mul(myMatrix[index], vt);
              	o.pos = mul(UNITY_MATRIX_MVP, vt);
              	o.color.xyz = stateInfo[index].x;
               	
				return o;
			}
			
			half4 frag(v2f o):COLOR{
			
				half4 h = tex2D(_MainTex,o.uv);
			
				h.w = h.w * o.color.x;
				return h;;
				
			}
			
			ENDCG
		
		}
	} 
	
	FallBack "Mobile/Diffuse"
}
