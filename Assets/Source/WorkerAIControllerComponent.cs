using System.Linq;
using UnityEngine;

public enum WorkerState
{
    Idle,
    Following,
    PickingUp,
    Carrying,
}

[RequireComponent(typeof(MovementComponent))]
public class WorkerAIControllerComponent : AIControllerComponent
{
    [SerializeField] private float _playerJoinDistance = 10.0f;
    [SerializeField] private float _playerMinFollowDistance = 3.0f;
    [SerializeField] private float _carryableMinDistance = 1.0f;
    [SerializeField] private float _dropOffMinDistance = 1.0f;

    private PlayerControllerComponent _player;
    private CarryableComponent _currentCarryable;
    private Transform _dropOffTransform;

    public WorkerState State { get; private set; } = WorkerState.Idle;

    private void Start()
    {
        Init();

        _player = FindFirstObjectByType<PlayerControllerComponent>();
        _dropOffTransform = GameObject.Find("DropOff").transform;

        State = WorkerState.Idle;
    }

    private void Update()
    {
        if (State == WorkerState.Idle)
        {
            UpdateIdleState();
        }
        else if (State == WorkerState.Following)
        {
            UpdateFollowingState();
        }
        else if (State == WorkerState.PickingUp)
        {
            UpdatePickingUpState();
        }
        else if (State == WorkerState.Carrying)
        {
            UpdateCarryingState();
        }
    }

    private void UpdateIdleState()
    {
        if (_player == null)
        {
            return;
        }

        _movementComponent.Movement = Vector3.zero;

        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance <= _playerJoinDistance)
        {
            State = WorkerState.Following;
            _player.CommanderComponent.RegisterWorker(this);
        }
    }

    private void UpdateFollowingState()
    {
        if (_player == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance >= _playerMinFollowDistance)
        {
            MoveTowardsAndLookAt(_player.transform.position);
        }
        else
        {
            LookAt(_player.transform.position);
            _movementComponent.Movement = Vector3.zero;
        }
    }

    private void UpdatePickingUpState()
    {
        if (_currentCarryable == null)
        {
            State = WorkerState.Idle;
            return;
        }

        MoveTowardsAndLookAt(_currentCarryable.transform.position);

        float distance = Vector3.Distance(transform.position, _currentCarryable.transform.position);
        if (distance < _carryableMinDistance)
        {
            State = WorkerState.Carrying;
        }
    }

    private void UpdateCarryingState()
    {
        if (_currentCarryable == null)
        {
            State = WorkerState.Idle;
            return;
        }

        _currentCarryable.transform.rotation = Quaternion.identity;
        _currentCarryable.transform.position = transform.position + (Vector3.up * 2.0f);

        MoveTowardsAndLookAt(_dropOffTransform.position);

        float distance = Vector3.Distance(transform.position, _dropOffTransform.position);
        if (distance < _dropOffMinDistance)
        {
            Destroy(_currentCarryable.gameObject);
            _currentCarryable = null;
            State = WorkerState.Idle;
        }
    }

    public void CommandCarryItem(CarryableComponent item)
    {
        _currentCarryable = item;
        State = WorkerState.PickingUp;
    }
}
