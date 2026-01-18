Shader "Gorilla/ContainerLiquid" {
	Properties {
		[HideInInspector] _LiquidPlanePosition ("_LiquidPlanePosition", Vector) = (0,0,0,0)
		[HideInInspector] _LiquidPlaneNormal ("_LiquidPlaneNormal", Vector) = (0,-1,0,0)
		[HideInInspector] _Color ("Color", Vector) = (0.3679245,0.2893169,0.2724724,1)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}