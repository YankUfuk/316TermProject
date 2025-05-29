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
        string playerTag,
        string coverTag,
        Vector3 coverPosition,
        float attackRange)
    {
        this.enemy       = enemy;
        this.sm          = sm;
        this.playerTag   = playerTag;
        this.coverTag    = coverTag;
        this.coverPos    = coverPosition;
        this.attackRange = attackRange;
    }

    public void Enter()
    {
        player       = GameObject.FindWithTag(playerTag)?.transform;
        shootTimer   = 0f;
        nextFireTime = Time.time;

        // step just out from cover toward the player
        if (player != null)
        {
            Vector3 dir      = (player.position - coverPos).normalized;
            Vector3 shootPos = coverPos + dir * attackRange * 0.9f;
            var agent = enemy.Agent;
            agent.isStopped = false;
            agent.SetDestination(shootPos);
        }
    }

    public void Tick()
    {
        // dead?
        var health = enemy.GetComponent<EnemyHealth>();
        if (health != null && health.currentHealth <= 0) return;

        // cover destroyed? go find new one
        if (!GameObject.FindGameObjectsWithTag(coverTag)
                     .Any(go => go.transform.position == coverPos))
        {
            sm.ChangeState(new RangedCoverState(enemy, sm, playerTag, coverTag));
            return;
        }

        var agent = enemy.Agent;
        if (agent == null || agent.pathPending) return;

        // wait until we reach our shoot spot
        if (agent.remainingDistance > agent.stoppingDistance + 0.1f)
            return;

        // face the player
        if (player != null)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer <= 5f && Time.time >= nextFireTime)
            {
                enemy.FireAt(player.position);
                nextFireTime = Time.time + 1f / enemy.FireRate;
            }
            else if (shootTimer > 5f)
            {
                sm.ChangeState(new RangedCoverState(enemy, sm, playerTag, coverTag));
            }
        }
        else
        {
            // Immediately change state if player is null (enemy defeated or missing)
            sm.ChangeState(new RangedChaseState(enemy, sm, enemy.CompareTag("Troop") ? "TroopEnemyBase" : "TroopBase"));
        }


        // fire at your fire-rate for up to 5 seconds
        shootTimer += Time.deltaTime;
        if (shootTimer <= 5f && Time.time >= nextFireTime)
        {
            enemy.FireAt(player.position);
            nextFireTime = Time.time + 1f / enemy.FireRate;
        }
        else if (shootTimer > 5f)
        {
            sm.ChangeState(new RangedCoverState(enemy, sm, playerTag, coverTag));
        }
    }

    public void Exit() { }
}