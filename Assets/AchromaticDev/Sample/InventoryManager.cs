using System.Collections;
using System.Collections.Generic;
using AchromaticDev.Util.Inventory;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory<ItemType> inventory;

    private void Start()
    {
        inventory = new Inventory<ItemType>(10);
        inventory.OnItemAdded += OnItemAdded;
        inventory.OnItemRemoved += OnItemRemoved;
    }

    private void OnItemAdded(ItemStack<ItemType> item)
    {
        Debug.Log($"Item Added: {item.Type} x {item.Count}");
    }

    private void OnItemRemoved(ItemStack<ItemType> item)
    {
        Debug.Log($"Item Removed: {item.Type} x {item.Count}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            inventory.AddItem(ItemType.Apple, 1);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventory.AddItem(ItemType.Bread, 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            inventory.AddItem(ItemType.Stone, 1);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventory.AddItem(ItemType.Iron, 1);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            inventory.AddItem(ItemType.Gold, 1);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            inventory.AddItem(ItemType.Diamond, 1);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            inventory.AddItem(ItemType.Fish, 1);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            inventory.AddItem(ItemType.Chicken, 1);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            inventory.AddItem(ItemType.Pork, 1);
        }
    }
}
