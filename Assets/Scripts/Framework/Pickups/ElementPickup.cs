using Framework.TrainMovement;
using UnityEngine;

namespace Framework.Pickups
{
    public sealed class ElementPickup : BasePickup
    {
        [field: SerializeField] public Elements Element { get; private set; }

        public override void Pickup()
        {
            Soul s = p_lastPickUpper.GetComponent<Train>().GetFirstSoul();
            
            if (s == null)
                return;
            
            s.AddElement(Element);
        }
    }
}