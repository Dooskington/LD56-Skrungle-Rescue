using System.Collections.Generic;
using UnityEngine;

public class CommanderComponent : MonoBehaviour
{
    [SerializeField] private float _commandRaycastDistance = 10.0f;
    [SerializeField] private LayerMask _carryableLayerMask;

    private List<WorkerAIControllerComponent> _workers = new List<WorkerAIControllerComponent>();

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
    }
}
