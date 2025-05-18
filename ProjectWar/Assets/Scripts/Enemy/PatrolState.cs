using UnityEngine;

public class PatrolState : StateBase {
    public PatrolState(Enemy enemy, EnemyStateMachine sm) : base(enemy, sm) {}
    public override void Enter() { /* set anim, choose first point */ }
    public override void Tick() {
        var target = enemy.GetPatrolPoint(enemy.PatrolIndex).position;
        enemy.MoveTo(target);
        if (Vector3.Distance(enemy.transform.position, target) < 0.2f)
            enemy.PatrolIndex = (enemy.PatrolIndex + 1) % enemy.PatrolPointsCount;

        if (enemy.DistanceToPlayer < enemy.VisionRange)
            sm.ChangeState(new ChaseState(enemy, sm));
    }
}