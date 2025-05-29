using UnityEngine;
using UnityEngine.AI;
using System.Linq;

// COVER STATE: move behind the nearest blocking cover, then switch to shoot-out
public class RangedCoverState : IState
{
    private readonly RangedEnemy enemy;
    private readonly EnemyStateMachine sm;
    private readonly string playerTag, coverTag;
    private Transform coverSpot;
    private Vector3 hidePosition;

    public RangedCoverState(RangedEnemy enemy, EnemyStateMachine sm, string playerTag, string coverTag)
    {
        this.enemy     = enemy;
        this.sm        = sm;
        this.playerTag = playerTag;
        this.coverTag  = coverTag;
    }

    public void Enter()
    {
        var allCovers = GameObject.FindGameObjectsWithTag(coverTag).Select(go => go.transform);
        var player    = GameObject.FindWithTag(playerTag)?.transform;

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
                enemy, sm, playerTag, coverTag, coverSpot.position, enemy.AttackRange));
        }
    }

    public void Exit() { }
}