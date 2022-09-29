using System.Collections.Generic;
using UnityEngine;

public class GameRunner : MonoBehaviour
{
    public Player player;
    public int renderDistance = 5;
    public HexGridLayout layout;

    private Dictionary<int, Dictionary<int, GameObject>> _tiles;
    private System.Random _rnd;

    public void Awake()
    {
        // Get random starting position
        _rnd = new System.Random();

        int x = _rnd.Next(-200, 200);
        int y = _rnd.Next(-200, 200);

        // Load tiles in render distance and save them
        _tiles = new Dictionary<int, Dictionary<int, GameObject>>();
        for (int i = x - renderDistance; i < x + renderDistance; i++)
        {
            _tiles[i] = new Dictionary<int, GameObject>();
            for (int j = y - renderDistance; j < y + renderDistance; j++)
            {
                GameObject tile = layout.LayoutGrid(i, j);
                _tiles[i][j] = tile;
            }
        }

        GameObject startSquare = _tiles[x][y];
        player.transform.position = startSquare.transform.position;
    }
}
