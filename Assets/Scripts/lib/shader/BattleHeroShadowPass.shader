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
			};


			sampler2D _MainTex;
			
			float4 positions[40];
			float index;
			float4x4 myMatrix[40];
			
			v2f vert(appdata v){
			
				v2f o;
				
				index = v.tangent.x;
				float4 vt1;
				float4 vt;
				o.uv = v.uv;
				
				//坐标
				float4 targetPos;
				
				vt = v.vertex;
				
				vt = vt * positions[index].w;
				vt = mul(myMatrix[index], vt);
				
				
				targetPos.x = positions[index].x;
				targetPos.y = positions[index].y;
				targetPos.z = positions[index].z;
				targetPos.w = 1;
				
               
              	//o.pos = mul(UNITY_MATRIX_P, (mul(UNITY_MATRIX_MV , targetPos) + float4(vt.x, vt.y, vt.z, 0.0)));
              	o.pos = mul(UNITY_MATRIX_MVP, vt);
               	
               	

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
