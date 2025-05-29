using System.Linq;
using UnityEngine;

public class RangedShootOutState : IState
{
    private readonly RangedEnemy enemy;
    private readonly EnemyStateMachine sm;
    private readonly string playerTag, coverTag;
    private readonly Vector3 coverPos;
    private readonly float attackRange;
    private Transform player;
    private float shootTimer;
    private float nextFireTime;

    public RangedShootOutState(
        RangedEnemy enemy,
        EnemyStateMachine sm,
        string coverTag,
        Vector3 coverPosition,
        float attackRange)
    {
        this.enemy       = enemy;
        this.sm          = sm;
        this.coverTag    = coverTag;
        this.coverPos    = coverPosition;
        this.attackRange = attackRange;

        // resolve enemy tag based on self
        if (enemy.CompareTag("Troop"))
            playerTag = "TroopEnemy";
        else if (enemy.CompareTag("TroopEnemy"))
            playerTag = "Troop";
        else
            playerTag = "";
    }

    public void Enter()
    {
        player = GameObject.FindGameObjectsWithTag(playerTag)
            .Select(go => go.transform)
            .Where(t => Vector3.Distance(enemy.transform.position, t.position) <= attackRange)
            .Where(t => enemy.HasLineOfSight(enemy.FirePoint.position, t.position))
            .OrderBy(t => Vector3.Distance(enemy.transform.position, t.position))
            .FirstOrDefault();

        shootTimer = 0f;
        nextFireTime = Time.time;

        if (player != null)
        {
            Vector3 dir = (player.position - coverPos).normalized;
            Vector3 shootPos = coverPos + dir * attackRange * 0.9f;
            var agent = enemy.Agent;
            agent.isStopped = false;
            agent.SetDestination(shootPos);
        }
    }

    public void Tick()
    {
        var health = enemy.GetComponent<EnemyHealth>();
        if (health != null && health.currentHealth <= 0) return;

        // Check if cover is still valid
        if (!GameObject.FindGameObjectsWithTag(coverTag)
                     .Any(go => go.transform.position == coverPos))
        {
            sm.ChangeState(new RangedCoverState(enemy, sm, coverTag));
            return;
        }

        var agent = enemy.Agent;
        if (agent == null || agent.pathPending || agent.remainingDistance > agent.stoppingDistance + 0.1f)
            return;

        // Re-acquire player if needed
        if (player == null)
        {
            sm.ChangeState(new RangedChaseState(enemy, sm, enemy.CompareTag("Troop") ? "TroopEnemyBase" : "TroopBase"));
            return;
        }

        // Rotate and shoot
        enemy.transform.LookAt(player.position);
        shootTimer += Time.deltaTime;

        if (shootTimer <= 5f && Time.time >= nextFireTime)
        {
            enemy.FireAt(player.position);
            nextFireTime = Time.time + 1f / enemy.FireRate;
        }
        else if (shootTimer > 5f)
        {
            sm.ChangeState(new RangedCoverState(enemy, sm, coverTag));
        }
    }

    public void Exit() { }
}
