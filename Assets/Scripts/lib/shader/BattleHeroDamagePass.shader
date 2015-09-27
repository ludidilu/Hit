Shader "Custom/BattleHeroDamagePass" {
	Properties {
	
		_MainTex("Main Texture", 2D) = ""{}
	}
	
	SubShader {
	
		Tags{"RenderType" = "Transparent"}
		
		Pass{
		
			Blend SrcAlpha OneMinusSrcAlpha  
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
			
			float4 positions[10];
			float4 fix[80];
			float index;
			float matrixIndex;
			float4x4 myMatrix[10];
			
			v2f vert(appdata v){
			
				v2f o;
				
				float4 vt;
				index = v.tangent.x;
				

				vt.x = v.uv.x + fix[index].y;
				vt.y = v.uv.y + fix[index].z;
				
				o.uv = vt.xy;
				//o.uv = v.uv;
				
				//坐标
				float4 targetPos;
				
				targetPos.x = positions[fix[index].w].x;
				targetPos.y = positions[fix[index].w].y;
				targetPos.z = positions[fix[index].w].z;
				targetPos.w = 1;
				
//				targetPos.x = myMatrix[index][0][3];
//				targetPos.y = myMatrix[index][1][3];
//				targetPos.z = myMatrix[index][2][3];
//				targetPos.w = 1;

				vt = v.vertex * positions[fix[index].w].w;
				vt.x = vt.x + fix[index].x;
				
				vt = mul(myMatrix[fix[index].w], vt);
               	
               	o.pos = mul(UNITY_MATRIX_P, (mul(UNITY_MATRIX_MV , targetPos) + float4(vt.x, vt.y, vt.z, 0.0)));
               	
               	
//				o.color.xyz = fix[index].z;
//				vt = v.vertex;
//				vt.x = vt.x + fix[index].y;
				//vt = positions[index].w;
//				o.pos = mul(UNITY_MATRIX_MVP, vt);
				
				
				return o;
			}
			
			half4 frag(v2f o):COLOR{
			
				return tex2D(_MainTex,o.uv);
//				return half4( o.color, 1 );
			}
			
			ENDCG
		
		}
	} 
	
	FallBack "Mobile/Diffuse"
}
