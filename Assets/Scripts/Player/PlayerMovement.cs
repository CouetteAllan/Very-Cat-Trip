using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Rayqdr.CatInputs;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _horizontalSpeed = 40.0f;
    [SerializeField] private float _maxHorizontalSpeedSad = 20.0f;
    [SerializeField] private float _maxHorizontalSpeedHappy = 30.0f;
    [SerializeField] private float _jumpForce = 20.0f;


    [Space]
    [SerializeField] private Transform _groundOffsetPoint;
    [SerializeField] private Vector3 _groundBoxCastSize;
    [SerializeField] private float _castDistance;
    [SerializeField] private float _radius;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Transform _centerOfMassOffset;

    private Rigidbody _rb3D;
    private MInputsAction _inputAction;
    private PlayerController _playerController;
    private Animator _anim;

    private float _jumpTimer = 1.0f;
    private float _currentJumpTimer = 0.0f;

    private float _currentMaxHorizontalSpeed;

    private bool _isGrounded= false;


    private void Awake()
    {
        _rb3D = GetComponent<Rigidbody>();
        _rb3D.centerOfMass = _centerOfMassOffset.localPosition;
        _anim = GetComponent<Animator>();

        _playerController = GetComponent<PlayerController>();
    }

    void Start()
    {
        _inputAction = _playerController.Inputs;
        _inputAction.Player.Jump.performed += Jump_performed;

        _inputAction.Enable();

        _currentMaxHorizontalSpeed = _maxHorizontalSpeedSad;
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        if (_isGrounded)
        {
            _currentJumpTimer = _jumpTimer;
            //play anim
        }
    }

    private void Update()
    {
        _currentJumpTimer -= Time.deltaTime;
        if (_isGrounded && _currentJumpTimer > 0.0f)
        {
            PerformJump();
        }

        _anim.SetBool("IsGrounded", _isGrounded);
    }

    private void FixedUpdate()
    {
        _rb3D.AddForce(Vector2.right * _horizontalSpeed, ForceMode.Acceleration);
        var clampedVelocityX = Mathf.Clamp(_rb3D.velocity.x, -25.0f, _currentMaxHorizontalSpeed);
        var newClampedVelocity = new Vector2(clampedVelocityX, _rb3D.velocity.y);
        _rb3D.velocity = newClampedVelocity;

    }

    private void PerformJump()
    {
        _rb3D.AddForce(Vector2.up * _jumpForce, ForceMode.Impulse);

        _anim.SetTrigger("Jump");
        _currentJumpTimer = 0.0f;
        _isGrounded = false;

    }

    public void ChangeMaxSpeed(bool transformed)
    {
        _currentMaxHorizontalSpeed = transformed ? _maxHorizontalSpeedHappy : _maxHorizontalSpeedSad;
    }

    private bool IsGrounded()
    {
        var cast = Physics.SphereCast(_rb3D.position, _radius, Vector3.down,out RaycastHit hitInfo,_castDistance);
        Debug.Log(hitInfo.collider);

        return cast;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position -transform.up * _castDistance, _radius );
    }

    private void OnDisable()
    {
        _inputAction.Player.Jump.performed -= Jump_performed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            _isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 6 && collision.contactCount <= 1)
        {
            //_isGrounded = false;
        }
    }
}
