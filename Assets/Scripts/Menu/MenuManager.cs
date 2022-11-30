using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject WoodCanvas;
    public GameObject FishCanvas;
    public GameObject StoneCanvas;
    public GameObject canvas = null;
    public GameObject BuildMenu;
    public HexManager hexMngr;
    public Hex currentHex;

    public Player Player;
    
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI woodText;

    
    public GameObject ResourceMenu;
    
    public void AddWoodAmount(int amount)
    {
        Player.WoodAmount += amount;
        woodText.text = "" + Player.WoodAmount;
    }

    public void AddStoneAmount(int amount)
    {
        Player.StoneAmount += amount;
        stoneText.text = ""+Player.StoneAmount;
    }
    
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

    public void CloseCanvas()
    {
        canvas.SetActive(false);
        BuildMenu.SetActive(false);
    }
    public void OpenMenu(Hex hex)
    {
        currentHex = hex;
        BuildMenu.SetActive(true);
        if (hex.transform.childCount < 1)
        {
            canvas.SetActive(false);
        }
        
    }

    public void ClickBuild()
    {
        hexMngr.GenerateHouse();
        BuildMenu.SetActive(false);
        canvas.SetActive(false);
    }

    public void ClickMove()
    {
        hexMngr.MovePlayer();
        BuildMenu.SetActive(false);
        canvas.SetActive(false);
    }
}