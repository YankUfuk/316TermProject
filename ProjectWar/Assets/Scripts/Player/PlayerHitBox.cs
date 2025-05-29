using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    public PlayerHealth playerHealth;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TroopEnemy"))
        {
            Debug.Log($"{name} triggered by enemy {other.name}");
            playerHealth.TakeDamage(1);
        }
    }
}