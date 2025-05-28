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
        var direction = (end - start).normalized;
        float dist = Vector3.Distance(start, end);
        return !Physics.Raycast(start, direction, dist, obstacleMask);
    }

    public Transform CurrentTarget { get; private set; }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.speed            = moveSpeed;
            agent.stoppingDistance = attackRange * 0.9f;

            // Snap the agent onto the NavMesh at start
            NavMeshHit startHit;
            if (NavMesh.SamplePosition(transform.position, out startHit, 5f, NavMesh.AllAreas))
            {
                agent.Warp(startHit.position);
            }
        }
    }

    public void MoveTo(Vector3 worldPos)
    {
        if (agent != null)
        {
            NavMeshHit hit;
            const float maxSampleDistance = 10f;
            if (NavMesh.SamplePosition(worldPos, out hit, maxSampleDistance, NavMesh.AllAreas))
            {
                agent.isStopped = false;
                agent.SetDestination(hit.position);
            }
            else
            {
                agent.isStopped = true;
            }
        }
        else
        {
            Vector3 dir = (worldPos - transform.position).normalized;
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
