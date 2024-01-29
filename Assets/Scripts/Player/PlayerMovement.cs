using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Rayqdr.CatInputs;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _horizontalSpeed = 40.0f;
    [SerializeField] private float _jumpForce = 20.0f;


    [Space]
    [SerializeField] private Transform _groundOffsetPoint;
    [SerializeField] private Vector2 _groundBoxCastSize;
    [SerializeField] private float _castDistance;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Transform _centerOfMassOffset;

    private Rigidbody2D _rb;
    private MInputsAction _inputAction;
    private PlayerController _playerController;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.centerOfMass = _centerOfMassOffset.localPosition;

        _playerController = GetComponent<PlayerController>();
    }

    void Start()
    {
        _inputAction = _playerController.Inputs;
        _inputAction.Player.Jump.performed += Jump_performed;

        _inputAction.Enable();
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        if (IsGrounded())
        {
            _rb.AddForce(Vector2.up * _jumpForce,ForceMode2D.Impulse);
            //play anim
        }
    }

    private void FixedUpdate()
    {
        _rb.AddForce(transform.right * _horizontalSpeed);
        var clampedVelocityX = Mathf.Clamp(_rb.velocity.x, -25.0f, 30.0f);
        var newClampedVelocity = new Vector2(clampedVelocityX,_rb.velocity.y);
        _rb.velocity = newClampedVelocity;
    }


    private bool IsGrounded()
    {
        return Physics2D.BoxCast(_groundOffsetPoint.position, _groundBoxCastSize, 0, -transform.up, _castDistance, _groundMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(_groundOffsetPoint.position -transform.up * _castDistance, _groundBoxCastSize);
    }

    private void OnDisable()
    {
        _inputAction.Player.Jump.performed -= Jump_performed;

    }
}
