Shader "Custom/OutlineShader"
{
    Properties
    {
        _Color ("Color", Color) = (1, 1, 1, 1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _OutlineColor ("OutlineColor", Color) = (0, 0, 0, 1)
    }
	
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _ScreenTexture;
		
		fixed4 _Color;
		fixed4 _OutlineColor;

		struct Input
		{
			float2 uv_MainTex;
			float3 viewDir;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			c *= clamp(dot(IN.viewDir, o.Normal) + 0.25, 0.75, 1.25);

			float d = max(abs(IN.uv_MainTex.x - 0.5), abs(IN.uv_MainTex.y - 0.5));
			bool edge = d > (0.5 * 0.98);

			o.Albedo = c.rgb;
			o.Emission = edge ? _OutlineColor : c.rgb;
		}

		ENDCG
    }
	
    FallBack "Diffuse"
}
