using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HexManager
{
    private Dictionary<int, Dictionary<int, HexRenderer>> hexes;
    private HexGridLayout HGL;
    private BiomeGeneration BG;
    private GameObject Player;
    public HexManager(Dictionary<int, Dictionary<int, HexRenderer>> hexes, HexGridLayout HGL, BiomeGeneration BG,
        GameObject Player)
    {
        this.Player = Player;
        this.hexes = hexes;
        this.HGL = HGL;
        this.BG = BG;
    }

    public void AddHex(int x, int z)
    {
        if (!hexes.ContainsKey(x)) hexes[x] = new Dictionary<int, HexRenderer>();
        hexes[x][z] = HGL.CreateTile(this, x, z);
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

    public void HoverHex(HexRenderer hex)
    {
        hex.transform.position -= new Vector3(0, 0.2f, 0);
    }

    public void LeaveHex(HexRenderer hex)
    {
        hex.transform.position += new Vector3(0, 0.2f, 0);
    }

    public void ClickHex(HexRenderer hex)
    {
        var position = hex.transform.position;
        Player.transform.position = new Vector3(position.x, 4, position.z);
    }

    public Biome GetBiome(HexRenderer hex)
    {
        return hex.biome;
    }

    public Material GetMaterial(HexRenderer hex)
    {
        return hex.biome.material;
    }
}