using System.Collections.Generic;
using UnityEngine;

public class CommanderComponent : MonoBehaviour
{
    [SerializeField] private float _commandRaycastDistance = 10.0f;
    [SerializeField] private LayerMask _carryableLayerMask;
    [SerializeField] private AudioClip _collectAudio;

    private PlayerControllerComponent _player;
    private bool _isCarryingSkrungles = false;
    private List<WorkerAIControllerComponent> _workers = new List<WorkerAIControllerComponent>();

    public IReadOnlyList<WorkerAIControllerComponent> Workers { get { return _workers; } }
    public int ItemsCollected { get; set; } = 0;

    private void Start()
    {
        _player = gameObject.GetComponent<PlayerControllerComponent>();
    }

    private void Update()
    {
        if (_player == null)
        {
            return;
        }

        if (_player.MovementComponent.IsClimbing && !_isCarryingSkrungles)
        {
            PickupSkrungles();
        }
        else if (!_player.MovementComponent.IsClimbing && _isCarryingSkrungles && _player.MovementComponent.IsGrounded)
        {
            DropSkrungles();
        }
    }

    public void Command()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _commandRaycastDistance, _carryableLayerMask))
        {
            GameObject hitObject = hit.collider.gameObject;
            CarryableComponent carryableComponent = hitObject.GetComponent<CarryableComponent>();
            if (!carryableComponent)
            {
                return;
            }

            WorkerAIControllerComponent worker = _workers.Find(w => w.State == WorkerState.Following);
            if (worker == null)
            {
                return;
            }

            worker.CommandCarryItem(carryableComponent);
        }
    }

    public void RegisterWorker(WorkerAIControllerComponent worker)
    {
        if (_workers.Contains(worker))
        {
            return;
        }

        _workers.Add(worker);

        AudioEvent.Play3D(_collectAudio, transform.position);
    }

    public void UnregisterWorker(WorkerAIControllerComponent worker)
    {
        if (!_workers.Contains(worker))
        {
            return;
        }

        _workers.Remove(worker);
    }

    public void PickupSkrungles()
    {
        _isCarryingSkrungles = true;
        foreach (WorkerAIControllerComponent worker in _workers)
        {
            worker.OnPickupSkrungle();
        }
    }

    public void DropSkrungles()
    {
        _isCarryingSkrungles = false;
        foreach (WorkerAIControllerComponent worker in _workers)
        {
            worker.OnDropSkrungle();
        }
    }
}
