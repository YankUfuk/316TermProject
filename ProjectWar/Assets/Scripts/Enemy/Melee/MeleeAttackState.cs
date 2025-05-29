using UnityEngine;

public class MeleeAttackState : StateBase
{
    private readonly string _playerTag     = "TroopEnemy"; 
    private readonly string _playerBaseTag = "TroopEnemyBase";
    private float attackCooldown = 1.0f; // seconds between attacks
    private float lastAttackTime;

    public MeleeAttackState(Enemy enemy, EnemyStateMachine sm)
        : base(enemy, sm)
    { }

    public override void Enter()
    {
        lastAttackTime = -attackCooldown; // so it can hit instantly
    }

    public override void Tick()
    {
        // 1) If the player has moved out of attack range, switch back to chasing:
        if (enemy.DistanceToPlayer > enemy.AttackRange)
        {
            sm.ChangeState(new ChaseState(
                enemy,
                sm,
                _playerTag,
                _playerBaseTag
            ));
            return;
        }

        // 2) Deal damage on cooldown
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;

            // Try to damage enemy troop
            if (enemy.CurrentTarget != null &&
                enemy.CurrentTarget.TryGetComponent<EnemyHealth>(out var enemyHealth))
            {
                enemyHealth.TakeDamage(1); // adjust damage value here
            }
            // Try to damage base
            else if (enemy.CurrentTarget != null &&
                     enemy.CurrentTarget.TryGetComponent<BaseHealth>(out var baseHealth))
            {
                baseHealth.TakeDamage(2); // maybe more damage to bases
            }
        }
    }

    public override void Exit()
    {
        // nothing for now
    }
}