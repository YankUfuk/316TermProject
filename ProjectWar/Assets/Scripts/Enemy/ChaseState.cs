using UnityEngine;

public class ChaseState : StateBase {
    public ChaseState(Enemy enemy, EnemyStateMachine sm) : base(enemy, sm) {}

    public override void Tick() {
        enemy.MoveTo(enemy.PlayerPosition.position);
        if (enemy.DistanceToPlayer < enemy.AttackRange)
            sm.ChangeState(enemy is MeleeEnemy
                ? (IState)new MeleeAttackState(enemy, sm)
                : enemy is RangedEnemy
                    ? new RangedAttackState(enemy, sm)
                    : new HeavyAttackState(enemy, sm));
        else if (enemy.DistanceToPlayer > enemy.VisionRange)
            sm.ChangeState(new PatrolState(enemy, sm));
    }
}