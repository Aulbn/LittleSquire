using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System;

namespace AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class AISystem : StateMachine
    {
        private Vector3 focusPoint;
        private Vector3 followTargetPos;
        private AudioSource audioSource;

        // Only for debugging
        public string CurrentState;

        [Header("Following settings")]
        [Tooltip("The target to follow")]
        [SerializeField] private Transform followTarget;
        [Tooltip("How far away the target is when the follower should start to run")]
        [SerializeField] private float startRunThreshold = 6f;
        [Tooltip("Radius how far away the follower should stop from the target")]
        [SerializeField] private float stopRadius = 2f;
        [Tooltip("Multiplys the speed that is set in the navmesh agent component")]
        [SerializeField] private float runSpeedMultiplier = 2.86f;

        [Header("Wandering settings")]
        [Tooltip("Time inbetween selecting a new waypoint")]
        [SerializeField] private float wanderDelay = 10f;
        [Tooltip("Radius to wander around in, and check for point of interests")]
        [SerializeField] private float wanderRadius = 10f;

        [Header("Visuals")]
        [SerializeField] private FollowerAnimationHandler animationHandler = null;

        public Transform FollowTarget { get => followTarget; set => followTarget = value; }
        public Vector3 FollowTargetPos { get => followTargetPos; set => followTargetPos = value; }
        public Vector3 WanderOrigin { get; set; }
        public float WanderRadius => wanderRadius;
        public NavMeshAgent NavAgent { get; set; }
        public float WanderDelayTimer { get; set; }
        public float WanderDelay { get => wanderDelay; set => wanderDelay = value; }
        public Vector3 MoveToPosition { get; set; }
        public float StartRunThreshold { get => startRunThreshold; set => startRunThreshold = value; }
        public float WalkSpeed { get; private set; }
        public float RunSpeedMultiplier { get => runSpeedMultiplier; set => runSpeedMultiplier = value; }
        public float StopRadius { get => stopRadius; set => stopRadius = value; }
        public PointOfInterest PointOfInterest { get; set; }
        public List<PointOfInterest> PointOfInterests { get; set; }
        public Vector3 FocusPoint => focusPoint;
        public FollowerAnimationHandler AnimationHandler { get => animationHandler; }
        public Vector3 CrawlStartPos { get; protected set; }
        public Vector3 CrawlEndPos { get; protected set; }
        public Vector3 FollowTargetFocusPoint => followTargetPos + (Vector3.up * 0.6f);
        public DialogueTrigger DialogueTrigger { get; protected set; }

        private void Awake()
        {
            NavAgent = GetComponent<NavMeshAgent>();
            audioSource = GetComponent<AudioSource>();
            DialogueTrigger = GetComponent<DialogueTrigger>();
            //if (walkingSound)
            //{
            //    audioSource.clip = walkingSound;
            //}
            WalkSpeed = NavAgent.speed;
            WanderOrigin = transform.position;
            PointOfInterests = new List<PointOfInterest>();
        }
        private void Start()
        {
            if (followTarget != null)
            {
                SetFocusPoint(followTarget.position);
                SetState(new FollowState(this));
            }
            else
            {
                SetState(new IdleState(this));
            }
        }

        private void Update()
        {
            state.Update();
        }

        public void Stop()
        {
            NavAgent.ResetPath();
            WanderDelayTimer = 0;
            SetState(new CutsceneState(this));
        }

        public void TeleportToPosition(Transform newPosition)
        {
            transform.position = newPosition.position;
            SetState(new CutsceneState(this));
        }

        public void SetFollowTarget(Transform target)
        {
            followTarget = target;
            SetFocusPoint(followTarget.position);
            SetState(new FollowState(this));
        }

        public void SetFocusPoint(Vector3 point)
        {
            focusPoint = point;
            AnimateLookAt();
        }

        public void SetFocusPoint(Transform point)
        {
            focusPoint = point.position;
            AnimateLookAt();
        }

        public bool SetNewDestination(Vector3 position)
        {
            if (NavAgent.enabled && CanReachDestination(position))
            {
                NavAgent.SetDestination(position);
                MoveToPosition = position;
                WanderDelayTimer = 0;
                return true;
            }
            return false;
        }
        public void SetNewDestination(Transform position)
        {
            SetNewDestination(position.position);
        }

        public bool CanReachDestination(Vector3 position)
        {
            NavMeshPath path = new NavMeshPath();
            NavAgent.CalculatePath(position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
                return true;

            return false;
        }

        public void SetWanderOrigin(Vector3 newPosition)
        {
            WanderOrigin = newPosition;
            PointOfInterests.Clear();
        }

        public void ScanPointsOfIntereset()
        {
            SetWanderOrigin(transform.position);
            PointOfInterests = FindObjectsOfType<PointOfInterest>()
             .Where(obj => ((obj.transform.position - transform.position).magnitude <= wanderRadius))
             .ToList();
        }

        public void CheckDistanceToTarget()
        {
            if (followTarget && NavAgent.enabled && followTargetPos != followTarget.position)
            {
                followTargetPos = followTarget.position;
                float distToWanderOrigin = (followTargetPos - WanderOrigin).magnitude;
                float distToTarget = (followTargetPos - transform.position).magnitude;
                if (distToWanderOrigin > (wanderRadius * 0.66f) && distToTarget > 5f)
                {
                    if (CanReachDestination(followTarget.position))
                    {
                        SetState(new FollowState(this));
                    }
                }
            }
        }
        public void SetMoveSpeed()
        {
            float distance = (MoveToPosition - transform.position).magnitude;
            if (distance > StartRunThreshold)
            {
                NavAgent.speed = WalkSpeed * RunSpeedMultiplier;
                //audioSource.clip = runningSound;
            }
            else
            {
                NavAgent.speed = WalkSpeed;
                //audioSource.clip = walkingSound;
            }
        }

        public void AnimateWalking()
        {
            AnimationHandler.SetVelocity(NavAgent.velocity.magnitude <= WalkSpeed ?
            (NavAgent.velocity.magnitude / WalkSpeed) * 0.5f :
            (NavAgent.velocity.magnitude / (WalkSpeed * RunSpeedMultiplier)));
        }
        public void AnimateLookAt()
        {
            AnimationHandler.LookAt(focusPoint);
        }

        public void RotateTowards(Vector3 point)
        {
            point = new Vector3(point.x, transform.position.y, point.z);
            Vector3 direction = point - transform.position;
            direction = Vector3.RotateTowards(transform.forward, direction, NavAgent.speed * Time.deltaTime, 1f);
            transform.rotation = Quaternion.LookRotation(direction);
        }

        public bool IsLookingAtPoint(Vector3 point, float angleThreshold = 1)
        {
            angleThreshold = angleThreshold > 1 ? 1 : angleThreshold;
            point.y = transform.position.y;
            point = (point - transform.position).normalized;
            if (Vector3.Dot(point, transform.forward) >= angleThreshold)
            {
                return true;
            }

            return false;
        }

        public void OnCrawl(CrawlController controller)
        {
            if (IsBusy())
                return;

            MoveToPosition = controller.startPosition;
            SetFocusPoint(controller.endPosition);

            if (!CanReachDestination(MoveToPosition))
            {
                // if no path found switch the start and end positions
                MoveToPosition = controller.endPosition;
                SetFocusPoint(controller.startPosition);
            }

            if (SetNewDestination(MoveToPosition))
                SetState(new WalkingState(this, new CrawlState(this, controller)));
            else
                SetState(new DialogueState(this));
        }

        public void OnStay(Vector3 position)
        {
            if (IsBusy())
                return;

            if (SetNewDestination(position))
            {
                animationHandler.StopLookAt();
                SetState(new WalkingState(this, new StayState(this)));
            }
            else
                SetState(new DialogueState(this));
        }

        public void OnPull(Vector3 position, PuzzleLeverController controller)
        {
            if (IsBusy())
                return;

            if (SetNewDestination(position))
                SetState(new WalkingState(this, new PullState(this, controller)));
            else
                SetState(new DialogueState(this));
        }

        /// <summary>
        /// Check if position a is infront of position b
        /// </summary>
        /// <param name="a"> Position a</param>
        /// <param name="b"> Position b</param>
        /// <returns></returns>
        public bool IsInfront(Vector3 a, Vector3 b)
        {
            Vector3 prep = Vector3.Cross(b, a);
            float dir = Vector3.Dot(prep, Vector3.up);
            if (dir > 0)
                return true;

            return false;
        }

        private bool IsBusy()
        {
            if (state.GetType() == typeof(CrawlState) || state.GetType() == typeof(PullState))
                return true;

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere((WanderOrigin == default ? transform.position : WanderOrigin) + (Vector3.up * 0.05f), wanderRadius);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position + Vector3.up, (transform.position + Vector3.up) + (transform.forward * 1.5f));

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(focusPoint, .2f);
        }
    }
}
