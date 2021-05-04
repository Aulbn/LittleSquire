using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State : ScriptableObject
{
    [HideInInspector] public Controller Controller;

    public Transform transform { get { return Controller.transform; } }
    public Vector3 Velocity { get { return Controller.Velocity; } set { Controller.Velocity = value; } }

    public virtual void Initialize(Controller owner) { }
    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
