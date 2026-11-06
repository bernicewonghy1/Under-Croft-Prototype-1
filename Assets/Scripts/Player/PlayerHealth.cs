
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;

    public float CurrentHealth { get; private set; }

    public event Action OnPlayerDeath;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (CurrentHealth <= 0f)
            return;

        CurrentHealth -= Mathf.Max(0f, damage);
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0f, maxHealth);

        Debug.Log("Player HP: " + CurrentHealth);

        if (CurrentHealth <= 0f)
        {
            Die();
        }
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

        Debug.Log("Player healed! HP: " + CurrentHealth);
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
