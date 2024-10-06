using UnityEngine;
using static Unity.Cinemachine.CinemachineFreeLookModifier;

[RequireComponent(typeof(CharacterController))]
public class MovementComponent : MonoBehaviour
{
    [SerializeField] private float _speed = 8.0f;
    [SerializeField] private float _sprintModifier = 1.75f;
    [SerializeField] private float _turnSpeed = 5.0f;
    [SerializeField] private float _jumpHeight = 1.0f;
    [SerializeField] private bool _canDoubleJump = false;

    [Header("Climbing")]
    [SerializeField] private bool _canClimb = false;
    [SerializeField] private float _climbSpeed = 3.0f;
    [SerializeField] private Vector3 _climbHitboxSize = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private LayerMask _climbableLayerMask;

    private CharacterController _characterController;
    private float _verticalVelocity = 0.0f;
    private int _jumpCount = 0;
    private bool _isLookingForClimbPoint = false;
    private bool _isClimbing = false;
    private bool _exitedClimb = false;
    private Vector3 _climbHitboxOrigin;
    private Vector3 _climbDirection;
    private Vector3 _climbDirectionHorizontal;
    private float _gravity;

    public Vector2 ClimbInput { get; set; }
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
            _jumpCount = 0;
            _verticalVelocity = 0.0f;
            _exitedClimb = false;
            _gravity = 0.0f;
        }

        if (!isGrounded)
        {
            _gravity += Physics.gravity.y * 2.0f * Time.deltaTime;
            _gravity = Mathf.Max(_gravity, -50.0f);
        }

        float modifier = (!isGrounded && !_isClimbing) ? 1.0f : (IsSprinting ? _sprintModifier : 1.0f);
        if (_isClimbing)
        {
            Vector3 climbMovement = (_climbDirectionHorizontal * -ClimbInput.x) + new Vector3(0.0f, ClimbInput.y, 0.0f);
            _characterController.Move((climbMovement * _climbSpeed * modifier) * Time.deltaTime);
        }
        else
        {
            _characterController.Move((Movement * _speed * modifier) * Time.deltaTime);
            if (Movement.magnitude > float.Epsilon)
            {
                transform.forward = Vector3.Lerp(transform.forward, Movement, _turnSpeed * Time.deltaTime);
            }
        }

        if (_canClimb && !_exitedClimb)
        {
            _climbHitboxOrigin = transform.position + (transform.forward * 0.5f);
            if (!isGrounded && (_jumpCount > 0))
            {
                _isLookingForClimbPoint = true;
            }
            else
            {
                _isClimbing = false;
                _isLookingForClimbPoint = false;
            }

            if (_isLookingForClimbPoint)
            {
                Collider[] colliders = Physics.OverlapBox(_climbHitboxOrigin, _climbHitboxSize, Quaternion.identity, _climbableLayerMask);
                if (colliders.Length > 0)
                {
                    _isClimbing = true;
                    _verticalVelocity = 0.0f;
                    
                    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 3.0f, _climbableLayerMask))
                    {
                        _climbDirection = (hit.point - transform.position).normalized;
                        _climbDirectionHorizontal = Vector3.Cross(_climbDirection, Vector3.up);

                        transform.forward = -hit.normal;
                    }
                }
                else
                {
                    _isClimbing = false;
                    _climbDirection = Vector3.zero;
                    _climbDirectionHorizontal = Vector3.zero;
                }
            }
        }

        if (IsJumping)
        {
            IsJumping = false;

            if (_isClimbing)
            {
                _isClimbing = false;
                _isLookingForClimbPoint = false;
                _exitedClimb = true;
                _jumpCount = 0;
            }

            if (isGrounded || (_canDoubleJump && (_jumpCount < 2)))
            {
                _jumpCount += 1;
                _verticalVelocity = Mathf.Sqrt(-_jumpHeight * Physics.gravity.y);
                _gravity = 0.0f;
            }

            if (_exitedClimb && (_jumpCount > 1))
            {
                _exitedClimb = false;
            }
        }

        _verticalVelocity += _gravity * Time.deltaTime;
        _characterController.Move((Vector3.up * _verticalVelocity) * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, Movement);

        if (_isClimbing || _isLookingForClimbPoint)
        {
            Gizmos.color = _isClimbing ? Color.green : Color.yellow;
            Gizmos.DrawWireCube(_climbHitboxOrigin, _climbHitboxSize);

            if (_isClimbing)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawRay(transform.position, _climbDirection);

                Gizmos.color = Color.cyan;
                Gizmos.DrawRay(transform.position, _climbDirectionHorizontal);
            }
        }
    }
}
