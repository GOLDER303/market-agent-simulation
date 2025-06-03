using UnityEngine;
using TMPro;

public class TimeScale : MonoBehaviour
{
    public TextMeshProUGUI timeScaleText;

    public void ChangeTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
        timeScaleText.text = $"Speed: x{newTimeScale}";
    }
}
