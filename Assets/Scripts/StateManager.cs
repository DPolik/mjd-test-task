using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StateManager : MonoBehaviour
{
    public enum GameState
    {
        Initialisation,
        Intro,
        Game
    }

    private Dictionary<GameState, string> stateList = new Dictionary<GameState, string> {
        { GameState.Intro, "Intro" },
        { GameState.Game, "Game" }
    };

    [SerializeField] private GameState currentState = GameState.Initialisation;

    public static StateManager Instance { get; private set; }
   
    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        SetupState(GameState.Intro);
    }

    private void SetupState(GameState state)
    {
        ShowGame(state, stateList[state]);
    }

    private void ShowGame(GameState state, string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        currentState = state;
    }

    public void SwitchToState(GameState state)
    {
        SetupState(state);
    }
}
