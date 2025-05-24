using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // --- PLAYER SETTINGS (default) ---
    [Header("Player Weapon Settings")]
    [SerializeField] public Camera playerCamera;
    [SerializeField] private GameObject playerBulletPrefab;
    [SerializeField] private Transform playerBulletSpawn;

    // --- TANK SETTINGS ---
    [Header("Tank Weapon Settings")]
    [SerializeField] public Camera tankCamera;
    [SerializeField] private GameObject tankShellPrefab;
    [SerializeField] private Transform  tankShellSpawn;

    // Shared shooting parameters
    [Header("Common Shooting Settings")]
    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 2f;
    public int   bulletsPerBurst = 3;
    private int   burstBulletsLeft;
    public float spreadIntensity;
    public float bulletVelocity        = 30f;
    public float bulletPrefabLifeTime  = 3f;

    public enum ShootingMode { Single, Burst, Auto }
    public ShootingMode currentShootingMode;

    // Internal state
    private bool      usingTank = false;
    private Camera    activeCamera;
    private GameObject activePrefab;
    private Transform activeSpawn;

    void Awake()
    {
        // initialize
        readyToShoot      = true;
        burstBulletsLeft  = bulletsPerBurst;
        // default to player
        SetUsingTank(false);
    }

    void Update()
    {
        // handle firing input
        if (currentShootingMode == ShootingMode.Auto)
            isShooting = Input.GetKey(KeyCode.Mouse0);
        else
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        readyToShoot = false;

        // calculate spread + direction using whichever camera & spawn we have active
        Vector3 dir = CalculateDirectionAndSpread().normalized;

        // spawn bullet/shell
        var proj = Instantiate(activePrefab, activeSpawn.position, Quaternion.LookRotation(dir));
        proj.GetComponent<Rigidbody>()
            .AddForce(dir * bulletVelocity, ForceMode.Impulse);

        StartCoroutine(DestroyAfter(proj, bulletPrefabLifeTime));

        // reset
        if (allowReset)
        {
            Invoke(nameof(ResetShot), shootingDelay);
            allowReset = false;
        }

        // burst logic
        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke(nameof(FireWeapon), shootingDelay);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset   = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        var ray = activeCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out var hit))
            ray.direction = (hit.point - activeSpawn.position);
        // add random spread on X,Y
        ray.direction += new Vector3(
            Random.Range(-spreadIntensity, spreadIntensity),
            Random.Range(-spreadIntensity, spreadIntensity),
            0);
        return ray.direction;
    }

    private IEnumerator DestroyAfter(GameObject go, float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(go);
    }

    /// <summary>
    /// Call this to switch between player firing and tank firing.
    /// </summary>
    public void SetUsingTank(bool enableTank)
    {
        usingTank      = enableTank;
        activeCamera   = usingTank ? tankCamera   : playerCamera;
        activePrefab   = usingTank ? tankShellPrefab : playerBulletPrefab;
        activeSpawn    = usingTank ? tankShellSpawn  : playerBulletSpawn;
    }
}
