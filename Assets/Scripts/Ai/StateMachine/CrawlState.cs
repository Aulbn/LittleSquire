using UnityEngine;

namespace AI
{
    internal class CrawlState : State
    {
        private CrawlController controller;
        private Vector3 startPosition;
        private Vector3 endPosition;

        public CrawlState(AISystem aiSystem, CrawlController crawlController) : base(aiSystem)
        {
            controller = crawlController;
            startPosition = controller.startPosition;
            endPosition = controller.endPosition;
        }
        public override void Enter()
        {
            if (!AISystem.CanReachDestination(startPosition))
            {
                // if no path found switch the start and end positions
                startPosition = controller.endPosition;
                endPosition = controller.startPosition;
            }

            startPosition.y = AISystem.transform.position.y;
            endPosition.y = AISystem.transform.position.y;

            AISystem.AnimationHandler.StopLookAt();
            AISystem.SetNewDestination(startPosition);

            base.Enter();
        }
        public override void Update()
        {
            if (AISystem.transform.position == startPosition && AISystem.NavAgent.enabled)
            {
                AISystem.NavAgent.enabled = false;
                AISystem.AnimationHandler.SetIsCrawling(true);
            }
            else if (AISystem.NavAgent.enabled == false)
            {
                AISystem.transform.position = Vector3.MoveTowards(
                    AISystem.transform.position,
                    endPosition,
                    (AISystem.WalkSpeed * 0.5f) * Time.deltaTime);
            }

            if (AISystem.transform.position == endPosition && AISystem.NavAgent.enabled == false)
            {
                AISystem.AnimationHandler.SetIsCrawling(false);
                AISystem.NavAgent.enabled = true;
                AISystem.ScanPointsOfIntereset();
                AISystem.SetState(new IdleState(AISystem));
            }
            base.Update();
        }
    }
}