using System;
using UnityEngine;

namespace AchromaticDev.Util.Inventory
{
    [Serializable]
    public abstract class ItemBase<T> where T : Enum
    {
        public T Type => _type;
        [SerializeField] private readonly T _type;

        public ItemBase(T type)
        {
            _type = type;
        }
    }
}