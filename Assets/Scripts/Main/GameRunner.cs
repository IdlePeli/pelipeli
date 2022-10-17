using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    public GameObject player;
    public int renderDistance = 5;

    public BiomeGeneration BG;
    public HexGridLayout HGL;
    
    private HexManager HM;
    private System.Random _rnd;
    
    public void Awake()
    {
        HM = new HexManager(HGL, BG, player);
        
        // Get random starting position
        _rnd = new System.Random();
        
        int x = _rnd.Next(-200, 200) + 2500;
        int z = _rnd.Next(-200, 200) + 2500;
        
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

        HM.GenerateSpecialBiomes();
        HM.SetMaterials();
        HexRenderer startSquare = HM.GetHex(x, z);
        player.transform.position = startSquare.transform.position;
    }
}