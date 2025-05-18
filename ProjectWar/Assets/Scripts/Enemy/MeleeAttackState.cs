using UnityEngine;

public class MeleeAttackState : StateBase {
    public MeleeAttackState(Enemy enemy, EnemyStateMachine sm) : base(enemy, sm) {}
    public override void Enter() { /* trigger melee anim */ }
    public override void Tick() {
        // when anim event fires, deal damage if still in range
        if (enemy.DistanceToPlayer > enemy.AttackRange) 
            sm.ChangeState(new ChaseState(enemy, sm));
    }
}