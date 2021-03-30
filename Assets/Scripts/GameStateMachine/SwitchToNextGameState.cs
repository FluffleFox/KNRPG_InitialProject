using UnityEngine;

public class SwitchToNextGameState : MonoBehaviour
{
    public void SwitchToNext()
    {
        GameManager.instance.SwitchToNextState();
    }
}
