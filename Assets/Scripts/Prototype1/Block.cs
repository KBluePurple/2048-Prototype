using System;
using System.Collections;
using AchromaticDev.Util.Pooling;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Block : MonoBehaviour
{
    public Vector2Int Position = Vector2Int.zero;
    public int Value;
    
    [SerializeField] private Text text;
    [SerializeField] private BlockEventObject blockEventObject;
    
    private Image _image;
    private GameManager _gameManager;
    private NodeManager _nodeManager;
    public bool isMerged = false;

    private void Initialize()
    {
        _gameManager = GameManager.Instance;
        _nodeManager = GameManager.Instance.nodeManager;
    }

    public void MoveTo(Vector2Int targetPos, bool useAnimation = true, bool isInit = false)
    {
        if (!isInit)
            _gameManager.Blocks[Position.x, Position.y] = null;
        
        if (_gameManager.Blocks[targetPos.x, targetPos.y] == null)
            _gameManager.Blocks[targetPos.x, targetPos.y] = this;
        
        Position = targetPos;
        
        Vector2 targetPosition = _nodeManager.GetNodePosition(Position);
        
        if (useAnimation)
            transform.DOMove(targetPosition, 0.2f);
        else
            transform.position = targetPosition;
    }
    
    public void SetValue(int newValue)
    {
        Value = newValue;
        text.text = newValue.ToString();
    }
    
    public void Merge(Block otherBlock)
    {
        SetValue(Value + otherBlock.Value);
        StartCoroutine(otherBlock.Destroy());
        blockEventObject.Invoke(this);
        isMerged = true;
    }
    
    public IEnumerator Destroy()
    {
        transform.SetSiblingIndex(0);
        _image.DOFade(0f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        StopMove();
        PoolManager.Destroy(gameObject);
    }

    public void StopMove()
    {
        DOTween.Kill(transform);
        if (_image != null)
            DOTween.Kill(_image);
        
        Vector2 targetPos = _nodeManager.GetNodePosition(Position);
        transform.position = targetPos;
    }
    
    public void OnSpawn()
    {
        Initialize();
        _image = GetComponent<Image>();
        _image.DOFade(0.5f, 0.2f).From(0);
    }
    
    public void OnDespawn()
    {
        StopMove();
        Position = Vector2Int.zero;
    }
}
