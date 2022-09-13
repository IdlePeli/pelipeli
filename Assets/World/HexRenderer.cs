using System;
using System.Collections.Generic;
using UnityEngine;

public struct Face
{
    public List<Vector3> Vertices { get; }
    public List<int> Triangles { get; }
    public List<Vector2> Uvs { get; }

    public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs)
    {
        this.Vertices = vertices;
        this.Triangles = triangles;
        this.Uvs = uvs;
    }
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class HexRenderer : MonoBehaviour
{
    public Material material;

    private List<Face> _faces;
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;

    public float innerSize;
    public float outerSize;
    public float height;
    public bool isFlatTopped;
    

    private void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _mesh = new Mesh();
        _mesh.name = "Hex";

        _meshFilter.mesh = _mesh;
        _meshRenderer.material = material;
    }

    private void OnEnable()
    {
        DrawMesh();
    }

    public void OnValidate()
    {
        if (Application.isPlaying)
        {
            DrawMesh();
        } 
    }

    public void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
    }

    private void CombineFaces()
    {
        var vertices = new List<Vector3>();
        var tris = new List<int>();
        var uvs = new List<Vector2>();

        for (var i = 0; i < _faces.Count; i++)
        {
            vertices.AddRange(_faces[i].Vertices);
            uvs.AddRange(_faces[i].Uvs);

            var offset = 4 * i;
            foreach (var triangle in _faces[i].Triangles) tris.Add(triangle + offset);
        }

        _mesh.vertices = vertices.ToArray();
        _mesh.triangles = tris.ToArray();
        _mesh.uv = uvs.ToArray();
        _mesh.RecalculateNormals();
    }

    private void DrawFaces()
    {
        _faces = new List<Face>();

        //TOP
        for (var point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(innerSize, outerSize, height / 2f, height /2f, point));
            _faces.Add(CreateFace(innerSize, outerSize, -height / 2f, -height /2f, point, true));
            _faces.Add(CreateFace(outerSize, outerSize, height/2f, -height/2f, point, true));
            _faces.Add(CreateFace(innerSize, innerSize, height/2f, -height/2f, point ));
        }
    }

    private Face CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point,
        bool reverse = false)
    {
        var pointA = GetPoint(innerRad, heightB, point);
        var pointB = GetPoint(innerRad, heightB, point < 5 ? point + 1 : 0);
        var pointC = GetPoint(outerRad, heightA, point < 5 ? point + 1 : 0);
        var pointD = GetPoint(outerRad, heightA, point);

        var vertices = new List<Vector3> { pointA, pointB, pointC, pointD };
        var triangles = new List<int> { 0, 1, 2, 2, 3, 0 };
        var uvs = new List<Vector2> { new(0, 0), new(1, 0), new(1, 1), new(0, 1) };
        if (reverse) vertices.Reverse();
        return new Face(vertices, triangles, uvs);
    }

    private Vector3 GetPoint(float size, float heightY, int index)
    {
        float angleDeg = isFlatTopped ? 60 * index : 60*index-30;
        var angleRad = Mathf.PI / 180f * angleDeg;
        return new Vector3(size * MathF.Cos(angleRad), heightY, size * Mathf.Sin(angleRad));
    }

    public void SetMaterial(Material mat)
    {
        this.material = mat;
    }
}