using UnityEngine;

public class HeavyEnemy : Enemy {
    [SerializeField] private float chargeTime = 1.5f;
    public float ChargeTime => chargeTime;
    //protected override void InitializeStates() {}
}
