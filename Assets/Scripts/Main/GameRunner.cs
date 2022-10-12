using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    public GameObject player;
    public int renderDistance = 5;

    public BiomeGeneration BG;
    public HexGridLayout HGL;
    
    private Dictionary<int, Dictionary<int, HexRenderer>> hexes = new();
    private HexManager HM;
    private System.Random _rnd;
    
    public void Awake()
    {
        HM = new HexManager(hexes, HGL, BG, player);
        
        // Get random starting position
        _rnd = new System.Random();
        
        int x = _rnd.Next(-200, 200) + 2000;
        int z = _rnd.Next(-200, 200) + 2000;
        
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