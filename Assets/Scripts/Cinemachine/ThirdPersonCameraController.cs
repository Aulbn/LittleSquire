using Cinemachine;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera Vcam;
    private CinemachinePOV VcamPOV;

    private void Start()
    {
        Vcam = GetComponent<CinemachineVirtualCamera>();
        VcamPOV = Vcam.GetCinemachineComponent<CinemachinePOV>();
    }

    public void ResetCameraRotation()
    {
        VcamPOV.m_HorizontalAxis.Value = 0;
        VcamPOV.m_VerticalAxis.Value = 0;
    }

    public void SetCameraXValue(float xValue)
    {
        VcamPOV.m_HorizontalAxis.Value = xValue;
    }
    public void SetCameraYValue(float yValue)
    {
        VcamPOV.m_VerticalAxis.Value = yValue;
    }

    public void SetInheritPosition(bool inheritPos)
    {
        Vcam.m_Transitions.m_InheritPosition = inheritPos;
    }
}
