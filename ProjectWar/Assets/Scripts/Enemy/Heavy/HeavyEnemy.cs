using UnityEngine;
using UnityEngine.AI;

public class HeavyEnemy : Enemy
{
    [Header("Heavy AI Settings")]
    [Tooltip("Tag of other troops to hunt")]
    [SerializeField] private string troopTag       = "TroopEnemy";
    [Tooltip("Tag of the enemy base to attack when no units remain")]
    [SerializeField] private string baseTag        = "TroopEnemyBase";
    [Tooltip("How big the area-blast radius is")]
    [SerializeField] private float  areaRadius     = 3f;
    [Tooltip("Damage dealt by each blast")]
    [SerializeField] private float  blastDamage    = 30f;
    [Tooltip("Seconds between each blast")]
    [SerializeField] private float  attackCooldown = 2f;
    [Tooltip("Slow turn speed so it moves like a tank")]
    [SerializeField] private float  angularSpeed   = 60f;

    [Header("Weapon Settings")]
    [Tooltip("Projectile this tank fires")]
    [SerializeField] private GameObject shellPrefab;
    [Tooltip("Initial speed of the shell")]
    [SerializeField] private float shellSpeed = 12f;

    // expose to states
    public string TroopTag       => troopTag;
    public string BaseTag        => baseTag;
    public float  AreaRadius     => areaRadius;
    public float  BlastDamage    => blastDamage;
    public float  AttackCooldown => attackCooldown;
    public GameObject ShellPrefab => shellPrefab;
    public float      ShellSpeed   => shellSpeed;

    private EnemyStateMachine _sm;
    private float             _lastAttackTime;

    protected override void Awake()
    {
        // 1) call your base class so moveSpeed, stoppingDistance, etc. get set
        base.Awake();

        // 2) now tweak the angular speed
        if (agent != null)
            agent.angularSpeed = angularSpeed;
    }

    private void Start()
    {
        _sm = new EnemyStateMachine();

        // pass both the unit-tag AND the fallback base-tag
        _sm.Initialize(new HeavyChaseState(this, _sm, TroopTag, BaseTag));
    }

    private void Update()
    {
        _sm.Tick();
    }

    public void ShootShell(Vector3 targetPos)
    {
        if (shellPrefab == null || FirePoint == null) return;

        Vector3 dir = (targetPos - FirePoint.position).normalized;
        var shell = Instantiate(shellPrefab,
            FirePoint.position,
            Quaternion.LookRotation(dir));

        // 3) Unity Rigidbody uses .velocity, not .linearVelocity
        if (shell.TryGetComponent<Rigidbody>(out var rb))
            rb.linearVelocity = dir * shellSpeed;

        shell.AddComponent<DestroyOnCollision>();
    }

    private class DestroyOnCollision : MonoBehaviour
    {
        void OnCollisionEnter(Collision _) => Destroy(gameObject);
    }
}
