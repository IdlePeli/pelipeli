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

        int x = _rnd.Next(-200, 200);
        int y = _rnd.Next(-200, 200);

        // Load tiles in render distance and save them
        _tiles = new Dictionary<int, Dictionary<int, HexRenderer>>();
        for (int i = x - renderDistance; i < x + renderDistance; i++)
        {
            _tiles[i] = new Dictionary<int, HexRenderer>();
            for (int j = y - renderDistance; j < y + renderDistance; j++)
            {
                HexRenderer hex = layout.CreateTile(i, j);
                Vector3 pos = hex.transform.position;
                
                hex.biome = biomeGen.Get(i, j);
                
                hex.SetMaterial(hex.biome.material);
                hex.transform.position = new Vector3(pos.x, hex.biome.yAxis, pos.z);
                
                _tiles[i][j] = hex;
            }
        }

        HexRenderer startSquare = _tiles[x][y];
        player.transform.position = startSquare.transform.position;
    }
}