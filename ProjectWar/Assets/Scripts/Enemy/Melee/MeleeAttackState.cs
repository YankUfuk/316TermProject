using UnityEngine;

public class MeleeAttackState : StateBase
{
    private readonly string _playerTag = "Player";

    public MeleeAttackState(Enemy enemy, EnemyStateMachine sm)
        : base(enemy, sm)
    { }

    public override void Enter()
    {
        //enemy.Animator.SetTrigger("Attack");
    }

    public override void Tick()
    {
        // 1) If the player has moved out of attack range, switch back to chasing them:
        if (enemy.DistanceToPlayer > enemy.AttackRange)
        {
            sm.ChangeState(new ChaseState(enemy, sm, _playerTag));
            return;
        }

       
    }

    public override void Exit()
    {
        
    }
}