using UnityEngine;
using UnityEngine.Serialization;

public class NodeManager : MonoBehaviour
{
    [SerializeField] private Transform[] nodes;
    
    public Vector2 GetNodePosition(Vector2Int position)
    {
        return nodes[position.x + position.y * 4].position;
    }
}
