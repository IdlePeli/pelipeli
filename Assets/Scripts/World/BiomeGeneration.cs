using UnityEngine;

public class BiomeGeneration : MonoBehaviour
{
    public Biome ocean;
    public Biome desert;
    public Biome forest;
    public Biome mountain;
    public Biome ice;


    public Biome Get(int x, int y)
    {
        float random = Mathf.PerlinNoise((float) x / 4, (float) y / 4) * 3 - 1;
        if (random <= 0) random = 0;
        random = Mathf.Pow(random, 1.3f);

        Biome biome = random switch
        {
            0 => ocean,
            < 0.5f => desert,
            < 2f => forest,
            _ => mountain
        };

        biome.yAxis = random * biome.terrainModifier;
        return biome;
    }
}