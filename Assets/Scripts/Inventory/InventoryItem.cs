
using System;
using UnityEngine;

[Serializable]
public class InventoryItem
{
    public ItemData itemData;
    public float currentDurability;

    public InventoryItem(ItemData data)
    {
        itemData = data;
        currentDurability = data.maxDurability;
    }
}
