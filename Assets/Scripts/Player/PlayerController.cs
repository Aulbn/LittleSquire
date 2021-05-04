using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    public PlayerControls Controls { get; private set; }
    public CharacterController cc { get; private set; }
    [Header("References")]
    public Camera mainCamera;
    public AnimationHandler anim;

    public bool HasControl { get { return inputLocks == 0; } }
    private int inputLocks;

    private new void Awake()
    {
        Controls = new PlayerControls();
        cc = GetComponent<CharacterController>();
        if (!mainCamera) mainCamera = FindObjectOfType<Camera>();
        base.Awake();
    }
}
