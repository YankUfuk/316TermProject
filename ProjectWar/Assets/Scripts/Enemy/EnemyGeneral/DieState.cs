using UnityEngine;

public class DieState : StateBase
{
    private float deathDelay = 2f; // seconds before destroying
    private float deathTimer;

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

        // Start countdown to destroy
        deathTimer = 0f;
    }

    public override void Tick()
    {
        deathTimer += Time.deltaTime;
        if (deathTimer >= deathDelay)
        {
            GameObject.Destroy(enemy.gameObject);
        }
    }
}