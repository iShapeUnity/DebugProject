using System.Collections.Generic;
using iShape.Collections;
using iShape.Geometry;
using iShape.Geometry.Container;
using iShape.Triangulation.Shape;
using iShape.Triangulation.Shape.Delaunay;
using Unity.Collections;
using UnityEngine;

public class PolygonsTest : MonoBehaviour {

    public int testIndex = 5;
    public GameObject polygon;
    private readonly Color[] aColors = {
        new Color(1, 0, 0, 1),
        new Color(1, 0.5f, 0, 1),
        new Color(1, 1f, 0, 1),
        new Color(0.5f, 1.0f, 0, 1),
        new Color(0.5f, 1.0f, 0.5f, 1),
        new Color(0.0f, 1.0f, 0, 1),
        new Color(0.0f, 0.5f, 1f, 1),
        new Color(0.0f, 1f, 0.5f, 1),
        new Color(0.0f, 1f, 1f, 1),
        new Color(1f, 0f, 0.5f, 1),
        new Color(1f, 1f, 0.5f, 1),
        new Color(0.5f, 0f, 1.0f, 1),
        new Color(1.0f, 0f, 1.0f, 1),
    };
    
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
        var polygons = delaunay.ConvexPolygons(shape, Allocator.Temp);
        delaunay.Dispose();

        Mesh mesh = new Mesh();
        var vertices = new List<Vector3>();
        var colors = new List<Color>();
        var triangles = new List<int>();

        for (int i = 0; i < polygons.Count; ++i) {
            var points = polygons.Get(i, Allocator.Temp);
            var subShape = new PlainShape(points, true, Allocator.Temp);
            var subDelaunay = subShape.Delaunay(Allocator.Temp);
            var subIndices = subDelaunay.Indices(Allocator.Temp).Convert();

            var subVertices = new Vector3[points.Length];
            var subColors = new Color[points.Length];

            var color = aColors[i % aColors.Length];
            
            for (int j = 0; j < points.Length; ++j) {
                subVertices[j] = iGeom.Float(points[j]);
                subColors[j] = color;
            }

            for (int j = 0; j < subIndices.Length; ++j) {
                subIndices[j] += vertices.Count;
            }

            vertices.AddRange(subVertices);
            colors.AddRange(subColors);
            triangles.AddRange(subIndices);

            subDelaunay.Dispose();
            subShape.Dispose();
            points.Dispose();
        }

        mesh.vertices = vertices.ToArray();
        mesh.colors = colors.ToArray();
        mesh.triangles = triangles.ToArray();

        if (Application.isPlaying) {
            polygon.GetComponent<MeshFilter>().mesh = mesh;
        } else {
            polygon.GetComponent<MeshFilter>().sharedMesh = mesh;    
        }

        polygons.Dispose();
        shape.Dispose();
    }

}
