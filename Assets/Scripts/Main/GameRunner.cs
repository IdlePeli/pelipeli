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
    public Material StartAndEndHexes;

    public void Awake()
    {
        HM = new HexManager(HGL, BG, player);
        if (DEBUG)
        {
            HM.DEBUG = true;
            HM.DebugCheckedHexes = debugCheckedHexesMat;
            HM.DebugCheckedRoute = debugCheckedRouteMat;
            HM.StartAndEndHexes = StartAndEndHexes;
        }

        // Get random starting position
        _rnd = new System.Random();

        int x = _rnd.Next(-200, 200) + 2500;
        int z = _rnd.Next(-200, 200) + 2500;

        player.Spawn(HM);

        // Load tiles in render distance and save them
        // Generate 2 dimensional empty dictionary to receive
        // HexRenderer for each possible x and y coordinate
        for (int xIndex = x - renderDistance; xIndex < x + renderDistance; xIndex++)
        {
            for (int zIndex = z - renderDistance; zIndex < z + renderDistance; zIndex++)
            {
                HM.AddHex(xIndex, zIndex);
                HM.SetBiome(xIndex, zIndex);
            }
        }

        // Spawn the player somewhere where player can move
        Hex spawnHex = HM.GetHex(x, z);
        int i = 1;
        while (!player.CanMove(spawnHex))
        {
            spawnHex = HM.GetHex(x + i, z + i);
            i++;
        }

        player.Move(spawnHex);
        HM.GenerateSpecialBiomes();
        HM.GenerateResources();
        HM.SetMaterials();
    }
}