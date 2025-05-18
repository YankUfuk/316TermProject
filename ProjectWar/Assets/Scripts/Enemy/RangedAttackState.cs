using UnityEngine;

public class RangedAttackState : StateBase {
    public RangedAttackState(Enemy enemy, EnemyStateMachine sm) : base(enemy, sm) {}
    public override void Enter() {
        /* spawn projectile prefab at enemy.FirePoint */
    }
    public override void Tick() {
        if (enemy.DistanceToPlayer > enemy.AttackRange)
            sm.ChangeState(new ChaseState(enemy, sm));
    }
}