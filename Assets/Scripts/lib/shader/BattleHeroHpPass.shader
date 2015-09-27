Shader "Custom/BattleHeroHpPass" {
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
			
			float4 positions[20];
			float4 fix[20];
			float index;
			float4x4 myMatrix[20];
			
			v2f vert(appdata v){
			
				v2f o;
				
				index = v.tangent.x;
				float4 vt1;
				float4 vt2;

				vt1.x = v.tangent.y * fix[index].x;
				vt2.x = v.uv.x + vt1.x;
				vt2.y = v.uv.y;
				
				vt1.x = v.tangent.z * fix[index].z;
				vt2.x = vt2.x + vt1.x;
				
				o.uv = vt2.xy;
				
				//坐标
				float4 targetPos;
				
				//血条
				vt1.x = v.tangent.y * fix[index].y;
				vt2.x = v.vertex.x + vt1.x;
				//怒气
				vt1.x = v.tangent.z * fix[index].w;
				vt2.x = vt2.x + vt1.x;
				
				
				vt2.y = v.vertex.y;
				vt2.z = v.vertex.z;
				vt2.w = v.vertex.w;
				
				//vt2 = vt2 * positions[index].w;
				
				
				targetPos.x = positions[index].x;
				targetPos.y = positions[index].y;
				targetPos.z = positions[index].z;
				targetPos.w = 1;
				
//				targetPos.x = myMatrix[index][0][3];
//				targetPos.y = myMatrix[index][1][3];
//				targetPos.z = myMatrix[index][2][3];
//				targetPos.w = 1;
				
				vt2 = mul(myMatrix[index], vt2);
               	
               	o.pos = mul(UNITY_MATRIX_P, (mul(UNITY_MATRIX_MV , targetPos) + float4(vt2.x, vt2.y, vt2.z, 0.0)));
				//o.color.xyz = alpha;
				
				
				return o;
			}
			
			half4 frag(v2f o):COLOR{
			
				return tex2D(_MainTex,o.uv);
				//return half4( o.color, 1 );
			}
			
			ENDCG
		
		}
	} 
	
	FallBack "Mobile/Diffuse"
}
