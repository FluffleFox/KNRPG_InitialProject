using UnityEngine;
using Cinemachine;

public class SwitchCameraOnGameState : MonoBehaviour
{

    public CinemachineVirtualCamera[] virtualcameras;
    private void Start()
    {
        ResetCameraPriority();
        GameManager.instance.SwitchingToFreeMove.AddListener(SwitchToCameraFreeMove);
        GameManager.instance.SwitchingToPlanning.AddListener(SwitchToCameraPlanning);
        GameManager.instance.SwitchingToFighting.AddListener(SwitchToCameraFighting);
        GameManager.instance.SwitchingToSummary.AddListener(SwitchToCameraSummary);
    }

    public void SwitchToDefaultCamera()
    {
        ResetCameraPriority();
        virtualcameras[0].Priority = 5;
    }

    public void SwitchToCameraFreeMove()
    {
        ResetCameraPriority();
        virtualcameras[1].Priority = 5;
    }
    
    public void SwitchToCameraPlanning()
    {
        ResetCameraPriority();
        virtualcameras[2].Priority = 5;
    }
    
    public void SwitchToCameraFighting()
    {
        ResetCameraPriority();
        virtualcameras[3].Priority = 5;
    }
    
    public void SwitchToCameraSummary()
    {
        ResetCameraPriority();
        virtualcameras[4].Priority = 5;
    }

    private void ResetCameraPriority()
    {
        foreach (var virtualcamera in virtualcameras)
        {
            virtualcamera.Priority = 1;
        }
    }

}
