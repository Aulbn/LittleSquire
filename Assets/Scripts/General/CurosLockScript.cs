using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurosLockScript : MonoBehaviour
{

   [SerializeField]private bool shouldLockMouse = false;

    void Awake()
    {
        CursorLockHandler();
    }

    public void CursorLockHandler()
    {
        if (shouldLockMouse)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }


}
