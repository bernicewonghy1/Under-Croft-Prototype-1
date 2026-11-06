
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer playerSprite;

    [Header("Attack Settings")]
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackRadius = 0.7f;
    [SerializeField] private LayerMask enemyLayer;

    private float nextAttackTime = 0f;
    private Vector2 facingDirection = Vector2.right;

    private void Update()
    {
        UpdateFacingDirection();

        if (Input.GetMouseButtonDown(0))
        {
            PlayAttackAnimation();
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

    private void PlayAttackAnimation()
    {
        if (playerAnimator == null)
            return;

        playerAnimator.ResetTrigger("LeftAttack");
        playerAnimator.ResetTrigger("RightAttack");

        if (playerSprite != null)
        {
            playerSprite.flipX = false;
        }

        if (facingDirection.x < 0f)
        {
            playerAnimator.SetTrigger("RightAttack");
        }
        else
        {
            playerAnimator.SetTrigger("LeftAttack");
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
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 attackPosition =
            (Vector2)transform.position + facingDirection * attackRange;

        Gizmos.DrawWireSphere(attackPosition, attackRadius);
    }
}
