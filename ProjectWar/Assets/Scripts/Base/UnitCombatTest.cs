using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    public int damage = 10;
    public float attackRate = 1f;
    private float attackTimer;

    void OnTriggerStay(Collider other)
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackRate)
        {
            if (other.CompareTag("EnemyTestUse"))
            {
                var enemyHealth = other.GetComponent<EnemyHealthTest>();
                if (enemyHealth != null)
                    enemyHealth.TakeDamage(damage);
            }
            else if (other.CompareTag("PlayerUnit"))
            {
                var health = other.GetComponent<EnemyHealthTest>();
                if (health != null)
                    health.TakeDamage(damage);
            }
            else if (other.CompareTag("PlayerBase") || other.CompareTag("EnemyBase"))
            {
                BaseHealth baseHealth = other.GetComponent<BaseHealth>();
                if (baseHealth != null)
                {
                    baseHealth.TakeDamage(damage);

                    UnitMovement movement = GetComponent<UnitMovement>();
                    if (movement != null)
                    {
                        movement.StopMoving();
                    }
                }
            }
            else if (other.CompareTag("EnemyBase"))
            {
                Debug.Log("ÇARPIŞTI: EnemyBase --> " + other.name);

                BaseHealth baseHealth = other.GetComponent<BaseHealth>();
                if (baseHealth != null)
                {
                    baseHealth.TakeDamage(damage);

                    UnitMovement movement = GetComponent<UnitMovement>();
                    if (movement != null)
                    {
                        movement.StopMoving();
                    }
                }

                attackTimer = 0f;
            }


            attackTimer = 0f;
        }
    }

}
