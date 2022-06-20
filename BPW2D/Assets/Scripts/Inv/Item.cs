using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{
    public enum ItemType
    {
        HealthPoition,
        ManaPotion,
        Coin
    }

    public ItemType itemType;
    public int amount = 1;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPoition:    return ItemAssets.I_ItemAssets.healthPoitionSprite;
            case ItemType.ManaPotion:       return ItemAssets.I_ItemAssets.manaPotionSprite;
            case ItemType.Coin:             return ItemAssets.I_ItemAssets.coinSprite;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.HealthPoition:
            case ItemType.ManaPotion:
            case ItemType.Coin:
                return true;
        }
    }
}
