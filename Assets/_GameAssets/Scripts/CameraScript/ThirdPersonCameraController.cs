using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _playerTransform;
    [SerializeField] Transform _orientationTransform;
    [SerializeField] Transform _playerVisualTransform;

    [Header("Settings")]
    [SerializeField] private float _rotationSpeed;

    private void Update()
    {
        Vector3 viewDirection = new Vector3(transform.position.x, _playerTransform.position.y, transform.position.z);
        _orientationTransform.forward = viewDirection.normalized;

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 inputDirection = _orientationTransform.forward * verticalInput + _orientationTransform.right * verticalInput;
        if (inputDirection!=Vector3.zero)
        {
            _playerVisualTransform.forward = Vector3.Slerp(_playerVisualTransform.forward, inputDirection.normalized, _rotationSpeed * Time.deltaTime);
        }
        
        
        

    }
}
