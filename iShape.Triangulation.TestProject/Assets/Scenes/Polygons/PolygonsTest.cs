﻿using System.Collections.Generic;
using iShape.Collections;
using iShape.Geometry;
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
        var delaunay = shape.Tessellate(Allocator.Temp, iGeom, 8);
        var list = delaunay.ConvexPolygons(iGeom, Allocator.Temp);
        shape.Dispose();
        delaunay.Dispose();

        Mesh mesh = new Mesh();
        var vertices = new List<Vector3>();
        var colors = new List<Color>();
        var triangles = new List<int>();

        for (int i = 0; i < list.Count; ++i) {
            var subPolygon = list.Get(i, Allocator.Temp);
            var subVertices = subPolygon.Vertices(Allocator.Temp).Convert();
            var subColors = ColorsArray(i % aColors.Length, subVertices.Length);
            var subIndices = subPolygon.ConvexIndices(Allocator.Temp, vertices.Count).Convert();

            vertices.AddRange(subVertices);
            colors.AddRange(subColors);
            triangles.AddRange(subIndices);

            subPolygon.Dispose();
        }

        mesh.vertices = vertices.ToArray();
        mesh.colors = colors.ToArray();
        mesh.triangles = triangles.ToArray();

        if (Application.isPlaying) {
            polygon.GetComponent<MeshFilter>().mesh = mesh;
        } else {
            polygon.GetComponent<MeshFilter>().sharedMesh = mesh;    
        }

        list.Dispose();
    }

    private Color[] ColorsArray(int index, int length) {
        var colors = new Color[length];

        var color = aColors[index % aColors.Length];
            
        for (int j = 0; j < length; ++j) {
            colors[j] = color;
        }

        return colors;
    }

}
