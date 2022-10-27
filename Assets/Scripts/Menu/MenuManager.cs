using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject WoodCanvas;
    public GameObject FishCanvas;
    public GameObject StoneCanvas;
    public GameObject canvas = null;

    public void SetCanvas(Biome biome)
    {
        if (canvas != null) canvas.SetActive(false);

        canvas = biome.name switch
        {
            "Forest" => WoodCanvas,
            "Ocean" => FishCanvas,
            "Mountain" => StoneCanvas,
            _ => canvas
        };

        canvas.SetActive(true);
        Debug.Log(canvas);
    }
}