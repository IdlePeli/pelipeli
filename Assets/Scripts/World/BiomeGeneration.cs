using System;
using System.Collections.Generic;
using UnityEngine;

public class BiomeGeneration : MonoBehaviour
{
    public Biome ocean;
    public Biome desert;
    public Biome forest;
    public Biome mountain;
    public Biome ice;

    public Biome deepOcean;
    //lisää biomei ja materiaalei joskus(isoi juttui tulos😎) ᓚᘏᗢ 

    public Biome Get(int x, int z)
    {
        //generate height for given x and y position
        float hexHeight = Mathf.PerlinNoise((float) x / 10, (float) z / 10) * 3 - 1;
        if (hexHeight <= 0) hexHeight = 0;
        hexHeight = Mathf.Pow(hexHeight, 1.3f);

        //generate temperature for given x and y position
        float temperature = Mathf.PerlinNoise((float) z / 10, (float) x / 10);

        //set biome based on height and temperature values
        Biome biome = hexHeight switch
        {
            //set world height specific biomes
            0 => ocean,
            > 1.5f => mountain,
            //set rest of the biomes based on temperature zones
            _ => temperature switch
            {
                < 0.33f => ice,
                < 0.66f => forest,
                _ => desert
            }
        };
        
        //set tile yAxis position adjusted by biome modifier
        biome.yAxis = (hexHeight - 1.5f) * biome.terrainModifier + 1.5f;
        return biome;
    }


    //generate deepOcean biomes if all adjacent tiles are water
    public void GenerateDeepOcean(Dictionary<int, Dictionary<int, HexRenderer>> tiles)
    {
        foreach (var zAxis in tiles)
        {
            foreach (var hexDict in zAxis.Value)
            {
                HexRenderer hex = hexDict.Value;
                if (hex.biome.type.Equals("ocean"))
                {
                    if (WaterInAdjacentTiles(hex, tiles))
                    { 
                        hex.SetBiome(deepOcean);
                    }
                }
            }
        }
    }
    
    private static bool WaterInAdjacentTiles(HexRenderer hex, Dictionary<int, Dictionary<int, HexRenderer>> tiles)
    {
        for (int x = hex.xAxis - 1; x < hex.xAxis + 2; x++)
        {
            for (int y = hex.zAxis - 1; y < hex.zAxis + 2; y++)
            {
                try
                {
                    if (!tiles[x][y].biome.type.Equals("ocean"))
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    if (false) Debug.Log(e);
                    //TODO: Exception occurs since were checking for a hex which has not yet been generated.
                    //Generate a new hex for given coordinates and check again. (Implement proper error handling)
                }
            }
        }
        return true;
    }
}