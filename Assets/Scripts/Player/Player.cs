using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float outerSize;
    public float innerSize;
    public float height;
    public Material material;

    private GameObject player;
    private Hex playerHex;
    public Hex currentHex;

    public void Spawn(HexManager HM)
    {
        player = new GameObject($"PlayerHex", typeof(Hex));
        playerHex = player.GetComponent<Hex>();

        playerHex.outerSize = outerSize;
        playerHex.innerSize = innerSize;
        playerHex.height = height;
        playerHex.HM = HM;
        playerHex.SetMaterial(material);

        playerHex.DrawMesh();
        player.transform.SetParent(transform);
    }

    public void Move(Hex hex)
    {
        currentHex = hex;
        transform.position = hex.GetPosition();
    }

    public bool CanMove(Hex hex)
    {
        return hex.biome.isPathable;
    }
}