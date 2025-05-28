using UnityEngine;

public class RangedAttackState : StateBase
{
    private readonly Transform _player;
    private readonly string    _coverTag;
    private readonly Transform _coverSpot; 
    private float              _lastFireTime;

    public RangedAttackState(
        Enemy enemy,
        EnemyStateMachine sm,
        Transform player,
        string coverTag,
        Transform coverSpot 
    ) : base(enemy, sm)
    {
        _player      = player;
        _coverTag    = coverTag;
        _coverSpot   = coverSpot; 
        _lastFireTime = Time.time;
    }

    public override void Enter()
    {
        enemy.Agent.isStopped = true;
    }

    public override void Tick()
    {
        if (_player == null) return;

        if (!enemy.HasLineOfSight(enemy.FirePoint.position, _player.position))
        {
            sm.ChangeState(new RangedShootOutState(
                (RangedEnemy)enemy,
                sm,
                _player.tag,
                _coverTag,              // pass along the same tag string you used in your CoverState
                _coverSpot.position,
                enemy.AttackRange
            ));

            return;
        }

        var lookDir = (_player.position - enemy.transform.position).normalized;
        enemy.transform.rotation = Quaternion.LookRotation(lookDir);

        var re = (RangedEnemy)enemy;
        if (Time.time - _lastFireTime >= 1f / re.FireRate)
        {
            re.FireAt(_player.position);
            _lastFireTime = Time.time;
        }
    }

    public override void Exit()
    {
        // cleanup
    }
}