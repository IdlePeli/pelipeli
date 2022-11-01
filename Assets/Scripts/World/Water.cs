using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public Biome biome;
    public Vector2Int gridCoord;
    
    public float outerSize;
    public float innerSize;
    public float height;
    
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
            name = "Water"
        };

        _meshFilter.mesh = _mesh;
        _meshCollider.sharedMesh = _mesh;
    }
}
