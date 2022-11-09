using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowGameResources : MonoBehaviour
{
    private void Awake()
    {
        GameResources.OnStoneAmountChanged += delegate(object sender, EventArgs e)
        {
            UpdateResourceTextObject();
        };
        UpdateResourceTextObject();
        
        GameResources.OnWoodAmountChanged += delegate(object sender, EventArgs e)
        {
            UpdateResourceTextObject();
        };
        UpdateResourceTextObject();
    }

    private void UpdateResourceTextObject()
    {
        transform.Find("StoneAmount").GetComponent<Text>().text = "" + GameResources.GetStoneAmount();
        transform.Find("WoodAmount").GetComponent<Text>().text = "" + GameResources.GetWoodAmount();
    }
    
}
