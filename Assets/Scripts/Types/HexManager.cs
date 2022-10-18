using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexManager
{
    private Dictionary<int, Dictionary<int, Hex>> hexes = new();
    private HexGridLayout HGL;
    private BiomeGeneration BG;
    private Player Player;

    public HexManager(
        HexGridLayout HGL,
        BiomeGeneration BG,
        Player Player)
    {
        this.Player = Player;
        this.HGL = HGL;
        this.BG = BG;
    }

    public void AddHex(int x, int z)
    {
        if (!hexes.ContainsKey(x)) hexes[x] = new Dictionary<int, Hex>();
        hexes[x][z] = HGL.CreateHex(this, x, z);
    }

    public void SetBiome(int x, int z)
    {
        hexes[x][z].SetBiome(BG.Get(x, z));
    }

    public void GenerateSpecialBiomes()
    {
        BG.GenerateDeepOcean(this);
    }

    public Hex GetHex(int x, int z)
    {
        return hexes[x][z];
    }

    public void SetMaterials()
    {
        foreach (Hex hex in GetHexList())
        {
            hex.SetMaterial();
        }
    }

    public Hex[] AdjacentHexes(Hex hex)
    {
        int x = hex.xAxis;
        int z = hex.zAxis;
        try
        {
            if (x % 2 == 0)
            {
                return new[]
                {
                    hexes[x - 1][z - 1],
                    hexes[x - 1][z],
                    hexes[x][z - 1],
                    hexes[x][z + 1],
                    hexes[x + 1][z - 1],
                    hexes[x + 1][z]
                };
            }

            return new[]
            {
                hexes[x - 1][z],
                hexes[x - 1][z + 1],
                hexes[x][z - 1],
                hexes[x][z + 1],
                hexes[x + 1][z],
                hexes[x + 1][z + 1]
            };
        }
        catch (Exception)
        {
            return new Hex[] { };
        }
    }

    public List<Hex> GetHexList()
    {
        List<Hex> hexList = new();
        foreach (KeyValuePair<int, Dictionary<int, Hex>> zArray in hexes)
        {
            foreach (KeyValuePair<int, Hex> hexDict in zArray.Value)
            {
                hexList.Add(hexDict.Value);
            }
        }

        return hexList;
    }

    public void HoverHex(Hex hex)
    {
        hex.transform.position -= new Vector3(0, 0.2f, 0);
    }

    public void LeaveHex(Hex hex)
    {
        hex.transform.position += new Vector3(0, 0.2f, 0);
    }

    public void ClickHex(Hex hex)
    {
        FindPath(Player.currentHex, hex);
        Player.Move(hex);
    }

    public void GenerateResources()
    {
        foreach (Hex hex in GetHexList())
        {
            GameObject resource = hex.biome.GenerateResource();
            if (resource == null) continue;

            Transform resTransform = resource.transform;
            Transform hexTransform = hex.transform;

            resTransform.SetParent(hexTransform);
            resTransform.position = hexTransform.position;
            resTransform.position += new Vector3(0, 1.5f, 0);
        }
    }

    public List<Hex> FindPath(Hex startHex, Hex endHex)
    {
        List<Hex> route = new();
        List<Hex> completeRoute = new();
        List<Hex> checkedHexes = new();
        completeRoute.Add(startHex);
        route.Add(startHex);

        int iteration = 0;
        while (iteration < 30)
        {
            foreach (Hex hex in completeRoute.Where(hex => !checkedHexes.Contains(hex)))
            {
                Hex[] adjHexes = AdjacentHexes(hex);
                checkedHexes.Add(hex);

                foreach (Hex adjHex in adjHexes.Where(adjHex => Player.CanMove(adjHex)))
                {
                    if (!route.Contains(adjHex)) route.Add(adjHex);
                }
            }

            completeRoute.AddRange(route);
            if (completeRoute.Contains(endHex)) break;
            iteration++;
        }

        Debug.Log(route);
        Debug.Log(iteration);
        return completeRoute;
    }
}