using UnityEngine;

using Framework.TrainMovement;
using NPC;

namespace Framework.Pickups
{
    [RequireComponent(typeof(TrainSegment))]
    public sealed class Soul : BasePickup
    {
        private Train _parent;
        private TrainSegment _thisSegment;
        private SoulVessel _soulVessel;

        private void Awake() => _thisSegment = GetComponent<TrainSegment>();

        public override void Pickup()
        {
            _parent = p_lastPickUpper.GetComponent<Train>();
            _parent.AddSegment(_thisSegment);
            SpawnPoints.Instance.Release(_soulVessel);
        }

        public void SetVessel(SoulVessel targetVessel)
        {
            _soulVessel = targetVessel;
        }
    }
}