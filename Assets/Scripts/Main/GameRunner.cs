using UnityEngine;
using Random = System.Random;

public class GameRunner : MonoBehaviour
{
    public Player player;

    public int renderDistance = 5;

    public BiomeGeneration bg;
    public HexGrid hgl;
    private HexManager _hexManager;
    private Random _rnd;
    
    //DEBUG
    public bool debug;
    public Material debugCheckedHexesMat;
    public Material debugCheckedRouteMat;
    public Material startAndEndHexes;


    public void Awake()
    {
        _hexManager = new HexManager(hgl, bg, player, renderDistance);
        if (debug)
        {
            _hexManager.Debug = true;
            _hexManager.DebugCheckedHexes = debugCheckedHexesMat;
            _hexManager.DebugCheckedRoute = debugCheckedRouteMat;
            _hexManager.StartAndEndHexes = startAndEndHexes;
        }

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
}