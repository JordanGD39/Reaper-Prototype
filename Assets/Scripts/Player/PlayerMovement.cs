using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody _rb;

    [Header("Movement")]
    [SerializeField] public float targetSpeed = 30;
    public float currentTargetSpeed = 30;
    [SerializeField] private float _maxForce = 1;
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private float _gravityWhenGrounded = 2;
    [SerializeField] private float _groundDrag = 1;
    [SerializeField] private float _speedPreservationAngle = 50;
    [SerializeField] private float _atFullSpeedDiff = 2;
    [SerializeField] private float _speedFalloff = 2;

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
    private Vector3 _groundNormal;

    private Camera _cam;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
        _rb.useGravity = false;
        currentTargetSpeed = targetSpeed;
    }

    private void Update()
    {
        GroundCheck();
        UpdateCurrentTargetSpeed();

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
            _rb.AddForce(-_groundNormal * _gravityWhenGrounded);
            return;
        }

        _rb.AddForce(Vector3.down * _gravity);
    }

    private void OnMove(InputValue inputValue)
    {
        _movementInput = inputValue.Get<Vector2>();
    }

    private void OnJump()
    {
        if (!_grounded)
            return;

        _rb.velocity = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        _rb.AddForce(_groundNormal * _jumpForce, ForceMode.Impulse);
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
                _groundNormal = Vector3.up;
                _rb.drag = 0;
                return;
            }
                
            _grounded = true;
            _groundNormal = hit.normal;
            _rb.drag = _groundDrag;
        }
        else
        {
            _grounded = false;
            _groundNormal = Vector3.up;
            _rb.drag = 0;
        }
    }

    private void UpdateMovement()
    {
        _movementDir = _cam.transform.TransformDirection(new Vector3(_movementInput.x, 0, _movementInput.y));
        _movementDir.y = 0;
        Vector3 projectedDir = Vector3.ProjectOnPlane(_movementDir, _groundNormal);

        Vector3 currentVel = _rb.velocity;
        currentVel.y = 0;
        projectedDir.Normalize();

        Vector3 targetVel = projectedDir * currentTargetSpeed;

        Vector3 velChange = targetVel - currentVel;
        
        if (_movementInput == Vector2.zero 
            || currentVel.magnitude < currentTargetSpeed - _atFullSpeedDiff
            || currentVel.magnitude > currentTargetSpeed
            || Vector3.Angle(currentVel.normalized, projectedDir) > _speedPreservationAngle)
        {
            velChange = Vector3.ClampMagnitude(velChange, _maxForce);
        }
        else if(Mathf.Abs(velChange.y) > 1)
        {
            velChange.y = Mathf.Clamp(velChange.y, -1, 1);
        }

        _rb.AddForce(velChange, ForceMode.VelocityChange);
    }

    private void UpdateCurrentTargetSpeed()
    {
        Vector3 currentVel = _rb.velocity;
        currentVel.y = 0;

        if (currentVel.magnitude > currentTargetSpeed)
        {
            currentTargetSpeed = currentVel.magnitude;
            return;
        }
        else if(currentVel.magnitude < targetSpeed && currentTargetSpeed != targetSpeed)
            currentTargetSpeed = targetSpeed;
            
        if (currentTargetSpeed == targetSpeed)
            return;

        currentTargetSpeed -= _speedFalloff * Time.deltaTime;

        if (currentTargetSpeed < targetSpeed)
            currentTargetSpeed = targetSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(_groundCheckPoint.position, _groundCheckRadius);
    }
}