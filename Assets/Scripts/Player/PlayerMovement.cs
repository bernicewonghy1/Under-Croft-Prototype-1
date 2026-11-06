
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

        // update walking and idle animations
        if (playerAnimator != null)
        {
            playerAnimator.SetBool(
                "IsMoving",
                movement.sqrMagnitude > 0f
            );
        }

        // check whether an attack animation is playing
        bool isAttacking = false;

        if (playerAnimator != null)
        {
            AnimatorStateInfo state =
                playerAnimator.GetCurrentAnimatorStateInfo(0);

            isAttacking =
                state.IsName("LeftPlayerAttack") ||
                state.IsName("RightPlayerAttack");

            if (playerAnimator.IsInTransition(0))
            {
                AnimatorStateInfo nextState =
                    playerAnimator.GetNextAnimatorStateInfo(0);

                isAttacking = isAttacking ||
                    nextState.IsName("LeftPlayerAttack") ||
                    nextState.IsName("RightPlayerAttack");
            }
        }

        // only flip the sprite when not attacking
        if (spriteRenderer != null && !isAttacking)
        {
            if (horizontal < 0f)
            {
                spriteRenderer.flipX = true;
            }
            else if (horizontal > 0f)
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
