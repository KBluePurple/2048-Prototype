using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace AchromaticDev.Util.Inventory
{
    [Serializable]
    public class ItemStack<T> where T : Enum
    {
        public T Type => type;
        [SerializeField] private T type;
        
        public Inventory<T> Inventory => inventory;
        [NonSerialized] internal Inventory<T> inventory;

        public int Count
        {
            get => count;
            set => SetCount(value);
        }
        [SerializeField] private int count;

        public ItemStack(T type, int count)
        {
            this.type = type;
            this.count = count;
        }

        public void SetCount(int i)
        {
            if (i < 0)
                throw new ArgumentOutOfRangeException("Count cannot be less than 0");

            if (i > ItemDatabase<T>.GetItemInfo(type).MaxStack)
                throw new ArgumentOutOfRangeException("Count cannot be greater than the max stack size");
        }
    }
}