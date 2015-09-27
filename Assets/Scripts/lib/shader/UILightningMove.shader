Shader "Custom/UILightningMove" {

	Properties {
	
	    _Color ("Tint", Color) = (1,1,1,1)
	    _StencilComp ("Stencil Comparison", Float) = 8
	    _Stencil ("Stencil ID", Float) = 0
	    _StencilOp ("Stencil Operation", Float) = 0
	    _StencilWriteMask ("Stencil Write Mask", Float) = 255
	    _StencilReadMask ("Stencil Read Mask", Float) = 255
	    _ColorMask ("Color Mask", Float) = 15
	
		_LTex("L Texture", 2D) = ""{}
		_StrongFix("Strong Fix", Float) = 1
//		_UFix("UFix",Float) = 0
	}
	
	SubShader {
	
		Tags{"QUEUE"="Transparent" "RenderType" = "Transparent" "IGNOREPROJECTOR"="true" "PreviewType"="Plane" "CanUseSpriteAtlas"="true"}
		
		Pass{
		
			ZTest Off
			ZWrite Off
			Cull Off
			
			Stencil {
			    Ref [_Stencil]
			    ReadMask [_StencilReadMask]
			    WriteMask [_StencilWriteMask]
			    Comp [_StencilComp]
			    Pass [_StencilOp]
			}
		
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask [_ColorMask]
		
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
				
				float2 ouv:TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _LTex;
			
			float _StrongFix;
			
			float _UFix = 0;
			float _VFix = 0;
			
			float _UOffset;
			float _VOffset;
			
			float _UScale;
			float _VScale;
			
			v2f vert(appdata v){
			
				v2f o;
			
				o.pos = mul(UNITY_MATRIX_MVP,v.vertex);
				
				o.uv = v.uv;
				
				o.ouv.x = (v.uv.x - _UOffset) * _UScale + _UFix;
				o.ouv.y = (v.uv.y - _VOffset) * _VScale + _VFix;
				
				return o;
			}
			
			half4 frag(v2f o):COLOR{
			
				half4 c = tex2D(_MainTex,o.uv);
				
				half4 d = tex2D(_LTex,o.ouv) * _StrongFix;
				
				c.x = c.x + d.w;
				c.y = c.y + d.w;
				c.z = c.z + d.w;
			
				return c;
			}
			
			ENDCG
		
		}
	} 
	
	FallBack "UI/Default"
}

