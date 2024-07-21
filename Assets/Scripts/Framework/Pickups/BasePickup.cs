using UnityEngine;

using Framework.Attributes;

namespace Framework.Pickups
{
    [RequireComponent(typeof(Collider))]
    public abstract class BasePickup : MonoBehaviour
    {
        [SerializeField, Tag] private string targetPickUpper;

        protected GameObject p_lastPickUpper;

        public bool IsPickedUpped { get; private set; }

        public abstract void Pickup();

        private void OnTriggerEnter(Collider other)
        {
            if (IsPickedUpped
                || !other.CompareTag(targetPickUpper))
                return;
            
            p_lastPickUpper = other.gameObject;
            Pickup();
            IsPickedUpped = true;
        }
    }
}