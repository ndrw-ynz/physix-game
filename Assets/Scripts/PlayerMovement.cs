using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private float _movementSpeed = 5.0f;
    [SerializeField] private float _moveSmoothTime = 0.1f;
    [SerializeField] private float _jumpSpeed = 6.0f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _lookSensitivity = 50.0f;


    private CharacterController _controller;
    private Vector3 _moveDirection;
    private Vector3 _moveDampVelocity;
    private Vector3 _forceVelocity;
    private bool _isJumping;

    void Start()
    {
        // Subscribing the functions on events.
        _input.MoveEvent += HandleMove;
        _input.JumpEvent += HandleJump;

        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        Jump();
    }

    private void HandleMove(Vector2 dir)
    {
        _moveDirection = new Vector3(dir.x, 0f, dir.y);
    }

    private void HandleJump()
    {
        _isJumping = _controller.isGrounded;
    }

    /// <summary>
    /// Move event triggered every frame in Update.
    /// </summary>
    private void Move()
    {
        if (_moveDirection == Vector3.zero) return;

        Vector3 moveVector = transform.TransformDirection(_moveDirection);
        if (moveVector.magnitude > 1) moveVector.Normalize();

        _moveDirection = Vector3.SmoothDamp(
            _moveDirection,
            moveVector * _movementSpeed,
            ref _moveDampVelocity,
            _moveSmoothTime
            );

        _controller.Move(_moveDirection * Time.deltaTime);
    }
    /// <summary>
    /// Jump event triggered every frame in Update.
    /// </summary>
    private void Jump()
    {
        Ray groundCheckRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(groundCheckRay, 1.1f))
        {
            _forceVelocity.y = -2f;

            if (_isJumping)
            {
                _forceVelocity.y = _jumpSpeed;
            }
        } else
        {
            _forceVelocity.y -= _gravity * Time.deltaTime;
        }
        _controller.Move(_forceVelocity * Time.deltaTime);
    }
}
