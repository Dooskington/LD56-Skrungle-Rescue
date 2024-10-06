using TMPro;
using UnityEngine;

public class CollectionCounterUIComponent : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private PlayerControllerComponent _player;

    private int? _lastTotalValue;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _player = FindFirstObjectByType<PlayerControllerComponent>();
    }

    private void Update()
    {
        bool dirty = false;

        int totalCollected = _player.CommanderComponent.ItemsCollected;

        if (!_lastTotalValue.HasValue || (_lastTotalValue.Value != totalCollected))
        {
            _lastTotalValue = totalCollected;
            dirty = true;
        }

        if (dirty)
        {
            string str = $"<b>{totalCollected}</b> Items Collected";
            _text.text = str;
        }
    }
}
