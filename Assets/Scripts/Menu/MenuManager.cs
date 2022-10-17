using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject WoodCanvas;
    public GameObject FishCanvas;
    public GameObject StoneCanvas;

    public void SetCanvas(Biome biome)
    {
        
        GameObject canvas = null;
        
        switch (biome.name)
        {
            case "Forest":
                canvas = WoodCanvas;
                break;
            case "Ocean":
                canvas = FishCanvas;
                break;
            case "Mountain":
                canvas = StoneCanvas;
                break;
        }
        canvas.SetActive(true);
    }
}
