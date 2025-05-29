using UnityEngine;

public class MeleeContactDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;
    public string enemyTag = "TroopEnemy"; 
    public float damageCooldown = 1f;

    private float lastDamageTime;

    private void OnCollisionStay(Collision collision)
    {
        if (Time.time - lastDamageTime < damageCooldown) return;

        if (collision.gameObject.CompareTag(enemyTag) &&
            collision.gameObject.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }
}