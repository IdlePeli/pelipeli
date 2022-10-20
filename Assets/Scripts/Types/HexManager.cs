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
        hexes[x][z].SetBiome(BG.Generate(x, z));
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
        if (Player.CanMove(hex)) FindPath(Player.currentHex, hex);
    }

    public void LeaveHex(Hex hex)
    {
        hex.transform.position += new Vector3(0, 0.2f, 0);
    }

    public void ClickHex(Hex hex)
    {
        if (Player.CanMove(hex))
        {
            Player.Move(hex);
        }
    }

    public void GenerateResources()
    {
        foreach (Hex hex in GetHexList())
        {
            GameObject resource = hex.biome.GenerateResource();
            if (resource == null) continue;

            Transform resTransform = resource.transform;
            resTransform.SetParent(hex.transform);
            resTransform.position = hex.GetCeilingPosition();
        }
    }

    private List<Hex> _route;
    private List<Hex> _checkedHexes = new();

    public List<Hex> FindPath(Hex startHex, Hex endHex)
    {
        _checkedHexes = new List<Hex>();
        List<Hex> hexesToCheck = new() {startHex};

        int iteration = 1;
        while (true)
        {
            // :D
            Hex hexToCheck =
                hexesToCheck
                    .Where(hex =>
                        hex.fCost == hexesToCheck.Min(hexMin => hexMin.fCost))
                    .OrderBy(hexSort => DistanceBetween(hexSort, endHex))
                    .ToList()
                    .First();

            hexesToCheck.Remove(hexToCheck);
            _checkedHexes.Add(hexToCheck);
            if (hexToCheck == endHex) break;

            foreach (Hex hex in AdjacentHexes(hexToCheck)
                         .Where(hex => Player.CanMove(hex) &&
                                       !_checkedHexes.Contains(hex)))
            {
                int fCost = DistanceBetween(hex, startHex) + DistanceBetween(hex, endHex);
                if (fCost >= hex.fCost && hexesToCheck.Contains(hex)) continue;
                hex.fCost = fCost;
                hex.parentHex = hexToCheck;
                if (!hexesToCheck.Contains(hex)) hexesToCheck.Add(hex);
            }

            if (iteration > 250)
            {
                Debug.Log("Calculated 250 cycles and couldn't reach destination.");
                break;
            }

            iteration++;
        }

        _route = RouteBuilder(endHex);
        if (DEBUG) DebugPathFinding(startHex, endHex);
        foreach (Hex hex in _checkedHexes) hex.parentHex = null;
        return _route;
    }

    private static List<Hex> RouteBuilder(Hex hex)
    {
        List<Hex> routeRB = new();
        Hex helperHex = hex;
        while (helperHex.parentHex != null)
        {
            routeRB.Add(helperHex);
            helperHex = helperHex.parentHex;
        }

        routeRB.Reverse();
        return routeRB;
    }

    private int DistanceBetween(Hex hex1, Hex hex2)
    {
        return (int) Vector2Int.Distance(hex1.GetGridCoordinate(), hex2.GetGridCoordinate());
    }


    // DEBUG SECTION

    public bool DEBUG;
    public Material DebugCheckedHexes;
    public Material DebugCheckedRoute;
    public Material StartAndEndHexes;
    private List<Hex> _debugColoredHexes = new();

    private void DebugPathFinding(Hex hex1, Hex hex2)
    {
        foreach (Hex hex in _debugColoredHexes)
        {
            hex.SetMaterial();
        }

        _debugColoredHexes = new List<Hex>();

        foreach (Hex hex in _checkedHexes)
        {
            hex.SetMaterial(DebugCheckedHexes);
        }

        foreach (Hex hex in _route)
        {
            hex.SetMaterial(DebugCheckedRoute);
        }

        hex1.SetMaterial(StartAndEndHexes);
        hex2.SetMaterial(StartAndEndHexes);

        _debugColoredHexes.Add(hex1);
        _debugColoredHexes.Add(hex2);
        _debugColoredHexes.AddRange(_checkedHexes);
        _debugColoredHexes.AddRange(_route);
    }
}