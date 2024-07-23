using UnityEngine;

namespace Framework.Extensions
{
    public static class VectorExtensions
    {
        public static ref Vector2 SetX(ref this Vector2 v, float x)
        {
            v.x = x;
            return ref v;
        }
        
        public static ref Vector2 SetY(ref this Vector2 v, float y)
        {
            v.y = y;
            return ref v;
        }
    }
}