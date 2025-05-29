using UnityEngine;

public class RangedEnemy : Enemy
{
    [Header("AI Tags & Layers")]
    [Tooltip("Tag of cover spot objects")]
    [SerializeField] private string coverTag = "Cover";

    [Header("Weapon Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 15f;
    [SerializeField] private float fireRate = 1f;

    private EnemyStateMachine _sm;

    public float FireRate => fireRate;

    private void Start()
    {
        _sm = new EnemyStateMachine();

        string unitTag, baseTag;
        if (CompareTag("Troop"))
        {
            unitTag = "TroopEnemy";
            baseTag = "TroopEnemyBase";
        }
        else if (CompareTag("TroopEnemy"))
        {
            unitTag = "Troop";
            baseTag = "TroopBase";
        }
        else
        {
            Debug.LogWarning($"[RangedEnemy] Unexpected tag '{tag}' on '{name}'. Expected 'Troop' or 'TroopEnemy'.");
            unitTag = baseTag = "";
        }

        _sm.Initialize(
            new RangedCoverState(
                this,
                _sm,
                coverTag
            )
        );

    }

    private void Update()
    {
        _sm.Tick();
    }

    public void FireAt(Vector3 targetPos)
    {
        if (projectilePrefab == null || FirePoint == null) return;

        Vector3 dir = (targetPos - FirePoint.position).normalized;
        GameObject proj = Instantiate(projectilePrefab, FirePoint.position, Quaternion.LookRotation(dir));
        if (proj.TryGetComponent<Rigidbody>(out var rb))
            rb.linearVelocity = dir * projectileSpeed;
    }
}