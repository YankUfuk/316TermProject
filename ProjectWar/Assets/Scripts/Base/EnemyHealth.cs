using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public event Action OnDeath;

    [Header("Health Settings")]
    public int maxHealth = 3;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}