using System.Runtime.CompilerServices;
using UnityEngine;

public class Water
{
    public void CreateWaterHex(Hex hex, float waterlevel,Biome biome, HexManager hexManager)
    {
        GameObject hexobject =
            new($"WaterHex {hex.gridCoord}", typeof(Hex));
        Hex waterHex = hexobject.GetComponent<Hex>();
        waterHex.HexManager = hexManager;
        waterHex.outerSize = hex.outerSize;
        waterHex.innerSize = hex.innerSize;
        waterHex.height = (waterlevel - hex.transform.position.y) * 2;
        waterHex.gridCoord = hex.gridCoord;

        waterHex.waterHex = true;
        waterHex.SetBiome(biome);
        waterHex.SetMaterial();
        waterHex.DrawMesh();
        hexobject.transform.SetParent(hex.transform);
        waterHex.transform.position = hex.GetCeilingPosition();
    }
}