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
        _rnd = new System.Random();
        _resourceWeight = emptiness;
        foreach (Resource res in resources)
        {
            _resourceWeight += res.rarity;
            res.CalculatedRarityScore = _resourceWeight;
        }
    }

    public Resource GenerateResource()
    {
        Debug.Log("jes täs myös" + _resourceWeight);
        int score = _rnd.Next(0, _resourceWeight);
        Debug.Log(score);
        foreach (Resource resource in resources)
        {
            if (resource.CalculatedRarityScore > score) return Instantiate(resource);
        }

        return null;
    }
}