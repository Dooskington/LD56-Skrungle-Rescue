using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MovementComponent))]
public class PlayerInputComponent : MonoBehaviour
{
    private MovementComponent _movementComponent;
    private InputAction _moveInput;
    private InputAction _jumpInput;

    private void Start()
    {
        _movementComponent = GetComponent<MovementComponent>();
        _moveInput = InputSystem.actions.FindAction("Move");
        _jumpInput = InputSystem.actions.FindAction("Jump");

        _jumpInput.performed += ctx => _movementComponent.IsJumping = true;
    }

    private void Update()
    {
        Vector2 moveInput = _moveInput.ReadValue<Vector2>();
        _movementComponent.Movement = new Vector3(moveInput.x, 0, moveInput.y).normalized;
    }
}
