using System.Linq;
using UnityEngine;

public class Biome : MonoBehaviour
{
    public Material material;
    public float yAxis = 1;
    public float terrainModifier = 1;
    public string type;
    public Resource[] resources;
    public int emptiness = 500;
    private int _resourceWeight;
    private System.Random _rnd;


    public void Awake()
    {
        _resourceWeight = emptiness;
        foreach (Resource res in resources)
        {
            _resourceWeight += res.rarity;
            res.CalculatedRarityScore = _resourceWeight;
        }
    }

    public GameObject GenerateResource()
    {
        _rnd = new System.Random();
        int score = _rnd.Next(0, _resourceWeight);
        foreach (Resource resource in resources.Reverse())
        {
            if (score > resource.CalculatedRarityScore - resource.rarity) return Instantiate(resource.model);
        }

        return null;
    }
}