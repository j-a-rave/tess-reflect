Shader "Custom/TessReflect" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Cube ("Reflection Cubemap", Cube) = "" { TexGen CubeReflect }
		_Displacement ("Displacement", Range(-1.0, 1.0)) = 0
		_Tess ("Tesselation", Range(1,32)) = 4
		_DispTex ("Displacement Texture", 2D) = "gray" {}
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 300
		
		CGPROGRAM
		
		#pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp tessellate:tessFixed nolightmap
		#pragma target 5.0
		
		struct appdata{
			float4 vertex: POSITION;
			float4 tangent: TANGENT;
			float3 normal: NORMAL;
			float2 texcoord: TEXCOORD0;
		};

		float _Tess;

		float4 tessFixed()
		{
			return _Tess;
		}
		
		sampler2D _DispTex;
		float _Displacement;
		samplerCUBE _Cube;
		fixed4 _Color;
		
		void disp(inout appdata v){
			float d = tex2Dlod(_DispTex, float4(v.texcoord.xy, 0, 0)).r * _Displacement;
			v.vertex.xyz += v.normal * d;
		}

		struct Input 
		{
			float2 uv_MainTex;
			float3 worldRefl;
		};
		
		sampler2D _MainTex;
		sampler2D _NormalMap;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c =  _Color;
			o.Albedo = c.rgb;
			fixed4 reflcol = texCUBE (_Cube, IN.worldRefl);
			o.Specular = fixed4(1.0,1.0,1.0,1.0);
			o.Albedo = reflcol.rgb;
			o.Alpha = reflcol.a;
		}
		
		ENDCG
	} 
	FallBack "Diffuse"
}
