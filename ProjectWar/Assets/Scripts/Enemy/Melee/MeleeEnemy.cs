using UnityEngine;

public class MeleeEnemy : Enemy
{
    private Animator animator;
    private EnemyStateMachine _sm;
    private IState _chaseState;

    private void Start()
    {
        StateMachine = new EnemyStateMachine(); 

        string unitTag, baseTag;
        if (CompareTag("Troop"))
        {
            unitTag = "TroopEnemy";
            baseTag = "TroopEnemyBase";
        }
        else if (CompareTag("TroopEnemy"))
        {
            unitTag = "Troop";
            baseTag = "TroopBase";
        }
        else
        {
            Debug.LogWarning($"[MeleeEnemy] unexpected tag '{tag}' on '{name}'. " +
                             "Expected 'troop' or 'troopenemy'.");
            unitTag = baseTag = "";
        }

        var chaseState = new ChaseState(this, StateMachine, unitTag, baseTag);
        StateMachine.Initialize(chaseState);
    }


    private void Update()
    {
        _sm.Tick();
    }
}