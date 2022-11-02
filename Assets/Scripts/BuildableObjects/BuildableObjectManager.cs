using UnityEngine;

public class BuildableObjectManager : MonoBehaviour
{
    public BuildableObject[] BuildableObjects;
    public Player player;
    public MenuManager MenuMgr;
    
    public BuildableObject GetObject()
    {
        return Instantiate(BuildableObjects[0]);
    }
    
}