using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using NPC;

namespace Framework.Pickups
{
    public sealed class SpawnPoints : Singleton<SpawnPoints>
    {
        private const string NO_POSITION_WARNING = "There was no available spawn position.";

        [SerializeField] private Transform[] spawnPoints;
        
        private List<SpawnPointData> _points = new ();

        protected override void Awake()
        {
            canDestroyOnLoad = false;
            
            base.Awake();
            
            foreach (Transform spawnPoint in spawnPoints)
            {
                SpawnPointData data = new()
                {
                    spawnPosition = spawnPoint
                };
                
                _points.Add(data);
            }
        }

        public Vector3 GetSpawnPoint(SoulVessel vessel)
        {
            List<Transform> availablePoints = (from point in _points
                where !point.isOccupied select point.spawnPosition).ToList();

            if (availablePoints.Count == 0)
            {
                Debug.LogWarning(NO_POSITION_WARNING);
                
                int r = Random.Range(0, _points.Count);
                SpawnPointData spawnPointData = _points[r];
                spawnPointData.occupier = vessel;
                _points[r] = spawnPointData;
                return spawnPointData.spawnPosition.position;
            }
            
            int l = _points.Count;
            int randomIndex = Random.Range(0, availablePoints.Count);
            Transform selectedPoint = availablePoints[randomIndex];

            for (int i = 0; i < l; i++)
            {
                if (_points[i].spawnPosition != selectedPoint)
                    continue;
                
                SpawnPointData pointData = _points[i];
                pointData.isOccupied = true;
                pointData.occupier = vessel;
                _points[i] = pointData;
                break;
            }

            return selectedPoint.position;
        }

        public void Release(SoulVessel vessel)
        {
            int l = _points.Count;

            for (int i = 0; i < l; i++)
            {
                if (_points[i].occupier != vessel)
                    continue;
                
                SpawnPointData data = _points[i];
                data.isOccupied = false;
                data.occupier = null;
                _points[i] = data;
            }
        }
    }
} 