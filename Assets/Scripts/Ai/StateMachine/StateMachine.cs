using System;
using UnityEngine;

namespace AI
{
    public abstract class StateMachine : MonoBehaviour
    {
        protected State state;
        public Type GetStateType() => state.GetType();

        public void SetState(State newState)
        {
            state = newState;
            state.Enter();
        }

    }
}
