using UnityEngine;
using System.Linq;

public class HeavyChaseState : IState
{
    private readonly HeavyEnemy      heavy;
    private readonly EnemyStateMachine sm;
    private readonly string          targetTag;

    public HeavyChaseState(HeavyEnemy heavy, EnemyStateMachine sm, string targetTag)
    {
        this.heavy     = heavy;
        this.sm        = sm;
        this.targetTag = targetTag;
    }

    public void Enter()
    {
        Debug.Log($"[{heavy.name}] HeavyChaseState.Enter()");
        AcquireTarget();
    }

    public void Tick()
    {
        var tgt = heavy.CurrentTarget;
        if (tgt == null)
        {
            AcquireTarget();
            tgt = heavy.CurrentTarget;
            if (tgt == null) return;
        }

        float dist = Vector3.Distance(heavy.transform.position, tgt.position);

        // if within firing distance, switch to attack
        if (dist <= heavy.AttackRange)
        {
            sm.ChangeState(new HeavyAttackState(heavy, sm));
            return;
        }

        // otherwise keep chasing
        heavy.MoveTo(tgt.position);
    }

    public void Exit() { }

    private void AcquireTarget()
    {
        // pick nearest alive other TroopEnemy
        var best = GameObject
            .FindGameObjectsWithTag(targetTag)
            .Select(g => g.transform)
            .Where(t => t != heavy.transform)
            .OrderBy(t => Vector3.Distance(heavy.transform.position, t.position))
            .FirstOrDefault();

        heavy.SetTarget(best);
        Debug.Log($"[{heavy.name}] acquired target: {best?.name}");
    }
}