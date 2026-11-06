
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HotbarUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Inventory")]
    [SerializeField] private InventoryManager inventoryManager;

    [Header("Player")]
    [SerializeField] private PlayerHealth playerHealth;

    [Header("Hotbar Slots")]
    [SerializeField] private Image[] slotImages = new Image[5];

    [Header("Item Icons")]
    [SerializeField] private Image[] itemIcons = new Image[5];

    [Header("Slot Colours")]
    [SerializeField] private Color normalColour = Color.gray;
    [SerializeField] private Color selectedColour = Color.yellow;

    [Header("Potion Settings")]
    [SerializeField] private ItemData healthPotion;
    [SerializeField] private float healAmount = 30f;

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (eventData.clickCount < 2)
            return;

        for (int i = 0; i < itemIcons.Length; i++)
        {
            if (itemIcons[i] == null)
                continue;

            if (RectTransformUtility.RectangleContainsScreenPoint(
                itemIcons[i].rectTransform,
                eventData.position,
                eventData.pressEventCamera))
            {
                UsePotion(i);
                break;
            }
        }
    }

    private void UsePotion(int index)
    {
        if (inventoryManager == null ||
            playerHealth == null ||
            healthPotion == null)
            return;

        InventoryItem item = inventoryManager.GetHotbarItem(index);

        if (item == null || item.itemData != healthPotion)
            return;

        playerHealth.Heal(healAmount);
        inventoryManager.RemoveHotbarItem(index);

        Debug.Log("Health potion consumed!");
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
