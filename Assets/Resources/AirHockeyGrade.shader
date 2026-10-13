Shader "Hidden/AirHockey/Grade"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Saturation ("Saturation", Float) = 1.22
		_Contrast ("Contrast", Float) = 1.08
		_Vignette ("Vignette", Float) = 0.28
		_Lift ("Lift", Color) = (0.02, 0.04, 0.07, 0)
	}
	SubShader
	{
		Pass
		{
			ZTest Always Cull Off ZWrite Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Saturation;
			float _Contrast;
			float _Vignette;
			fixed4 _Lift;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata_img v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				fixed3 color = tex2D(_MainTex, i.uv).rgb;
				float luma = dot(color, float3(0.2126, 0.7152, 0.0722));
				color = lerp(luma.xxx, color, _Saturation);
				color = (color - 0.5) * _Contrast + 0.5;
				color += _Lift.rgb * (1.0 - luma);
				float2 fromCenter = (i.uv - 0.5) * float2(1.15, 1.35);
				float vignette = saturate(length(fromCenter));
				color *= lerp(1.0, 0.78, vignette * _Vignette);
				return fixed4(saturate(color), 1);
			}
			ENDCG
		}
	}
}
