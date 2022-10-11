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
        float hexHeight = Mathf.PerlinNoise((float) x / 10, (float) y / 10) * 3 - 1;
        if (hexHeight <= 0) hexHeight = 0;
        hexHeight = Mathf.Pow(hexHeight, 1.3f);

        float temperature = Mathf.PerlinNoise((float) y / 6, (float) x / 6);

        Biome biome = hexHeight switch
        {
            0 => ocean,
            > 1.5f => mountain,
            
            _ => temperature switch
            {
                < 0.33f => ice,
                < 0.66f => forest,
                _ => desert
            }
        };

        biome.yAxis = hexHeight * biome.terrainModifier;
        return biome;
    }
}