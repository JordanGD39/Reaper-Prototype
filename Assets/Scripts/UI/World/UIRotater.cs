using UnityEngine;

namespace UI.World
{
    public sealed class UIRotater : MonoBehaviour
    {
        private Transform _target;

        private void Start() => _target = Camera.main.transform;

        private void Update()
        {
            Quaternion targetRotation = _target.rotation;
            Quaternion newYRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
            transform.rotation = newYRotation;
        }
    }
}