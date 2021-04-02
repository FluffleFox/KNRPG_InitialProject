using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableGameObjectOnStateQuit : MonoBehaviour
{
    public GameStateMachine.GameState state;
    public GameObject gameObjectToEnable;

    private void Start()
    {
        switch (state)
        {
            case GameStateMachine.GameState.MainMenu:
                GameManager.instance.QuittingMainMenu.AddListener(EnableGameObject);
                break;
            case GameStateMachine.GameState.Planning:
                GameManager.instance.QuittingPlanning.AddListener(EnableGameObject);
                break;
            case GameStateMachine.GameState.Fighting:
                GameManager.instance.QuittingFighting.AddListener(EnableGameObject);
                break;
            case GameStateMachine.GameState.Summary:
                GameManager.instance.QuittingSummary.AddListener(EnableGameObject);
                break;
        }
    }

    void EnableGameObject()
    {
        gameObjectToEnable.SetActive(true);
        Debug.Log("Enabled: " + gameObjectToEnable.name);
    }
}
