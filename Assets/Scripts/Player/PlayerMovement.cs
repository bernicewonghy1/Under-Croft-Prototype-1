
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Animation")]
    [SerializeField] private Animator playerAnimator;

    private Rigidbody2D rb;
    private Vector2 movement;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (playerAnimator != null)
        {
            spriteRenderer = playerAnimator.GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontal, vertical).normalized;

        // switch between idle and walking
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsMoving", movement.sqrMagnitude > 0f);
        }

        // flip the sprite left or right
        if (spriteRenderer != null)
        {
            if (horizontal < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (horizontal > 0)
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(
            rb.position + movement * moveSpeed * Time.fixedDeltaTime
        );
    }
}
