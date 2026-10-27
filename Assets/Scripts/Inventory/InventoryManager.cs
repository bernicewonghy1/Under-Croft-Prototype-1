
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] private int bagSize = 12;

    [Header("Starting Items")]
    [SerializeField] private ItemData startingSword;
    private InventoryItem[] hotbarItems = new InventoryItem[5];
    private InventoryItem[] bagItems;

    private void Awake()
    {
        bagItems = new InventoryItem[bagSize];
    }

    private void Start()
    {
        AddItem(startingSword);
    }

    public bool AddItem(ItemData itemData)
    {
        if (itemData == null)
            return false;

        InventoryItem newItem = new InventoryItem(itemData);

        // Fill empty hotbar slots first.
        for (int i = 0; i < hotbarItems.Length; i++)
        {
            if (hotbarItems[i] == null)
            {
                hotbarItems[i] = newItem;
                Debug.Log(itemData.itemName + " added to hotbar slot " + (i + 1));
                return true;
            }
        }

        // If hotbar is full, put item in bag.
        for (int i = 0; i < bagItems.Length; i++)
        {
            if (bagItems[i] == null)
            {
                bagItems[i] = newItem;
                Debug.Log(itemData.itemName + " added to bag slot " + (i + 1));
                return true;
            }
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    public InventoryItem GetHotbarItem(int index)
    {
        if (index < 0 || index >= hotbarItems.Length)
            return null;

        return hotbarItems[index];
    }

    public InventoryItem GetBagItem(int index)
    {
        if (index < 0 || index >= bagItems.Length)
            return null;

        return bagItems[index];
    }

    public void SwapItems(bool firstIsHotbar, int firstIndex,
                          bool secondIsHotbar, int secondIndex)
    {
        InventoryItem[] firstInventory = firstIsHotbar ? hotbarItems : bagItems;
        InventoryItem[] secondInventory = secondIsHotbar ? hotbarItems : bagItems;

        if (firstIndex < 0 || firstIndex >= firstInventory.Length ||
            secondIndex < 0 || secondIndex >= secondInventory.Length)
            return;

        InventoryItem temp = firstInventory[firstIndex];
        firstInventory[firstIndex] = secondInventory[secondIndex];
        secondInventory[secondIndex] = temp;
    }
}
