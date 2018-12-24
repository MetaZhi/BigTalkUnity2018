Shader "BigTalkUnity/HalfLambertShader"
{
	Properties
	{
		_Diffuse("Diffuse", Color) = (1,1,1,1)
	}

	SubShader
	{
		Pass
		{
			Tags {"LightMode" = "ForwardBase"}

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
			};

			fixed4 _Diffuse;

			v2f vert(a2v v)
			{
				v2f o;
				// 转换顶点坐标从物体坐标系到裁剪坐标系
				o.pos = UnityObjectToClipPos(v.vertex);

				// 将法线从物体坐标系转换到世界坐标系
				o.worldNormal = mul(v.normal, (float3x3)unity_WorldToObject);

				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// 获取环境光
				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz;

				// 将法线从物体坐标系转换到世界坐标系
				fixed3 worldNormal = normalize(i.worldNormal);

				// 获取光照的方向
				fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);

				// 计算半兰伯特
				fixed halfLambert = dot(worldNormal, worldLight) * 0.5 + 0.5;

				//计算漫反射
				fixed3 diffuse = _LightColor0.rgb * _Diffuse.rgb * halfLambert;

				fixed3 color = ambient + diffuse;

				return fixed4(color, 1.0);
			}
			ENDCG
		}
	}
}
