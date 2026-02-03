using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _orientationTransform;

    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed;

    [SerializeField] private KeyCode _movementKey;

    private Rigidbody _playerRigidBody;
    private float _verticalInput, _horizontalInput;
    private Vector3 _movementDirection;

    [Header("Jump Settings")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _jumpForce;
    [SerializeField] private bool _canJump;
    [SerializeField] private float _jumpCoolDown;

    [Header("Ground Settings")]

    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _groundDrag;

    [Header("Slide Settings")]

    [SerializeField] private KeyCode _slideKey;
    [SerializeField] private float _slideMultiplier;
    [SerializeField] private float _slideDrag;
    private bool _isSliding;

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
        _playerRigidBody.freezeRotation = true;
    }
    private void Start()
    {
        //_canJump = true;
    }
    private void Update()
    {
        SetInputs();
        SetPlayerDrag();
        LimitPlayerSpeed();
    }
    private void FixedUpdate()
    {
        SetPlayerMovement();
    }


    private void SetPlayerMovement()
    {
        _movementDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;
        if (_isSliding)
        {
            _playerRigidBody.AddForce(_movementDirection.normalized * _movementSpeed * _slideMultiplier, ForceMode.Force);
        }
        else
        {
            _playerRigidBody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force);
        }


    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(_slideKey))
        {
            _isSliding = true;
            Debug.Log("karakterimiz kayýyor");
        }
        else if (Input.GetKeyDown(_movementKey))
        {
            _isSliding = false;
            Debug.Log("karakterimiz kendi hýzýnda gidiyor");
        }

        else if (Input.GetKey(_jumpKey) && _canJump && isGrounded())
        {
            //zýplama iþlemi gerçekleþecek
            _canJump = false;
            SetPlayerJumping();
            Invoke(nameof(ResetJumping), _jumpCoolDown);
        }
    }
    private void SetPlayerDrag()
    {
        if (_isSliding)
        {
            _playerRigidBody.linearDamping = _slideDrag;
        }
        else
        {
            _playerRigidBody.linearDamping = _groundDrag;
        }

    }

    private void LimitPlayerSpeed()
    {
        Vector3 flatVelocity= new Vector3(_playerRigidBody.linearVelocity.x, 0f, _playerRigidBody.linearVelocity.z);
        if (flatVelocity.magnitude>_movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _movementSpeed;
            _playerRigidBody.linearVelocity = new Vector3(limitedVelocity.x,_playerRigidBody.linearVelocity.y,limitedVelocity.z);

        }

    }
    private void SetPlayerJumping()
    {
        _playerRigidBody.linearVelocity = new Vector3(_playerRigidBody.linearVelocity.x, 0f, _playerRigidBody.linearVelocity.z);
        _playerRigidBody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }
    private void ResetJumping()
    {
        _canJump = true;
    }

    private bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);
    }
}
