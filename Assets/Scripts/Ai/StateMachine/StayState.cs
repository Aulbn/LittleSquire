namespace AI
{
    internal class StayState : State
    {
        public StayState(AISystem aiSystem) : base(aiSystem)
        {
        }
        public override void Enter()
        {
            AISystem.SetFocusPoint(AISystem.FollowTargetFocusPoint);
            base.Enter();
        }
        public override void Update()
        {

        }
    }
}