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
        
        public IEnumerator InvokeDelayed(UnityAction action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action?.Invoke();
        }
    }
}
