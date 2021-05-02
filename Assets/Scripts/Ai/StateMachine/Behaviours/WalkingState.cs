using UnityEngine;

namespace AI
{
    internal class WalkingState : State
    {
        private State nextState;
        public WalkingState(AISystem aiSystem) : base(aiSystem)
        {
            nextState = null;
        }
        public WalkingState(AISystem aiSystem, State targetState) : base(aiSystem)
        {
            nextState = targetState;
        }

        public override void Update()
        {
            if (nextState == null)
                AISystem.CheckDistanceToTarget();
            else
                AISystem.SetMoveSpeed();

            if (AISystem.transform.position == new Vector3(AISystem.MoveToPosition.x,
                AISystem.transform.position.y,
                AISystem.MoveToPosition.z))
                AISystem.SetState(new RotateState(AISystem, nextState == null ? new IdleState(AISystem) : nextState));

            base.Update();
        }
    }
}