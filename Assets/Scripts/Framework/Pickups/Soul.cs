using System;
using UnityEngine;
using UnityEngine.Events;

using Framework.Attributes;
using Framework.TrainMovement;
using NPC;

namespace Framework.Pickups
{
    [RequireComponent(typeof(TrainSegment))]
    public sealed class Soul : BasePickup
    {
        [SerializeField, Range(0,1)] private float addAmount = 0.1f;
        [SerializeField, Range(0, 1)] private float inBalanceThreshold = 0.3f;
        [Tooltip("(0,0) Air\n(0,1) Fire\n(1,0) Water\n(1,1) Earth")]
        [SerializeField, RangeVector2(0, 1, 0, 1)] private Vector2 balance;
        public Vector2 Balance => balance;
        
        [SerializeField] private UnityEvent onFade = new();
        [SerializeField] private UnityEvent onDeliver = new();
        [SerializeField] private UnityEvent onDeliverUnbalanced = new();
        
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

        public void FadeOutOfWorld()
        {
            onFade?.Invoke();
        }

        public void DeliverSoul()
        {
            UnityEvent u = IsInBalance() ? onDeliver : onDeliverUnbalanced;
            u?.Invoke();
        }

        public void AddElement(int e) => AddElement((Elements) e);
        
        public void AddElement(Elements elementToAdd)
        {
            Vector2 target = elementToAdd switch
            {
                Elements.NONE => throw new($"{gameObject.name} was trying to add {Elements.NONE} to it."),
                Elements.FIRE => new(0, 1),
                Elements.WATER => new(1, 0),
                Elements.EARTH => new(1, 1),
                Elements.AIR => new(0, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(elementToAdd), elementToAdd, null)
            };

            balance = Vector2.MoveTowards(balance, target, addAmount);
        }

        private bool IsInBalance()
        {
            float u = 1 - inBalanceThreshold;
            return balance.x > inBalanceThreshold && balance.x < u
                   && balance.y > inBalanceThreshold && balance.y < u;
        }
    }
}