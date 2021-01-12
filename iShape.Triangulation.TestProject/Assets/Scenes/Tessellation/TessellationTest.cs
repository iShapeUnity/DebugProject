using iShape.Collections;
using iShape.Geometry;
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
        
        var tesselation = shape.Tessellate(Allocator.Temp, iGeom, 0.0f);
        var vertices = tesselation.Vertices(Allocator.Temp, iGeom).Convert();
        var indices = tesselation.Indices(Allocator.Temp).Convert();
        
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;

        tesselation.Dispose();
        shape.Dispose();

        if (Application.isPlaying) {
            polygon.GetComponent<MeshFilter>().mesh = mesh;
        } else {
            polygon.GetComponent<MeshFilter>().sharedMesh = mesh;    
        }
    }

}