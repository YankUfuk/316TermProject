// TechnicianAI.cs
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class TechnicianAI : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Tag of your Base object")]
    [SerializeField] private string baseTag = "Base";
    [Tooltip("How much health to add when you arrive")]
    [SerializeField] private float  repairAmount = 25f;
    [Tooltip("Optional: destroy technician after repairing")]
    [SerializeField] private bool   destroyOnComplete = true;

    private NavMeshAgent agent;
    private Transform    baseTransform;
    private BaseHealth         baseComponent;    

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // find the Base by tag
        var go = GameObject.FindWithTag(baseTag);
        if (go == null)
        {
            enabled = false;
            return;
        }

        baseTransform = go.transform;
        baseComponent = go.GetComponent<BaseHealth>();
        if (baseComponent == null)

        agent.SetDestination(baseTransform.position);

        StartCoroutine(DriveAndRepair());
    }

    private IEnumerator DriveAndRepair()
    {
        while (agent.pathPending) 
            yield return null;

        while (agent.remainingDistance > agent.stoppingDistance)
            yield return null;

        if (baseComponent != null)
            baseComponent.IncreaseHealth(repairAmount);

        if (destroyOnComplete)
            Destroy(gameObject);
    }
}