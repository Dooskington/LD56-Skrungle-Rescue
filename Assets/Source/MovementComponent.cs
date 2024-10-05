using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
    [SerializeField] private float _sprintModifier = 1.75f;
    [SerializeField] private float _turnSpeed = 5.0f;
    [SerializeField] private float _jumpHeight = 1.0f;

    private CharacterController _characterController;
    private float _verticalVelocity = 0.0f;

    public Vector3 Movement { get; set; }
    public bool IsJumping { get; set; }
    public bool IsSprinting { get; set; }

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        bool isGrounded = _characterController.isGrounded;
        if (isGrounded && (_verticalVelocity < 0.0f)){
            _verticalVelocity = 0.0f;
        }

        float modifier = IsSprinting ? _sprintModifier : 1.0f;
        _characterController.Move((Movement * _speed * modifier) * Time.deltaTime);
        if (Movement.magnitude > float.Epsilon)
        {
            transform.forward = Vector3.Lerp(transform.forward, Movement, _turnSpeed * Time.deltaTime);
        }

        if (IsJumping)
        {
            IsJumping = false;

            if (isGrounded)
            {
                _verticalVelocity += Mathf.Sqrt(-_jumpHeight * Physics.gravity.y);
            }
        }

        _verticalVelocity += Physics.gravity.y * Time.deltaTime;
        _characterController.Move((Vector3.up * _verticalVelocity) * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Movement);
    }
}
