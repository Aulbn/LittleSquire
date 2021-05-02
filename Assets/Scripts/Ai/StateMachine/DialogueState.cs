namespace AI
{
    internal class DialogueState : State
    {
        public DialogueState(AISystem aiSystem) : base(aiSystem)
        {
        }
        public override void Enter()
        {
            AISystem.DialogueTrigger.TriggerRandomDialogue();
            AISystem.SetState(new IdleState(AISystem));
            base.Enter();
        }
    }
}