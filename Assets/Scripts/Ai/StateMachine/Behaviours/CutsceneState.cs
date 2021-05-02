namespace AI
{
    internal class CutsceneState : State
    {
        public CutsceneState(AISystem aiSystem) : base(aiSystem)
        {
        }
        public override void Enter()
        {
            AISystem.AnimationHandler.StopLookAt();
            base.Enter();
        }

        public override void Update()
        {
            AISystem.CheckDistanceToTarget();
            base.Update();
        }
    }
}
