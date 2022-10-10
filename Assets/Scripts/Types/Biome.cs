using System;
using UnityEngine;

public class Biome
{
    public Material material;
    public String name;
    public float yAxis;
    public float terrainModifier;
    public Biome(Material material, String name, float yAxis, float terrainModifier)
    {
        this.material = material;
        this.name = name;
        this.yAxis = yAxis;
        this.terrainModifier = terrainModifier;
    }
}