using System;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI TimeText;

    public TextMeshProUGUI DayText;
    
    private void OnEnable()
    {
        WorldTime.OnMinuteChanged += UpdateTime;
        WorldTime.OnHourChanged += UpdateTime;
        WorldTime.OnDayChanged += UpdateDay;
    }

    private void OnDisable()
    {
        WorldTime.OnMinuteChanged -= UpdateTime;
        WorldTime.OnHourChanged -= UpdateTime;
        WorldTime.OnDayChanged -= UpdateDay;
    }

    private void UpdateDay()
    {
        DayText.text = "Day " + WorldTime.Day;
    }
    private void UpdateTime()
    {
        TimeText.text = WorldTime.Hour.ToString("00")  + ":" + WorldTime.Minute.ToString("00");
    }
}
