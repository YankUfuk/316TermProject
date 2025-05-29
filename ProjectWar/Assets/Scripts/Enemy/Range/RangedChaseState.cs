using UnityEngine;
using UnityEngine.AI;

public class RangedChaseState : IState
{
    private readonly RangedEnemy enemy;
    private readonly EnemyStateMachine sm;
    private readonly string targetBaseTag;

    public RangedChaseState(RangedEnemy enemy, EnemyStateMachine sm, string targetBaseTag)
    {
        this.enemy = enemy;
        this.sm = sm;
        this.targetBaseTag = targetBaseTag;
    }

    public void Enter()
    {
        var targetBase = GameObject.FindWithTag(targetBaseTag)?.transform;
        if (targetBase == null) return;

        var agent = enemy.Agent;
        agent.isStopped = false;
        agent.SetDestination(targetBase.position);
    }

    public void Tick()
    {
        var agent = enemy.Agent;
        if (agent == null || agent.pathPending) return;

        if (agent.remainingDistance <= enemy.AttackRange)
        {
            enemy.transform.LookAt(agent.destination);
            enemy.FireAt(agent.destination);
        }
    }

    public void Exit()
    {
        if (enemy.Agent != null)
            enemy.Agent.isStopped = true;
    }
}