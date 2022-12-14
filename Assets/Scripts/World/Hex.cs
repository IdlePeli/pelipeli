using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class Hex : MonoBehaviour
{
    public Biome biome;
    public Vector2Int gridCoord;

    public float innerSize;
    public float outerSize;
    public float height;
    public List<Resource> resources;
    public List<BuildableObject> BuildableObjects;


    // Pathfinding
    public int fCost;
    public Hex parentHex;
    private List<Face> _faces;
    private Mesh _mesh;
    private MeshCollider _meshCollider;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    public HexManager HexManager;
    private void Awake()
    {
        _meshCollider = GetComponent<MeshCollider>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _mesh = new Mesh
        {
            name = "Hex"
        };

        _meshFilter.mesh = _mesh;
        _meshCollider.sharedMesh = _mesh;
    }

    private void OnMouseDown()
    {
        HexManager.ClickHex(this);
    }

    private void OnMouseEnter()
    {
        HexManager.HoverHex(this);
    }

    private void OnMouseExit()
    {
        HexManager.LeaveHex(this);
    }

    public void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
    }

    public bool waterHex;

    private void CombineFaces()
    {
        List<Vector3> vertices = new();
        List<int> tris = new();
        List<Vector2> uvs = new();

        for (int i = 0; i < _faces.Count; i++)
        {
                vertices.AddRange(_faces[i].Vertices);
                uvs.AddRange(_faces[i].Uvs);

                int offset = 4 * i;
                tris.AddRange(_faces[i].Triangles.Select(triangle => triangle + offset));
        }

        _mesh.vertices = vertices.ToArray();
        _mesh.triangles = tris.ToArray();
        _mesh.uv = uvs.ToArray();
        _mesh.RecalculateNormals();
    }


    private void DrawFaces()
    {
        _faces = new List<Face>();

        for (int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(innerSize, outerSize, height / 2f, height / 2f, point));
            if (waterHex) continue;
            _faces.Add(CreateFace(innerSize, outerSize, -height / 2f, -height / 2f, point, true));
            _faces.Add(CreateFace(outerSize, outerSize, height / 2f, -height / 2f, point, true));
            _faces.Add(CreateFace(innerSize, innerSize, height / 2f, -height / 2f, point));
        }
    }

    private static Face CreateFace(float innerRad, float outerRad, float heightA, float heightB, int point,
        bool reverse = false)
    {
        Vector3 pointA = GetPoint(innerRad, heightB, point);
        Vector3 pointB = GetPoint(innerRad, heightB, point < 5 ? point + 1 : 0);
        Vector3 pointC = GetPoint(outerRad, heightA, point < 5 ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRad, heightA, point);

        List<Vector3> vertices = new() { pointA, pointB, pointC, pointD };
        List<int> triangles = new() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
        if (reverse) vertices.Reverse();
        return new Face(vertices, triangles, uvs);
    }

    private static Vector3 GetPoint(float size, float heightY, int index)
    {
        float angleDeg = 60 * index;
        float angleRad = Mathf.PI / 180f * angleDeg;
        return new Vector3(size * MathF.Cos(angleRad), heightY, size * Mathf.Sin(angleRad));
    }

    public void SetBiome(Biome newBiome)
    {
        transform.position += new Vector3(0, newBiome.yAxis, 0);
        biome = newBiome;
    }

    public void SetMaterial(Material material = null)
    {
        // If material is null then use Hex's biomes material
        _meshRenderer.material = material ? material : biome.material;
    }

    public Vector3 GetCeilingPosition()
    {
        Vector3 position = transform.position;
        return new Vector3(position.x, position.y + height / 2, position.z);
    }

    public Vector2Int GetGridCoordinate()
    {
        return gridCoord;
    }
}
