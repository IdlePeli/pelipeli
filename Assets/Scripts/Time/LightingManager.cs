using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public Light directionalLight;
    public GameObject lightRotator;
    private Vector3 rotation;
    private float lightlevel;
    private float time;
    private float intensity;
    private void Update()
    {
        time = WorldTime.GetTime();
        if (time > 12)
        {
            intensity = 1 - (12 - time) / time;
        }
        else
        {
            intensity = 1-((12 - time) / 12 * -1);
        }

        directionalLight.intensity = intensity;
        
        lightlevel = WorldTime.GetTime() / 12;
       
        directionalLight.intensity = lightlevel;
        lightRotator.transform.rotation = Quaternion.Euler(0, WorldTime.GetTime()*15, 0);
        directionalLight.transform.LookAt(lightRotator.transform.position);
    }
}
