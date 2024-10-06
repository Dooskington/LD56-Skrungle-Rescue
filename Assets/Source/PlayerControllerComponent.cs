using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementComponent))]
public class PlayerControllerComponent : MonoBehaviour
{
    private Camera _camera;
    private InputAction _moveInput;
    private InputAction _jumpInput;
    private InputAction _interactInput;
    private InputAction _attackInput;
    private InputAction _sprintInput;

    public MovementComponent MovementComponent { get; private set; }
    public CommanderComponent CommanderComponent { get; private set; }
    public CombatComponent CombatComponent { get; private set; }
    public HealthComponent HealthComponent { get; private set; }

    private void Start()
    {
        _camera = Camera.main;
        MovementComponent = GetComponent<MovementComponent>();
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
        _jumpInput.performed += ctx => MovementComponent.IsJumping = true;
        _jumpInput.canceled += ctx => MovementComponent.IsJumping = false;
        _sprintInput.performed += ctx => MovementComponent.IsSprinting = true;
        _sprintInput.canceled += ctx => MovementComponent.IsSprinting = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        MovementComponent.Movement = (forward * moveInput.y) + (right * moveInput.x);
        MovementComponent.ClimbInput = moveInput;
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
