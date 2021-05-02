using System.Collections;
using UnityEngine;

namespace AI
{
    internal class WanderState : State
    {
        public WanderState(AISystem aiSystem) : base(aiSystem)
        {
        }

        public override void Update()
        {
            if (AISystem.NavAgent.hasPath)
            {
                AISystem.SetState(new WalkingState(AISystem));
            }
            else
            {
                AISystem.PointOfInterest = null;
                AISystem.SetFocusPoint(AISystem.FollowTargetFocusPoint);
                Vector3 point = AISystem.WanderRadius * Random.insideUnitCircle;
                //Vector3 newPosition = AISystem.WanderOrigin + new Vector3(point.x, AISystem.transform.position.y, point.y);
                Vector3 newPosition = AISystem.WanderOrigin + point;
                AISystem.SetNewDestination(newPosition);
            }
            AISystem.CheckDistanceToTarget();
            base.Update();
        }
    }
}