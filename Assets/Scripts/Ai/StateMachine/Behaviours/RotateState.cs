namespace AI
{
    internal class RotateState : State
    {
        private State nextState;
        public RotateState(AISystem aiSystem, State targetState) : base(aiSystem)
        {
            nextState = targetState;
        }
        public override void Enter()
        {
            AISystem.AnimationHandler.TurnTowards(AISystem.FocusPoint);
            //AISystem.TurnTowards(AISystem.FocusPoint, () => AISystem.SetState(nextState));
            base.Enter();
        }
        public override void Update()
        {
            if (nextState.GetType() == typeof(IdleState))
                AISystem.CheckDistanceToTarget();

            if (AISystem.IsLookingAtPoint(AISystem.FocusPoint))
            {
                AISystem.SetState(nextState);
                return;
            }
            else
                AISystem.RotateTowards(AISystem.FocusPoint);

            base.Update();
        }
    }
}