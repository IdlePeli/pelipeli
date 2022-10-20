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
    public float HeightAdjuster = 10f;
    public float TemperatureAdjuster = 10f;
    public float TerrainDepth = 1;
    private float oldH;
    private float oldT;
    
    
    public float waterLevel = 1f;
    public float mountainLevel = 2.5f;
    private float oldWLevel;
    private float oldMLevel;
    
    public float a = 5f;
    public float b = 2.2f;
    public float c = 1.2f;
    private float oldA;
    private float oldB;
    private float oldC;
    
    
    public HexManager HM;

    public Biome Generate(int xCoord, int zCoord)
    {
        float height = GetHeight(xCoord, zCoord);
        float temperature = GetTemperature(xCoord, zCoord);

        Biome biome = GetBiome(height, temperature);

        biome.yAxis = height;
        return biome;
    }

    public void Update()
    {
        if (oldA != a || oldB != b || oldC != c || oldWLevel != waterLevel || oldMLevel != mountainLevel || oldH != HeightAdjuster || oldT != TemperatureAdjuster)
        {
            oldA = a;
            oldB = b;
            oldC = c;
            oldWLevel = waterLevel;
            oldMLevel = mountainLevel;
            oldH = HeightAdjuster;
            oldT = TemperatureAdjuster;

            foreach (Hex hex in HM.GetHexList())
            {
                Biome biome = Generate(hex.xAxis, hex.zAxis);
                hex.SetMaterial(biome.material);
                hex.transform.position += new Vector3(0, biome.yAxis - hex.transform.position.y, 0);
            }
        }
    }

    public void Awake()
    {
        oldA = a;
        oldB = b;
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

    private float GetTemperature(int xCoord, int zCoord)
    {
        //generate temperature for given xCoord and y position
        return Mathf.PerlinNoise(
            zCoord / TemperatureAdjuster,
            xCoord / TemperatureAdjuster
        );
    }

    private float GetHeight(int xCoord, int zCoord)
    {
        //generate height for given x and y position
        float x =10 * Mathf.PerlinNoise(xCoord / HeightAdjuster, zCoord / HeightAdjuster);
        
        float height = Mathf.Pow(x - a, 3) *  (b * 0.01f) + c;
        
        return height;
    }

    //set biome based on height and temperature values
    private Biome GetBiome(float height, float temperature)
    {

        if (height < waterLevel)
        {
            return temperature switch
            {
                < 0.33f => iceWater,
                _ => ocean
            };
        }
        
        if (height >= mountainLevel) return mountain;

        return temperature switch
        {
            < 0.33f => snow,
            < 0.66f => forest,
            _ => desert
        };
    }

    private static float CubeRoot(float x)
    {
        if (x < 0)
            return -Mathf.Pow(-x, 1f / 3f);
        return Mathf.Pow(x, 1f / 3f);
    }
}