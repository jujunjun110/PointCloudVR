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

            // C#から受け渡されるバッファ・パラメータの受け取り
            StructuredBuffer<float3> colBuffer;
            StructuredBuffer<float3> posBuffer;
            float _Size;
            float _Radius;

            v2f vert (uint id : SV_VertexID, out float4 vpos : SV_POSITION) {
                v2f o;

                // 連番で渡ってくる頂点IDを利用して、描画する頂点の座標・色を取り出し
                float4 pos = float4(posBuffer[id], 1);

                // Ptsファイルで255段階で保存されている色を0-1の階調に変換
                o.col = fixed4(colBuffer[id] / 255, 1);

                // 点群のサイズの補正のためカメラと点群の距離を計算
                float dist = length(_WorldSpaceCameraPos - pos);

                // 四角形の座標を返り値ではないoutの中に保存
                vpos = UnityObjectToClipPos(pos);

                // 四角形の中央のスクリーン座標を返り値の中に保存
                o.center = ComputeScreenPos(vpos);
                o.dist = dist;
                o.size = _Size / dist;
                return o;
            }

            fixed4 frag (v2f i, UNITY_VPOS_TYPE vpos : VPOS) : SV_Target {
                i.center.xy /= i.center.w;
                i.center.x *= _ScreenParams.x;
                i.center.y *= _ScreenParams.y;                

                float dis =length(vpos.xy - i.center.xy);
                if (dis > _Radius / i.dist) {
                    discard;
                }
                return i.col;
            }
            ENDCG
        }
    }
}
