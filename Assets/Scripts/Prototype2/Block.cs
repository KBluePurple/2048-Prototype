using System;
using UnityEngine;

namespace Prototype2
{
    public class Block : MonoBehaviour
    {
        public Board board;
        public Vector2Int position;
        public int value;

        public void Move(Vector2Int direction)
        {
            if (Mathf.Abs(direction.x) != 1 && Mathf.Abs(direction.y) != 1) return;

            while (true)
            {
                var targetPosition = position + direction;
                if (board.IsBlockAt(targetPosition))
                {
                    var targetBlock = board.GetBlockAt(targetPosition);
                    if (targetBlock.value != value) break;
                    targetBlock.Merge(this);
                    board.RemoveBlock(this);
                    break;
                }

                if (board.IsPositionValid(targetPosition))
                {
                    board.MoveBlock(this, targetPosition);
                }
                else
                {
                    break;
                }
            }
        }

        public void MoveTo(Vector2Int targetPosition)
        {
            position = targetPosition;
            transform.position = board.GetPositionFromIndex(targetPosition);
        }

        private void Merge(Block other)
        {
            value += other.value;
            board.RemoveBlock(other);
        }

        public void Initialize(Board parentBoard, Vector2Int vector2Int, float nodeSize)
        {
            board = parentBoard;
            position = vector2Int;
            transform.position = parentBoard.GetPositionFromIndex(vector2Int);
        }
    }
}