using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private Text timerText = null;

    private bool timerRun = false;
    private System.Action callback = null;
    private System.TimeSpan remainingTime = new System.TimeSpan();

    public void TimerStart(System.TimeSpan timeSpan, System.Action callback)
    {
        this.callback = callback;
        this.remainingTime = timeSpan;
        timerRun = true;
        timerText.gameObject.SetActive(true);
        SetTime(remainingTime);
    }

    void Update()
    {
        if (timerRun)
        {
            float spendTime = Time.deltaTime;
            float remainigTimeSeconds = remainingTime.Seconds + remainingTime.Milliseconds / 1000.0f - spendTime;
            if (remainigTimeSeconds < 0.0f)
            {
                timerRun = false;
                timerText.gameObject.SetActive(false);
                callback();
            }
            else
            {
                remainingTime = System.TimeSpan.FromSeconds(remainigTimeSeconds);
                SetTime(remainingTime);
            }
        }
    }

    private void SetTime(System.TimeSpan time)
    {
        timerText.text = $"<size=160>残り</size>\n{time.Seconds.ToString("D2")}<size=160>\"{Mathf.FloorToInt(time.Milliseconds / 10).ToString("D2")}</size>";
    }
}
