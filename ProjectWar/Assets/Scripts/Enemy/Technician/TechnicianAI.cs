using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TechnicianAI : Enemy  // Inherit Enemy so it can use Agent, Tag logic, etc.
{
    [Header("Repair Settings")]
    [Tooltip("How much health to add when you arrive")]
    [SerializeField] private float repairAmount = 25f;

    [Tooltip("Optional: destroy technician after repairing")]
    [SerializeField] private bool destroyOnComplete = true;

    private EnemyStateMachine _sm;

    private void Start()
    {
        string baseTag;

        if (CompareTag("Troop"))
            baseTag = "TroopBase";
        else if (CompareTag("TroopEnemy"))
            baseTag = "TroopEnemyBase";
        else
        {
            Debug.LogWarning($"[TechnicianAI] Unexpected tag '{tag}' on '{name}'. Expected 'Troop' or 'TroopEnemy'.");
            return;
        }

        _sm = new EnemyStateMachine();
        _sm.Initialize(new TechnicianRepairState(this, _sm, baseTag, repairAmount, destroyOnComplete));
    }

    private void Update()
    {
        _sm.Tick();
    }
}