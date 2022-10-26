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