// StateBase.cs
using UnityEngine;

public abstract class StateBase : IState
{
    protected Enemy enemy;
    protected EnemyStateMachine sm;

    protected StateBase(Enemy enemy, EnemyStateMachine stateMachine)
    {
        this.enemy = enemy;
        this.sm = stateMachine;
    }

    /// <summary>
    /// Called once when the state is entered.
    /// Use this to trigger animations, reset timers, etc.
    /// </summary>
    public virtual void Enter() { }

    /// <summary>
    /// Called every frame while this is the current state.
    /// Put your per-frame logic here (movement, checks, transitions).
    /// </summary>
    public abstract void Tick();

    /// <summary>
    /// Called once when exiting this state.
    /// Use this to clean up (stop animations, reset flags, etc.).
    /// </summary>
    public virtual void Exit() { }
}