using System.Linq;
using UnityEngine;

public class ChaseState : IState
{
    private readonly EnemyStateMachine _sm;
    private readonly Enemy            _enemy;
    private readonly string           _targetTag;

    public ChaseState(Enemy enemy, EnemyStateMachine sm, string targetTag) {
        _enemy    = enemy;
        _sm       = sm;
        _targetTag = targetTag;
    }

    public void Enter() {
        AcquireTarget();
        if (_enemy.CurrentTarget != null) 
        {
            _enemy.Agent.isStopped = false;  // make extra sure it’s running
            _enemy.MoveTo(_enemy.CurrentTarget.position);
        } 
        else 
        {
            Debug.Log($"[{_enemy.name}] no targets found");
            _enemy.Agent.isStopped = true;
        }
    }

    public void Tick()
    {
        if (_enemy.CurrentTarget == null) { /* reacquire… */ }

        _enemy.MoveTo(_enemy.CurrentTarget.position);

        // **see what the agent thinks it’s doing**
        var a = _enemy.Agent;
        Debug.Log($"[{_enemy.name}] Tick: onNavMesh={a.isOnNavMesh} " +
                  $"hasPath={a.hasPath} pathPending={a.pathPending} " +
                  $"remainingDist={a.remainingDistance}");
    }

    public void Exit() {
        if (_enemy.Agent != null)
            _enemy.Agent.isStopped = true;
    }

    private void AcquireTarget() {
        // find all candidates
        var candidates = GameObject
            .FindGameObjectsWithTag(_targetTag)
            .Select(go => go.transform)
            .Where(t => t != _enemy.transform && !IsDead(t));

        // pick closest
        var next = candidates
            .OrderBy(t => Vector3.Distance(_enemy.transform.position, t.position))
            .FirstOrDefault();

        _enemy.SetTarget(next);
    }

    private bool IsDead(Transform t) {
        var h = t.GetComponent<EnemyHealth>();
        return h != null && h.currentHealth <= 0;
    }
}