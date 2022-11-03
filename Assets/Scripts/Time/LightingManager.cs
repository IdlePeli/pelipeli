using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public Light directionalLight;
    public GameObject lightRotator;
    private Vector3 rotation;
    private float lightlevel;
    private float time;
    private float intensity;
    public float worldTime = 0; // for debugging
    public float nightLengthAdjuster = -1.5f;
    public float brightnessCap = 1;
    private float lightY;
    private void Update()
    {
        time = WorldTime.GetTime();
        //time = worldTime;
        if (time < 12)
        {
            intensity = 1 - (12 - time * -nightLengthAdjuster) / 12;
        }
        else
        {
            intensity = 1+(12 - time * nightLengthAdjuster + (nightLengthAdjuster * 24)) 
                / 12 * -1;
        }
        // https://www.desmos.com/calculator/rhwsjcu1pt
        // a = nightLengthAdjuster
        // lower value for shorter nights 

        if (intensity > brightnessCap)
        {
            intensity = brightnessCap;
        } 

        directionalLight.intensity = intensity;

        lightY = 50 * intensity + 10;
        
        lightRotator.transform.rotation = Quaternion.Euler(0, time*15, 0);
        directionalLight.transform.localPosition = (new Vector3(60, lightY, 0));
        directionalLight.transform.LookAt(lightRotator.transform.position);
    }
}
