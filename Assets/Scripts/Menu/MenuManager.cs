using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Canvas woodCanvas;
    public Canvas fishCanvas;
    public Canvas canvas;
    public GameObject tile;

    public void SetCanvas(Biome biome)
    {
        Debug.Log(biome);
    }
}