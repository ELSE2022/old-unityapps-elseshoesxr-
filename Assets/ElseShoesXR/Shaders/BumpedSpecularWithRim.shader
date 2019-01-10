Shader "Bumped Specular with rim" {

	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess("Shininess", Range(0.03, 1)) = 0.078125
		_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
	_BumpMap("Normalmap", 2D) = "bump" {}
	_RimColor("Rim Color(RGB) Power(A)", Color) = (1,1,1,1)
	}

		SubShader{
		Tags{ "RenderType" = "Opaque" }
		LOD 400

		CGPROGRAM

#pragma surface surf BlinnPhong

		struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
		float3 viewDir;
	};

	sampler2D _MainTex;
	sampler2D _BumpMap;
	float4 _RimColor;
	fixed4 _Color;
	half _Shininess;

	void surf(Input IN, inout SurfaceOutput o) {
		half4 c = tex2D(_MainTex, IN.uv_MainTex);
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		half rim = 1.0 - saturate(dot(normalize(IN.viewDir), o.Normal));
		o.Albedo = c.rgb * _Color.rgb;
		o.Albedo += _RimColor.rgb * pow(rim, _RimColor.a) * c.a;
		o.Gloss = c.a;
		o.Alpha = c.a * _Color.a;
		o.Specular = _Shininess;
	}
	ENDCG
	}
		FallBack "Diffuse"
}