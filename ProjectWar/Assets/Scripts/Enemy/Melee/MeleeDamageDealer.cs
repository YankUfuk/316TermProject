using UnityEngine;

public class MeleeDamageDealer : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damageToUnits = 1;
    public int damageToBases = 3;
    public float damageCooldown = 1f;

    private float lastDamageTime;

    private string enemyTag;
    private string enemyBaseTag;

    private void Start()
    {
        if (CompareTag("Troop"))
        {
            enemyTag = "TroopEnemy";
            enemyBaseTag = "TroopEnemyBase";
        }
        else if (CompareTag("TroopEnemy"))
        {
            enemyTag = "Troop";
            enemyBaseTag = "TroopBase";
        }
        else
        {
            Debug.LogWarning($"{name} has unexpected tag '{tag}'");
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (Time.time - lastDamageTime < damageCooldown)
            return;

        lastDamageTime = Time.time;

        // Damage enemy unit
        if (collision.gameObject.CompareTag(enemyTag) &&
            collision.gameObject.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            Debug.Log($"{name} collided with {collision.gameObject.name} (tag: {collision.gameObject.tag})");

            enemyHealth.TakeDamage(damageToUnits);
        }

        // Damage enemy base
        if (collision.gameObject.CompareTag(enemyBaseTag) &&
            collision.gameObject.TryGetComponent<BaseHealth>(out var baseHealth))
        {
            baseHealth.TakeDamage(damageToBases);
        }
    }
}