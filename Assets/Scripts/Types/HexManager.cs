using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class HexManager
{
    private Dictionary<int, Dictionary<int, HexRenderer>> hexes = new();
    private HexGridLayout HGL;
    private BiomeGeneration BG;
    private GameObject Player;

    public HexManager(
        HexGridLayout HGL,
        BiomeGeneration BG,
        GameObject Player)
    {
        this.Player = Player;
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
        BG.GenerateDeepOcean(this);
    }

    public HexRenderer GetHex(int x, int z)
    {
        return hexes[x][z];
    }

    public void SetMaterials()
    {
        foreach (HexRenderer hex in GetHexList())
        {
            hex.SetMaterial();
        }
    }

    public HexRenderer[] AdjacentHexes(HexRenderer hexRenderer)
    {
        int x = hexRenderer.xAxis;
        int z = hexRenderer.zAxis;
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
            return new HexRenderer[] { };
        }
    }

    public List<HexRenderer> GetHexList()
    {
        List<HexRenderer> hexList = new();
        foreach (KeyValuePair<int, Dictionary<int, HexRenderer>> zArray in hexes)
        {
            foreach (KeyValuePair<int, HexRenderer> hexDict in zArray.Value)
            {
                hexList.Add(hexDict.Value);
            }
        }

        return hexList;
    }

    public void HoverHex(HexRenderer hexRenderer)
    {
        hexRenderer.transform.position -= new Vector3(0, 0.2f, 0);
    }

    public void LeaveHex(HexRenderer hexRenderer)
    {
        hexRenderer.transform.position += new Vector3(0, 0.2f, 0);
    }

    public void ClickHex(HexRenderer hexRenderer)
    {
        var position = hexRenderer.transform.position;
        HexRenderer startHexRenderer = GetHexList()[0];
        HexRenderer testihexi = hexes[startHexRenderer.xAxis + 2][startHexRenderer.zAxis + 2];
        FindPath(testihexi, hexRenderer);
        Player.transform.position = new Vector3(position.x, 4, position.z);
    }

    public void GenerateResources()
    {
        foreach (HexRenderer hex in GetHexList())
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

    public List<HexRenderer> FindPath(HexRenderer startHexRenderer, HexRenderer endHexRenderer)
    {
        List<HexRenderer> route = new();
        List<HexRenderer> completeRoute = new();
        List<HexRenderer> checkedHexes = new();
        completeRoute.Add(startHexRenderer);
        route.Add(startHexRenderer);
        int x = 0;
        bool pathNotFound = true;

        while (pathNotFound)
        {
            foreach (HexRenderer hex in completeRoute)
            {
                if (checkedHexes.Contains(hex)) continue;
                checkedHexes.Add(hex);

                HexRenderer[] adjHexes = AdjacentHexes(hex);

                foreach (HexRenderer adjHex in adjHexes)
                {
                    if (hex.biome.isPathable && !route.Contains(adjHex))
                    {
                        route.Add(adjHex);
                    }
                }

                if (route.Contains(endHexRenderer))
                {
                    pathNotFound = false;
                }
            }
            completeRoute.AddRange(route);
            x++;
            if (x > 50) break;
        }

        Debug.Log(route);
        Debug.Log(x);
        return completeRoute;
    }
}