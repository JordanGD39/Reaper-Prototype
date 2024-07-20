using UnityEngine;

using Framework.Attributes;

namespace Framework.Pickups
{
    [RequireComponent(typeof(Collider))]
    public abstract class BasePickup : MonoBehaviour
    {
        [SerializeField, Tag] private string targetPickUpper;
        [SerializeField, Tag] private string deliveryPoint;

        protected GameObject p_lastPickUpper;

        public bool IsPickedUpped { get; private set; }

        public abstract void Pickup();
        
        public abstract void Deliver();

        private void OnTriggerEnter(Collider other)
        {
            if (!IsPickedUpped
                && other.CompareTag(targetPickUpper))
            {
                p_lastPickUpper = other.gameObject;
                Pickup();
                IsPickedUpped = true;
                return;
            }

            return;
            
            if (IsPickedUpped
                && other.CompareTag(deliveryPoint))
            {
                IsPickedUpped = false;
                Deliver();
            }
        }
    }
}