using UnityEngine;

public class SwitchToState : MonoBehaviour
{
    public GameStateMachine.GameState stateToSwitch;

    public void SwitchState()
    {
        GameManager.instance.SwitchToGameState(stateToSwitch);
    }
}
