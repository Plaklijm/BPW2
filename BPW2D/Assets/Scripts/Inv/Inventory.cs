using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Inventory 
{
    public event EventHandler OnItemListChanged;

    private List<Item> itemList;
    private Action<Item> useitemAction;

    public Inventory(Action<Item> useitemAction)
    {
        this.useitemAction = useitemAction;
        itemList = new List<Item>();
    }

    public void AddItem(Item item)
    {
        if (item.IsStackable())
        {
            bool itemAlreadyInInv = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += 1;
                    itemAlreadyInInv = true;
                }
            }
            if (!itemAlreadyInInv)
            {
                itemList.Add(item);
            }
        }
        else
        {
            itemList.Add(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void RemoveItem(Item item)
    {
        if (item.IsStackable())
        {
            Item itemInInventory = null;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount -= item.amount;
                    itemInInventory = inventoryItem;
                }
            }
            if (itemInInventory != null && itemInInventory.amount <= 0)
            {
                itemList.Remove(itemInInventory);
            }
        }
        else
        {
            itemList.Remove(item);
        }
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

    public void UseItem(Item item)
    {
        useitemAction(item);
    }

    public List<Item> GetItemList()
    {
        return itemList;
    }
}