using UnityEngine;

public class MeleeDamageDealer : MonoBehaviour
{
    [Header("Damage Settings")] public int damageToUnits = 1;
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

        // Make sure only the player takes damage from enemies
        if (CompareTag("TroopEnemy") && collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<PlayerHealth>(out var playerHealth))
            {
                playerHealth.TakeDamage(damageToUnits);
            }


        }

    }
}