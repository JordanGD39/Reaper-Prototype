using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Attributes;

public class DashRing : MonoBehaviour
{
    [SerializeField, Tag] private string _target;
    [SerializeField] private float _targetVelocity = 50;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;

        if (rb == null || !rb.CompareTag(_target))
            return;

        Vector3 currentVel = rb.velocity;
        currentVel.y = 0;

        float force = _targetVelocity - currentVel.magnitude;

        if (force <= 0)
            return;

        rb.AddForce(currentVel.normalized * force, ForceMode.Impulse);
    }
}
