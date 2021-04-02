using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjectOnStateSwitch : MonoBehaviour
{
    public GameStateMachine.GameState state;
    public GameObject gameObjectToDisable;

    void Start()
    {
        switch (state)
        {
            case GameStateMachine.GameState.MainMenu:
                GameManager.instance.SwitchingToMainMenu.AddListener(DisableGameObject);
                break;
            case GameStateMachine.GameState.Planning:
                GameManager.instance.SwitchingToPlanning.AddListener(DisableGameObject);
                break;
            case GameStateMachine.GameState.Fighting:
                GameManager.instance.SwitchingToFighting.AddListener(DisableGameObject);
                break;
            case GameStateMachine.GameState.Summary:
                GameManager.instance.SwitchingToSummary.AddListener(DisableGameObject);
                break;
        }
    }

    void DisableGameObject()
    {
        gameObjectToDisable.SetActive(false);
        Debug.Log("Disabled: " + gameObjectToDisable.name);
    }
}
