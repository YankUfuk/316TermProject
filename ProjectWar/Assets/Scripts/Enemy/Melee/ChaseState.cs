using System.Linq;
using UnityEngine;

public class ChaseState : IState
{
    private readonly Enemy  _enemy;
    private readonly string _unitTag;   // tag of enemy-units to hunt
    private readonly string _baseTag;   // tag of base to fallback to
    private readonly EnemyStateMachine _sm;

    public ChaseState(Enemy enemy, EnemyStateMachine sm, string unitTag, string baseTag)
    {
        _enemy    = enemy;
        _sm       = sm;
        _unitTag  = unitTag;
        _baseTag  = baseTag;
    }

    public void Enter()
    {
        AcquireTarget();
        if (_enemy.CurrentTarget != null)
        {
            _enemy.Agent.isStopped = false;
            _enemy.MoveTo(_enemy.CurrentTarget.position);
        }
    }

    public void Tick()
    {
        AcquireTarget();
        if (_enemy.CurrentTarget != null)
            _enemy.MoveTo(_enemy.CurrentTarget.position);

        // TODO: insert attack-in-range logic here
    }

    public void Exit()
    {
        if (_enemy.Agent != null)
            _enemy.Agent.isStopped = true;
    }

    private void AcquireTarget()
    {
        Transform next = null;

        // 1) hunt closest alive unit
        var units = GameObject
            .FindGameObjectsWithTag(_unitTag)
            .Select(go => go.transform)
            .Where(t => t != _enemy.transform && !IsDead(t));

        if (units.Any())
        {
            next = units
                .OrderBy(t => Vector3.Distance(_enemy.transform.position, t.position))
                .First();
        }
        else
        {
            // 2) no units → fallback to base
            var baseGO = GameObject.FindGameObjectWithTag(_baseTag);
            if (baseGO != null)
                next = baseGO.transform;
        }

        _enemy.SetTarget(next);
    }

    private bool IsDead(Transform t)
    {
        var h = t.GetComponent<EnemyHealth>();
        return h != null && h.currentHealth <= 0;
    }
}