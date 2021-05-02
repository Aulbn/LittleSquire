using Cinemachine;
using UnityEngine;

public class CinematicCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera boxCamera;
    public void ActivateCamera()
    {
        boxCamera.Priority = 15; // change prio for the camera to take effect
    }

    public void DeactivateCamera()
    {
        boxCamera.Priority = 0; // change prio for the camera to return to defualt
    }
}
