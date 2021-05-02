namespace AI
{
    public abstract class State
    {
        protected AISystem AISystem;
        //public AISystem GetAISystem() => AISystem;

        public State(AISystem aiSystem)
        {
            AISystem = aiSystem;
        }

        public virtual void Enter()
        {
            AISystem.CurrentState = AISystem.GetStateType().ToString();
        }
        public virtual void Update()
        {
            AISystem.AnimateWalking();
        }
        public virtual void Exit()
        {

        }
    }
}
