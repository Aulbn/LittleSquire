using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCursor : MonoBehaviour
{
    void Update()
    {
        if (Cursor.visible) return;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnDisable()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
