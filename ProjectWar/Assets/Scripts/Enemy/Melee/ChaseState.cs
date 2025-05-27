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
            _enemy.MoveTo(_enemy.CurrentTarget.position);
    }

    public void Tick() {
        var tgt = _enemy.CurrentTarget;
        if (tgt == null || IsDead(tgt)) {
            AcquireTarget();
            // if no more targets, just stop moving
            if (_enemy.CurrentTarget == null) {
                _enemy.Agent.isStopped = true;
                return;
            }
        }
        // always refresh destination (in case the target moves)
        _enemy.MoveTo(_enemy.CurrentTarget.position);
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
        var h = t.GetComponent<EnemyHealthTest>();
        return h != null && h.currentHealth <= 0;
    }
}