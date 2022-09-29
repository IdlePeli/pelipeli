using TMPro;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI dayText;

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
        dayText.text = "Day " + WorldTime.Day;
    }

    private void UpdateTime()
    {
        timeText.text = WorldTime.Hour.ToString("00") + ":" + WorldTime.Minute.ToString("00");
    }
}
