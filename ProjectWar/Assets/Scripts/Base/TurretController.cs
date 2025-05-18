using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurretController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float range = 10f;
    public float fireRate = 1f;

    private float fireTimer = 0f;

    void Update()
    {
        fireTimer += Time.deltaTime;

        GameObject target = FindClosestEnemy();
        if (target != null && fireTimer >= 1f / fireRate)
        {
            Fire(target.transform);
            fireTimer = 0f;
        }
    }

    GameObject FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("EnemyTestUse");

        float closestDist = Mathf.Infinity;
        GameObject closestEnemy = null;

        foreach (var enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closestDist && dist <= range)
            {
                closestDist = dist;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    void Fire(Transform target)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        bullet.GetComponent<BulletTest>().SetTarget(target);
    }
}
