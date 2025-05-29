using System.Linq;
using UnityEngine;

public class HeavyChaseState : IState
{
    private readonly HeavyEnemy      heavy;
    private readonly EnemyStateMachine sm;
    private readonly string          unitTag;   
    private readonly string          baseTag;   

    public HeavyChaseState(HeavyEnemy heavy, EnemyStateMachine sm, string unitTag, string baseTag)
    {
        this.heavy   = heavy;
        this.sm      = sm;
        this.unitTag = unitTag;
        this.baseTag = baseTag;
    }

    public void Enter()
    {
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

        // switch to attack if in range
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
        // 1) hunt nearest alive unit of unitTag
        var unitCandidates = GameObject
            .FindGameObjectsWithTag(unitTag)
            .Select(g => g.transform)
            .Where(t => t != heavy.transform)
            .Where(t => !IsDead(t));

        Transform next = unitCandidates
            .OrderBy(t => Vector3.Distance(heavy.transform.position, t.position))
            .FirstOrDefault();

        // 2) fallback to base if no units remain
        if (next == null)
        {
            var baseGO = GameObject.FindGameObjectWithTag(baseTag);
            if (baseGO != null)
                next = baseGO.transform;
        }

        heavy.SetTarget(next);
    }

    private bool IsDead(Transform t)
    {
        var h = t.GetComponent<EnemyHealth>();
        return h != null && h.currentHealth <= 0;
    }
}
