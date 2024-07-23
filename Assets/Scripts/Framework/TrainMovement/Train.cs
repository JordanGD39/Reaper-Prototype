using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using Framework.Pickups;

namespace Framework.TrainMovement
{
    /// <summary>
    /// A train that sets the segments to follow the correct transform.
    /// </summary>
    [DefaultExecutionOrder(1)]
    public sealed class Train : MonoBehaviour
    {
        private const string LENGTH_WARING = "There should be trainsegments assignt to segments for:";
        private const string SAME_PART_ERROR = "The segment is already part of the train.";
        private const string PART_NOT_FOUND_ERROR = "The segment is not part of the train.";
        
        [SerializeField] private bool useTrainStats = true;
        [SerializeField] private TrainStats stats;
        [SerializeField] private TrainSegment[] segments;

        [SerializeField] private UnityEvent onAddSegment = new ();
        [SerializeField] private UnityEvent onRemoveSegment = new ();
        
        private void Awake() => InitSegments();
        
        private void InitSegments()
        {
            int l = segments.Length;

            if (l == 0)
            {
                Debug.LogWarning($"{LENGTH_WARING} {gameObject.name}");
                return;
            }
            
            for (int index = 0; index < l; index++)
            {
                if (useTrainStats)
                    segments[index].StatsInUse = stats;
                else
                    segments[index].SetDefaultStats();
                
                if (index == 0)
                {
                    segments[index].FollowTarget = transform;
                    continue;
                }
                
                segments[index].FollowTarget = segments[index - 1].transform;
            }
        }

        public void AddSegment(TrainSegment newSegment)
        {
            if (segments.Any(segment => segment == newSegment))
            {
                Debug.LogWarning(SAME_PART_ERROR);
                return;
            }
            
            if (useTrainStats)
                newSegment.StatsInUse = stats;
            else
                newSegment.SetDefaultStats();
            
            newSegment.FollowTarget = segments.Length > 0 
                ? segments[^1].transform 
                : transform;
            
            Array.Resize(ref segments, segments.Length + 1);
            segments[^1] = newSegment;
            newSegment.CanFollow = true;
            onAddSegment?.Invoke();
        }

        public bool RemoveSegment(TrainSegment segmentToRemove)
        {
            int index = Array.IndexOf(segments, segmentToRemove);

            if (index == -1)
            {
                Debug.LogWarning(PART_NOT_FOUND_ERROR);
                return false;
            }

            for (int i = index; i < segments.Length - 1; i++)
            {
                segments[i] = segments[i + 1];
                segments[i].FollowTarget = i > 0 
                    ? segments[i - 1].transform 
                    : transform;
            }

            Array.Resize(ref segments, segments.Length - 1);
            segmentToRemove.CanFollow = false;
            onRemoveSegment?.Invoke();
            return true;
        }

        public Soul GetFirstSoul()
        {
            if (segments.Length == 0)
                return null;
            
            bool a = segments[0].gameObject.TryGetComponent(out Soul s);
            return a ? s : null;
        }
    }
}