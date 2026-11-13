
using UnityEngine;
using System;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Damage Flash")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Color damageColour = Color.red;
    [SerializeField] private float flashDuration = 0.15f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip playerHurtSound;

    public float CurrentHealth { get; private set; }

    public event Action OnPlayerDeath;

    private PlayerCombat playerCombat;
    private Color originalColour;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        playerCombat = GetComponent<PlayerCombat>();

        if (playerSprite == null)
            playerSprite = GetComponentInChildren<SpriteRenderer>();

        if (playerSprite != null)
            originalColour = playerSprite.color;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth <= 0f)
            return;

        if (playerCombat != null && playerCombat.IsBlocking)
        {
            Debug.Log("Attack blocked!");
            return;
        }

        CurrentHealth -= Mathf.Max(0f, damage);
        CurrentHealth = Mathf.Clamp(
            CurrentHealth, 0f, maxHealth
        );

        // play player hurt sound
        if (audioSource != null && playerHurtSound != null)
            audioSource.PlayOneShot(playerHurtSound);

        if (flashCoroutine != null)
            StopCoroutine(flashCoroutine);

        flashCoroutine = StartCoroutine(DamageFlash());

        if (CurrentHealth <= 0f)
            Die();
    }

    public void Heal(float amount)
    {
        if (CurrentHealth <= 0f)
            return;

        CurrentHealth = Mathf.Clamp(
            CurrentHealth + Mathf.Max(0f, amount),
            0f,
            maxHealth
        );
    }

    private IEnumerator DamageFlash()
    {
        if (playerSprite == null)
            yield break;

        playerSprite.color = damageColour;

        yield return new WaitForSeconds(flashDuration);

        playerSprite.color = originalColour;
        flashCoroutine = null;
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        OnPlayerDeath?.Invoke();
    }

    public float GetHealthPercentage()
    {
        return CurrentHealth / maxHealth;
    }
}
