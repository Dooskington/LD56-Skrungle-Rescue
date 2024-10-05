using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
    [SerializeField] private float _jumpHeight = 1.0f;
    [SerializeField] private float _gravityValue = -9.81f;

    private CharacterController _characterController;
    private float _verticalVelocity = 0.0f;

    public Vector3 Movement { get; set; }
    public bool IsJumping { get; set; }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        bool isGrounded = _characterController.isGrounded;
        if (isGrounded && (_verticalVelocity < 0.0f))
        {
            _verticalVelocity = 0.0f;
        }

        _characterController.Move((Movement * _speed) * Time.deltaTime);
        if (Movement != Vector3.zero)
        {
            transform.forward = Movement;
        }

        if (IsJumping)
        {
            IsJumping = false;

            if (isGrounded)
            {
                _verticalVelocity += Mathf.Sqrt(-_jumpHeight * _gravityValue);
            }
        }

        _verticalVelocity += _gravityValue * Time.deltaTime;
        _characterController.Move((Vector3.up * _verticalVelocity) * Time.deltaTime);
    }
}
