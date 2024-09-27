using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private Action onTimerEndedCallback;
    private bool timerIsPaused = false;
    private Coroutine timerRoutine = null;

    [SerializeField] private TextMeshProUGUI timerView;

    public int MaxTimeInSeconds { get; set; }
    public float CurrentTime { get; private set; }

    public void InitTimer(int maxTimeInSeconds, Action onTimerEndedCallback)
    {
        MaxTimeInSeconds = maxTimeInSeconds;
        this.onTimerEndedCallback = onTimerEndedCallback;
    }

    public void StartTimer()
    {
        CurrentTime = MaxTimeInSeconds;
        timerRoutine = StartCoroutine(RunTimer());
    }

    public void PauseTimer(bool pause)
    {
        timerIsPaused = pause;
    }

    public void ResetTimer()
    {
        StopCoroutine(timerRoutine);
        DisplayTime();
    }

    private IEnumerator RunTimer()
    {
        while(CurrentTime >= 0)
        {
            if(!timerIsPaused)
            {
                CurrentTime -= Time.deltaTime;
                DisplayTime();
            }
            yield return 0;
        }

        onTimerEndedCallback?.Invoke();
    }

    private void DisplayTime()
    {
        var time = CurrentTime + 1;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timerView.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
