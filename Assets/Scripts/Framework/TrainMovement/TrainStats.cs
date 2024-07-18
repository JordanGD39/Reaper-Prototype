using System;
using UnityEngine;

namespace Framework.TrainMovement
{
    [Serializable]
    public struct TrainStats
    {
        [Range(0.1f, 25)] public float followDistance;
        [Range(0.1f, 1)] public float smoothSpeed;

        private TrainStats(float? a, float? b)
        {
            followDistance = a ?? 0.5f;
            smoothSpeed = b ?? 1f;
        }
    }
}