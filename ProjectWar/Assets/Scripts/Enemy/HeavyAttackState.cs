// HeavyAttackState.cs
using UnityEngine;

public class HeavyAttackState : StateBase
{
    private HeavyEnemy _heavy;
    private string     _troopTag;
    private float      _lastAttackTime;

    public HeavyAttackState(Enemy enemy, EnemyStateMachine sm) 
        : base(enemy, sm)
    {
        _heavy    = (HeavyEnemy) enemy;
        _troopTag = _heavy.TroopTag;
        _lastAttackTime = Time.time;
    }

    public override void Enter()
    {
        // play “wind up” or “ready” animation if you have one
        if (_heavy.Agent != null) 
            _heavy.Agent.isStopped = true;
    }

    public override void Tick()
    {
        // 1) If no target or out of attack range, go back to chasing
        var target = _heavy.CurrentTarget;
        if (target == null ||
            Vector3.Distance(_heavy.transform.position, target.position) > _heavy.AttackRange)
        {
            sm.ChangeState(new ChaseState(_heavy, sm, _troopTag));
            return;
        }

        // 2) Time to blast?
        if (Time.time - _lastAttackTime >= _heavy.AttackCooldown)
        {
            _heavy.DoAreaBlast();
            _lastAttackTime = Time.time;
        }
    }

    public override void Exit()
    {
        // cleanup animations or VFX
    }
}