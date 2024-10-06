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
    [SerializeField] private float _idleWanderIntervalMinSeconds = 3.0f;
    [SerializeField] private float _idleWanderIntervalMaxSeconds = 6.0f;
    [SerializeField] private float _idleWanderRadius = 6.0f;

    private PlayerControllerComponent _player;
    private CarryableComponent _currentCarryable;
    private Transform _dropOffTransform;
    private Vector3 _idleStartPosition;
    private float _lastIdleWanderTime;
    private float _idleWanderInterval;
    private Vector3? _idleWanderDestination;
    private bool _isCaptured = false;

    public WorkerState State { get; private set; } = WorkerState.Idle;

    private void Start()
    {
        Init();

        _player = FindFirstObjectByType<PlayerControllerComponent>();
        _dropOffTransform = GameObject.Find("DropOff").transform;

        BeginIdleState();
    }

    private void OnDisable()
    {
        if (_player != null)
        {
            _player.CommanderComponent.UnregisterWorker(this);
        }
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

    private void BeginIdleState()
    {
        State = WorkerState.Idle;
        _idleStartPosition = transform.position;
    }

    private void UpdateIdleState()
    {
        if (_isCaptured)
        {
            return;
        }

        if ((Time.time - _lastIdleWanderTime) > _idleWanderInterval)
        {
            _lastIdleWanderTime = Time.time;
            _idleWanderInterval = Random.Range(_idleWanderIntervalMinSeconds, _idleWanderIntervalMaxSeconds);

            Vector3 randomDirection = Random.insideUnitSphere * _idleWanderRadius;
            randomDirection.y = 0.0f;

            Vector3 dest = _idleStartPosition + randomDirection;
            dest.y = transform.position.y;
            _idleWanderDestination = dest;
        }

        if (_idleWanderDestination.HasValue)
        {
            MoveTowardsAndLookAt(_idleWanderDestination.Value);

            if (Vector3.Distance(transform.position, _idleWanderDestination.Value) < 0.1f)
            {
                _idleWanderDestination = null;
            }
        }
        else
        {
            _movementComponent.Movement = Vector3.zero;
        }

        if (_player == null)
        {
            return;
        }

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
            BeginIdleState();

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
            _currentCarryable.SetIsBeingCarried(true);
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
            _currentCarryable.SetIsBeingCarried(false);
            Destroy(_currentCarryable.gameObject);
            _currentCarryable = null;
            BeginIdleState();

            _player.CommanderComponent.ItemsCollected += 1;
        }
    }

    public void CommandCarryItem(CarryableComponent item)
    {
        _currentCarryable = item;
        State = WorkerState.PickingUp;
    }

    public void SetIsCaptured(bool isCaptured)
    {
        _isCaptured = isCaptured;

        if (!_isCaptured)
        {
            BeginIdleState();
        }
    }
}
