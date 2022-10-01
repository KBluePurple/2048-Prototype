using System.Collections;
using System.Collections.Generic;
using AchromaticDev.Util;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public NodeManager nodeManager;
    
    [SerializeField] private Transform blockParent;
    [SerializeField] private GameObject blockPrefab;
    
    public readonly Block[,] Blocks = new Block[4, 4];
    
    private IEnumerator Start()
    {
        yield return null;
        var blockObject = Instantiate(blockPrefab, blockParent);
        var block = blockObject.GetComponent<Block>();
        block.SetValue(2);
        block.Position = new Vector2Int(0, 0);
        block.MoveTo(new Vector2Int(0, 0), false);
        Blocks[0, 0] = block;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveBlock(Vector2Int.down);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            MoveBlock(Vector2Int.up);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveBlock(Vector2Int.left);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveBlock(Vector2Int.right);
        }
    }

    private void MoveBlock(Vector2Int direction)
    {
        for (int i = 0; i < 4; i++)
        {
            List<Block> blocks = new List<Block>();

            for (int j = 0; j < 4; j++)
            {
                if (Mathf.Abs(direction.y) == 1)
                {
                    blocks.Add(direction.x == 1 ? Blocks[i, j] : Blocks[i, 3 - j]);
                }
                else
                {
                    blocks.Add(direction.x == 1 ? Blocks[j, i] : Blocks[3 - j, i]);
                }
            }
        }
        
        // TODO: Process blocks
    }
    
    private bool CanMoveBlock(int x, int y, Vector2Int direction)
    {
        int newX = x + direction.x;
        int newY = y + direction.y;

        if (newX is < 0 or > 3 || newY is < 0 or > 3)
            return false;
        if (Blocks[newX, newY] is null)
            return true;

        return true;
    }
}
