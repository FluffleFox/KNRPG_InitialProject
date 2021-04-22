using UnityEngine;


    public class PlaySoundOnStage : MonoBehaviour
    {
        public GameStateMachine.GameState state;
        public AudioSource source;
        private const float Delay = 0.5f;

        private void Start()
        {
            switch (state)
            {
                case GameStateMachine.GameState.FreeMove:
                {
                    GameManager.instance.SwitchingToFreeMove.AddListener(PlaySound);
                    GameManager.instance.QuittingFreeMove.AddListener(StopSound);
                    break;
                }
                case GameStateMachine.GameState.Planning:
                {
                    GameManager.instance.SwitchingToPlanning.AddListener(PlaySound);
                    GameManager.instance.QuittingPlanning.AddListener(StopSound);
                    break;
                }
                case GameStateMachine.GameState.Fighting:
                {
                    GameManager.instance.SwitchingToFighting.AddListener(PlaySound);
                    GameManager.instance.QuittingFighting.AddListener(StopSound);
                    break;
                }
                case GameStateMachine.GameState.Summary:
                {
                    GameManager.instance.SwitchingToSummary.AddListener(PlaySound);
                    GameManager.instance.QuittingSummary.AddListener(StopSound);
                    break; 
                }
            }
        }

        private void PlaySound()
        {
            if (!source.gameObject.activeSelf) return;
            source.PlayDelayed(Delay); 
            Debug.Log("Now playing:" + source.name);
        }

        private void StopSound()
        {
            if(source.gameObject.activeSelf) {source.Stop();}
        }
    }