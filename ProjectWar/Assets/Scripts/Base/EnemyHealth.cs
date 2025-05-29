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
        Debug.Log($"{name} took {damage} damage. Current health: {currentHealth}");
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            if (TryGetComponent<EnemyStateMachine>(out var sm))
                sm.ChangeState(new DieState(GetComponent<Enemy>(), sm));
            else
                Die(); 
        }

    }

    private void Die()
    {
        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}