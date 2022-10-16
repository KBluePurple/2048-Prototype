using System;
using System.Linq;
using AchromaticDev.Util.Pooling;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

namespace Prototype2
{
    public class Board : MonoBehaviour
    {
        [Header("Details")] [SerializeField] private bool isAnimating = true;

        [Header("Board Settings")] [SerializeField]
        private int size = 4;

        [SerializeField] private int spacing = 20;
        [SerializeField] private GameObject cellPrefab;

        [Header("Events")] public UnityEvent onBoardGenerated;
        public UnityEvent onBoardCleared;

        [Header("Block Settings")] [SerializeField]
        private GameObject blockPrefab;

        private float _nodeSize;
        private Transform[,] _cellTransforms;
        private Block[,] _blocks;
        private RectTransform _rectTransform;
        private bool _isAnimating;

        public int Size => size;

        private void Awake()
        {
            _rectTransform = transform as RectTransform;
            GenerateBoard();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            if (_blocks == null)
                return;
            
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (_blocks[i, j] != null)
                    {
                        Gizmos.DrawWireCube(_cellTransforms[i, j].position, Vector3.one);
                    }
                }
            }
        }

        private void GenerateBoard()
        {
            if (_isAnimating) return;
            if (_rectTransform == null) return;

            _isAnimating = true;

            _blocks = new Block[size, size];
            _cellTransforms = new Transform[size, size];

            var rect = _rectTransform.rect;
            rect.height = rect.width;

            _nodeSize = rect.width / size - (float)spacing / size;

            var offset = new Vector2(spacing, -spacing);
            offset.x -= rect.width / 2 - _nodeSize / 2 + (float)spacing / 2;
            offset.y += rect.height / 2 - _nodeSize / 2 + (float)spacing / 2;
            offset.y -= (size - 1) * _nodeSize;

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    var cell = Instantiate(cellPrefab, transform);
                    var rectTransform = cell.transform as RectTransform;

                    var cellSize = _nodeSize - spacing;
                    rectTransform!.sizeDelta = new Vector2(cellSize, cellSize);
                    rectTransform.anchoredPosition = offset + new Vector2(_nodeSize * j, _nodeSize * i);
                    _cellTransforms[j, i] = cell.transform;

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
            if (_cellTransforms == null) return;

            _isAnimating = true;

            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    var cell = _cellTransforms[i, j];
                    var block = _blocks[i, j];

                    if (cell != null)
                    {
                        if (isAnimating)
                            cell.transform.DOScale(0, 0.2f).SetDelay(0.1f * (i + j) + 1f)
                                .OnComplete(() => Destroy(cell.gameObject));
                        else
                            Destroy(cell.gameObject);
                    }

                    if (block != null)
                    {
                        if (isAnimating)
                            block.transform.DOScale(0, 0.2f).SetDelay(0.1f * (i + j))
                                .OnComplete(() => Destroy(block.gameObject));
                        else
                            Destroy(block.gameObject);
                    }
                }
            }

            StartCoroutine(GameManager.Instance.InvokeDelayed(() =>
            {
                _isAnimating = false;
                onBoardCleared?.Invoke();
                _blocks = null;
                _cellTransforms = null;
            }, 0.1f * (size * size)));
        }

        public void MoveBlock(Vector2Int direction)
        {
            Vector2Int tempDirection = direction;
            (direction.x, direction.y) = (direction.y, direction.x);

            Block[] blocks;
            var query = from Block block in this._blocks where block != null select block;
            if (tempDirection == Vector2Int.up)
            {
                var query2 = from block in query
                    orderby block.transform.position.y descending
                    select block;

                blocks = query2.ToArray();
            }
            else if (tempDirection == Vector2Int.down)
            {
                var query2 = from block in query
                    orderby block.transform.position.y
                    select block;

                blocks = query2.ToArray();
            }
            else if (tempDirection == Vector2Int.left)
            {
                var query2 = from block in query
                    orderby block.transform.position.x
                    select block;

                blocks = query2.ToArray();
            }
            else if (tempDirection == Vector2Int.right)
            {
                var query2 = from block in query
                    orderby block.transform.position.x descending
                    select block;

                blocks = query2.ToArray();
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
            block.MoveTo(position);
        }

        public bool IsBlockAt(Vector2Int targetPosition)
        {
            if (IsPositionValid(targetPosition))
                return _blocks[targetPosition.y, targetPosition.x] != null;
            return false;
        }

        public Block GetBlockAt(Vector2Int targetPosition)
        {
            return _blocks[targetPosition.y, targetPosition.x];
        }

        public bool IsPositionValid(Vector2Int targetPosition)
        {
            return targetPosition.x >= 0 && targetPosition.x < size && targetPosition.y >= 0 && targetPosition.y < size;
        }

        public void RemoveBlock(Block block)
        {
            _blocks[block.position.y, block.position.x] = null;
            Destroy(block.gameObject);
        }

        public void AddBlock(Vector2Int position)
        {
            // TODO: 풀 매니저 문제 해결 필요
            var blockObject = Instantiate(blockPrefab, transform);
            var block = blockObject.GetComponent<Block>();
            block.Initialize(this, position, _nodeSize);
            _blocks[position.y, position.x] = block;
        }

        public Vector3 GetPositionFromIndex(Vector2Int targetPosition)
        {
            return IsPositionValid(targetPosition) ? _cellTransforms[targetPosition.y, targetPosition.x].position : Vector3.zero;
        }
    }
}