using UnityEngine;

public class HexGrid : MonoBehaviour
{
    [Header("Hex Settings")] public float outerSize = 1f;

    public float innerSize;
    public float height = 1f;

    public Hex CreateHex(HexManager hm, Vector2Int gridCoord)
    {
        GameObject tile = new($"Hex {gridCoord}", typeof(Hex))
        {
            transform =
            {
                position = GetPositionForHexFromCoordinate(gridCoord)
            }
        };
        Hex hex = tile.GetComponent<Hex>();
        hex.outerSize = outerSize;
        hex.innerSize = innerSize;
        hex.height = height;
        hex.gridCoord = gridCoord;
        hex.HexManager = hm;


        hex.DrawMesh();
        tile.transform.SetParent(transform);
        return hex;
    }

    private Vector3 GetPositionForHexFromCoordinate(Vector2Int coordinate)
    {
        int column = coordinate.x;
        int row = coordinate.y;
        float size = outerSize;

        bool shouldOffset = column % 2 == 0;
        float width = 2f * size;
        float posHeight = Mathf.Sqrt(3f) * size;
        float horizontalDistance = width * (3f / 4f);
        float offset = shouldOffset ? posHeight / 2 : 0;
        float xPosition = column * horizontalDistance;
        float yPosition = row * posHeight - offset;


        return new Vector3(xPosition, 0, yPosition);
    }
}