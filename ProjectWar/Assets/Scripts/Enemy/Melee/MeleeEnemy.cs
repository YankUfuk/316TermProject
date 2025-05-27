using UnityEngine;

public class MeleeEnemy : Enemy
{
    [Header("AI")]
    [Tooltip("Tag given to all other troop units this AI should hunt")]
    [SerializeField] private string troopTag = "Troop";

    private EnemyStateMachine _sm;
    private IState            _chaseState;

    private void Start() {
        _sm         = new EnemyStateMachine();
        _chaseState = new ChaseState(this, _sm, troopTag);
        _sm.Initialize(_chaseState);
    }

    private void Update() {
        _sm.Tick();
    }
}