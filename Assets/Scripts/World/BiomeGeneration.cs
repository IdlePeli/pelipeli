using System.Linq;
using UnityEngine;

public class BiomeGeneration : MonoBehaviour
{
    public Material waterMaterial;
    public Biome ocean;
    public Biome desert;
    public Biome forest;
    public Biome mountain;
    public Biome iceWater;
    public Biome snow;
    public Biome oceanFloor;
    
    public Biome deepOcean;
    //lisää biomei ja materiaalei joskus(isoi juttui tulos😎) ᓚᘏᗢ 

    //Change in unity to manipulate biomegeneration default value 10 with lower values biomes will be smaller
    public float heightAdjuster = 10f;
    public float temperatureAdjuster = 10f;
    public float terrainDepth = 1;

    private Water _water;

    public float waterLevel = 1f;
    public float mountainLevel = 2.5f;

    public float a = 5f;
    public float b = 2.2f;
    public float c = 1.2f;
    private float _oldA;
    private float _oldB;
    private float _oldC;
    private float _oldH;
    private float _oldMLevel;
    private float _oldT;
    private float _oldWLevel;


    public HexManager HexManager;

    public void Awake()
    {
        _oldA = a;
        _oldB = b;
    }

    public void Update()
    {
        if (_oldA != a || _oldB != b || _oldC != c || _oldWLevel != waterLevel || _oldMLevel != mountainLevel ||
            _oldH != heightAdjuster || _oldT != temperatureAdjuster)
        {
            _oldA = a;
            _oldB = b;
            _oldC = c;
            _oldWLevel = waterLevel;
            _oldMLevel = mountainLevel;
            _oldH = heightAdjuster;
            _oldT = temperatureAdjuster;

            foreach (Hex hex in HexManager.GetHexList())
            {
                Biome biome = Generate(hex.gridCoord);
                hex.SetMaterial(biome.material);
                hex.transform.position += new Vector3(0, biome.yAxis - hex.transform.position.y, 0);
            }
        }
    }

    public Biome Generate(Vector2Int gridCoord)
    {
        float height = GetHeight(gridCoord);
        float temperature = GetTemperature(gridCoord);

        Biome biome = GetBiome(height, temperature);

        biome.yAxis = height;
        return biome;
    }

    //generate deepOcean biomes if all adjacent tiles are water
    public void GenerateDeepOcean(HexManager hm)
    {
        foreach (Hex hex in hm.GetHexList())
        {
            if (!hex.biome.type.Equals("ocean")) continue;
            if (WaterInAdjacentHexes(hm.AdjacentHexes(hex))) hex.SetBiome(deepOcean);
        }
    }

    public void GenerateWater(Hex hex)
    {
        _water = new Water();
        _water.CreateWaterHex(hex, waterLevel, ocean, HexManager);
    }
    
    private static bool WaterInAdjacentHexes(Hex[] adjHexes)
    {
        return adjHexes.All(adjHex => adjHex.biome.type.Equals("ocean"));
    }

    private float GetTemperature(Vector2Int gridCoord)
    {
        //generate temperature for given xCoord and y position
        return Mathf.PerlinNoise(
            gridCoord.y / temperatureAdjuster,
            gridCoord.x / temperatureAdjuster
        );
    }

    private float GetHeight(Vector2Int gridCoord)
    {
        //generate height for given x and y position
        float x = 10 * Mathf.PerlinNoise(
            gridCoord.x / heightAdjuster,
            gridCoord.y / heightAdjuster
        );

        float height = Mathf.Pow(x - a, 3) * (b * 0.01f) + c;

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
                _ => oceanFloor
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
}