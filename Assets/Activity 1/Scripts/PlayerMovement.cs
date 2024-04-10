using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputReader _input;
    [SerializeField] private float _movementSpeed = 2.5f;
    [SerializeField] private float _moveSmoothTime = 0.1f;
    [SerializeField] private float _jumpSpeed = 6.0f;
    [SerializeField] private float _gravity = 9.81f;
    [SerializeField] private float _lookSensitivity = 0.5f; 


    private CharacterController _controller;
    private Vector3 _moveDirection;
    private Vector3 _moveDampVelocity;
    private Vector3 _forceVelocity;

    private Vector2 _lookRotation;
    private bool _isJumping;

    public Transform playerCamera;

    void Start()
    {
        // Subscribing the functions on events.
        _input.MoveEvent += HandleMove;
        _input.JumpEvent += HandleJump;
        _input.LookEvent += HandleLook;

        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Look();
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

    private void HandleLook(Vector2 dir)
    {
        _lookRotation.x -= dir.y * _lookSensitivity;
        _lookRotation.y += dir.x * _lookSensitivity;

        _lookRotation.x = Mathf.Clamp(_lookRotation.x, -90f, 90f);
    }

    /// <summary>
    /// Move event triggered every frame in Update.
    /// </summary>
    private void Move()
    {
        if (_moveDirection == Vector3.zero) return;

        Vector3 moveVector = _moveDirection;
        if (moveVector.magnitude > 1) moveVector.Normalize();

        _moveDirection = Vector3.SmoothDamp(
            _moveDirection,
            moveVector * _movementSpeed,
            ref _moveDampVelocity,
            _moveSmoothTime
            );

        _controller.Move(transform.TransformDirection(_moveDirection) * _movementSpeed * Time.deltaTime);
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
    /// <summary>
    /// Look event triggered every frame in Update.
    /// </summary>
    private void Look()
    {
        transform.eulerAngles = new Vector3(0f, _lookRotation.y, 0f);
        playerCamera.localEulerAngles = new Vector3(_lookRotation.x, 0f, 0f);
    }
}
