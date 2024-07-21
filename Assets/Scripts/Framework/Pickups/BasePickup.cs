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
            if (other.attachedRigidbody == null 
                || IsPickedUpped
                || !other.attachedRigidbody.CompareTag(targetPickUpper))
                return;
            
            p_lastPickUpper = other.attachedRigidbody.gameObject;
            Pickup();
            IsPickedUpped = true;
        }
    }
}