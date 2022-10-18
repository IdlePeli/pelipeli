using UnityEngine;

public class HexGridLayout : MonoBehaviour
{
    [Header("Hex Settings")] public float outerSize = 1f;

    public float innerSize;
    public float height = 1f;

    public Hex CreateHex(HexManager HM, int x, int z)
    {
        GameObject tile = new($"Hex {x.ToString()},{z.ToString()}", typeof(Hex))
        {
            transform =
            {
                position = GetPositionForHexFromCoordinate(new Vector2Int(x, z))
            }
        };
        Hex hex = tile.GetComponent<Hex>();
        hex.outerSize = outerSize;
        hex.innerSize = innerSize;
        hex.height = height;
        hex.xAxis = x;
        hex.zAxis = z;
        hex.HM = HM;
        
        
        hex.DrawMesh();
        tile.transform.SetParent(transform);
        return hex;
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