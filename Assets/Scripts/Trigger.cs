using System;
using AchromaticDev.Util.Event;
using AchromaticDev.Util.Notification;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public Vector2Int position;
    [SerializeField] private EventObject @event;
    
    public void OnMerged(Block block)
    {
        if (block.Position == position)
        {
            NotificationManager.Instance.ShowNotification($"- {block.Value} Damage");
            @event.Invoke();
        }
    }
}
