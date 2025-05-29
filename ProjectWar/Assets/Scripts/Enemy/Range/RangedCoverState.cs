using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class RangedCoverState : IState
{
    private readonly RangedEnemy enemy;
    private readonly EnemyStateMachine sm;
    private readonly string playerTag, coverTag;
    private Transform coverSpot;
    private Vector3 hidePosition;

    public RangedCoverState(RangedEnemy enemy, EnemyStateMachine sm, string coverTag)
    {
        this.enemy    = enemy;
        this.sm       = sm;
        this.coverTag = coverTag;

        if (enemy.CompareTag("Troop"))
            this.playerTag = "TroopEnemy";
        else if (enemy.CompareTag("TroopEnemy"))
            this.playerTag = "Troop";
        else
        {
            Debug.LogWarning($"[RangedCoverState] Unexpected tag: {enemy.tag}");
            this.playerTag = "";
        }
    }

    public void Enter()
    {
        var allCovers = GameObject.FindGameObjectsWithTag(coverTag).Select(go => go.transform);

        var potentialTargets = GameObject.FindGameObjectsWithTag(playerTag)
            .Select(go => go.transform)
            .Where(t => Vector3.Distance(enemy.transform.position, t.position) <= enemy.AttackRange)
            .Where(t => enemy.HasLineOfSight(enemy.FirePoint.position, t.position))
            .ToList();

        Transform player = potentialTargets
            .OrderBy(t => Vector3.Distance(enemy.transform.position, t.position))
            .FirstOrDefault();

        // pick only those that block LOS, else fall back to nearest
        var candidates = player != null
            ? allCovers.Where(t => !enemy.HasLineOfSight(t.position, player.position))
            : allCovers;
        coverSpot = candidates
            .DefaultIfEmpty(allCovers.FirstOrDefault())
            .OrderBy(t => Vector3.Distance(enemy.transform.position, t.position))
            .FirstOrDefault();

        if (coverSpot == null) return;

        // back off a bit from the cover so it actually hides you
        Vector3 dirToPlayer = player != null
            ? (player.position - coverSpot.position).normalized
            : Vector3.forward;
        Vector3 rawHide = coverSpot.position - dirToPlayer * 1.0f;

        // snap to navmesh
        NavMeshHit hit;
        hidePosition = NavMesh.SamplePosition(rawHide, out hit, 2f, NavMesh.AllAreas)
                     ? hit.position
                     : coverSpot.position;

        var agent = enemy.Agent;
        agent.isStopped = false;
        agent.SetDestination(hidePosition);
    }

    public void Tick()
    {
        var agent = enemy.Agent;
        if (agent == null || agent.pathPending) return;

        // only transition the *moment* we've truly arrived
        if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            sm.ChangeState(new RangedShootOutState(
                enemy, sm, coverTag, coverSpot.position, enemy.AttackRange));



        }
    }

    public void Exit() { }
}