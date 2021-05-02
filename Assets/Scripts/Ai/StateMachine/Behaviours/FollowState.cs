namespace AI
{
    public class FollowState : State
    {
        public FollowState(AISystem aiSystem) : base(aiSystem) { }

        public override void Update()
        {
            AISystem.SetFocusPoint(AISystem.FollowTargetFocusPoint);
            Follow();
            base.Update();
        }
        
        private void Follow()
        {
            float distance = (AISystem.FollowTarget.position - AISystem.transform.position).magnitude;
            if (distance > AISystem.StopRadius)
            {
                SetDestination();
                AISystem.SetMoveSpeed();
                AISystem.SetWanderOrigin(AISystem.transform.position);
            }
            else
            {
                StopFollowing();
            }
        }

        private void StopFollowing()
        {
            AISystem.NavAgent.ResetPath();
            AISystem.ScanPointsOfIntereset();

            // Make sure the delay is shorter when stop following the target
            AISystem.WanderDelayTimer = AISystem.WanderDelay - 
                UnityEngine.Random.Range(AISystem.WanderDelay * 0.5f, AISystem.WanderDelay * 0.75f);

            AISystem.SetState(new IdleState(AISystem));
        }

        private void SetDestination()
        {
            if (AISystem.FollowTarget && AISystem.FollowTargetPos != AISystem.FollowTarget.position)
            {
                AISystem.FollowTargetPos = AISystem.FollowTarget.position;
                float distToWanderOrigin = (AISystem.FollowTargetPos - AISystem.WanderOrigin).magnitude;
                float distToTarget = (AISystem.FollowTargetPos - AISystem.transform.position).magnitude;
                if (distToWanderOrigin > (AISystem.WanderRadius * 0.66f) && distToTarget > 5f)
                {
                    AISystem.SetNewDestination(AISystem.FollowTargetPos);
                }
            }
        }
    }
}