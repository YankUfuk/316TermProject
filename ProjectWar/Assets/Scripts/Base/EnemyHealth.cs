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
        Debug.Log($"{name} took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log($"{name} died!");
    
            Enemy enemyComponent = GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.StateMachine.ChangeState(new DieState(enemyComponent, enemyComponent.StateMachine));
            }
            else
            {
                Die(); 
            }

            
        }
    }


    private void Die()
    {
        OnDeath?.Invoke();

        Destroy(gameObject);
    }
}