using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _rb.velocity = Vector2.right * 5.0f;
    }


    private void FixedUpdate()
    {
        _rb.AddForce(Vector2.right * 25.0f);
        var clampedVelocityX = Mathf.Clamp(_rb.velocity.x, -25.0f, 25.0f);
        var newClampedVelocity = new Vector2(clampedVelocityX,_rb.velocity.y);
        _rb.velocity = newClampedVelocity;
    }
}
