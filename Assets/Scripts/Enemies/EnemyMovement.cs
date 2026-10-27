
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float detectionRange = 6f;
    [SerializeField] private float stoppingDistance = 1.2f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(rb.position, player.position);

        // Only chase the player when within detection range.
        if (distance > detectionRange)
            return;

        // Stop when close enough to attack.
        if (distance <= stoppingDistance)
            return;

        Vector2 direction =
            ((Vector2)player.position - rb.position).normalized;

        Vector2 newPosition =
            rb.position + direction * moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(newPosition);
    }
}
