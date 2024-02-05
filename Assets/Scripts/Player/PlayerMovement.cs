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
    private bool _isBoosting = false;

    private Coroutine _storedCoroutine = null;
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
        _inputAction.Player.Jump.canceled += Jump_canceled;

        _inputAction.Enable();

        _currentMaxHorizontalSpeed = _maxHorizontalSpeedSad;
    }

    private void Jump_canceled(InputAction.CallbackContext obj)
    {
        if (_isGrounded)
            return;
        var newVelY = _rb3D.velocity;
        newVelY.y /= 2.0f;
        _rb3D.velocity = newVelY;
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
        if (_isBoosting)
            return;
        _rb3D.AddForce(Vector2.right * _horizontalSpeed, ForceMode.Acceleration);
        var clampedVelocityX = Mathf.Clamp(_rb3D.velocity.x, -25.0f, _currentMaxHorizontalSpeed);
        var newClampedVelocity = new Vector2(clampedVelocityX, _rb3D.velocity.y);
        _rb3D.velocity = newClampedVelocity;

    }

    private void PerformJump()
    {
        if (_isBoosting)
            return;
        _rb3D.AddForce(Vector2.up * _jumpForce, ForceMode.Impulse);

        _anim.SetTrigger("Jump");
        _currentJumpTimer = 0.0f;
        _isGrounded = false;

    }

    public void ChangeJumpForce()
    {
        _jumpForce = 8.0f;
    }

    public void ChangeMaxSpeed(bool transformed)
    {
        _currentMaxHorizontalSpeed = transformed ? _maxHorizontalSpeedHappy : _maxHorizontalSpeedSad;
    }

    public void SpeedBoost(float bonusSpeed,Vector3 dir)
    {
        if (_storedCoroutine != null)
            StopCoroutine(_storedCoroutine);
        _storedCoroutine = StartCoroutine(StraightLineCoroutine(bonusSpeed, dir));
    }

    private IEnumerator StraightLineCoroutine(float speed, Vector3 dir)
    {
        //go in a straight line at a high speed
        _isBoosting = true;
        float startTime = Time.time;
        float endTime = 0.6f;

        _currentMaxHorizontalSpeed *= speed;
        _rb3D.useGravity = false;
        _rb3D.velocity = Vector3.zero;
        _rb3D.velocity = dir.normalized;
        while(startTime + endTime > Time.time)
        {
            _rb3D.AddForce(dir.normalized * _horizontalSpeed * 2.0f, ForceMode.Acceleration);
            var clampedVelocityX = Mathf.Clamp(_rb3D.velocity.x, -25.0f, _currentMaxHorizontalSpeed);
            var clampedVelocityY = Mathf.Clamp(_rb3D.velocity.y, -25.0f, _currentMaxHorizontalSpeed);
            var newClampedVelocity = new Vector2(clampedVelocityX, clampedVelocityY);
            _rb3D.velocity = newClampedVelocity;
            yield return new WaitForFixedUpdate();
        }

        _isBoosting = false;
        _currentMaxHorizontalSpeed = _playerController.IsTransformed ? _maxHorizontalSpeedHappy : _maxHorizontalSpeedSad;
        _rb3D.useGravity = true;
        _rb3D.velocity /= 3.0f;

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
