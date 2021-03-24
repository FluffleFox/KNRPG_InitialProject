using UnityEngine;

public class GameLoopMenager : MonoBehaviour
{
    public static GameLoopMenager instance;
    public enum Phase { Prepare, Move, Fight}
    public Phase currentPhase = Phase.Prepare;
    void Start()
    {
        if (instance != null) { Destroy(instance); }
        instance = this;
    }

    public void NextPhase()
    {
        switch (currentPhase)
        {
            case Phase.Prepare: { currentPhase = Phase.Move; break; }
            case Phase.Move: { currentPhase = Phase.Fight; break; }
            case Phase.Fight: { currentPhase = Phase.Prepare; break; }
        }
    }
}
