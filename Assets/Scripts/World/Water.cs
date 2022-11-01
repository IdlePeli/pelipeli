using UnityEngine;

public class Water
{
    public void CreateWaterHex(Hex hex, float waterlevel,Material watermaterial)
    {
        GameObject hexobject =
            new($"WaterHex {hex.gridCoord}", typeof(Hex));

        Hex waterHex = hexobject.GetComponent<Hex>();
        waterHex.outerSize = hex.outerSize;
        waterHex.innerSize = hex.innerSize;
        waterHex.height = (waterlevel - hex.transform.position.y) * 2;
        waterHex.gridCoord = hex.gridCoord;
        waterHex.SetMaterial(watermaterial);

        waterHex.waterHex = true;
        waterHex.DrawMesh();
        hexobject.transform.SetParent(hex.transform);
        waterHex.transform.position = hex.GetCeilingPosition();
    }
}