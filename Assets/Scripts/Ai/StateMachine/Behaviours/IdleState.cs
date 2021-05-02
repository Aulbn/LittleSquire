using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    internal class IdleState : State
    {
        public IdleState(AISystem aiSystem) : base(aiSystem) { }

        public override void Update()
        {
            AISystem.SetMoveSpeed();
            AISystem.WanderDelayTimer += Time.fixedDeltaTime;
            if (AISystem.WanderDelayTimer >= (AISystem.PointOfInterest == null ?
                AISystem.WanderDelay : AISystem.PointOfInterest.InspectTime))
            {
                if (AISystem.PointOfInterests.Count > 0)
                    AISystem.SetState(new PoiState(AISystem));
                else
                    AISystem.SetState(new WanderState(AISystem));
            }
            AISystem.CheckDistanceToTarget();
            base.Update();
        }
    }
}