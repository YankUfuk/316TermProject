using UnityEngine;
using System.Linq;

public class FindCoverState : StateBase
{
    private readonly string _playerTag;
    private readonly string _coverTag;
    private Transform       _player;

    public FindCoverState(
        Enemy enemy,
        EnemyStateMachine sm,
        string playerTag,
        string coverTag
    ) : base(enemy, sm)
    {
        _playerTag = playerTag;
        _coverTag  = coverTag;
    }

    public override void Enter()
    {
        // refresh player reference
        _player = GameObject.FindWithTag(_playerTag)?.transform;
        AcquireCover();
    }

    public override void Tick()
    {
        if (_player == null) return;

        // if LOS to player exists before reaching cover, skip straight to attack
        if (enemy.HasLineOfSight(enemy.transform.position, _player.position))
        {
            sm.ChangeState(new RangedAttackState(enemy, sm, _player, _coverTag));
            return;
        }

        // keep moving toward cover spot
        if (enemy.CurrentTarget != null)
            enemy.MoveTo(enemy.CurrentTarget.position);

        // once we’ve arrived, switch to attack
        if (enemy.CurrentTarget != null &&
            Vector3.Distance(enemy.transform.position, enemy.CurrentTarget.position) <= 0.5f)
        {
            sm.ChangeState(new RangedAttackState(enemy, sm, _player, _coverTag));
        }
    }

    public override void Exit() { }

    private void AcquireCover()
    {
        // pull the mask off the Enemy instance
        LayerMask mask = enemy.ObstacleMask;

        // find cover spots that block line‐of‐sight from them to the player
        var spots = GameObject
            .FindGameObjectsWithTag(_coverTag)
            .Select(go => go.transform)
            .Where(t => !Physics.Raycast(
                t.position,
                (_player.position - t.position).normalized,
                Vector3.Distance(t.position, _player.position),
                mask))
            .OrderBy(t => Vector3.Distance(enemy.transform.position, t.position));

        var best = spots.FirstOrDefault();
        if (best != null)
            enemy.SetTarget(best);
    }
}
