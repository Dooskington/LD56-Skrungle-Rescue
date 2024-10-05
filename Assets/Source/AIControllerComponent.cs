using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class AIControllerComponent : MonoBehaviour
{
    protected MovementComponent _movementComponent;

    protected void Init()
    {
        _movementComponent = GetComponent<MovementComponent>();
    }

    protected void MoveTowardsAndLookAt(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        _movementComponent.Movement = direction;

        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
    }

    protected void LookAt(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            transform.forward = direction;
        }
    }
}
