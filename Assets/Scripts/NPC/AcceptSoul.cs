using UnityEngine;

using Framework.Attributes;
using Framework.Pickups;
using Framework.TrainMovement;

namespace NPC
{
    public sealed class AcceptSoul : MonoBehaviour
    {
        [SerializeField, Tag] private string playerTag;
        [SerializeField] private GameObject soulToSpawn;
        [SerializeField] private SoulPair npcSoul;

        private bool _hasSoul;
        
        private void Start() => SpawnSoul();

        private void OnTriggerEnter(Collider other)
        {
            if (_hasSoul
                || !other.CompareTag(playerTag))
                return;

            bool b = other.GetComponent<Train>().RemoveSegment(npcSoul.trainSegment);
            
            if (b)
                _hasSoul = true;
        }
        
        private void SpawnSoul()
        {
            // todo: better location
            Vector3 p = Vector3.one * 10 + transform.position;
            p.y = 0;
            GameObject s = Instantiate(soulToSpawn, p, Quaternion.identity);
            SetRandomColor(s);
            
            npcSoul.soul = s.GetComponent<Soul>();
            npcSoul.trainSegment = s.GetComponent<TrainSegment>();
        }
        
        // test function for visuals
        private void SetRandomColor(GameObject soul)
        {
            Color randomColor = new (Random.value, Random.value, Random.value);
            
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = randomColor;
            
            Renderer renderer2 = soul.GetComponent<Renderer>();
            renderer2.material.color = randomColor;
        }
    }
}