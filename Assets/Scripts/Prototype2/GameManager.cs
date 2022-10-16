using System;
using System.Collections;
using System.Collections.Generic;
using AchromaticDev.Util;
using UnityEngine;
using UnityEngine.Events;

namespace Prototype2
{
    public class GameManager : MonoSingleton<GameManager>
    {
        public UnityAction OnGameStart;
        public UnityAction OnGameEnd;
        public Board board;

        private void Start()
        {
            board.AddBlock(Vector2Int.one);
        }

        public IEnumerator InvokeDelayed(UnityAction action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                board.MoveBlock(Vector2Int.up);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                board.MoveBlock(Vector2Int.down);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                board.MoveBlock(Vector2Int.left);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                board.MoveBlock(Vector2Int.right);
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                board.AddBlock(Vector2Int.one);
            }
        }
    }
}
