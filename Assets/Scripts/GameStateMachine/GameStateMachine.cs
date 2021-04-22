using UnityEngine;

public class GameStateMachine : MonoBehaviour
{
    public enum GameState
    {
        FreeMove,
        Planning,
        Fighting,
        Summary,
    }

    [SerializeField]
    public GameState currentState;

    public void SwitchToState(GameState state)
    {
        currentState = state;
        Debug.Log("Current State:" + currentState);
    }
}
