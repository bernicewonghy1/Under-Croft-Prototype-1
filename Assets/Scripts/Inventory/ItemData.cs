
using UnityEngine;

public enum ItemType
{
    Weapon,
    Helmet,
    Chestplate,
    Leggings,
    Boots,
    Consumable,
    Material
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Basic Information")]
    public string itemName;
    public Sprite itemIcon;
    [TextArea] public string description;
    public ItemType itemType;

    [Header("Weapon Stats")]
    public float damage;
    public float attackSpeed = 1f;

    [Header("Armour Stats")]
    public float maxDurability;
    [Range(0, 100)] public float damageReduction;
}
