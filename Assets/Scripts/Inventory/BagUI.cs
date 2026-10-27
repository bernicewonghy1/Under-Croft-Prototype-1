
using UnityEngine;
using UnityEngine.UI;

public class BagUI : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Image[] itemIcons = new Image[12];

    private void Update()
    {
        if (inventoryManager == null)
            return;

        for (int i = 0; i < itemIcons.Length; i++)
        {
            if (itemIcons[i] == null)
                continue;

            InventoryItem item = inventoryManager.GetBagItem(i);

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
