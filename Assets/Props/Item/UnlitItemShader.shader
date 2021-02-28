Shader "Custom/UnlitItemShader"
{
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color("Line Color", Color) = (1,1,1,1)
		_CellColor("Cell Color", Color) = (0,0,0,0)
		[PerRendererData] _MainTex("Albedo (RGB)", 2D) = "white" {}
		[IntRange] _GridSize("Grid Size", Range(1,100)) = 10
		_LineSize("Line Size", Range(0,1)) = 0.15
	}

		SubShader{
			Tags { "Queue" = "AlphaTest" "RenderType" = "TransparentCutout" }
			LOD 200

		CGPROGRAM

		#pragma surface surf NoLighting  noambient

		sampler2D _MainTex;

		float4 _Color;
		float4 _CellColor;

		float _GridSize;
		float _LineSize;

		struct Input {
			half2 uv_MainTex;
		};

			fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
			{
				fixed4 c;
				c.rgb = s.Albedo;
				c.a = s.Alpha;
				return c;
			}

		void surf(Input IN, inout SurfaceOutput o)
		{
			//o.Albedo = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			float2 uv = IN.uv_MainTex;
			fixed4 c = float4(0.0, 0.0, 0.0, 0.0);
			float brightness = 1.;
			float gsize = floor(_GridSize);
			gsize += _LineSize;

			float2 id;

			id.x = floor(uv.x / (1.0 / gsize));
			id.y = floor(uv.y / (1.0 / gsize));

			float4 color = _CellColor;
			brightness = _CellColor.w;

			if (frac(uv.x * gsize) <= _LineSize || frac(uv.y * gsize) <= _LineSize)
			{
				brightness = _Color.w;
				color = _Color;
			}
			//Clip transparent spots using alpha cutout
			if (brightness == 0.0) {
				clip(c.a - 1.0);
			}
			o.Albedo = float4(color.x * brightness, color.y * brightness, color.z * brightness, brightness);
			o.Alpha = 0.0;
		}
		ENDCG
		}

			Fallback "Mobile/VertexLit"
}