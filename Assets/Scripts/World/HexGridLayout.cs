using System.Collections.Generic;
using UnityEngine;

public class HexGridLayout : MonoBehaviour
{
    [Header("Tile Settings")]
    public float outerSize = 1f;

    public float innerSize;
    public float height = 1f;
    public Material[] materials;
    public Material water;
    public Material mountain;

    public GameObject LayoutGrid(int x, int y)
    {
        GameObject tile =
            new($"Hex {x.ToString()},{y.ToString()}", typeof(HexRenderer))
            {
                transform = { position = GetPositionForHexFromCoordinate(new Vector2Int(x, y)) }
            };
        HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
        hexRenderer.outerSize = outerSize;
        hexRenderer.innerSize = innerSize;
        hexRenderer.height = height;

        switch (tile.transform.position.y)
        {
            case 0:
                hexRenderer.SetMaterial(water);
                break;
            case > 1.5f:
                hexRenderer.SetMaterial(mountain);
                break;
            default:
                hexRenderer.SetMaterial(GetMaterial(x, y));
                break;
        }

        hexRenderer.DrawMesh();
        tile.transform.SetParent(transform);
        return tile;
    }

    private Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x;
        int row = coordinate.y;
        float size = outerSize;

        bool shouldOffset = (column % 2) == 0;
        float width = 2f * size;
        float posHeight = Mathf.Sqrt(3f) * size;
        float horizontalDistance = width * (3f / 4f);
        float offset = (shouldOffset) ? posHeight / 2 : 0;
        float xPosition = (column * horizontalDistance);
        float yPosition = row * posHeight - offset;

        float noise = Mathf.PerlinNoise((float)column / 4, (float)row / 4) * 3 - 1;
        if (noise <= 0)
            noise = 0;
        noise = Mathf.Pow(noise, 1.3f);
        return new Vector3(xPosition, noise, -yPosition);
    }

    private Material GetMaterial(float x, float y)
    {
        float noise = Mathf.PerlinNoise(y / 4, x / 4);
        return noise switch
        {
            < 0.3f => materials[0],
            < 0.7f => materials[1],
            _ => materials[2]
        };
    }
}
