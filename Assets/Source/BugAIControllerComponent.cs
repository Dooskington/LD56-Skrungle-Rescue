using UnityEngine;

public enum BugState
{
    Idle,
    Following,
}

public class BugAIControllerComponent : AIControllerComponent
{
    [SerializeField] private float _idleWanderIntervalMinSeconds = 3.0f;
    [SerializeField] private float _idleWanderIntervalMaxSeconds = 6.0f;

    public BugState State { get; private set; } = BugState.Idle;

    private Vector3 _idleStartPosition;
    private float _idleWanderRadius = 6.0f;
    private float _lastIdleWanderTime;
    private float _idleWanderInterval;
    private Vector3? _idleWanderDestination;

    private void Start()
    {
        Init();

        BeginIdleState();
    }

    private void Update()
    {
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
    }

    private void UpdateFollowingState()
    {

    }
}
