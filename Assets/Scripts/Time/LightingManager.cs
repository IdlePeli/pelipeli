using System;
using System.Net.Mail;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    public Light directionalLight;
    public GameObject lightRotator;
    private Vector3 rotation;
    private float lightlevel;
    private float time;
    private float intensity;
    public float lightY;
    
    //Default
    public float nightTypeAdjusterA = 8.7f;
    public float nightLengthAdjusterB = 1.4f;
    public float brightnessCap = 1;
    public float nightlevel = 0.2f;

    public bool Preset1;
    public bool Preset2;
    public bool Preset3;
    
    public void Awake()
    {
        if (Preset1)
        {
            // Normal/Default
            nightTypeAdjusterA = 8.7f;
            nightLengthAdjusterB = 1.4f;
            brightnessCap = 1;
            nightlevel = 0.2f;
        } else if (Preset2)
        {
            //Short nights
            nightTypeAdjusterA = 1.4f;
            nightLengthAdjusterB = 8.2f;
            brightnessCap = 1;
            nightlevel = 0.2f;
        } else if (Preset3)
        {
            //Short days
            nightTypeAdjusterA = 9.5f;
            nightLengthAdjusterB = 0.5f;
            brightnessCap = 1;
            nightlevel = 0f;
        }
    }
    
    private void Update()
    {
        time = WorldTime.GetTime();
        if (time < 12 )
        {
            intensity = (time - 12) / nightTypeAdjusterA + nightLengthAdjusterB;
        }
        else
        {
            intensity = -((time - 12) / nightTypeAdjusterA) + nightLengthAdjusterB;
        }
        // https://www.desmos.com/calculator/30hjpkahcx

        if (intensity > brightnessCap)
        {
            intensity = brightnessCap;
        } else if (intensity < nightlevel)
        {
            intensity = nightlevel;
        }

        directionalLight.intensity = intensity;

        lightY = 50 * intensity + 10;
        
        lightRotator.transform.rotation = Quaternion.Euler(0, time*15, 0);
        directionalLight.transform.localPosition = (new Vector3(60, lightY, 0));
        directionalLight.transform.LookAt(lightRotator.transform.position);
    }
}
