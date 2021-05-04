using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("State Machine")]
    [SerializeField] private State[] _states;
    private readonly Dictionary<Type, State> _stateDictionary = new Dictionary<Type, State>();
    public State CurrentState;

    /*[HideInInspector] */public Vector3 Velocity;

    private bool runEnter = false;

    public void Awake()
    {
        foreach (State state in _states)
        {
            State instance = Instantiate(state);
            instance.Controller = this;
            instance.Initialize(this);
            _stateDictionary.Add(instance.GetType(), instance);

            if (CurrentState != null) continue;

            CurrentState = instance;
            CurrentState.Enter();
        }
    }
    private void Update()
    {
        if (runEnter)
        {
            CurrentState.Enter();
            runEnter = false;
        }
        if (CurrentState != null) CurrentState.Update();
    }
    public T GetState<T>()
    {
        Type type = typeof(T);
        if (!_stateDictionary.ContainsKey(type)) throw new NullReferenceException("No state of type: " + type + " found");
        return (T)Convert.ChangeType(_stateDictionary[type], type);
    }

    public void TransitionTo<T>(bool runStartThisFrame = false, bool runUpdate = false)
    {
        if (CurrentState != null) CurrentState.Exit();
        CurrentState = GetState<T>() as State;
        if (CurrentState == null) return;
        if (runStartThisFrame)
            CurrentState.Enter();
        else
            runEnter = true;
        if (runUpdate) CurrentState.Update();
    }
}
