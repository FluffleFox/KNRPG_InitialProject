using UnityEngine;


    public class PlaySoundOnStage : MonoBehaviour
    {
        public GameStateMachine.GameState state;
        public AudioSource source;
        private float delay = 0.5f;

        private void Start()
        {
            switch (state)
            {
                case GameStateMachine.GameState.FreeMove:
                {
                    break;
                }
                case GameStateMachine.GameState.Planning:
                {
                    break;
                }
                case GameStateMachine.GameState.Fighting:
                {
                    break;
                }
                case GameStateMachine.GameState.Summary:
                {
                    break; 
                }
            }
        }

        private void PlaySound()
        {
            if(source.gameObject.activeSelf){source.PlayDelayed(delay);}
        }

        private void StopSound()
        {
            if(source.gameObject.activeSelf){source.Stop();}
        }
    }