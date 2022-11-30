using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexManager
{
    private readonly List<Hex> _activeHexes = new();
    private readonly BiomeGeneration _biomeGen;
    private readonly Dictionary<Vector2Int, Hex> _hexes = new();
    private readonly HexGrid _hexGrid;
    private readonly Player _player;
    private readonly int _renderDistance;
    private HashSet<Hex> _checkedHexes = new();
    private List<Hex> _debugColoredHexes = new();
    private List<Hex> _route;
    private MenuManager MenuManager;
    private BuildableObjectManager _buildableObjectManager;
    private Hex currentHex;


    // DEBUG SECTION
    public bool Debug;
    public Material DebugCheckedHexes;
    public Material DebugCheckedRoute;
    public Material StartAndEndHexes;

    public HexManager(
        HexGrid hexGrid,
        BiomeGeneration biomeGen,
        Player player,
        int renderDistance,
        MenuManager menuManager,
        BuildableObjectManager buildableObjectManager)
    {
        _player = player;
        _hexGrid = hexGrid;
        _biomeGen = biomeGen;
        _renderDistance = renderDistance;
        MenuManager = menuManager;
        _buildableObjectManager = buildableObjectManager;
        MenuManager.hexMngr = this;
    }

    private void CreateHex(Vector2Int gridCoord)
    {
        if (_hexes.ContainsKey(gridCoord)) return;
        Hex hex = _hexes[gridCoord] = _hexGrid.CreateHex(this, gridCoord);
        SetBiome(hex);
        GenerateResource(hex);
        SetMaterial(hex);
        hex.gameObject.SetActive(false);
    }


    public void GenerateSpecialBiomes()
    {
        _biomeGen.GenerateDeepOcean(this);
    }

    public Hex GetHex(Vector2Int gridCoord)
    {
        return _hexes[gridCoord];
    }

    private void SetMaterial(Hex hex)
    {
        hex.SetMaterial();
    }

    private void SetBiome(Hex hex)
    {
        hex.SetBiome(_biomeGen.Generate(hex.gridCoord));
        if (hex.biome.type.Equals("ocean")) _biomeGen.GenerateWater(hex);
    }

    public Hex[] AdjacentHexes(Hex hex)
    {
        Vector2Int gridCoord = hex.gridCoord;
        try
        {
            if (gridCoord.x % 2 == 0)
                return new[]
                {
                    _hexes[gridCoord - Vector2Int.one],
                    _hexes[gridCoord + Vector2Int.left],
                    _hexes[gridCoord + Vector2Int.down],
                    _hexes[gridCoord + Vector2Int.up],
                    _hexes[gridCoord + Vector2Int.right + Vector2Int.down],
                    _hexes[gridCoord + Vector2Int.right]
                };

            return new[]
            {
                _hexes[gridCoord + Vector2Int.left],
                _hexes[gridCoord + Vector2Int.left + Vector2Int.up],
                _hexes[gridCoord + Vector2Int.down],
                _hexes[gridCoord + Vector2Int.up],
                _hexes[gridCoord + Vector2Int.right],
                _hexes[gridCoord + Vector2Int.one]
            };
        }
        catch (Exception)
        {
            if (gridCoord.x % 2 == 0)
                return new[]
                {
                    _hexes[GetOrCreate(gridCoord - Vector2Int.one).gridCoord],
                    _hexes[GetOrCreate(gridCoord + Vector2Int.left).gridCoord],
                    _hexes[GetOrCreate(gridCoord + Vector2Int.down).gridCoord],
                    _hexes[GetOrCreate(gridCoord + Vector2Int.up).gridCoord],
                    _hexes[GetOrCreate(gridCoord + Vector2Int.right + Vector2Int.down).gridCoord],
                    _hexes[GetOrCreate(gridCoord + Vector2Int.right).gridCoord]
                };

            return new[]
            {
                _hexes[GetOrCreate(gridCoord + Vector2Int.left).gridCoord],
                _hexes[GetOrCreate(gridCoord + Vector2Int.left + Vector2Int.up).gridCoord],
                _hexes[GetOrCreate(gridCoord + Vector2Int.down).gridCoord],
                _hexes[GetOrCreate(gridCoord + Vector2Int.up).gridCoord],
                _hexes[GetOrCreate(gridCoord + Vector2Int.right).gridCoord],
                _hexes[GetOrCreate(gridCoord + Vector2Int.one).gridCoord]
            };
        }
    }

    public List<Hex> GetHexList()
    {
        return _hexes.Select(zArray => zArray.Value).ToList();
    }

    public void HoverHex(Hex hex)
    {
        if (_player.CanMove(hex))
        {
            hex.transform.position -= new Vector3(0, 0.2f, 0);
            FindPath(_player.currentHex, hex);
        }
    }

    public void LeaveHex(Hex hex)
    {
        if (_player.CanMove(hex))
        {
            hex.transform.position += new Vector3(0, 0.2f, 0);
        }
    }

    public void ClickHex(Hex hex)
    {
        if (!MenuManager.BuildMenu.activeSelf)
        {
            currentHex = hex;
            MenuManager.SetCanvas(hex.biome);
            MenuManager.OpenMenu(hex);
            
        }
    }

    public void MovePlayer()
    {
        Hex hex = currentHex;
        if (!_player.CanMove(hex)) return;
        if (hex.transform.Find("Tree_02(Clone)"))
        {
            GameRunner.addWoodToCollect(100);
            MenuManager.AddWoodAmount(5);
        } else if (hex.transform.Find("Rock_01(Clone)"))
        {
            GameRunner.addStonesToCollect(100);
            MenuManager.AddStoneAmount(5);
        }
        
        _player.Move(hex);
        RenderTilesInRenderDistance();
        
    }
    

    public void RenderTilesInRenderDistance(Vector2Int coordinates = default, bool fromCoords = false)
    {
        Vector2Int gridCoordinate = !fromCoords ? _player.currentHex.GetGridCoordinate() : coordinates;

        _activeHexes
            .Where(hex => Vector2Int.Distance(gridCoordinate, hex.GetGridCoordinate()) > _renderDistance + 1)
            .ToList()
            .ForEach(hex =>
            {
                hex.gameObject.SetActive(false);
                _activeHexes.Remove(hex);
            });

        for (int xIndex = gridCoordinate.x - _renderDistance; xIndex < gridCoordinate.x + _renderDistance; xIndex++)
        for (int zIndex = gridCoordinate.y - _renderDistance; zIndex < gridCoordinate.y + _renderDistance; zIndex++)
        {
            Hex hex = GetOrCreate(new Vector2Int(xIndex, zIndex));
            if (Vector2Int.Distance(gridCoordinate, hex.GetGridCoordinate()) > _renderDistance) continue;
            hex.gameObject.SetActive(true);
            _activeHexes.Add(hex);
        }
    }

    private Hex GetOrCreate(Vector2Int gridCoord)
    {
        try
        {
            return _hexes[gridCoord];
        }
        catch (Exception)
        {
            CreateHex(gridCoord);
            return _hexes[gridCoord];
        }
    }

    public void GenerateHouse()
    {
        Hex hex = currentHex;
        BuildableObject house = _buildableObjectManager.GetObject();

        Transform houseTransform = house.transform;
        houseTransform.SetParent(hex.transform);
        houseTransform.position = hex.GetCeilingPosition();
    }

    private static void GenerateResource(Hex hex)
    {
        GameObject resource = hex.biome.GenerateResource();
        if (resource == null) return;

        Transform resTransform = resource.transform;
        resTransform.SetParent(hex.transform);
        resTransform.position = hex.GetCeilingPosition();
    }

    public List<Hex> FindPath(Hex startHex, Hex endHex)
    {
        // A* Pathfinding implementation (https://www.youtube.com/watch?v=-L-WgKMFuhE)
        _checkedHexes = new HashSet<Hex>();
        HashSet<Hex> hexesToCheck = new() { startHex };

        int iteration = 1;
        while (true)
        {
            // fCost is distance between startHex and endHex
            // Choose hex with lowest fCost
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

            // Loop through hexes where player can move and hex isnt already checked
            foreach (Hex hex in AdjacentHexes(hexToCheck)
                         .Where(hex => _player.CanMove(hex) &&
                                       !_checkedHexes.Contains(hex)))
            {
                // Calculate fCost for hex and if new fCost is larger than last checked fCost
                // for given hex and hex is to be checked continue with the loop
                int fCost = DistanceBetween(hex, startHex) + DistanceBetween(hex, endHex);
                if (fCost >= hex.fCost && hexesToCheck.Contains(hex)) continue;
                hex.fCost = fCost;
                hex.parentHex = hexToCheck;
                if (!hexesToCheck.Contains(hex)) hexesToCheck.Add(hex);
            }

            if (iteration > 250)
            {
                UnityEngine.Debug.Log("Calculated 250 cycles and couldn't reach destination.");
                break;
            }

            iteration++;
        }

        _route = RouteBuilder(endHex);
        if (Debug) DebugPathFinding(startHex, endHex);
        foreach (Hex hex in _checkedHexes) hex.parentHex = null;
        return _route;
    }

    private static List<Hex> RouteBuilder(Hex hex)
    {
        List<Hex> routeRb = new();
        Hex helperHex = hex;
        int iteration = 1;
        while (helperHex.parentHex != null)
        {
            routeRb.Add(helperHex);
            helperHex = helperHex.parentHex;
            if (iteration > 250) break;
            iteration++;
        }

        routeRb.Reverse();
        return routeRb;
    }

    private int DistanceBetween(Hex hex1, Hex hex2)
    {
        return (int)Vector2Int.Distance(hex1.GetGridCoordinate(), hex2.GetGridCoordinate());
    }

    private void DebugPathFinding(Hex hex1, Hex hex2)
    {
        foreach (Hex hex in _debugColoredHexes) hex.SetMaterial();

        _debugColoredHexes = new List<Hex>();

        foreach (Hex hex in _checkedHexes) hex.SetMaterial(DebugCheckedHexes);

        foreach (Hex hex in _route) hex.SetMaterial(DebugCheckedRoute);

        hex1.SetMaterial(StartAndEndHexes);
        hex2.SetMaterial(StartAndEndHexes);

        _debugColoredHexes.Add(hex1);
        _debugColoredHexes.Add(hex2);
        _debugColoredHexes.AddRange(_checkedHexes);
        _debugColoredHexes.AddRange(_route);
    }
}