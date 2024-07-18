using UnityEngine;

namespace Framework
{
    /// <summary>
    /// A test script so something moves randomly in all 3 dimensions.
    /// </summary>
    public sealed class RandomMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        
        private readonly Vector3 _cubeCenter = Vector3.zero;
        private readonly Vector3 _cubeSize = new (25, 25, 25);
        private Vector3 _targetPosition;

        private void Start() => SetNewTargetPosition();

        private void Update() => MoveTowardsTarget();

        private void SetNewTargetPosition()
        {
            float x = Random.Range(_cubeCenter.x - _cubeSize.x / 2, _cubeCenter.x + _cubeSize.x / 2);
            float y = Random.Range(_cubeCenter.y - _cubeSize.y / 2, _cubeCenter.y + _cubeSize.y / 2);
            float z = Random.Range(_cubeCenter.z - _cubeSize.z / 2, _cubeCenter.z + _cubeSize.z / 2);

            _targetPosition = new Vector3(x, y, z);
        }

        private void MoveTowardsTarget()
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
            
            if (Vector3.Distance(transform.position, _targetPosition) < 0.001f)
                SetNewTargetPosition();
        }
    }
}