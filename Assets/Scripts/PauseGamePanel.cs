using System;
using UnityEngine;

public class PauseGamePanel : MonoBehaviour
{
    private Action onResumeCallback;
    private Action onPauseCallback;
    [SerializeField] private GameObject pausePanel;

    public void Init(Action onPauseCallback, Action onResumeCallback)
    {
        this.onResumeCallback = onResumeCallback;
        this.onPauseCallback = onPauseCallback;
    }

    public void OnPauseClicked()
    {
        pausePanel.SetActive(true);
        onPauseCallback?.Invoke();
    }

    public void OnResumeClicked()
    {
        pausePanel.SetActive(false);
        onResumeCallback?.Invoke();
    }
}
