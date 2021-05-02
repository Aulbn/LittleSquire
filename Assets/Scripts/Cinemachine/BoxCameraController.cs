using UnityEngine;
using Cinemachine;

public class BoxCameraController : MonoBehaviour
{
    public CinemachineVirtualCamera boxCamera;
    public Vector3 pushPositiveXcam;
    public Vector3 pushNegativeXcam;
    public Vector3 pushPositiveZcam;
    public Vector3 pushNegativeZcam;

    public void ActivateCamera()
    {
        var transposer = boxCamera.GetCinemachineComponent<CinemachineTransposer>();
        transposer.m_FollowOffset = pushPositiveZcam; //Example of how to change camera pos

        boxCamera.Priority = 11; // change prio for the camera to take effect
    }

    public void DeactivateCamera()
    {
        boxCamera.Priority = 0; // change prio for the camera to return to defualt
        Debug.LogWarning("test");
    }

}
