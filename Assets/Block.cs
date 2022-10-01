using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public Vector2Int Position;
    
    [SerializeField] private int value;
    [SerializeField] private Text text;
    
    private GameManager _gameManager;
    private NodeManager _nodeManager;

    private void Start()
    {
        _gameManager = GameManager.Instance;
        _nodeManager = GameManager.Instance.nodeManager;
    }

    public void MoveTo(Vector2Int targetPos, bool useAnimation = true)
    {
        Vector2 targetPosition = _nodeManager.GetNodePosition(targetPos);
        
        if (useAnimation)
            transform.DOMove(targetPosition, 0.2f);
        else
            transform.position = targetPosition;
    }
    
    public void SetValue(int newValue)
    {
        value = newValue;
        text.text = newValue.ToString();
    }
}
