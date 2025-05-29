using UnityEngine;

public class MeleeAttackState : StateBase
{
    private readonly string _playerTag     = "TroopEnemy"; 
    private readonly string _playerBaseTag = "TroopEnemyBase";

    public MeleeAttackState(Enemy enemy, EnemyStateMachine sm)
        : base(enemy, sm)
    { }

    public override void Enter()
    {
        
    }

    public override void Tick()
    {
        // 1) If the player has moved out of attack range, switch back to chasing:
        if (enemy.DistanceToPlayer > enemy.AttackRange)
        {
            // now passing BOTH the unit-tag and the fallback base-tag:
            sm.ChangeState(new ChaseState(
                enemy,
                sm,
                _playerTag,      
                _playerBaseTag   
            ));
            return;
        }
    }

    public override void Exit()
    {
        // any exit cleanup
    }
}