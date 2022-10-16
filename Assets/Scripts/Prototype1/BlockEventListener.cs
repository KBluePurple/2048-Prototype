using UnityEngine;
using UnityEngine.Events;

public class BlockEventListener : MonoBehaviour
{
    public BlockEventObject blockEvent;
    public UnityEvent<Block> @event;
    
    private void OnEnable()
    {
        blockEvent.RegisterListener(this);
    }
    
    private void OnDisable()
    {
        blockEvent.UnregisterListener(this);
    }

    public void OnEventInvoked(Block block)
    {
        @event.Invoke(block);
    }
}