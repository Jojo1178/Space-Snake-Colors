// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OrbShader"
{
	/*Properties{
	_Color("Tint", Color) = (0, 0, 0, 1)
	_MainTex("Texture", 2D) = "white" {}
	_Emission("Emission", Color) = (0,0,0,1)
	}
		SubShader{
			Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}

			CGPROGRAM

			#pragma surface surf Standard fullforwardshadows

			sampler2D _MainTex;
			fixed4 _Color;
			half3 _Emission;

			struct Input {
				float2 uv_MainTex;
			};

			void surf(Input i, inout SurfaceOutputStandard o) {
				fixed4 col = tex2D(_MainTex, i.uv_MainTex);
				col *= _Color;
				o.Albedo = col.rgb;
				o.Emission = _Emission;
			}
			ENDCG
	}*/

	Properties{
_Color("Main Color", Color) = (1,1,1,1)
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 2.0
					#pragma multi_compile_fog

					#include "UnityCG.cginc"

					struct appdata_t {
						float4 vertex : POSITION;
						UNITY_VERTEX_INPUT_INSTANCE_ID
					};

					struct v2f {
						float4 vertex : SV_POSITION;
						UNITY_FOG_COORDS(0)
						UNITY_VERTEX_OUTPUT_STEREO
					};

					fixed4 _Color;

					v2f vert(appdata_t v)
					{
						v2f o;
						UNITY_SETUP_INSTANCE_ID(v);
						UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
						o.vertex = UnityObjectToClipPos(v.vertex);
						UNITY_TRANSFER_FOG(o,o.vertex);
						return o;
					}

					fixed4 frag(v2f i) : COLOR
					{
						fixed4 col = _Color;
						UNITY_APPLY_FOG(i.fogCoord, col);
						UNITY_OPAQUE_ALPHA(col.a);
						return col;
					}
				ENDCG
			}

	}
}