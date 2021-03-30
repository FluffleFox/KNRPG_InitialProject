using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnGameState : MonoBehaviour
{
    public GameStateMachine.GameState state;
    public GameObject gameObjectToEnable;

    private void Start()
    {
        switch(state)
        {
            case GameStateMachine.GameState.MainMenu:
        }
    }

}
