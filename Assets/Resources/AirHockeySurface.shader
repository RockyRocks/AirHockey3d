Shader "AirHockey/Surface"
{
	Properties
	{
		_BaseColor ("Base", Color) = (0.03, 0.16, 0.28, 1)
		_Accent ("Accent", Color) = (0.25, 0.95, 1, 1)
		_Smoothness ("Smoothness", Range(0, 1)) = 0.86
		_Metallic ("Metallic", Range(0, 1)) = 0.08
		_Emission ("Emission", Range(0, 2)) = 0.45
		_Stripe ("Stripe", Range(0, 1)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		fixed4 _BaseColor;
		fixed4 _Accent;
		half _Smoothness;
		half _Metallic;
		half _Emission;
		half _Stripe;

		struct Input
		{
			float3 worldPos;
			float3 viewDir;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float edge = saturate(1.0 - abs(IN.worldPos.z) * 0.22);
			float lane = saturate(1.0 - abs(IN.worldPos.z) * 6.0);
			float center = saturate(1.0 - abs(IN.worldPos.x) * 18.0) * saturate(1.0 - abs(IN.worldPos.z) * 0.18);
			float3 color = _BaseColor.rgb;
			color += _Accent.rgb * lane * 0.22 * _Stripe;
			color += _Accent.rgb * center * 0.65 * _Stripe;
			color = lerp(color, color * 1.15 + _Accent.rgb * 0.08, edge * 0.35);
			float fresnel = pow(1.0 - saturate(dot(normalize(IN.viewDir), float3(0, 1, 0))), 3.0);
			o.Albedo = color;
			o.Metallic = _Metallic;
			o.Smoothness = _Smoothness;
			o.Emission = _Accent.rgb * _Emission * (0.08 + lane * 0.25 + fresnel * 0.35);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
