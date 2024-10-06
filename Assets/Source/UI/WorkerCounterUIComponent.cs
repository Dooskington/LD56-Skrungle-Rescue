using TMPro;
using UnityEngine;

public class WorkerCounterUIComponent : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private PlayerControllerComponent _player;

    private int? _lastTotalValue;
    private int? _lastFollowingValue;
    private int? _lastBusyValue;
    private int? _lastIdleValue;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _player = FindFirstObjectByType<PlayerControllerComponent>();
    }

    // TODO
}
