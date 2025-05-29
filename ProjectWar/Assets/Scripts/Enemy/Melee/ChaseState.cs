using System.Linq;
using UnityEngine;

public class ChaseState : IState
{
    private readonly Enemy _enemy;
    private readonly string _unitTag;
    private readonly string _baseTag;
    private readonly EnemyStateMachine _sm;
    private readonly Animator _animator;

    public ChaseState(Enemy enemy, EnemyStateMachine sm, string unitTag, string baseTag)
    {
        _enemy = enemy;
        _sm = sm;
        _unitTag = unitTag;
        _baseTag = baseTag;

        // Animator'ı enemy üstünden çek
        _animator = _enemy.GetComponent<Animator>();
    }

    public void Enter()
    {
        AcquireTarget();
        if (_enemy.CurrentTarget != null)
        {
            _enemy.Agent.isStopped = false;
            _enemy.MoveTo(_enemy.CurrentTarget.position);

            // yürümeye başlıyor
            _animator.SetBool("isWalking", true);
        }
        else
        {
            // hedef yok → yürümüyor
            _animator.SetBool("isWalking", false);
        }
    }

    public void Tick()
    {
        AcquireTarget();

        if (_enemy.CurrentTarget != null)
        {
            _enemy.MoveTo(_enemy.CurrentTarget.position);

            // yürüyorsa yürümeye devam
            if (_enemy.Agent.velocity.magnitude > 0.1f)
                _animator.SetBool("isWalking", true);
            else
                _animator.SetBool("isWalking", false);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }

        // TODO: attack logic
    }

    public void Exit()
    {
        if (_enemy.Agent != null)
            _enemy.Agent.isStopped = true;

        // çıkarken durdur
        _animator.SetBool("isWalking", false);
    }

    private void AcquireTarget()
    {
        Transform next = null;

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
