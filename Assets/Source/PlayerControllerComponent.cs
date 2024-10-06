using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementComponent))]
public class PlayerControllerComponent : MonoBehaviour
{
    private Camera _camera;
    private MovementComponent _movementComponent;
    private InputAction _moveInput;
    private InputAction _jumpInput;
    private InputAction _interactInput;
    private InputAction _attackInput;
    private InputAction _sprintInput;

    public CommanderComponent CommanderComponent { get; private set; }
    public CombatComponent CombatComponent { get; private set; }
    public HealthComponent HealthComponent { get; private set; }

    private void Start()
    {
        _camera = Camera.main;
        _movementComponent = GetComponent<MovementComponent>();
        CommanderComponent = GetComponent<CommanderComponent>();
        CombatComponent = GetComponent<CombatComponent>();
        HealthComponent = GetComponent<HealthComponent>();

        _moveInput = InputSystem.actions.FindAction("Move");
        _jumpInput = InputSystem.actions.FindAction("Jump");
        _interactInput = InputSystem.actions.FindAction("Interact");
        _attackInput = InputSystem.actions.FindAction("Attack");
        _sprintInput = InputSystem.actions.FindAction("Sprint");

        _attackInput.performed += ctx => Attack();
        _interactInput.performed += ctx => Interact();
        _jumpInput.performed += ctx => _movementComponent.IsJumping = true;
        _jumpInput.canceled += ctx => _movementComponent.IsJumping = false;
        _sprintInput.performed += ctx => _movementComponent.IsSprinting = true;
        _sprintInput.canceled += ctx => _movementComponent.IsSprinting = false;
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 forward = _camera.transform.forward;
        forward.y = 0;
        forward.Normalize();
        Vector3 right = _camera.transform.right;
        right.y = 0;
        right.Normalize();

        Vector2 moveInput = _moveInput.ReadValue<Vector2>().normalized;
        _movementComponent.Movement = (forward * moveInput.y) + (right * moveInput.x);
        //_movementComponent.ClimbInput = (Vector3.up * moveInput.y) + (right * moveInput.x);
        _movementComponent.ClimbInput = moveInput;
    }

    private void Interact()
    {
        if (!CommanderComponent)
        {
            return;
        }

        CommanderComponent.Command();
    }

    private void Attack()
    {
        if (!CombatComponent)
        {
            return;
        }

        CombatComponent.Attack();
    }
}
