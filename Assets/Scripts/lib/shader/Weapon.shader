Shader "Custom/Weapon" {

	Properties {
	
		_MainTex("Main Texture", 2D) = ""{}
		_OutlineColor("Outline Color",Color) = (0,0,0,1)
	}

	SubShader {
	
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM

		#pragma surface surf Lambert noforwardadd vertex:vert finalcolor:myColor

		sampler2D _MainTex;
		half4 _OutlineColor;
		
		struct Input {
		
			float2 uv_MainTex;
			
			int isOutline;
		};

		void surf (Input IN, inout SurfaceOutput o) {
		
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
		}
		
		void vert(inout appdata_full v,out Input data){
		
			UNITY_INITIALIZE_OUTPUT(Input,data);

			if(v.texcoord1.y != 0){
			
				data.isOutline = 1;
				
			}else{
			
				data.isOutline = 0;
			}
		}
		
		void myColor (Input IN, SurfaceOutput o, inout fixed4 color)
	    {
	        if(IN.isOutline == 1){
	        
	        	color = _OutlineColor;
	        }
	    }
		
		ENDCG
	} 
	
	FallBack "Mobile/Diffuse"
}
