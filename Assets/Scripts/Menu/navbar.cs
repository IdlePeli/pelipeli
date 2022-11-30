using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class navbar : MonoBehaviour
{
    
    [SerializeField] GameObject[] panels;
    public GameObject resourceCanvas;
   
    public void navbarclick(GameObject activePanel)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        activePanel.SetActive(true);
    }
    public void OpenResourceMenu()
    {
        resourceCanvas.SetActive(!resourceCanvas.activeSelf);
    }
}

