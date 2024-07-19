using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 10;
    public float MoveSpeed { get { return _moveSpeed; } }
    [SerializeField] private float _maxForce = 1;
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private float _gravityWhenGrounded = 2;
    [SerializeField] private float _groundDrag = 1;

    [Header("Ground check")]
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _groundCheckRadius = 1;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _hitAngle = 50;
    private bool _grounded = false;
    public bool Grounded { get { return _grounded; } }

    public bool UseGravity { get; set; } = true;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 10;
    private bool _jumping = false;

    private Vector2 _movementInput = Vector2.zero;
    public Vector2 MovementInput { get { return _movementInput; } }
    private Vector3 _movementDir = Vector3.zero;
    public Vector3 MovementDir { get { return _movementDir; } }
    private Vector3 _hitNormal;

    private Camera _cam;

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    private void Update()
    {
        GroundCheck();

        if (_rb.velocity.y < 0)
            _jumping = false;
    }

    void FixedUpdate()
    {
        ApplyGravity();
        UpdateMovement();
    }

    private void ApplyGravity()
    {
        if (!UseGravity)
            return;

        if (_grounded && !_jumping)
        {
            _rb.AddForce(-_hitNormal * _rb.mass * _gravityWhenGrounded);
            return;
        }

        _rb.AddForce(Vector3.down * _rb.mass * _gravity);
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>().normalized;
    }

    private void OnJump()
    {
        if (!_grounded)
            return;

        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        _jumping = true;
    }

    private void GroundCheck()
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, _groundCheckRadius, -transform.up, out hit, 1, _groundLayer))
        {
            if (hit.normal != Vector3.up && Vector3.Angle(Vector3.up, hit.normal) > _hitAngle)
            {
                _grounded = false;
                _hitNormal = Vector3.up;
                _rb.drag = 0;
                return;
            }
                
            _grounded = true;
            _hitNormal = hit.normal;
            _rb.drag = _groundDrag;
        }
        else
        {
            _grounded = false;
            _hitNormal = Vector3.up;
            _rb.drag = 0;
        }
    }

    private void UpdateMovement()
    {
        _movementDir = _cam.transform.TransformDirection(new Vector3(_movementInput.x, 0, _movementInput.y));
        _movementDir.y = 0;
        Vector3 projectedDir = Vector3.ProjectOnPlane(_movementDir, _hitNormal);

        Vector3 currentVel = _rb.velocity;
        currentVel.y = 0;

        Vector3 targetVel = projectedDir * _moveSpeed;
        Debug.DrawRay(_groundCheckPoint.position, projectedDir);

        Vector3 velChange = targetVel - currentVel;

        velChange = Vector3.ClampMagnitude(velChange, _maxForce);

        _rb.AddForce(velChange, ForceMode.VelocityChange);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
    }
}