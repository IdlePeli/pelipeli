using UnityEngine;

public class Player : MonoBehaviour
{
    public float outerSize;
    public float innerSize;
    public float height;
    public Material material;
    public Hex currentHex;

    private GameObject _player;
    private Hex _playerHex;



    public void Spawn(HexManager hexManager)
    {
        _player = new GameObject("PlayerHex", typeof(Hex));
        _playerHex = _player.GetComponent<Hex>();

        _playerHex.outerSize = outerSize;
        _playerHex.innerSize = innerSize;
        _playerHex.height = height;
        _playerHex.HexManager = hexManager;
        _playerHex.SetMaterial(material);

        _playerHex.DrawMesh();
        _player.transform.SetParent(transform);
    }

    public void Move(Hex hex)
    {
        currentHex = hex;
        transform.position = hex.GetCeilingPosition() + new Vector3(0, 0.15f, 0);
    }

    public bool CanMove(Hex hex)
    {
        return hex.biome.isPathable;
    }
}
