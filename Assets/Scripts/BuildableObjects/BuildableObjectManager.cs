using UnityEngine;

public class BuildableObjectManager : MonoBehaviour
{
    public BuildableObject[] BuildableObjects;
    
    public BuildableObject GetObject()
    {
        return Instantiate(BuildableObjects[0]);
    }
}