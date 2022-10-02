using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "New BlockEvent", menuName = "AchromaticDev/Event/BlockEvent")]
public class BlockEventObject : ScriptableObject
{
    [SerializeField] private List<BlockEventListener> listeners = new List<BlockEventListener>();

    public void Invoke(Block block)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventInvoked(block);
        }
    }

    public void RegisterListener(BlockEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterListener(BlockEventListener listener)
    {
        listeners.Remove(listener);
    }
}
