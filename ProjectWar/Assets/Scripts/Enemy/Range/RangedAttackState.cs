using UnityEngine;

public class RangedAttackState : StateBase
{
    private readonly Transform _player;
    private readonly string    _coverTag;
    private float              _lastFireTime;

    public RangedAttackState(
        Enemy enemy,
        EnemyStateMachine sm,
        Transform player,
        string coverTag
    ) : base(enemy, sm)
    {
        _player      = player;
        _coverTag    = coverTag;
        _lastFireTime = Time.time;
    }

    public override void Enter()
    {
        // stop in cover, ready to fire
        enemy.Agent.isStopped = true;
    }

    public override void Tick()
    {
        if (_player == null) return;

        // 1) If LOS breaks, search for new cover
        if (!enemy.HasLineOfSight(enemy.FirePoint.position, _player.position))
        {
            sm.ChangeState(new FindCoverState(
                enemy,
                sm,
                _player.tag,
                _coverTag
            ));
            return;
        }

        // 2) Face the player
        var lookDir = (_player.position - enemy.transform.position).normalized;
        enemy.transform.rotation = Quaternion.LookRotation(lookDir);

        // 3) Fire at fixed rate
        var re = (RangedEnemy)enemy;
        if (Time.time - _lastFireTime >= 1f / re.FireRate)
        {
            re.FireAt(_player.position);
            _lastFireTime = Time.time;
        }
    }

    public override void Exit()
    {
        // cleanup (lower weapon, reset anim)…
    }
}