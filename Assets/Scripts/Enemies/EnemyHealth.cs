
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Health Bar")]
    [SerializeField] private Slider healthBar;

    [Header("Damage Flash")]
    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private Color damageColour = Color.red;
    [SerializeField] private float flashDuration = 0.15f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip enemyHurtSound;

    private float currentHealth;
    private Color originalColour;
    private Coroutine flashCoroutine;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (enemySprite == null)
            enemySprite = GetComponentInChildren<SpriteRenderer>();

        if (enemySprite != null)
            originalColour = enemySprite.color;

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0f)
            return;

        currentHealth -= Mathf.Max(0f, damage);
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        // play hurt sound, including on the killing hit
        if (enemyHurtSound != null)
        {
            if (currentHealth <= 0f)
            {
                AudioSource.PlayClipAtPoint(
                    enemyHurtSound,
                    transform.position
                );
            }
            else if (audioSource != null)
            {
                audioSource.PlayOneShot(enemyHurtSound);
            }
        }

        UpdateHealthBar();

        if (currentHealth <= 0f)
        {
            Die();
        }
        else
        {
            if (flashCoroutine != null)
                StopCoroutine(flashCoroutine);

            flashCoroutine = StartCoroutine(DamageFlash());
        }
    }

    private IEnumerator DamageFlash()
    {
        if (enemySprite == null)
            yield break;

        enemySprite.color = damageColour;

        yield return new WaitForSeconds(flashDuration);

        enemySprite.color = originalColour;
        flashCoroutine = null;
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.value = currentHealth / maxHealth;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died!");
        Destroy(gameObject);
    }
}
