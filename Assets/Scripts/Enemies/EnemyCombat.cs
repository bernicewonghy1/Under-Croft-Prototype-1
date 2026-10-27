
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float attackCooldown = 2f;

    private PlayerHealth playerHealth;
    private float nextAttackTime;

    private void Start()
    {
        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (player == null || playerHealth == null)
            return;

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance <= attackRange && Time.time >= nextAttackTime)
        {
            Attack();
        }
    }

    private void Attack()
    {
        nextAttackTime = Time.time + attackCooldown;

        playerHealth.TakeDamage(attackDamage);

        Debug.Log(gameObject.name + " attacked the player for "
            + attackDamage + " damage!");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
