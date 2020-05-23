Shader "Unlit/PointCloud" {
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f {
                float4 center: TEXCOORD1;
                fixed4 col: COLOR;
                float size: PSIZE;
                float dist: TEXCOORD2;
            };

            StructuredBuffer<float3> colBuffer;
            StructuredBuffer<float3> posBuffer;
            float _Size;
            float _Radius;

            v2f vert (uint id : SV_VertexID, out float4 vpos : SV_POSITION) {
                v2f o;
                float4 pos = float4(posBuffer[id], 1);
                float dist = length(_WorldSpaceCameraPos - pos);
                pos.y += sin(dist) * 0.2;
                o.dist = dist;
                o.size = _Size / dist;
                vpos = UnityObjectToClipPos(pos);
                o.center = ComputeScreenPos(vpos);
                o.col = fixed4(colBuffer[id] / 255, 1);
                return o;
            }

            fixed4 frag (v2f i, UNITY_VPOS_TYPE vpos : VPOS) : SV_Target {
                i.center.xy /= i.center.w;
                i.center.x *= _ScreenParams.x;
                i.center.y *= _ScreenParams.y;                

                float dis = distance(vpos.xy, i.center.xy);
                if (dis > _Radius / i.dist) {
                    discard;
                }
                return i.col;
            }
            ENDCG
        }
    }
}
