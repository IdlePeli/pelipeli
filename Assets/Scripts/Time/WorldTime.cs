using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
    public static Action OnMinuteChanged;
    public static Action OnDayChanged;
    public static Action OnHourChanged;
    public float time;
    
    public LightingManager LightingManager;
    public float timeSpeed = 0.1f;

    private float _timer = 0.5f;
    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int Day { get; private set; } = 1;
    public static int Year { get; private set; }

    public static float dayMinutes;

    private void Update()
    {
        time += Time.deltaTime * timeSpeed;
        
        if (!(time >= 10)) return;

        Minute++;
        time = 0;
        OnMinuteChanged?.Invoke();
        dayMinutes++;
        if (!(Minute >= 60)) return;
        Minute = 0;
        Hour++;
        if (Hour == 24)
        {
            Hour = 0;
            Day++;
            dayMinutes = 0;
            OnDayChanged?.Invoke();
        }

        OnHourChanged?.Invoke();

        if (!(Day >= 366)) return;
        Day = 1;
        Year++;
    }

    // return time of day float
    public static float GetTime()
    {
        return dayMinutes / 60f;
    }
}
