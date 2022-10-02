using UnityEngine;
using UnityEngine.Events;

namespace AchromaticDev.Util.Event
{
    public class EventListener : MonoBehaviour
    {
        public EventObject eventObject;
        public UnityEvent @event;
        
        public void OnEventInvoked()
        {
            @event?.Invoke();
        }
        
        private void OnEnable()
        {
            eventObject.RegisterListener(this);
        }
        
        private void OnDisable()
        {
            eventObject.UnregisterListener(this);
        }
    }
}
