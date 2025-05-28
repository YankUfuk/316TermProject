using UnityEngine;
using UnityEngine.AI;

public class HeavyEnemy : Enemy
{
    [Header("Heavy AI Settings")]
    [Tooltip("Tag of other troops to hunt")]
    [SerializeField] private string troopTag       = "TroopEnemy";
    [Tooltip("How big the area‐blast radius is")]
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

    public GameObject ShellPrefab => shellPrefab;
    public float      ShellSpeed   => shellSpeed;

    private EnemyStateMachine _sm;
    private float             _lastAttackTime;

    public string TroopTag       => troopTag;
    public float  AreaRadius     => areaRadius;
    public float  BlastDamage    => blastDamage;
    public float  AttackCooldown => attackCooldown;

    protected override void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            NavMeshHit startHit;
            if (NavMesh.SamplePosition(transform.position, out startHit, 5f, NavMesh.AllAreas))
            {
                agent.Warp(startHit.position);
            }
        }
    }

    private void Start()
    {
        _sm = new EnemyStateMachine();
        _sm.Initialize(new HeavyChaseState(this, _sm, TroopTag));
    }



    private void Update()
    {
        _sm.Tick();
    }
    
    public void DoAreaBlast()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, areaRadius);
        foreach (var col in hits)
        {
            if (col.gameObject == gameObject) 
                continue;
            
            var h = col.GetComponent<EnemyHealthTest>();
            if (h != null)
            {
                Debug.Log($"[{name}] blasting {col.name}");
                h.TakeDamage((int)blastDamage);
            }
        }
    }
    
    public void ShootShell(Vector3 targetPos)
    {
        if (shellPrefab == null || FirePoint == null) return;

        Vector3 dir = (targetPos - FirePoint.position).normalized;
        var shell = Instantiate(shellPrefab,
            FirePoint.position,
            Quaternion.LookRotation(dir));
        if (shell.TryGetComponent<Rigidbody>(out var rb))
            rb.linearVelocity = dir * shellSpeed;
    }
    private class DestroyOnCollision : MonoBehaviour
    {
        void OnCollisionEnter(Collision _) => Destroy(gameObject);
    }
}