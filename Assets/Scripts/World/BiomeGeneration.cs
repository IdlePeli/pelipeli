using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BiomeGeneration : MonoBehaviour
{
    public Biome ocean;
    public Biome desert;
    public Biome forest;
    public Biome mountain;
    public Biome iceWater;
    public Biome snow;
    public Biome deepOcean;
    //lisää biomei ja materiaalei joskus(isoi juttui tulos😎) ᓚᘏᗢ 
    
    //Change in unity to manipulate biomegeneration default value 10 with lower values biomes will be smaller
    public int HeightAdjuster = 10;
    public int TemperatureAdjuster = 10;
    public float TerrainDepth = 1;
    public Biome Get(int x, int z)
    {
        //generate height for given x and y position
        float hexHeight = Mathf.PerlinNoise((float) x / HeightAdjuster, (float) z / HeightAdjuster) * 3 - TerrainDepth;
        if (hexHeight <= 0) hexHeight = 0;
        hexHeight = Mathf.Pow(hexHeight, 1.3f);

        //generate temperature for given x and y position
        float temperature = Mathf.PerlinNoise((float) z / TemperatureAdjuster, (float) x / TemperatureAdjuster);

        //set biome based on height and temperature values
        Biome biome = hexHeight switch
        {
            //set world height specific biomes
            0 => temperature switch
            {
                < 0.33f => iceWater,
                _ => ocean
            },
            > 1.5f => mountain,
            //set rest of the biomes based on temperature zones
            _ => temperature switch
            {
                < 0.33f => snow,
                < 0.66f => forest,
                _ => desert
            }
        };
        
        //set tile yAxis position adjusted by biome modifier
        biome.yAxis = (hexHeight - 1.5f) * biome.terrainModifier + 1.5f;
        return biome;
    }


    //generate deepOcean biomes if all adjacent tiles are water
    public void GenerateDeepOcean(HexManager HM)
    {
        foreach (HexRenderer hex in HM.GetHexList())
        {
            if (!hex.biome.type.Equals("ocean")) continue;
            if (WaterInAdjacentHexes(HM.AdjacentHexes(hex)))
            {
                hex.SetBiome(deepOcean);
            }
        }
    }

    private static bool WaterInAdjacentHexes(HexRenderer[] adjHexes)
    {
        return adjHexes.All(adjHex => adjHex.biome.type.Equals("ocean"));
    }
}