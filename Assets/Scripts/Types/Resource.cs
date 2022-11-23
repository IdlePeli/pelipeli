using UnityEngine;

public class Resource : MonoBehaviour
{
    public GameObject model;
    public int rarity;
    private int hp;
    
    public int CalculatedRarityScore { set; get; }
}