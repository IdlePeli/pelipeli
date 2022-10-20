using UnityEngine;

public class GameRunner : MonoBehaviour
{
    public Player player;

    public int renderDistance = 5;

    public BiomeGeneration BG;
    public HexGridLayout HGL;
    private HexManager HM;

    private System.Random _rnd;

    //DEBUG
    public bool DEBUG;
    public Material debugCheckedHexesMat;
    public Material debugCheckedRouteMat;
    public Material startAndEndHexes;

    public void Awake()
    {
        HM = new HexManager(HGL, BG, player, renderDistance);
        if (DEBUG)
        {
            HM.DEBUG = true;
            HM.DebugCheckedHexes = debugCheckedHexesMat;
            HM.DebugCheckedRoute = debugCheckedRouteMat;
            HM.StartAndEndHexes = startAndEndHexes;
        }

        BG.HM = HM;

        // Get random starting position
        _rnd = new System.Random();
        Vector2Int gridCoordinate = new(_rnd.Next(-200, 200) + 2500, _rnd.Next(-200, 200) + 2500);

        // Spawn player with access to HexManager
        player.Spawn(HM);

        // Render tiles in starting location
        HM.RenderTilesInRenderDistance(gridCoordinate, true);

        // Move the player somewhere where player can move
        Hex spawnHex = HM.GetHex(gridCoordinate.x, gridCoordinate.y);
        int i = 1;
        while (!player.CanMove(spawnHex))
        {
            spawnHex = HM.GetHex(gridCoordinate.x + i, gridCoordinate.y + i);
            i++;
        }
        player.Move(spawnHex);
        
        // TODO: Generate special biomes
        HM.GenerateSpecialBiomes();
    }
}