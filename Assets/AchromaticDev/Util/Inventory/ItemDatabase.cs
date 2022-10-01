using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AchromaticDev.Util.Inventory
{
    public class ItemInfo<T> where T : Enum
    {
        public T Type { get; }
        public Sprite Icon { get; }
        public string Name { get; }
        public string Description { get; }
        public int MaxStack { get; }

        public ItemBase<T> ItemClass { get; }

        public ItemInfo(T type, Sprite icon, string name, string description, int maxStack, ItemBase<T> itemClass)
        {
            Type = type;
            Icon = icon;
            Name = name;
            Description = description;
            MaxStack = maxStack;
            ItemClass = itemClass;
        }
    }

    public static class ItemDatabase<T> where T : Enum
    {
        private static Dictionary<T, ItemInfo<T>> _itemInfos = new Dictionary<T, ItemInfo<T>>();

        public static void RegisterItemClass(T type, Sprite icon, string name, string description, int maxStack)
        {
            if (_itemInfos.ContainsKey(type))
                throw new ArgumentException("Item type already registered");

            _itemInfos.Add(type, new ItemInfo<T>(type, icon, name, description, maxStack, null));
        }

        public static void RegisterItemClass(T type, Sprite icon, string name, string description, int maxStack = 99, ItemBase<T> itemClass = null)
        {
            if (_itemInfos.ContainsKey(type))
                throw new ArgumentException("Item type already registered");

            _itemInfos.Add(type, new ItemInfo<T>(type, icon, name, description, maxStack, itemClass));
        }

        public static ItemBase<T> GetItemClass(T type)
        {
            if (!_itemInfos.ContainsKey(type))
                throw new ArgumentException($"Item type {type} not registered");

            return _itemInfos[type].ItemClass;
        }

        public static ItemInfo<T> GetItemInfo(T type)
        {
            if (!_itemInfos.ContainsKey(type))
                throw new ArgumentException($"Item type {type} not registered");

            return _itemInfos[type];
        }
    }
}
