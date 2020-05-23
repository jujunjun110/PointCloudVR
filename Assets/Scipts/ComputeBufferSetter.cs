using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Linq;

public class ComputeBufferSetter : MonoBehaviour {

    public Shader PointCloudShader;
    public TextAsset PtsFile;
    [Range(0, 500)] public float Radius = 50;
    [Range(0, 500)] public float Size = 100;

    private ComputeBuffer posbuffer;
    private ComputeBuffer colbuffer;
    private Material material;
    private List<(Vector3, Vector3)> pts;
    private bool bufferLoaded = false;

    async void OnEnable() {
        if (PointCloudShader == null) {
            return;
        }

        pts = await PtsReader.Load(PtsFile);
        List<Vector3> positions = pts.Select(item => item.Item1).ToList();
        List<Vector3> colors = pts.Select(item => item.Item2).ToList();

        int size = Marshal.SizeOf(new Vector3());
        posbuffer = new ComputeBuffer(positions.Count, size);
        colbuffer = new ComputeBuffer(colors.Count, size);
        posbuffer.SetData(positions);
        colbuffer.SetData(colors);

        material = new Material(PointCloudShader);
        material.SetBuffer("colBuffer", colbuffer);
        material.SetBuffer("posBuffer", posbuffer);
        material.SetFloat("_Radius", Radius);
        material.SetFloat("_Size", Size);
        bufferLoaded = true;
    }

    void OnValidate() {
        if (material != null) {
            material.SetFloat("_Radius", Radius);
            material.SetFloat("_Size", Size);
        }
    }

    void OnDisable() {
        posbuffer.Release();
        colbuffer.Release();
    }

    void OnRenderObject() {
        if (bufferLoaded) {
            material.SetPass(0);
            Graphics.DrawProceduralNow(MeshTopology.Points, pts.Count);
        }
    }
}
