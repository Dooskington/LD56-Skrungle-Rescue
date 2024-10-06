using UnityEngine;

public enum BugState
{
    Idle,
    Following,
    Dead,
}

public class BugAIControllerComponent : AIControllerComponent
{
    [SerializeField] private float _playerAttackDistance = 16.0f;
    [SerializeField] private float _playerMinFollowDistance = 3.0f;
    [SerializeField] private float _idleWanderIntervalMinSeconds = 3.0f;
    [SerializeField] private float _idleWanderIntervalMaxSeconds = 6.0f;
    [SerializeField] private float _idleWanderRadius = 6.0f;

    public BugState State { get; private set; } = BugState.Idle;
    public HealthComponent HealthComponent { get; private set; }
    public CombatComponent CombatComponent { get; private set; }

    private PlayerControllerComponent _player;
    private Vector3 _idleStartPosition;
    private float _lastIdleWanderTime;
    private float _idleWanderInterval;
    private Vector3? _idleWanderDestination;

    private void Start()
    {
        Init();

        _player = FindFirstObjectByType<PlayerControllerComponent>();

        HealthComponent = GetComponent<HealthComponent>();
        CombatComponent = GetComponent<CombatComponent>();

        BeginIdleState();
    }

    private void Update()
    {
        if ((State != BugState.Dead) && HealthComponent.IsDead)
        {
            State = BugState.Dead;
            return;
        }

        if (State == BugState.Idle)
        {
            UpdateIdleState();
        }
        else if (State == BugState.Following)
        {
            UpdateFollowingState();
        }
    }

    private void BeginIdleState()
    {
        State = BugState.Idle;
        _idleStartPosition = transform.position;
    }

    private void UpdateIdleState()
    {
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

        if (_player == null)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance <= _playerAttackDistance)
        {
            State = BugState.Following;
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
        if (distance > (_playerAttackDistance * 1.5f))
        {
            BeginIdleState();
        }
        else if (distance >= _playerMinFollowDistance)
        {
            MoveTowardsAndLookAt(_player.transform.position);
        }
        else
        {
            LookAt(_player.transform.position);
            _movementComponent.Movement = Vector3.zero;

            CombatComponent.Attack();
        }
    }
}
