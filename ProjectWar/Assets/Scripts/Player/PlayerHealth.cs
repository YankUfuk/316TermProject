using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public event Action OnDeath;

    [Header("Health Settings")]
    public int maxHealth = 5;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"{name} took {damage} damage. Current health: {currentHealth}");
        currentHealth -= damage;

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log($"{name} died.");
        OnDeath?.Invoke(); // notify ControlSwitcher
        Destroy(gameObject); // or deactivate instead
    }
}