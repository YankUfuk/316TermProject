using UnityEngine;

public class IdleState : StateBase {
    public IdleState(Enemy enemy, EnemyStateMachine sm) : base(enemy, sm) {}
    public override void Enter() { /* play idle anim */ }
    public override void Tick() {
        if (enemy.DistanceToPlayer < enemy.VisionRange)
            sm.ChangeState(new ChaseState(enemy, sm));
        else
            sm.ChangeState(new PatrolState(enemy, sm));
    }
}