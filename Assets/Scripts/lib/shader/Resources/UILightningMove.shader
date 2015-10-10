Shader "Custom/UILightningMove" {

	Properties {

		_LTex("L Texture", 2D) = ""{}
	}
	
	SubShader {
	
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Pass{
		
			Lighting Off
			ZWrite Off
			ZTest [unity_GUIZTestMode]
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#include "UnityUI.cginc"
			
			struct appdata{
			
				float4 vertex:POSITION;
				float2 uv:TEXCOORD;
			};
			
			struct v2f{
			
				float4 pos:POSITION;
				float2 uv:TEXCOORD;
				
				float2 ouv:TEXCOORD2;
				
				float4 worldPosition : TEXCOORD1;
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
			
			bool _UseClipRect;
			float4 _ClipRect;
			
			v2f vert(appdata v){
			
				v2f o;
				
				o.worldPosition = v.vertex;
			
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
				
				if (_UseClipRect){
				
					c *= UnityGet2DClipping(o.worldPosition.xy, _ClipRect);
				}
			
				return c;
			}
			
			ENDCG
		
		}
	} 
	
	FallBack "UI/Default"
}

