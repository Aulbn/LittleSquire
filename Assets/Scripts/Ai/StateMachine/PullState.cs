namespace AI
{
    internal class PullState : State
    {
        private PuzzleLeverController puzzleLeverController;
        public PullState(AISystem aiSystem, PuzzleLeverController controller) : base(aiSystem)
        {
            puzzleLeverController = controller;
        }
        public override void Enter()
        {
            AISystem.AnimationHandler.StopLookAt();
            AISystem.AnimationHandler.PullLever();
            puzzleLeverController.StartPullAnimation(PuzzleLeverAnimationHandler.Puller.Cedric);
            //AISystem.SetState(new IdleState(AISystem));
            base.Enter();
        }
    }
}