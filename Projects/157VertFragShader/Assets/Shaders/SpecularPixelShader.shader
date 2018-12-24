Shader "BigTalkUnity/SpecularPixelShader"
{
	Properties
	{
		_Diffuse("Diffuse", Color) = (1,1,1,1)
		_Specular("Specular", Color) = (1,1,1,1)
		_Gloss("Gloss", Range(8.0, 256)) = 20
	}
	SubShader
	{
		Pass
		{
			Tags {"LightMode"="ForwardBase"}
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "Lighting.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				fixed3 worldNormal : TEXCOORD0;
				fixed3 worldPos : TEXCOORD1;
			};

			fixed4 _Diffuse;
			fixed4 _Specular;
			float _Gloss;
			
			v2f vert (a2v v)
			{
				v2f o;
				// 转换顶点坐标从物体坐标系到裁剪坐标系
				o.pos = UnityObjectToClipPos(v.vertex);

				// 将法线从物体坐标系转换到世界坐标系
				o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);

				// 将顶点坐标从物体空间转到世界空间
				o.worldPos = mul(unity_WorldToObject, v.vertex).xyz;

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// 获取环境光
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

				// 将法线从物体坐标系转换到世界坐标系
				fixed3 worldNormal = normalize(mul(i.worldNormal, (float3x3)unity_WorldToObject));

				// 获取光照的方向
				fixed3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz);

				//计算漫反射
				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * saturate(dot(worldNormal, worldLightDir));

				//获取世界空间中的反射方向
				fixed3 reflectDir = normalize(reflect(-worldLightDir, worldNormal));

				// 获取世界空间中的视角方向
				fixed3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos);

				// 计算高光
				fixed3 specular = _LightColor0.rgb*_Specular.rgb*pow(saturate(dot(reflectDir, viewDir)), _Gloss);

				fixed3 color = ambient + diffuse + specular;

				return fixed4(color, 1.0);
			}
			ENDCG
		}
	}
}
