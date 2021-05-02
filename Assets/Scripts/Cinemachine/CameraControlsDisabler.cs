using Cinemachine;
using UnityEngine;

public class CameraControlsDisabler : MonoBehaviour
{
    private CinemachineBrain brain;
    public bool freezeOnBlend = true;

    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
        brain = Camera.main.GetComponent<CinemachineBrain>();
    }
    public void SetFreezeOnBlend(bool freeze)
    {
        freezeOnBlend = freeze;
        Debug.Log(freezeOnBlend);
    }

    // Custom "overridden" method for CinemachineCore.GetInputAxis
    private float GetAxisCustom(string axisName)
    {
        Debug.Log(freezeOnBlend);
        // return 0 inputs if the cinemachine is blending the camera
        //if (brain.IsBlending && freezeOnBlend)
        //    return 0;

        // if no camera blending is currently done, return the player input data
        return Input.GetAxis(axisName);
    }

}
