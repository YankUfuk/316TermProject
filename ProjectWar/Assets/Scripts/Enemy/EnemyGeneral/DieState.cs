using UnityEngine;

public class DieState : StateBase
{
    public DieState(Enemy enemy, EnemyStateMachine sm) : base(enemy, sm) {}

    public override void Enter()
    {
        // Stop movement
        if (enemy.Agent != null)
        {
            enemy.Agent.isStopped = true;
            enemy.Agent.enabled = false;
        }

        // Disable collider
        if (enemy.TryGetComponent<Collider>(out var col))
            col.enabled = false;

        // Destroy the enemy immediately
        Debug.Log($"[DieState] Destroying {enemy.name} immediately");
        GameObject.Destroy(enemy.gameObject);
    }

    public override void Tick() { }

    public override void Exit() { }
}