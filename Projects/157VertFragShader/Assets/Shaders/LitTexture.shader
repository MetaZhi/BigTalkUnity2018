Shader "Unity Shaders Book/Chapter 7/Single Texture" {
	Properties{
		_Color("Color Tint", Color) = (1, 1, 1, 1)
		_MainTex("Main Tex", 2D) = "white" {}
		_Specular("Specular", Color) = (1, 1, 1, 1)
		_Gloss("Gloss", Range(8.0, 256)) = 20
	}
		SubShader{
			Pass {
				Tags { "LightMode" = "ForwardBase" }

				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag

				#include "Lighting.cginc"

				fixed4 _Color;
				sampler2D _MainTex;
				float4 _MainTex_ST;
				fixed4 _Specular;
				float _Gloss;

				struct a2v {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 pos : SV_POSITION;
					float3 worldNormal : TEXCOORD0;
					float3 worldPos : TEXCOORD1;
					float2 uv : TEXCOORD2;
				};

				v2f vert(a2v v) {
					// 定义输出结构
					v2f o;
					// 将模型的顶点坐标从物体坐标系转到裁剪空间
					o.pos = UnityObjectToClipPos(v.vertex);
					// 计算世界空间的法线
					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					// 计算世界空间的位置
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
					// 计算每个顶点的UV
					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target {
					// 将法线单位化
					fixed3 worldNormal = normalize(i.worldNormal);
					// 使用内置方法，计算光照的方向
					fixed3 worldLightDir = normalize(UnityWorldSpaceLightDir(i.worldPos));
					// 基础颜色，从纹理和MainColor中计算
					fixed3 albedo = tex2D(_MainTex, i.uv).rgb * _Color.rgb;
					// 获取环境光
					fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz * albedo;
					//计算漫反射
					fixed3 diffuse = _LightColor0.rgb * albedo * max(0, dot(worldNormal, worldLightDir));

					fixed3 viewDir = normalize(UnityWorldSpaceViewDir(i.worldPos));
					fixed3 halfDir = normalize(worldLightDir + viewDir);
					fixed3 specular = _LightColor0.rgb * _Specular.rgb * pow(max(0, dot(worldNormal, halfDir)), _Gloss);

					return fixed4(ambient + diffuse + specular, 1.0);
				}

				ENDCG
			}
		}
			FallBack "Specular"
}