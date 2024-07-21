using UnityEngine;

using Framework.Attributes;
using Framework.Pickups;
using Framework.TrainMovement;

namespace NPC
{
    public sealed class SoulVessel : MonoBehaviour
    {
        [SerializeField, Tag] private string playerTag;
        [SerializeField] private GameObject soulToSpawn;
        [SerializeField] private SoulPair npcSoul;
        [SerializeField] private bool debugMode;

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
            Vector3 p = SpawnPoints.Instance.GetSpawnPoint(this);
            GameObject spawnedSoul = Instantiate(soulToSpawn, p, Quaternion.identity);
            
            npcSoul.trainSegment = spawnedSoul.GetComponent<TrainSegment>();
            npcSoul.soul = spawnedSoul.GetComponent<Soul>();
            npcSoul.soul.SetVessel(this);
            _hasSoul = false;
            
            if (debugMode)
                SetRandomColor(spawnedSoul);
        }
        
        // test function for visuals
        private void SetRandomColor(GameObject soul)
        {
            Color randomColor = new (Random.value, Random.value, Random.value);
            
            Renderer renderer1 = GetComponent<Renderer>();
            renderer1.material.color = randomColor;
            
            Renderer renderer2 = soul.GetComponent<Renderer>();
            renderer2.material.color = randomColor;
        }
    }
}