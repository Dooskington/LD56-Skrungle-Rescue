using System.Linq;
using TMPro;
using UnityEngine;

public class WorkerCounterUIComponent : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private PlayerControllerComponent _player;

    private int? _lastTotalValue;
    /*
    private int? _lastFollowingValue;
    private int? _lastBusyValue;
    private int? _lastIdleValue;
    */

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _player = FindFirstObjectByType<PlayerControllerComponent>();
    }

    private void Update()
    {
        bool dirty = false;

        int totalRescued = _player.CommanderComponent.Workers.Count;
        int following = _player.CommanderComponent.Workers.Count(w => w.State == WorkerState.Following);
        int busy = _player.CommanderComponent.Workers.Count(w => w.State == WorkerState.PickingUp || w.State == WorkerState.Carrying);
        int idle = _player.CommanderComponent.Workers.Count(w => w.State == WorkerState.Idle);

        if (!_lastTotalValue.HasValue || (_lastTotalValue.Value != totalRescued))
        {
            _lastTotalValue = totalRescued;
            dirty = true;
        }

        /*
        if (!_lastFollowingValue.HasValue || (_lastFollowingValue.Value != following))
        {
            _lastFollowingValue = following;
            dirty = true;
        }

        if (!_lastBusyValue.HasValue || (_lastBusyValue.Value != busy))
        {
            _lastBusyValue = busy;
            dirty = true;
        }

        if (!_lastIdleValue.HasValue || (_lastIdleValue.Value != idle))
        {
            _lastIdleValue = idle;
            dirty = true;
        }
        */

        if (dirty)
        {
            int totalSkrungle = GameManager.Instance.TotalSkrungle;
            string str = $"<b>{totalRescued}/{totalSkrungle}</b> Skrungles Rescued";
            /*
            if (totalRescued > 0)
            {
                str += $"\n<size=75%><b>{_lastFollowingValue.Value}</b> Following\n<b>{_lastBusyValue.Value}</b> Busy\n<b>{_lastIdleValue.Value}</b> Idle</size>";
            }
            */

            _text.text = str;
        }
    }
}
