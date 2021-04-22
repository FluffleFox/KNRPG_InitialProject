using UnityEngine;
using Cinemachine;
    public class ShakeCameraOnGameState : MonoBehaviour
    {
        public CinemachineImpulseSource source;

        private void Start()
        {
            //TODO
            // add places when you want camera to shake
        }

        private void ShakeCamera()
        {
            source.GenerateImpulse();
        }
    }
