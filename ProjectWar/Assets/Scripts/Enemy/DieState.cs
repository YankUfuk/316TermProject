using UnityEngine;

public class DieState : StateBase {
    public DieState(Enemy enemy, EnemyStateMachine sm) : base(enemy, sm) {}
    public override void Enter() {
        // play death anim, disable collider, Destroy after anim
    }

    public override void Tick()
    {
        
    }
}
