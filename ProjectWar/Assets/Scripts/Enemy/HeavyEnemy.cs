// HeavyEnemy.cs
using UnityEngine;
using UnityEngine.AI;

public class HeavyEnemy : Enemy
{
    [Header("Heavy AI Settings")]
    [Tooltip("Tag of other troops to hunt")]
    [SerializeField] private string troopTag       = "Troop";
    [Tooltip("How big the area‐blast radius is")]
    [SerializeField] private float  areaRadius     = 3f;
    [Tooltip("Damage dealt by each blast")]
    [SerializeField] private float  blastDamage    = 30f;
    [Tooltip("Seconds between each blast")]
    [SerializeField] private float  attackCooldown = 2f;
    [Tooltip("Slow turn speed so it moves like a tank")]
    [SerializeField] private float  angularSpeed   = 60f;

    private EnemyStateMachine _sm;
    private float             _lastAttackTime;

    public string TroopTag       => troopTag;
    public float  AreaRadius     => areaRadius;
    public float  BlastDamage    => blastDamage;
    public float  AttackCooldown => attackCooldown;

    protected override void Awake()
    {
        base.Awake();
        // give it a slow‐tank turn
        if (agent != null)
            agent.angularSpeed = angularSpeed;
    }

    private void Start()
    {
        _sm = new EnemyStateMachine();
        // start by chasing the nearest troop
        _sm.Initialize(new ChaseState(this, _sm, troopTag));
    }

    private void Update()
    {
        _sm.Tick();
    }

    /// <summary>
    /// Does the AoE blast around this enemy.
    /// </summary>
    public void DoAreaBlast()
    {
        // optional: play explosion VFX here

        Collider[] hits = Physics.OverlapSphere(transform.position, areaRadius);
        foreach (var col in hits)
        {
            var h = col.GetComponent<EnemyHealthTest>();
            if (h != null)
                h.TakeDamage((int)blastDamage);
        }
    }
}