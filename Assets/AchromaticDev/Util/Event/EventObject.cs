using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace AchromaticDev.Util.Event
{
    [CreateAssetMenu(fileName = "New Event", menuName = "AchromaticDev/Event/Event")]
    public class EventObject : ScriptableObject
    {
        [SerializeField] private List<EventListener> listeners = new List<EventListener>();

        public void Invoke()
        {
            for (int i = listeners.Count - 1; i >= 0; i--)
            {
                listeners[i].OnEventInvoked();
            }
        }

        public void RegisterListener(EventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(EventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
