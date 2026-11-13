
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
    private PlayerCombat playerCombat;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCombat = GetComponent<PlayerCombat>();

        if (playerAnimator != null)
        {
            spriteRenderer =
                playerAnimator.GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2(horizontal, vertical).normalized;

        bool isBlocking =
            playerCombat != null && playerCombat.IsBlocking;

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

            // stop walking animation while blocking
            playerAnimator.SetBool(
                "IsMoving",
                movement.sqrMagnitude > 0f && !isBlocking
            );
        }

        // don't flip sprite during attacks or blocking
        if (spriteRenderer != null &&
            !isAttacking &&
            !isBlocking)
        {
            if (horizontal < 0f)
                spriteRenderer.flipX = true;
            else if (horizontal > 0f)
                spriteRenderer.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(
            rb.position +
            movement * moveSpeed * Time.fixedDeltaTime
        );
    }
}
