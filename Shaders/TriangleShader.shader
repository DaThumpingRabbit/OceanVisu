Shader "Unlit/TriangleShader"
{
    Properties
    {
		_StripSize("StripSize", float) = 0.2
    }
    SubShader
    {
        //Tags { "RenderType"="Transparent"}
        //Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        //Tags { "RenderType"="Opaque" "Queue"="Geometry-1" }
        Tags { "RenderType"="Opaque" "Queue"="Geometry-1" }
        LOD 300
		ZTest Always
        Cull Back ZWrite On Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma geometry geom

            #include "UnityCG.cginc"

			float _StripSize;

            struct InputData {
                float4 vertex : POSITION;
				fixed4 color : COLOR;
            };

            struct GeomInputData {
                float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
            };

            struct FragInputData {
                float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
            };
			
			GeomInputData vert (InputData v) {
				GeomInputData o;
                o.vertex = (v.vertex);
				o.color = v.color;
                return o;
            }

			[maxvertexcount(3)]
			void geom (triangle GeomInputData i[3], inout TriangleStream<FragInputData> outStream) {
				FragInputData o;
				o.color = i[0].color;
				o.vertex = UnityObjectToClipPos(i[0].vertex);
				outStream.Append(o);
				o.color = i[1].color;
				o.vertex = UnityObjectToClipPos(i[1].vertex);
				outStream.Append(o);
				o.color = i[2].color;
				o.vertex = UnityObjectToClipPos(i[2].vertex);
				outStream.Append(o);
			}

            fixed4 frag (FragInputData i) : SV_Target {
                return i.color;
            }
            ENDCG
        }
    }
}
