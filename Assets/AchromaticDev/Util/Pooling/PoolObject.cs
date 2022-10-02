using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace AchromaticDev.Util.Pooling
{
    public class PoolObject : MonoBehaviour
    {
        [HideInInspector] public Pool pool;
        public UnityEvent onSpawn;
        public UnityEvent onDespawn;
    }
}