using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AchromaticDev.Util;
using AchromaticDev.Util.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    public NodeManager nodeManager;
    public readonly Block[,] Blocks = new Block[4, 4];
    
    [SerializeField] private Transform blockParent;
    [SerializeField] private GameObject blockPrefab;
    
    private bool _isMoving = false;
    private Coroutine _coroutine;
    
    private IEnumerator Start()
    {
        yield return null;
        SpawnBlock();
        SpawnBlock();
    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (Blocks[i, j] == null) continue;
                Gizmos.color = Blocks[i, j].Value switch
                {
                    2 => Color.red,
                    4 => Color.blue,
                    8 => Color.green,
                    16 => Color.yellow,
                    32 => Color.magenta,
                    64 => Color.cyan,
                    128 => Color.gray,
                    256 => Color.black,
                    512 => Color.white,
                    _ => Color.clear
                };
                Gizmos.DrawWireCube(nodeManager.GetNodePosition(new Vector2Int(i, j)), Vector3.one);
            }
        }
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
        if (_isMoving)
        {
            foreach (var block in Blocks)
            {
                if (block != null)
                {
                    block.StopMove();
                }
            }
            
            _isMoving = false;
            StopCoroutine(_coroutine);
        };
        
        List<Block> blocks = new List<Block>();
        
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (Mathf.Abs(direction.x) == 1)
                {
                    blocks.Add(direction.x == -1 ? Blocks[j, i] : Blocks[3 - j, i]);
                }
                else
                {
                    blocks.Add(direction.y == -1 ? Blocks[i, j] : Blocks[i, 3 - j]);
                }
            }
        }

        foreach (var block in blocks.Where(block => block != null))
        {
            while (true)
            {
                var nextPosition = block.Position + direction;
                if (nextPosition.x is < 0 or >= 4 || nextPosition.y is < 0 or >= 4) break;
                if (Blocks[nextPosition.x, nextPosition.y] is not null)
                {
                    if (Blocks[nextPosition.x, nextPosition.y].Value == block.Value && !Blocks[nextPosition.x, nextPosition.y].isMerged)
                    {
                        Blocks[nextPosition.x, nextPosition.y].Merge(block);
                        Blocks[block.Position.x, block.Position.y] = null;
                        block.MoveTo(nextPosition);
                    }
                    break;
                };
                block.MoveTo(nextPosition);
            }
            
            _isMoving = true;
        }
        
        if (_isMoving)
        {
            _coroutine = StartCoroutine(Delay());

            foreach (var block in blocks.Where(block => block != null))
            {
                Blocks[block.Position.x, block.Position.y].isMerged = false;
            }
        }
        
        SpawnBlock();
    }
    
    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);
        _isMoving = false;
    }
    
    private void SpawnBlock()
    {
        var emptyBlocks = new List<Vector2Int>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (Blocks[i, j] == null)
                {
                    emptyBlocks.Add(new Vector2Int(i, j));
                }
            }
        }

        if (emptyBlocks.Count == 0) return;
        
        var blockObject = PoolManager.Instantiate(blockPrefab, blockParent);
        var block = blockObject.GetComponent<Block>();
        block.SetValue(2);
        var position = emptyBlocks[Random.Range(0, emptyBlocks.Count)];
        block.MoveTo(position, false, true);
    }
}
