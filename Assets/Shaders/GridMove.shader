Shader "Custom/GridMove" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_NearColor ("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Emission (RGB)", 2D) = "black" {}
		_Brightness("Brightness", Float) = 1.0
		_TimeScale("Time Scale", Float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		fixed4 _Color;
		fixed4 _NearColor;

		float _Brightness;
		float _TimeScale;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float2 offset = float2(0, _Time.y * _TimeScale);

			float f = max(IN.worldPos.z + 5, 0) / 30;

			float4 tint = _Color * f + _NearColor * (1.0 - f);

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex + offset) * tint * _Brightness;

			c += _NearColor * 0.1f * (1.0f - f);

			o.Emission = c.rgb;

			o.Albedo = fixed3(0, 0, 0);
			o.Metallic = 0.0;
			o.Smoothness = 0.0;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
