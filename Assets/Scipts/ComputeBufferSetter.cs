using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;

public class ComputeBufferSetter : MonoBehaviour {

    public Shader PointCloudShader;
    public TextAsset PtsFile;
    [Range(0, 500)] public float PointRadius = 50;
    [Range(0, 500)] public float PointSize = 100;

    private ComputeBuffer posbuffer;
    private ComputeBuffer colbuffer;
    private Material material;
    private List<(Vector3, Vector3)> pts;
    private bool bufferReady = false;

    async void OnEnable() {
        if (PointCloudShader == null) {
            Debug.LogError("Point Cloud Shader Not Set!");
            return;
        }

        if (pts == null) {
            pts = await PtsReader.Load(PtsFile);
        }

        List<Vector3> positions = pts.Select(item => item.Item1).ToList();
        List<Vector3> colors = pts.Select(item => item.Item2).ToList();

        // バッファ領域を確保・セット
        // 確保する領域サイズは、データ数 × データ一つあたりのサイズ
        int size = Marshal.SizeOf(new Vector3());
        posbuffer = new ComputeBuffer(positions.Count, size);
        colbuffer = new ComputeBuffer(colors.Count, size);
        posbuffer.SetData(positions);
        colbuffer.SetData(colors);

        // マテリアルを作成しバッファとパラメータをセット
        material = new Material(PointCloudShader);
        material.SetBuffer("colBuffer", colbuffer);
        material.SetBuffer("posBuffer", posbuffer);
        material.SetFloat("_Radius", PointRadius);
        material.SetFloat("_Size", PointSize);

        bufferReady = true;
    }

    void OnRenderObject() {
        if (bufferReady) {
            // レンダリングのたびに頂点の個数分シェーダーを実行
            // MeshTopology.Pointsを指定することで、面ではなく頂点が描画される            
            material.SetPass(0);
            Graphics.DrawProceduralNow(MeshTopology.Points, pts.Count);
        }
    }
    void OnValidate() {
        if (material != null) {
            material.SetFloat("_Radius", PointRadius);
            material.SetFloat("_Size", PointSize);
        }
    }

    void OnDisable() {
        if (bufferReady) {
            posbuffer.Release();
            colbuffer.Release();
        }
        bufferReady = false;
    }
}
