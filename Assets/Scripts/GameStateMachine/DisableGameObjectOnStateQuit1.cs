using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableGameObjectOnStateQuit1 : MonoBehaviour
{
    public GameStateMachine.GameState state;
    public GameObject gameObjectToDisable;

    void Start()
    {
        switch (state)
        {
            case GameStateMachine.GameState.FreeMove:
                GameManager.instance.QuittingFreeMove.AddListener(DisableGameObject);
                break;
            case GameStateMachine.GameState.Planning:
                GameManager.instance.QuittingPlanning.AddListener(DisableGameObject);
                break;
            case GameStateMachine.GameState.Fighting:
                GameManager.instance.QuittingFighting.AddListener(DisableGameObject);
                break;
            case GameStateMachine.GameState.Summary:
                GameManager.instance.QuittingSummary.AddListener(DisableGameObject);
                break;
        }
    }

    void DisableGameObject()
    {
        gameObjectToDisable.SetActive(false);
        Debug.Log("Disabled: " + gameObjectToDisable.name);
    }
}
