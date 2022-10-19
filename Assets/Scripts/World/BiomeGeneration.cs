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
    public float a = 0.24f;
    public float b = 0.47f;
    
    public Biome Get(int xCoord, int zCoord)
    {
        //f(xCoord)=((nroot(xCoord-a,3)+b)/(2))
        
        
        //generate height for given x and y position
        float x = Mathf.PerlinNoise((float)xCoord / HeightAdjuster, (float)zCoord / HeightAdjuster);
        float hexHeight = CubeRoot(x-a) - b;
        
        //generate temperature for given xCoord and y position
        float temperature = Mathf.PerlinNoise((float) zCoord / TemperatureAdjuster, (float) xCoord / TemperatureAdjuster);

        //set biome based on height and temperature values
        Biome biome = hexHeight switch
        {
            //set world height specific biomes
            < 0 => temperature switch
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
        Debug.Log(hexHeight);
        //set tile yAxis
        biome.yAxis = hexHeight;
        return biome;
    }


    //generate deepOcean biomes if all adjacent tiles are water
    public void GenerateDeepOcean(HexManager HM)
    {
        foreach (Hex hex in HM.GetHexList())
        {
            if (!hex.biome.type.Equals("ocean")) continue;
            if (WaterInAdjacentHexes(HM.AdjacentHexes(hex)))
            {
                hex.SetBiome(deepOcean);
            }
        }
    }

    private static bool WaterInAdjacentHexes(Hex[] adjHexes)
    {
        return adjHexes.All(adjHex => adjHex.biome.type.Equals("ocean"));
    }
    
    private static float CubeRoot(float x) {
        if (x < 0)
            return -Mathf.Pow(-x, 1f / 3f);
        else
            return Mathf.Pow(x, 1f / 3f);
    }
}