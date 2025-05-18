using UnityEngine;

public class HeavyAttackState : StateBase {
    public HeavyAttackState(Enemy enemy, EnemyStateMachine sm) : base(enemy, sm) {}
    public override void Enter() {
        /* maybe charge up, play heavy swing anim */
    }
    public override void Tick() {
        if (enemy.DistanceToPlayer > enemy.AttackRange)
            sm.ChangeState(new ChaseState(enemy, sm));
    }
}
