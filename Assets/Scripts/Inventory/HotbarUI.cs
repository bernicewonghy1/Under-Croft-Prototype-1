
using UnityEngine;
using UnityEngine.UI;

public class HotbarUI : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] private InventoryManager inventoryManager;

    [Header("Hotbar Slots")]
    [SerializeField] private Image[] slotImages = new Image[5];

    [Header("Item Icons")]
    [SerializeField] private Image[] itemIcons = new Image[5];

    [Header("Slot Colours")]
    [SerializeField] private Color normalColour = Color.gray;
    [SerializeField] private Color selectedColour = Color.yellow;

    private int selectedSlot = 0;

    private void Start()
    {
        UpdateHotbar();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectSlot(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectSlot(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectSlot(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectSlot(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) SelectSlot(4);

        UpdateHotbar();
    }

    private void SelectSlot(int slotIndex)
    {
        selectedSlot = slotIndex;
        UpdateHotbar();
    }

    private void UpdateHotbar()
    {
        for (int i = 0; i < 5; i++)
        {
            if (slotImages[i] != null)
            {
                slotImages[i].color =
                    i == selectedSlot ? selectedColour : normalColour;
            }

            if (inventoryManager == null || itemIcons[i] == null)
                continue;

            InventoryItem item = inventoryManager.GetHotbarItem(i);

            if (item == null || item.itemData == null)
            {
                itemIcons[i].enabled = false;
                continue;
            }

            itemIcons[i].enabled = true;

            if (item.itemData.itemIcon != null)
            {
                itemIcons[i].sprite = item.itemData.itemIcon;
                itemIcons[i].color = Color.white;
            }
            else
            {
                itemIcons[i].sprite = null;
                itemIcons[i].color =
                    item.itemData.itemType == ItemType.Weapon
                    ? new Color(0.8f, 0.5f, 0.3f)
                    : new Color(0.4f, 0.6f, 0.9f);
            }
        }
    }
}
