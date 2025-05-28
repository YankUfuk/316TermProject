using UnityEngine;

public class EnemyStateMachine {
    public IState CurrentState { get; private set; }
    public void Initialize(IState startingState) {
        CurrentState = startingState;
        CurrentState.Enter();
    }
    public void ChangeState(IState newState) {
        CurrentState.Exit();
        CurrentState = newState;
        Debug.Log($"[StateMachine] Ticking state: {CurrentState.GetType().Name}");

        CurrentState.Enter();
    }
    public void Tick() {
        CurrentState.Tick();
    }
}