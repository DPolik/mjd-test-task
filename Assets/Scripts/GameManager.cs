using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameBoard gameBoard;
    private int currentScore = 0;
    private float lastMatchTime = 0;
    private int scoreMultiplier = 1;
    private Action onTimerEndedCallback;
    private Action<float> onTimeStepCallback;
    private bool timerIsPaused = false;
    private Coroutine timerRoutine = null;
    private float currentTime;

    [Header("Prefabs")]
    [SerializeField] private GameObject gameBoardPrefab;
    [Header("Scene References")]
    [SerializeField] private UIManager uiManager;
    [Header("Game Settings")]
    [SerializeField] private int boardHeight = 4;
    [SerializeField] private int boardWidth = 4;
    [SerializeField] private int boardLength = 4;
    [SerializeField] private int timeInSeconds = 60*5;
    [SerializeField] private int cellTypesAmount = 6;
    [SerializeField] private int matchScore = 100;

    // Start is called before the first frame update
    void Start()
    {
        // init board state
        gameBoard = Instantiate(gameBoardPrefab).GetComponent<GameBoard>();
        gameBoard.Init(boardHeight, boardWidth, boardLength, cellTypesAmount);
        gameBoard.onMatchedCells += OnMatchedCells;
        gameBoard.onGameFinished += OnGameFinished;
        gameBoard.ShowBoard();
        gameBoard.StartGame();

        // init ui
        uiManager.Init(OnTimerEnded, OnPauseClicked, OnResumeClicked);

        // init timer
        InitTimer(OnTimerEnded, OnTimeStep);
        StartTimer();
    }

    private void OnResumeClicked()
    {
        PauseTimer(false);
        gameBoard.StartGame();
    }

    private void OnPauseClicked()
    {
        PauseTimer(true);
        gameBoard.StopGame();
    }

    private void OnTimeStep(float time)
    {
        uiManager.DisplayTime(time);
    }

    private void OnMatchedCells()
    {
        if(Time.realtimeSinceStartup - lastMatchTime <= 3)
        {
            scoreMultiplier++;
        }
        else
        {
            scoreMultiplier = 1;
        }
        currentScore += matchScore * scoreMultiplier;
        uiManager.UpdateScore(currentScore);
        lastMatchTime = Time.realtimeSinceStartup;
    }

    private void OnGameFinished()
    {
        EndGame();
    }

    private void OnTimerEnded()
    {
        EndGame();
    }

    private void EndGame()
    {
        var timeEnded = currentTime;
        ResetTimer();
        gameBoard.StopGame();
        uiManager.ShowEndGamePanel(currentScore, timeEnded, OnRestartClicked);
        currentScore = 0;
    }

    private void OnRestartClicked()
    {
        // reset board state
        gameBoard.Reset();
        gameBoard.StartGame();

        // start time left
        StartTimer();

        // reset score
        scoreMultiplier = 1;
        lastMatchTime = 0;

        // reset ui
        uiManager.Reset();
    }
    
    private void InitTimer(Action onTimerEndedCallback, Action<float> onTimeStepCallback)
    {
        this.onTimerEndedCallback = onTimerEndedCallback;
        this.onTimeStepCallback = onTimeStepCallback;
    }

    private void StartTimer()
    {
        currentTime = timeInSeconds;
        timerRoutine = StartCoroutine(RunTimer());
    }

    private void PauseTimer(bool pause)
    {
        timerIsPaused = pause;
    }

    private void ResetTimer()
    {
        StopCoroutine(timerRoutine);
    }

    private IEnumerator RunTimer()
    {
        while (currentTime >= 0)
        {
            if (!timerIsPaused)
            {
                currentTime -= Time.deltaTime;
                onTimeStepCallback?.Invoke(currentTime);
            }
            yield return 0;
        }

        onTimerEndedCallback?.Invoke();
    }
}
