using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public UnityEvent OnStageChnage;

    public UnityEvent SwitchingToFreeMove;
    public UnityEvent SwitchingToPlanning;
    public UnityEvent SwitchingToFighting;
    public UnityEvent SwitchingToSummary;


    public UnityEvent QuittingFreeMove;
    public UnityEvent QuittingPlanning;
    public UnityEvent QuittingFighting;
    public UnityEvent QuittingSummary;

    GameStateMachine gameStateMachine;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        gameStateMachine = gameObject.GetComponent<GameStateMachine>();

    }


    public void SwitchToGameState(GameStateMachine.GameState state)
    {
        GameStateMachine.GameState lastState = gameStateMachine.currentState;

        switch (lastState)
        {
            case GameStateMachine.GameState.FreeMove:
                Debug.Log("Quitting" + lastState);
                QuittingFreeMove.Invoke();
                break;
            case GameStateMachine.GameState.Planning:
                Debug.Log("Quitting" + lastState);
                QuittingPlanning.Invoke();
                break;
            case GameStateMachine.GameState.Fighting:
                Debug.Log("Quitting" + lastState);
                QuittingFighting.Invoke();
                break;
            case GameStateMachine.GameState.Summary:
                Debug.Log("Quitting" + lastState);
                QuittingSummary.Invoke();
                break;
        }

        gameStateMachine.SwitchToState(state);

        switch (state)
        {
            case GameStateMachine.GameState.FreeMove:
                Debug.Log("SwitchingTo" + state);
                SwitchingToFreeMove.Invoke();
                break;
            case GameStateMachine.GameState.Planning:
                Debug.Log("SwitchingTo" + state);
                SwitchingToPlanning.Invoke();
                break;
            case GameStateMachine.GameState.Fighting:
                Debug.Log("SwitchingTo" + state);
                SwitchingToFighting.Invoke();
                break;
            case GameStateMachine.GameState.Summary:
                Debug.Log("SwitchingTo" + state);
                SwitchingToSummary.Invoke();
                break;
        }

        OnStageChnage.Invoke();
    }


    public void SwitchToNextState()
    {
        switch(gameStateMachine.currentState)
        {
            case GameStateMachine.GameState.FreeMove:
                SwitchToGameState(GameStateMachine.GameState.Planning);
                break;
            case GameStateMachine.GameState.Planning:
                SwitchToGameState(GameStateMachine.GameState.Fighting);
                break;
            case GameStateMachine.GameState.Fighting:
                SwitchToGameState(GameStateMachine.GameState.Summary);
                break;
            case GameStateMachine.GameState.Summary:
                SwitchToGameState(GameStateMachine.GameState.FreeMove);
                break;
        }
    }
}
