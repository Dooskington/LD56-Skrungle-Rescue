using TMPro;
using UnityEngine;

public class HealthUIComponent : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private PlayerControllerComponent _player;

    private int? _lastValue;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _player = FindFirstObjectByType<PlayerControllerComponent>();
    }

    private void Update()
    {
        int currentValue = (int)_player.HealthComponent.Health;
        if (!_lastValue.HasValue || (_lastValue.Value != currentValue))
        {
            _text.text = $"<b>{currentValue}</b> HP";
            _lastValue = currentValue;
        }
    }
}
