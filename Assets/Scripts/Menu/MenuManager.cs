using UnityEditor;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject WoodCanvas;
    public GameObject FishCanvas;
    public GameObject StoneCanvas;
    public GameObject SandCanvas;
    public GameObject canvas = null;
    public GameObject BuildMenu;
    public HexManager hexMngr;
    public Hex currentHex;

    public void SetCanvas(Biome biome)
    {
        if (canvas != null) canvas.SetActive(false);

        canvas = biome.name switch
        {
            "Forest" => WoodCanvas,
            "Ocean" => FishCanvas,
            "Mountain" => StoneCanvas,
            "Desert" => SandCanvas,
            _ => canvas
        };

        canvas.SetActive(true);
        Debug.Log(canvas);
    }

    public void CloseCanvas()
    {
        canvas.SetActive(false);
        BuildMenu.SetActive(false);
    }
    public void OpenMenu(Hex hex)
    {
        currentHex = hex;
        BuildMenu.SetActive(true);
        
    }

    public void ClickBuild()
    {
        hexMngr.GenerateHouse();
        CloseCanvas();
    }

    public void ClickMove()
    {
        hexMngr.MovePlayer();
        CloseCanvas();
    }
    
}