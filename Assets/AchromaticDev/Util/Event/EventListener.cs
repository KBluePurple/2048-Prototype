using UnityEngine;
using UnityEngine.Events;

namespace AchromaticDev.Util.Event
{
    public class EventListener : MonoBehaviour
    {
        public UnityEvent @event;
        
        public void OnEventInvoked()
        {
            @event.Invoke();
        }
    }
}
