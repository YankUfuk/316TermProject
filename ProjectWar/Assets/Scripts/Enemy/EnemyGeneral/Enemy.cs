using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float visionRange = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed   = 2f;
    [SerializeField] private Transform firePoint;

    // optional obstacle mask for LOS checks
    [Header("General Settings")]
    [Tooltip("Layers that block line of sight")]
    [SerializeField] private LayerMask obstacleMask;

    // your NavMeshAgent
    protected NavMeshAgent agent;
    public NavMeshAgent Agent => agent;

    public float VisionRange    => visionRange;
    public float AttackRange    => attackRange;
    public Transform FirePoint  => firePoint;
    public LayerMask ObstacleMask => obstacleMask;

    /// <summary>
    /// Distance from this enemy to the player (or float.MaxValue if not found)
    /// </summary>
    public float DistanceToPlayer
    {
        get
        {
            var player = GameObject.FindWithTag("Player");
            if (player == null) return float.MaxValue;
            return Vector3.Distance(transform.position, player.transform.position);
        }
    }

    /// <summary>
    /// Shortcut to the player's transform (or null if not found)
    /// </summary>
    public Transform PlayerPosition
    {
        get
        {
            var player = GameObject.FindWithTag("Player");
            return player != null ? player.transform : null;
        }
    }

    /// <summary>
    /// Raycasts from start to end, returns true if no obstacle blocks.
    /// </summary>
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
        }
    }

    public void MoveTo(Vector3 worldPos)
    {
        if (agent != null)
            agent.SetDestination(worldPos);
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
