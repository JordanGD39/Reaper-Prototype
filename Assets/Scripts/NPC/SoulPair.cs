using System;

using Framework.Pickups;
using Framework.TrainMovement;

namespace NPC
{
    [Serializable]
    public struct SoulPair
    {
        public Soul soul;
        public TrainSegment trainSegment;
    }
}