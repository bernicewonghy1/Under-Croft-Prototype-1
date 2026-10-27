
using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private GameObject swordSlash;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float attackRadius = 0.7f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Visual Settings")]
    [SerializeField] private float slashDuration = 0.15f;

    private float nextAttackTime = 0f;
    private Vector2 facingDirection = Vector2.down;
    private Coroutine slashCoroutine;

    private void Update()
    {
        UpdateFacingDirection();

        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void UpdateFacingDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(horizontal, vertical);

        if (movement.sqrMagnitude > 0.01f)
        {
            facingDirection = movement.normalized;
        }
    }

    private void Attack()
    {
        if (inventoryManager == null)
            return;

        InventoryItem equippedItem = inventoryManager.GetHotbarItem(0);

        if (equippedItem == null || equippedItem.itemData == null)
            return;

        ItemData weapon = equippedItem.itemData;

        if (weapon.itemType != ItemType.Weapon)
            return;

        if (Time.time < nextAttackTime)
            return;

        float attackSpeed = Mathf.Max(0.01f, weapon.attackSpeed);
        nextAttackTime = Time.time + (1f / attackSpeed);

        Vector2 attackPosition =
            (Vector2)transform.position + facingDirection * attackRange;

        Collider2D[] enemies = Physics2D.OverlapCircleAll(
            attackPosition,
            attackRadius,
            enemyLayer
        );

        foreach (Collider2D enemy in enemies)
        {
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(weapon.damage);
            }
        }

        ShowSwordSlash();
    }

    private void ShowSwordSlash()
    {
        if (swordSlash == null)
            return;

        // Position the slash in the direction the player faces.
        swordSlash.transform.localPosition =
            new Vector3(facingDirection.x, facingDirection.y, 0f)
            * attackRange;

        // Rotate the slash so it faces the correct direction.
        float angle = Mathf.Atan2(
            facingDirection.y,
            facingDirection.x
        ) * Mathf.Rad2Deg;

        swordSlash.transform.localRotation =
            Quaternion.Euler(0f, 0f, angle + 90f);

        if (slashCoroutine != null)
        {
            StopCoroutine(slashCoroutine);
        }

        slashCoroutine = StartCoroutine(DisplaySlash());
    }

    private IEnumerator DisplaySlash()
    {
        swordSlash.SetActive(true);

        yield return new WaitForSeconds(slashDuration);

        swordSlash.SetActive(false);
        slashCoroutine = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 attackPosition =
            (Vector2)transform.position + facingDirection * attackRange;

        Gizmos.DrawWireSphere(attackPosition, attackRadius);
    }
}
