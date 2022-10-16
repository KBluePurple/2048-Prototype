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
                position += direction;
                    
                var targetPosition = position + direction;
                if (board.IsBlockAt(targetPosition))
                {
                    var targetBlock = board.GetBlockAt(targetPosition);
                    if (targetBlock.value != value) continue;
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
                    position -= direction;
                    break;
                }
            }
        }
        
        private void Merge(Block other)
        {
            value += other.value;
            board.RemoveBlock(other);
        }
    }
}