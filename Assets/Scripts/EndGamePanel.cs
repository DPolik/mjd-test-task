using System;
using TMPro;
using UnityEngine;

public class EndGamePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreValueText;
    [SerializeField] private TextMeshProUGUI timeValueText;

    private Action onRestartCallback; 

    public void Init(int score, float timeLeft, Action onRestartCallback)
    {
        scoreValueText.text = score.ToString();
        var time = timeLeft + 1;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        timeValueText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        this.onRestartCallback = onRestartCallback;
    }

    public void OnRestartClicked()
    {
        onRestartCallback?.Invoke();
    }
}
