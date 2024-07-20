using System;
using UnityEngine;

namespace Framework.TrainMovement
{
    /// <summary>
    /// Will follow the target with the give stats to use.
    /// </summary>>
    [DefaultExecutionOrder(0)]
    public sealed class TrainSegment : MonoBehaviour
    {
        private const string NO_TARGET = "There is no target to follow";
        
        public TrainStats StatsInUse { get; set; }

        public bool CanFollow
        {
            get => this;

            set
            {
                if (value == false)
                    gameObject.SetActive(false);

                Start();
            }
        }
        
        [field: SerializeField] public Transform FollowTarget { get; set; }
        
        [SerializeField] private TrainStats stats;
        
        private bool _isFollowTargetNull;
        private Vector3 _targetPosition;

        private void Awake()
        {
            CanFollow = true;
            StatsInUse = stats;
        }

        private void Start()
        {
            _isFollowTargetNull = FollowTarget == null;

            // if (_isFollowTargetNull)
            //     throw new Exception(NO_TARGET);
        }

        private void FixedUpdate()
        {
            if (!CanFollow
                || _isFollowTargetNull)
                return;
            
            Vector3 currentPosition = transform.position;
            Vector3 targetPosition = FollowTarget.position;
            Vector3 direction = (currentPosition - targetPosition).normalized;
            
            _targetPosition = targetPosition + direction * StatsInUse.followDistance;
            currentPosition = Vector3.Lerp(currentPosition, _targetPosition, StatsInUse.smoothSpeed);
            transform.position = currentPosition;
        }

        public void SetDefaultStats() => StatsInUse = stats;
    }
}