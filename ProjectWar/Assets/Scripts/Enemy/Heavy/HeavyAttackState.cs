using UnityEngine;

public class HeavyAttackState : StateBase
{
    private HeavyEnemy _heavy;
    private string     _unitTag;
    private string     _baseTag;
    private float      _lastAttackTime;

    public HeavyAttackState(Enemy enemy, EnemyStateMachine sm) 
        : base(enemy, sm)
    {
        _heavy = (HeavyEnemy)enemy;

        // derive tags from this GameObject's tag:
        if (_heavy.gameObject.tag == "Troop")
        {
            _unitTag = "TroopEnemy";
            _baseTag = "TroopEnemyBase";
        }
        else if (_heavy.gameObject.tag == "TroopEnemy")
        {
            _unitTag = "Troop";
            _baseTag = "TroopBase";
        }
        else
        {
            Debug.LogWarning($"[HeavyAttackState] unexpected tag '{_heavy.gameObject.tag}' on {_heavy.name}");
            _unitTag = _baseTag = "";
        }

        _lastAttackTime = Time.time;
    }

    public override void Enter()
    {
        // stop moving and play wind-up animation...
        if (_heavy.Agent != null) 
            _heavy.Agent.isStopped = true;
        
        // make sure we have a valid CurrentTarget:
        if (_heavy.CurrentTarget == null)
            sm.ChangeState(new ChaseState(_heavy, sm, _unitTag, _baseTag));
    }

    public override void Tick()
    {
        var target = _heavy.CurrentTarget;

        bool outOfRange = target == null 
            || Vector3.Distance(_heavy.transform.position, target.position) > _heavy.AttackRange;

        if (outOfRange)
        {
            // go chase (units or base)
            sm.ChangeState(new ChaseState(_heavy, sm, _unitTag, _baseTag));
            return;
        }

        // in range → fire shells on cooldown
        if (Time.time - _lastAttackTime >= _heavy.AttackCooldown)
        {
            _heavy.ShootShell(target.position);
            _lastAttackTime = Time.time;
        }
    }

    public override void Exit()
    {
        // clean up any VFX or reset animation flags
    }
}
