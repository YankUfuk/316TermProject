using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float visionRange = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Transform firePoint;

    private int patrolIndex;

    public Transform GetPatrolPoint(int index) => patrolPoints[index];
    public int PatrolPointsCount => patrolPoints.Length;
    public int PatrolIndex
    {
        get => patrolIndex;
        set => patrolIndex = value;
    }
    public float VisionRange => visionRange;
    public float AttackRange => attackRange;
    public Transform FirePoint => firePoint;

    // Returns the distance from this enemy to the player
    public float DistanceToPlayer
    {
        get
        {
            var player = GameObject.FindWithTag("Player");
            if (player == null) return float.MaxValue;
            return Vector3.Distance(transform.position, player.transform.position);
        }
    }

    // Returns the player's transform, or null if not found
    public Transform PlayerPosition
    {
        get
        {
            var player = GameObject.FindWithTag("Player");
            return player != null ? player.transform : null;
        }
    }

    // Moves the enemy towards the target position
    public void MoveTo(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}