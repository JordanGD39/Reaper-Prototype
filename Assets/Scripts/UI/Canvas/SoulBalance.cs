using UnityEngine;
using UnityEngine.UI;

using Framework.Pickups;
using Framework.TrainMovement;

namespace UI.Canvas
{
    public sealed class SoulBalance : MonoBehaviour
    {
        private const int SCALE = 100;
        private static readonly Vector2 Half = Vector2.one * 0.5f;

        [SerializeField] private Image pointer;
        [SerializeField] private Soul currentSoul;
        [SerializeField] private Train train;

        private Vector2 _lastBalance;

        private void Start()
        {
            if (train == null)
                throw new ($"train == null - {name}");
        }

        private void Update()
        {
            Vector2 b = currentSoul.Balance;
            if (_lastBalance == currentSoul.Balance)
                return;

            _lastBalance = b;
            Vector2 balance = b - Half;
            pointer.rectTransform.localPosition = (Vector3)balance * SCALE;
        }

        public void SetSoul()
        {
            Soul s = train.GetFirstSoul();
            
            if (currentSoul != null
                && s == currentSoul)
                return;

            currentSoul = s;
        }
    }
}