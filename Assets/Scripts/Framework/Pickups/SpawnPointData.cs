using System;
using UnityEngine;

using NPC;

namespace Framework.Pickups
{
    [Serializable]
    public struct SpawnPointData
    {
        public Transform spawnPosition;
        public bool isOccupied;
        public SoulVessel occupier;
    }
}