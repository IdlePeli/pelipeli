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

    public Biome Get(int x, int y)
    {
        //generate height for given x and y position
        float hexHeight = Mathf.PerlinNoise((float) x / 10, (float) y / 10) * 3 - 1;
        if (hexHeight <= 0) hexHeight = 0;
        hexHeight = Mathf.Pow(hexHeight, 1.3f);

        //generate temperature for given x and y position
        float temperature = Mathf.PerlinNoise((float) y / 10, (float) x / 10);

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
    public void generateDeepOcean(Dictionary<int, Dictionary<int, HexRenderer>> tiles)
    {
        foreach (var zAxis in tiles)
        {
            foreach (var hexDict in zAxis.Value)
            {
                HexRenderer hex = hexDict.Value;
                if (hex.biome.type.Equals("ocean"))
                {
                    if (waterInAdjacentTiles(hex, tiles))
                    { 
                        hex.SetBiome(deepOcean);
                    }
                }
            }
        }
    }
    
    private bool waterInAdjacentTiles(HexRenderer hex, Dictionary<int, Dictionary<int, HexRenderer>> tiles)
    {
        for (int x = hex.xAxis - 1; x < hex.xAxis + 2; x++)
        {
            for (int y = hex.yAxis - 1; y < hex.yAxis + 2; y++)
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
                    Console.WriteLine(e);
                }
            }
        }
        return true;
    }
}