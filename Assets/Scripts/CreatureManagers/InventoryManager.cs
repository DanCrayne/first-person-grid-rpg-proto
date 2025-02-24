using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<ItemData> items = new List<ItemData>();

    public void AddItem(ItemData item)
    {
        items.Add(item);
    }

    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
    }

    public void TransferItem(ItemData item, InventoryManager otherInventory)
    {
        // TODO
    }

    public void TransferAllItems(InventoryManager otherInventory)
    {
        // TODO
    }

    public void SortItems()
    {
        // TODO
    }
}
