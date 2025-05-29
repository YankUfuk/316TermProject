using UnityEngine;

public class MeleeEnemy : Enemy
{
    private EnemyStateMachine _sm;
    private IState            _chaseState;

    private void Start()
    {
        _sm = new EnemyStateMachine();

        // --- derive tags based on this object's own tag ---
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

        // initialize the state machine to hunt units, then fallback to base
        _chaseState = new ChaseState(this, _sm, unitTag, baseTag);
        _sm.Initialize(_chaseState);
    }

    private void Update()
    {
        _sm.Tick();
    }
}