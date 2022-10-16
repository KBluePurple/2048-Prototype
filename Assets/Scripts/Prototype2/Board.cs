using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

namespace Prototype2
{   
    public class Board : MonoBehaviour
    {
        [Header("Details")]
        [SerializeField] private bool isAnimating = true;
        
        [Header("Board Settings")]
        [SerializeField] private int size = 4;
        [SerializeField] private int spacing = 20;
        [SerializeField] private GameObject cellPrefab;
        
        [Header("Events")]
        public UnityEvent onBoardGenerated;
        public UnityEvent onBoardCleared;

        private float _nodeSize;
        private Block[,] _blocks;
        private RectTransform _rectTransform;
        private bool _isAnimating;
        
        public int Size => size;
        public Block[,] Blocks => _blocks;

        private void Awake()
        {
            _rectTransform = transform as RectTransform;
            GenerateBoard();
        }

        private void GenerateBoard()
        {
            if (_isAnimating) return;
            if (_rectTransform == null) return;
            
            _isAnimating = true;

            _blocks = new Block[size, size];
            
            var rect = _rectTransform.rect;
            rect.height = rect.width;
            
            _nodeSize = rect.width / size - (float)spacing / size;

            var offset = new Vector2(spacing, -spacing);
            offset.x -= rect.width / 2 - _nodeSize / 2 + (float)spacing / 2;
            offset.y += rect.height / 2 - _nodeSize / 2 + (float)spacing / 2;

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    var cell = Instantiate(cellPrefab, transform);
                    var rectTransform = cell.transform as RectTransform;

                    var cellSize = _nodeSize - spacing;
                    rectTransform!.sizeDelta = new Vector2(cellSize, cellSize);
                    rectTransform.anchoredPosition = offset + new Vector2(_nodeSize * j, -_nodeSize * i);
                    _blocks[i, j] = cell.GetComponent<Block>();

                    if (isAnimating)
                        cell.transform.DOScale(1, 0.2f).From(Vector3.zero).SetDelay(0.1f * (i + j));
                }
            }

            StartCoroutine(GameManager.Instance.InvokeDelayed(() =>
            {
                _isAnimating = false;
                onBoardGenerated?.Invoke();
            }, 0.1f * (size * size)));
        }
        
        private void ClearBoard()
        {
            if (_isAnimating) return;
            if (_blocks == null) return;
            
            _isAnimating = true;
            
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    var cell = _blocks[i, j];
                    
                    if (cell == null) continue;
                    
                    if (isAnimating)
                        cell.transform.DOScale(0, 0.2f).SetDelay(0.1f * (i + j)).OnComplete(() => Destroy(cell.gameObject));
                    else
                        Destroy(cell.gameObject);
                }
            }

            StartCoroutine(GameManager.Instance.InvokeDelayed(() =>
            {
                _isAnimating = false;
                onBoardCleared?.Invoke();
                _blocks = null;
            }, 0.1f * (size * size)));
        }

        public void MoveBlock(Vector2Int direction)
        {
            Block[] blocks;
            if (direction == Vector2Int.up)
            {
                var query = from Block block in _blocks orderby block.transform.position.y descending select block;
                blocks = query.ToArray();
            }
            else if (direction == Vector2Int.down)
            {
                var query = from Block block in _blocks orderby block.transform.position.y select block;
                blocks = query.ToArray();
            }
            else if (direction == Vector2Int.left)
            {
                var query = from Block block in _blocks orderby block.transform.position.x select block;
                blocks = query.ToArray();
            }
            else if (direction == Vector2Int.right)
            {
                var query = from Block block in _blocks orderby block.transform.position.x descending select block;
                blocks = query.ToArray();
            }
            else
            {
                return;
            }
            
            foreach (var block in blocks)
            {
                block.Move(direction);
            }
        }

        public void MoveBlock(Block block, Vector2Int position)
        {
            _blocks[block.position.y, block.position.x] = null;
            _blocks[position.y, position.x] = block;
            block.Move(position);
        }

        public bool IsBlockAt(Vector2Int targetPosition)
        {
            return _blocks[targetPosition.x, targetPosition.y] != null;
        }

        public Block GetBlockAt(Vector2Int targetPosition)
        {
            return _blocks[targetPosition.x, targetPosition.y];
        }

        public bool IsPositionValid(Vector2Int targetPosition)
        {
            return targetPosition.x >= 0 && targetPosition.x < size && targetPosition.y >= 0 && targetPosition.y < size;
        }

        public void RemoveBlock(Block block)
        {
            _blocks[block.position.x, block.position.y] = null;
        }
    }
}