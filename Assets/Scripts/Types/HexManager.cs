using System.Collections.Generic;
using UnityEngine;

public class HexManager
{
    private Dictionary<int, Dictionary<int, HexRenderer>> hexes;
    private HexGridLayout HGL;
    private BiomeGeneration BG;

    public HexManager(Dictionary<int, Dictionary<int, HexRenderer>> hexes, HexGridLayout HGL, BiomeGeneration BG)
    {
        this.hexes = hexes;
        this.HGL = HGL;
        this.BG = BG;
    }
    
    public void AddHex(int x, int z)
    {
        if (!hexes.ContainsKey(x)) hexes[x] = new Dictionary<int, HexRenderer>();
        hexes[x][z] = HGL.CreateTile(x, z);
    }

    public void SetBiome(int x, int z)
    {
        hexes[x][z].SetBiome(BG.Get(x, z));
    }

    public void GenerateSpecialBiomes()
    {
        BG.GenerateDeepOcean(hexes);
    }

    public HexRenderer GetHex(int x, int z)
    {
        return hexes[x][z];
    }

    public void SetMaterials()
    {
        foreach (var zAxis in hexes)
        {
            foreach (var hexDict in zAxis.Value)
            {
                HexRenderer hex = hexDict.Value;
                hex.SetMaterial();
            }
        }
    }
}