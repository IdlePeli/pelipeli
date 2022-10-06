using UnityEngine;

public class BiomeGeneration : MonoBehaviour
{
    public Material water;
    public Material grass;
    public Material ice;
    public Material sand;
    public Material mountain;


    public Biome Get(int x, int y)
    {
        Biome biome = new();

        float hexHeight = Mathf.PerlinNoise((float) x / 4, (float) y / 4) * 3 - 1;
        if (hexHeight <= 0) hexHeight = 0;
        hexHeight = Mathf.Pow(hexHeight, 1.3f);
        
        biome.yAxis = hexHeight;

        biome.material = hexHeight switch
        {
            0 => water,
            > 1.5f => mountain,
            _ => GetMaterial(x, y)
        };

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