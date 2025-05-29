using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float visionRange = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed   = 2f;
    [SerializeField] private Transform firePoint;

    [Header("General Settings")]
    [Tooltip("Layers that block line of sight")]
    [SerializeField] private LayerMask obstacleMask;
    public EnemyStateMachine StateMachine { get; protected set; }

    private string targetTag;
    public Transform CurrentTarget { get; private set; }

    protected NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    public float VisionRange    => visionRange;
    public float AttackRange    => attackRange;
    public Transform FirePoint  => firePoint;
    public LayerMask ObstacleMask => obstacleMask;

    public float DistanceToPlayer
    {
        get
        {
            var player = GameObject.FindWithTag("Player");
            if (player == null) return float.MaxValue;
            return Vector3.Distance(transform.position, player.transform.position);
        }
    }

    public Transform PlayerPosition
    {
        get
        {
            var player = GameObject.FindWithTag("Player");
            return player != null ? player.transform : null;
        }
    }

    public bool HasLineOfSight(Vector3 start, Vector3 end)
    {
        var dir = (end - start).normalized;
        float dist = Vector3.Distance(start, end);
        return !Physics.Raycast(start, dir, dist, obstacleMask);
    }


    protected virtual void Awake()
    {
        StateMachine = new EnemyStateMachine();

        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed            = moveSpeed;
            agent.stoppingDistance = attackRange * 0.9f;
            NavMeshHit startHit;
            if (NavMesh.SamplePosition(transform.position, out startHit, 5f, NavMesh.AllAreas))
                agent.Warp(startHit.position);
        }

        // Decide which tag we should chase:
        if (gameObject.tag == "Troop")
            targetTag = "TroopEnemy";
        else if (gameObject.tag == "TroopEnemy")
            targetTag = "Troop";
        else
            Debug.LogWarning($"Enemy '{name}' has unexpected tag '{gameObject.tag}'");
    }
    private void Update()
    {
        AcquireTarget();

        if (CurrentTarget != null)
        {
            float dist = Vector3.Distance(transform.position, CurrentTarget.position);

            if (dist > attackRange)
            {
                // Chase
                MoveTo(CurrentTarget.position);
            }
            else
            {
                // In range → stop and attack
                if (agent != null) agent.isStopped = true;
                // ▶ your attack logic here, e.g. FireWeapon();
            }
        }
    }
    private void AcquireTarget()
    {
        // Grab all of the opposite-tagged units
        var candidates = GameObject.FindGameObjectsWithTag(targetTag);
        Transform best = null;
        float bestDist = float.MaxValue;

        foreach (var go in candidates)
        {
            float d = Vector3.Distance(transform.position, go.transform.position);
            if (d <= visionRange && d < bestDist)
            {
                // optional: check line of sight
                if (HasLineOfSight(firePoint.position, go.transform.position))
                {
                    bestDist = d;
                    best     = go.transform;
                }
            }
        }

        // Set or clear the target
        SetTarget(best);
    }

    public void MoveTo(Vector3 worldPos)
    {
        if (agent != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(worldPos, out hit, 10f, NavMesh.AllAreas))
            {
                agent.isStopped = false;
                agent.SetDestination(hit.position);
            }
            else agent.isStopped = true;
        }
        else
        {
            var dir = (worldPos - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    public void SetTarget(Transform t)
    {
        CurrentTarget = t;
        if (agent != null && t != null)
            agent.SetDestination(t.position);
    }
    
}
