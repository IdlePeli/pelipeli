using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class GameRunner : MonoBehaviour
{
    public Player player;

    public int renderDistance = 5;

    public BiomeGeneration bg;
    public HexGrid hgl;
    private HexManager _hexManager;
    private Random _rnd;
    public MenuManager MenuManager;
    [FormerlySerializedAs("ObjectManager")] public BuildableObjectManager buildableObjectManager;
    
    //DEBUG
    public bool debug;
    public Material debugCheckedHexesMat;
    public Material debugCheckedRouteMat;
    public Material startAndEndHexes;


    private static List<int> stoner;
    private static List<int> wooder;
    private int hourFromPast;
    public void Awake()
    {
        hourFromPast = 0;
        stoner = new List<int>();
        wooder = new List<int>();
        _hexManager = new HexManager(hgl, bg, player, renderDistance, MenuManager, buildableObjectManager);
        if (debug)
        {
            _hexManager.Debug = true;
            _hexManager.DebugCheckedHexes = debugCheckedHexesMat;
            _hexManager.DebugCheckedRoute = debugCheckedRouteMat;
            _hexManager.StartAndEndHexes = startAndEndHexes;
        }

        MenuManager.Player = player;
        bg.HexManager = _hexManager;

        // Get random starting position
        _rnd = new Random();
        Vector2Int gridCoordinate = new(_rnd.Next(-200, 200) + 2500, _rnd.Next(-200, 200) + 2500);

        // Spawn player with access to HexManager
        player.Spawn(_hexManager);
        
        // Render tiles in starting location
        _hexManager.RenderTilesInRenderDistance(gridCoordinate, true);

        // Move the player somewhere where player can move
        Hex spawnHex = _hexManager.GetHex(gridCoordinate);
        int i = 1;
        while (!player.CanMove(spawnHex))
        {
            spawnHex = _hexManager.GetHex(gridCoordinate + new Vector2Int(i, i));
            i++;
        }

        player.Move(spawnHex);

        // TODO: Generate special biomes
        _hexManager.GenerateSpecialBiomes();
    }

    public static void addStonesToCollect(int timeLenghtInHours)
    {
        stoner.Add(timeLenghtInHours);
    }

    public static void addWoodToCollect(int timeLenghtInHours)
    {
        wooder.Add(timeLenghtInHours);
    }
    
    public void Update()
    {
        int nowHour = WorldTime.Hour;
        if (hourFromPast == nowHour) return;
        hourFromPast = nowHour;
        for (int i = 0; i < stoner.Count; i++)
        {
            stoner[i]--;
            MenuManager.AddStoneAmount(1);
        }

        stoner = stoner.FindAll(stone => stone > 0);

        for (int i = 0; i < wooder.Count; i++)
        {
            wooder[i]--;
            MenuManager.AddWoodAmount(1);
        }
        
        wooder = wooder.FindAll(stone => stone > 0);
    }
}