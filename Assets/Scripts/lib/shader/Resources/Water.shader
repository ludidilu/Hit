Shader "Custom/Water" {
	Properties {
		_WaveScale ("Wave scale", Range (0.02,0.15)) = 0.063

		[NoScaleOffset] _BumpMap ("Normalmap ", 2D) = "bump" {}
		WaveSpeed ("Wave speed (map1 x,y; map2 x,y)", Vector) = (19,9,-16,-7)
		_HorizonColor ("Simple water horizon color", COLOR)  = ( .172, .463, .435, 1)
		[NoScaleOffset] _ReflectiveColor ("Reflective color (RGB) fresnel (A) ", 2D) = "" {}
		
	}


	// -----------------------------------------------------------
	// Fragment program cards


	Subshader {
		Tags { "WaterMode"="Refractive" "RenderType"="Opaque" }
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			uniform float4 _WaveScale4;
			uniform float4 _WaveOffset;


			struct appdata {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f {
				float4 pos : SV_POSITION;

				float2 bumpuv0 : TEXCOORD0;
				float2 bumpuv1 : TEXCOORD1;
				float3 viewDir : TEXCOORD2;

				UNITY_FOG_COORDS(4)
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				

				// scroll bump waves
				float4 temp;
				float4 wpos = mul (_Object2World, v.vertex);
				temp.xyzw = wpos.xzxz * _WaveScale4 + _WaveOffset;
				o.bumpuv0 = temp.xy;
				o.bumpuv1 = temp.wz;
				
				// object space view direction (will normalize per pixel)
				o.viewDir.xzy = WorldSpaceViewDir(v.vertex);

				UNITY_TRANSFER_FOG(o,o.pos);
				return o;
			}

			uniform float4 _HorizonColor;
			sampler2D _ReflectiveColor;
			sampler2D _BumpMap;

			half4 frag( v2f i ) : SV_Target
			{
				i.viewDir = normalize(i.viewDir);
				
				// combine two scrolling bumpmaps into one
				half3 bump1 = UnpackNormal(tex2D( _BumpMap, i.bumpuv0 )).rgb;
				half3 bump2 = UnpackNormal(tex2D( _BumpMap, i.bumpuv1 )).rgb;
				half3 bump = (bump1 + bump2) * 0.5;
				
				// fresnel factor
				half fresnelFac = dot( i.viewDir, bump );

				// final color is between refracted and reflected based on fresnel
				half4 color;

				half4 water = tex2D( _ReflectiveColor, float2(fresnelFac,fresnelFac) );
				color.rgb = lerp( water.rgb, _HorizonColor.rgb, water.a );
				color.a = _HorizonColor.a;


				UNITY_APPLY_FOG(i.fogCoord, color);
				return color;
			}
			ENDCG

		}
	}

}
