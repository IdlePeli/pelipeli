using System;
using UnityEngine;

public class WorldTime : MonoBehaviour
{
    public static Action OnMinuteChanged;
    public static Action OnDayChanged;
    public static Action OnHourChanged;

    public float timeSpeed = 0.1f;

    private float _timer = 0.5f;
    public static int Minute { get; private set; }
    public static int Hour { get; private set; }
    public static int Day { get; private set; } = 1;
    public static int Year { get; private set; }

    private void FixedUpdate()
    {
        _timer += 0.02f;

        if (!(_timer >= timeSpeed)) return;
        Minute++;
        OnMinuteChanged?.Invoke();
        _timer = 0;

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