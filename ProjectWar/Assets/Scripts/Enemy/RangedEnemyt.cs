using UnityEngine;

public class RangedEnemy : Enemy {
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    public GameObject ProjectilePrefab => projectilePrefab;
    public Transform FirePoint => firePoint;

    //protected override void InitializeStates() {}
}
