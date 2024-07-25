using UnityEngine;
using UnityEngine.UI;

using Framework.Pickups;

namespace UI.World
{
    public sealed class SoulBalance : MonoBehaviour
    {
        private const int SCALE = 100;
        private static readonly Vector2 Half = Vector2.one * 0.5f;

        [SerializeField] private Image pointer;
        [SerializeField] private Soul currentSoul;

        private Vector2 _lastBalance;

        private void Update()
        {
            Vector2 b = currentSoul.Balance;
            if (_lastBalance == currentSoul.Balance)
                return;

            _lastBalance = b;
            Vector2 balance = b - Half;
            pointer.rectTransform.localPosition = (Vector3)balance * SCALE;
        }
    }
}