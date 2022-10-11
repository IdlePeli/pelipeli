using UnityEngine;

public class HexGridLayout : MonoBehaviour
{
    [Header("Tile Settings")] public float outerSize = 1f;

    public float innerSize;
    public float height = 1f;

    public HexRenderer CreateTile(int x, int y)
    {
        GameObject tile = new($"Hex {x.ToString()},{y.ToString()}", typeof(HexRenderer))
        {
            transform =
            {
                position = GetPositionForHexFromCoordinate(new Vector2Int(x, y))
            }
        };
        HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
        hexRenderer.outerSize = outerSize;
        hexRenderer.innerSize = innerSize;
        hexRenderer.height = height;
        hexRenderer.xAxis = x;
        hexRenderer.yAxis = y;
        
        
        hexRenderer.DrawMesh();
        tile.transform.SetParent(transform);
        return hexRenderer;
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


        return new Vector3(xPosition, 0, yPosition);
    }

}