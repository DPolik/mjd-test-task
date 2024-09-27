using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Scene References")]
    [SerializeField] private TextMeshProUGUI timerView;
    [SerializeField] private TextMeshProUGUI scoreView;
    [SerializeField] private EndGamePanel endPanel;
    [SerializeField] private PauseGamePanel pausePanel;

    public void Init(Action onTimerEnded, Action onPauseClicked, Action onResumeClicked)
    {
        Reset();
        // init pause button
        pausePanel.Init(onPauseClicked, onResumeClicked);
    }

    public void Reset()
    {
        // init timer
        DisplayTime(0);

        // init score
        scoreView.text = "0";

        endPanel.gameObject.SetActive(false);
    }

    public void UpdateScore(int currentScore)
    {
        scoreView.text = currentScore.ToString();
    }

    public void DisplayTime(float timeInSeconds)
    {
        var time = timeInSeconds + 1;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timerView.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ShowEndGamePanel(int currentScore, float timeEnded, Action onRestartClickedCallback)
    {
        endPanel.gameObject.SetActive(true);
        endPanel.Init(currentScore, timeEnded, onRestartClickedCallback);
    }
}
