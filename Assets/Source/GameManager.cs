using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }

    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _gameUI;
    [SerializeField] private GameObject _gameOverUI;
    [SerializeField] private GameObject _gameWinUI;
    [SerializeField] private CinemachineCamera _mainMenuCamera;

    private PlayerControllerComponent _player;
    private bool _isMainMenu = true;
    private bool _isGameOver;
    private bool _isGameWon;
    private List<WorkerAIControllerComponent> _workers = new List<WorkerAIControllerComponent>();

    public int TotalSkrungle { get { return _workers.Count; } }

    private void Start()
    {
        InitSingleton();

        _player = FindFirstObjectByType<PlayerControllerComponent>();
    }

    private void Update()
    {
        if (_player != null)
        {
            if (_player.HealthComponent.IsDead)
            {
                _isGameOver = true;
            }
            else if (_workers.Count > 0)
            {
                if (_player.CommanderComponent.Workers.Count >= _workers.Count)
                {
                    _isGameWon = true;
                }
            }
        }

        if (_isMainMenu)
        {
            if (_player != null)
            {
                _player.gameObject.SetActive(false);
            }
            _menuUI.SetActive(true);
            _gameUI.SetActive(false);
            _gameOverUI.SetActive(false);
            _gameWinUI.SetActive(false);

            _mainMenuCamera.Priority = 20;
        }
        else if (_isGameWon)
        {
            if (_player != null)
            {
                _player.gameObject.SetActive(false);
            }
            _menuUI.SetActive(false);
            _gameUI.SetActive(false);
            _gameOverUI.SetActive(false);
            _gameWinUI.SetActive(true);

            _mainMenuCamera.Priority = 20;

            // TODO
            // game win camera?
        }
        else if (!_isGameOver)
        {
            if (_player != null)
            {
                _player.gameObject.SetActive(true);
            }
            _menuUI.SetActive(false);
            _gameUI.SetActive(true);
            _gameOverUI.SetActive(false);
            _gameWinUI.SetActive(false);

            _mainMenuCamera.Priority = 0;
        }
        else
        {
            if (_player != null)
            {
                _player.gameObject.SetActive(false);
            }
            _menuUI.SetActive(false);
            _gameUI.SetActive(false);
            _gameOverUI.SetActive(true);
            _gameWinUI.SetActive(false);

            _mainMenuCamera.Priority = 20;

            // TODO
            // game over camera?
        }
    }

    private void InitSingleton()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    public void StartGame()
    {
        _isMainMenu = false;
        _isGameOver = false;
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void RegisterWorker(WorkerAIControllerComponent worker)
    {
        if (_workers.Contains(worker))
        {
            return;
        }

        _workers.Add(worker);
    }

    public void UnregisterWorker(WorkerAIControllerComponent worker)
    {
        if (!_workers.Contains(worker))
        {
            return;
        }

        _workers.Remove(worker);
    }
}
