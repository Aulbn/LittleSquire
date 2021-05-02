using System.Collections;
using Random = UnityEngine.Random;

namespace AI
{
    internal class PoiState : State
    {
        public PoiState(AISystem aiSystem) : base(aiSystem)
        {
        }
        public override void Enter()
        {
            base.Enter();
        }

        public override void Update()
        {
            if (AISystem.NavAgent.hasPath)
            {
                AISystem.SetState(new WalkingState(AISystem));
            }
            else
            {
                AISystem.PointOfInterest = AISystem.PointOfInterests[Random.Range(0, AISystem.PointOfInterests.Count)];
                AISystem.SetFocusPoint(AISystem.PointOfInterest.FocusPoint);
                AISystem.SetNewDestination(AISystem.PointOfInterest.transform.position);
            }
            AISystem.CheckDistanceToTarget();
            base.Enter();
        }
    }
}