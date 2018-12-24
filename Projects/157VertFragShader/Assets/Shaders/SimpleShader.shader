Shader "BigTalkUnity/Simple Shader" {
	Properties{
		// 声明一个Color类型的属性
		_Color("Color Tint", Color) = (1.0, 1.0,1.0,1.0)
	}

	SubShader{
		Pass {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag

			// 在CG代码中，需要定义一个与属性名称和类型都匹配的变量
			fixed4 _Color;

			// 使用结构体定义输入
			struct a2v {
				// POSITION语义代表用模型空间的顶点坐标填充vertex变量
				float4 vertex : POSITION;
				// NORMAL语义代码用模型空间的法线方向填充normal变量
				float3 normal : NORMAL;
				// TEXCOORD0语义代表用模型的纹理坐标UV0填充texcoord变量
				float4 texcoord : TEXCOORD0;
			};

			// 使用结构体定义顶点着色器的输出
			struct v2f {
				// SV_POSITION语义代表pos中包含了顶点在裁剪空间中的位置
				float4 pos : SV_POSITION;
				// COLOR0语义可以用于存储顶点颜色信息
				fixed3 color : COLOR0;
			};

			v2f vert(a2v v) {
				// 声明输出结构
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				// v.normal的分量范围是[-1, 1]，通过下面代码可以映射到[0, 1]
				// 然后存到o.color中传递给片元着色器
				o.color = v.normal*0.5 + fixed3(0.5, 0.5, 0.5);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				fixed3 c = i.color;
				// 使用_Color属性来控制输出的颜色
				c *= _Color.rgb;
				return fixed4(c, 1.0);
			}

			ENDCG
		}
	}
}