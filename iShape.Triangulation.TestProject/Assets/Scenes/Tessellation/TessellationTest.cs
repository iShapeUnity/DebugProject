using iShape.Geometry;
using iShape.Triangulation.Shape;
using iShape.Triangulation.Shape.Delaunay;
using Unity.Collections;
using UnityEngine;

public class TessellationTest : MonoBehaviour {

    public int testIndex = 5;
    public GameObject polygon;

    public void Start() {
        this.SetMesh();
    }

    public void Next() {
        this.testIndex += 1;
        this.SetMesh();
    }

    public void Prev() {
        this.testIndex -= 1;
        this.SetMesh();
    }

    private void SetMesh() {
        var iGeom = IntGeom.DefGeom;
        var shape = Data.Shape(testIndex, Allocator.Temp);
        shape.Modify(iGeom.Int(8), Allocator.Temp);
        var delaunay = shape.Delaunay(Allocator.Temp);
        var extraVertex = delaunay.Tessellate(0.5f * Mathf.PI,iGeom.Int(6), Allocator.Temp);
        // var extraVertex = new NativeArray<Vertex>(0, Allocator.Temp);
        
        var triangles = delaunay.Indices(Allocator.Temp);
        delaunay.Dispose();

        var vertices = new Vector3[shape.points.Length + extraVertex.Length];
        for (int i = 0; i < shape.points.Length; ++i) {
            var iv = shape.points[i];
            Vector3 v = iGeom.Float(iv);
            vertices[i] = v;
        }

        int offset = shape.points.Length;
        
        for (int i = 0; i < extraVertex.Length; ++i) {
            var iv = extraVertex[i].point;
            Vector3 v = iGeom.Float(iv);
            vertices[i + offset] = v;
        }

        extraVertex.Dispose();

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();

        if (Application.isPlaying) {
            polygon.GetComponent<MeshFilter>().mesh = mesh;
        } else {
            polygon.GetComponent<MeshFilter>().sharedMesh = mesh;    
        }

        triangles.Dispose();
        shape.Dispose();
    }

}
