using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Canvas woodCanvas;
    public Canvas FishCanvas;
    public Canvas Canvas;
    public GameObject tile;

    public void SetCanvas(Biome biome)
    {
        Debug.Log(biome);
    }
}
