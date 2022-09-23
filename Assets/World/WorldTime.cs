using System;
using System.Diagnostics;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
    public static Action OnMinuteChanged;
    public static Action OnDayChanged;
    public static Action OnHourChanged;

    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int Day { get; private set; }
    public static int Year { get; private set; }

    private float _minuteToRealTime = 0.5f;
    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        Minute = 50;
        Hour = 23;
        Day = 1;
        Year = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _timer -= Time.deltaTime;

        if (!(_timer <= 0)) return;
        Minute++;
        OnMinuteChanged?.Invoke();
        _timer = _minuteToRealTime;
        
        if (!(Minute >= 60)) return;
        Minute = 0;
        Hour++;
        if (Hour == 24)
        {
            Hour = 0;
            Day++;
            OnDayChanged?.Invoke();
        }
        OnHourChanged?.Invoke();

        if (!(Day >= 366)) return;
        Day = 1;
        Year++; 
        
    }
}