using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public void OnPlayClicked()
    {
        StateManager.Instance.SwitchToState(StateManager.GameState.Game);
    }
}
