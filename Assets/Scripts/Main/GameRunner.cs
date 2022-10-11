using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    public Player player;
    public int renderDistance = 5;

    public HexGridLayout layout;
    public BiomeGeneration biomeGen;

    private Dictionary<int, Dictionary<int, HexRenderer>> _tiles;
    private System.Random _rnd;

    public void Awake()
    {
        // Get random starting position
        _rnd = new System.Random();
        
        int x = _rnd.Next(-200, 200) + 2000;
        int z = _rnd.Next(-200, 200) + 2000;
        
        // Load tiles in render distance and save them
        // Generate 2 dimensional empty dictionary to receive
        // HexRenderer for each possible x and y coordinate
        _tiles = new Dictionary<int, Dictionary<int, HexRenderer>>();
        for (int xIndex = x - renderDistance; xIndex < x + renderDistance; xIndex++)
        {
            _tiles[xIndex] = new Dictionary<int, HexRenderer>();
            for (int zIndex = z - renderDistance; zIndex < z + renderDistance; zIndex++)
            {
                HexRenderer hex = layout.CreateTile(xIndex, zIndex);
                hex.SetBiome(biomeGen.Get(xIndex, zIndex));
                _tiles[xIndex][zIndex] = hex;
            }
        }
        
        biomeGen.generateDeepOcean(_tiles);
        
        HexRenderer startSquare = _tiles[x][z];
        player.transform.position = startSquare.transform.position;
    }
}