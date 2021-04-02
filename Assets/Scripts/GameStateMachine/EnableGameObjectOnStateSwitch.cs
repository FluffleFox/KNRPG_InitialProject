using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectOnStateSwitch : MonoBehaviour
{
    public GameStateMachine.GameState state;
    public GameObject gameObjectToEnable;

    private void Start()
    {
        switch (state)
        {
            case GameStateMachine.GameState.MainMenu:
                GameManager.instance.SwitchingToMainMenu.AddListener(EnableGameObject);
                break;
            case GameStateMachine.GameState.Planning:
                GameManager.instance.SwitchingToPlanning.AddListener(EnableGameObject);
                break;
            case GameStateMachine.GameState.Fighting:
                GameManager.instance.SwitchingToFighting.AddListener(EnableGameObject);
                break;
            case GameStateMachine.GameState.Summary:
                GameManager.instance.SwitchingToSummary.AddListener(EnableGameObject);
                break;
        }
    }

    void EnableGameObject()
    {
        gameObjectToEnable.SetActive(true);
    }
}
