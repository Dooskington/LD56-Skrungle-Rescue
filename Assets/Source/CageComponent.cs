using System.Collections.Generic;
using UnityEngine;

public class CageComponent : MonoBehaviour
{
    [SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
    [SerializeField] private GameObject _workerPrefab;
    [SerializeField] private GameObject _door;

    private List<WorkerAIControllerComponent> _prisoners = new List<WorkerAIControllerComponent>();

    private void Start()
    {
        foreach (Transform spawnPoint in _spawnPoints)
        {
            GameObject worker = Instantiate(_workerPrefab, spawnPoint.position, spawnPoint.rotation);
            WorkerAIControllerComponent workerAIController = worker.GetComponent<WorkerAIControllerComponent>();
            if (workerAIController != null)
            {
                workerAIController.SetIsCaptured(true);
                _prisoners.Add(workerAIController);
            }
        }
    }

    private void Update()
    {
        if (_door == null)
        {
            foreach (WorkerAIControllerComponent prisoner in _prisoners)
            {
                prisoner.SetIsCaptured(false);
            }

            _prisoners.Clear();

            Destroy(this);
        }
    }
}
