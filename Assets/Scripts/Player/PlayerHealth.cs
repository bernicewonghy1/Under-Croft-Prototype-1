
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    public float CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth <= 0f) return;

        CurrentHealth -= Mathf.Max(0f, damage);
        CurrentHealth = Mathf.Clamp(
            CurrentHealth, 0f, maxHealth
        );

        Debug.Log("Player HP: " + CurrentHealth);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player has died!");
        // We'll add the death animation later.
    }

    public float GetHealthPercentage()
    {
        return CurrentHealth / maxHealth;
    }
}
