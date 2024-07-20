using UnityEngine;

using Framework.TrainMovement;

namespace Framework.Pickups
{
    [RequireComponent(typeof(TrainSegment))]
    public sealed class Soul : BasePickup
    {
        private Train _parent;
        private TrainSegment _thisSegment;

        private void Awake() => _thisSegment = GetComponent<TrainSegment>();

        public override void Pickup()
        {
            _parent = p_lastPickUpper.GetComponent<Train>();
            _parent.AddSegment(_thisSegment);
        }

        public override void Deliver()
        {
            _parent.RemoveSegment(_thisSegment);
        }
    }
}