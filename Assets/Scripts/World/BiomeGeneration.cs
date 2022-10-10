using System;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;

public class BiomeGeneration : MonoBehaviour
{
    public Material water;
    public Material grass;
    public Material ice;
    public Material sand;
    public Material mountain;
    
    public Biome waterBiome;
    public Biome sandBiome;
    public Biome grassBiome;
    public Biome mountainBiome;
    public Biome iceBiome;

    public void Awake()
    {
        waterBiome = new(water, "water", 1, 1);
        sandBiome = new(sand, "sand", 1, 1.5f);
        grassBiome = new(grass, "grass", 1, 0.5f);
        mountainBiome = new(mountain, "mountain", 1, 1f);
        iceBiome = new(ice, "ice", 1, 1);
    }

    public Biome Get(int x, int y)
    {
        float random = Mathf.PerlinNoise((float) x / 4, (float) y / 4) * 3 - 1;
        if (random <= 0) random = 0;
        random = Mathf.Pow(random, 1.3f);
        
        Biome biome = random switch
        {
            0 => waterBiome,
            < 0.5f => sandBiome,
            < 2f => grassBiome,
            _ => mountainBiome
        };
        
        Debug.Log(random);

        biome.yAxis = random * biome.terrainModifier;
        
        HexRenderer.SetMaterial(biome.material);
        return biome;
    }

    private Material GetMaterial(float x, float y)
    {
        float noise = Mathf.PerlinNoise(y / 4, x / 4);
        return noise switch
        {
            < 0.3f => ice,
            < 0.7f => grass,
            _ => sand
        };
    }
}