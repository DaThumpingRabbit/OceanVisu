Shader "Unlit/TriangleShaderForClipping"
{
    Properties
    {
		_StripSize("StripSize", float) = 0.2
        _Plane("_Plane", Vector) = (1, 1, 0, 0)
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
				GeomInputData o ;
                o.vertex = v.vertex ;
				o.color = v.color ;
                return o ;
            }

    	    float4 _Plane;

			[maxvertexcount (3)]
			void geom (triangle GeomInputData i[3], inout TriangleStream<FragInputData> outStream) {
				FragInputData o;
                //float3 p = UnityObjectToViewPos (_Plane) ;
                float3 p = _Plane.xyz ;
                float d0 = dot (UnityObjectToViewPos (i[0].vertex), p) + _Plane.w ;
                float d1 = dot (UnityObjectToViewPos (i[1].vertex), p) + _Plane.w ;
                float d2 = dot (UnityObjectToViewPos (i[2].vertex), p) + _Plane.w ;
                // float d0 = dot (UnityObjectToClipPos (i[0].vertex), p) + _Plane.w ;
                // float d1 = dot (UnityObjectToClipPos (i[1].vertex), p) + _Plane.w ;
                // float d2 = dot (UnityObjectToClipPos (i[2].vertex), p) + _Plane.w ;
                //float distance = (d0 + d1 + d2) / 3 ;
			    //distance = distance + _Plane.w ;
			    //discard surface above plane
			    //clip (-distance) ;
                if (! ((d0 < 0) || (d1 < 0) || (d2 < 0))) {
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
			}

            fixed4 frag (FragInputData i) : SV_Target {
                return i.color;
            }

            ENDCG
        }
    }
    FallBack "Standard" //fallback adds a shadow pass so we get shadows on other objects

}
