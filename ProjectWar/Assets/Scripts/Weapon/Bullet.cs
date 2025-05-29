using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Damage Settings")]
    public int damage = 1;  // Default damage (you can override this in prefabs)

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"[Bullet] Hit: {collision.gameObject.name}");

        if (collision.gameObject.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            Debug.Log("[Bullet] Damaging enemy.");
            enemyHealth.TakeDamage(damage);
        }
        // Deal damage to bases
        else if (collision.gameObject.TryGetComponent<BaseHealth>(out var baseHealth))
        {
            baseHealth.TakeDamage(damage);
        }

        CreateBulletImpactEffect(collision);
        Destroy(gameObject);
    }

    void CreateBulletImpactEffect(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];

        GameObject hole = Instantiate(
            References.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
        );

        hole.transform.SetParent(collision.transform);
    }
}