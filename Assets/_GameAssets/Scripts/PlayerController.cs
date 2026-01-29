using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _orientationTransform;

    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed;

    private Rigidbody _playerRigidBody;
    private float _verticalInput, _horizontalInput;
    private Vector3 _movementDirection;

    [Header("Jump Settings")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool _canJump;
    [SerializeField] private float _jumpCoolDown;
    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        _playerRigidBody.freezeRotation = true;
    }
    private void Start()
    {
        _canJump = true;
    }
    private void Update()
    {
        SetInputs();
    }
    private void FixedUpdate()
    {
        SetPlayerMovement();
    }

    private void SetPlayerMovement()
    {
        _movementDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;
        _playerRigidBody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force);

    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(_jumpKey) && _canJump)
        {
            //zýplama iþlemi gerçekleþecek
            _canJump = false;
            SetPlayerJumping();
            Invoke(nameof(JumpingReset),_jumpCoolDown);
        }
    }

    private void SetPlayerJumping()
    {
        _playerRigidBody.linearVelocity = new Vector3(_playerRigidBody.linearVelocity.x, 0f, _playerRigidBody.linearVelocity.z);
        _playerRigidBody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }
    private void JumpingReset()
    {
        _canJump = true;
    }
}
