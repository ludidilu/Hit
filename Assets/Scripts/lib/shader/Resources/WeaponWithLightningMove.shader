Shader "Custom/WeaponWithLightningMove" {

	Properties {
	
		_MainTex("Main Texture", 2D) = ""{}
		_OutlineColor("Outline Color",Color) = (0,0,0,1)
		_LightningTex("Lightning Texture", 2D) = ""{}
		_StrongFix("Strong Fix", Float) = 1
		
		_UFix("UFix", Float) = 0
	}

	SubShader {
	
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM

		#pragma surface surf Lambert noforwardadd vertex:vert finalcolor:myColor

		sampler2D _MainTex;
		half4 _OutlineColor;
		
		sampler2D _LightningTex;
		float _StrongFix;
		
		float _UScale = 1;
		float _VScale = 1;
		
		float _UFix = 0;
		float _VFix = 0;
		
		struct Input {
		
			float2 uv_MainTex;
			
			bool isOutline;
			
			float2 lightningUV;
		};

		void surf (Input IN, inout SurfaceOutput o) {
		
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			
			o.Albedo = c.rgb;
			
			o.Alpha = c.a;
		}
		
		void vert(inout appdata_full v,out Input data){
		
			UNITY_INITIALIZE_OUTPUT(Input,data);

			if(v.texcoord1.y != 0){
			
				data.isOutline = true;
				
			}else{
			
				data.isOutline = false;
				
				data.lightningUV = float2(v.texcoord.x * _UScale + _UFix,v.texcoord.y * _VScale + _VFix);
			}
		}
		
		void myColor (Input IN, SurfaceOutput o, inout fixed4 color)
	    {
	        if(IN.isOutline){
	        
	        	color = _OutlineColor;
	        	
	        }else{
	        
	        	fixed4 d = tex2D(_LightningTex,IN.lightningUV) * _StrongFix;
	        	
	        	color = color + o.Alpha * d;
	        }
	    }
		
		ENDCG
	} 
	
	FallBack "Mobile/Diffuse"
}
