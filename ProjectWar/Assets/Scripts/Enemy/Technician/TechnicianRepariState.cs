using UnityEngine;
using UnityEngine.AI;

public class TechnicianRepairState : IState
{
    private readonly TechnicianAI technician;
    private readonly EnemyStateMachine sm;
    private readonly string baseTag;
    private readonly float repairAmount;
    private readonly bool destroyOnComplete;

    private Transform baseTransform;
    private BaseHealth baseComponent;

    public TechnicianRepairState(
        TechnicianAI technician,
        EnemyStateMachine sm,
        string baseTag,
        float repairAmount,
        bool destroyOnComplete)
    {
        this.technician = technician;
        this.sm = sm;
        this.baseTag = baseTag;
        this.repairAmount = repairAmount;
        this.destroyOnComplete = destroyOnComplete;
    }

    public void Enter()
    {
        var go = GameObject.FindWithTag(baseTag);
        if (go == null)
        {
            technician.enabled = false;
            return;
        }

        baseTransform = go.transform;
        baseComponent = go.GetComponent<BaseHealth>();

        if (technician.Agent != null)
        {
            technician.Agent.isStopped = false;
            technician.Agent.SetDestination(baseTransform.position);
        }
    }

    public void Tick()
    {
        var agent = technician.Agent;
        if (agent == null || agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance + 0.1f)
        {
            if (baseComponent != null)
                baseComponent.IncreaseHealth(repairAmount);

            if (destroyOnComplete)
                GameObject.Destroy(technician.gameObject);
        }
    }

    public void Exit() { }
}