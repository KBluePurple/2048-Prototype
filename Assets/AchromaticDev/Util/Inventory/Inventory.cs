using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace AchromaticDev.Util.Inventory
{
    [Serializable]
    public class Inventory<T> : IEnumerable where T : Enum
    {
        public int Size => size;
        [FormerlySerializedAs("_size")] [SerializeField]
        private int size;

        public Action<ItemStack<T>> OnItemAdded;
        public Action<ItemStack<T>> OnItemRemoved;

        [SerializeField] private List<ItemStack<T>> items = new List<ItemStack<T>>();

        public Inventory(int size)
        {
            this.size = size;
        }

        public IEnumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public void AddItem(T type, int count)
        {
            var item = GetItem(type);
            if (item == null)
            {
                if (items.Count >= size)
                    return;

                item = new ItemStack<T>(type, count);
                item.inventory = this;
                items.Add(item);
                OnItemAdded?.Invoke(item);
            }
            else
            {
                item.Count += count;
                OnItemAdded?.Invoke(new ItemStack<T>(type, count));
            }
        }

        public void RemoveItem(T type, int count)
        {
            var item = GetItem(type);
            if (item == null)
                return;

            item.Count -= count;
            if (item.Count <= 0)
            {
                items.Remove(item);
            }
            OnItemRemoved?.Invoke(new ItemStack<T>(type, count));
        }

        public ItemStack<T> GetItem(T type)
        {
            foreach (var item in items)
            {
                if (item.Type.Equals(type))
                    return item;
            }

            return null;
        }

        public bool IsEmpty()
        {
            return items.Count == 0;
        }

        public bool IsFull()
        {
            return items.Count == size;
        }

        public void Clear()
        {
            items.Clear();
        }
    }
}
