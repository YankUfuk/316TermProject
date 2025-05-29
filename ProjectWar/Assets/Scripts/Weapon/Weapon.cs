using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponClass { Player, Tank, Melee }
    public WeaponClass weaponClass = WeaponClass.Player;
    
    // --- PLAYER SETTINGS (default) ---
    [Header("Player Weapon Settings")]
    [SerializeField] public Camera playerCamera;
    [SerializeField] private GameObject playerBulletPrefab;
    [SerializeField] private Transform playerBulletSpawn;

    // --- TANK SETTINGS ---
    [Header("Tank Weapon Settings")]
    [SerializeField] public Camera tankCamera;
    [SerializeField] private GameObject tankShellPrefab;
    [SerializeField] private Transform tankShellSpawn;
    [SerializeField] private Transform tankBody;           // assign your tank’s bottom/base
    [SerializeField] private Transform tankTurret;         // assign the rotating turret (top)
    [SerializeField] private float tankMoveSpeed = 8f;
    [SerializeField] private float turretRotateSpeed = 60f;
    [SerializeField] private float turretMinPitch = -5f;
    [SerializeField] private float turretMaxPitch = 15f;
    [Header("UI")]
    [SerializeField] private RectTransform crosshairUI;
    [SerializeField] private float crosshairMaxDistance = 1000f;
    [Header("Tank Camera Orbit")]
    [SerializeField] private float cameraOrbitSpeed = 60f;
    private Vector3 tankCamOffset;
    // --- MELEE SETTINGS ---
    [Header("Melee Settings")]
    [SerializeField] private KeyCode meleeKey = KeyCode.E;
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private float meleeDelay = 0.6f;
    [SerializeField] private LayerMask meleeHitLayers;
    [SerializeField] private Animator animator;

    // Shared shooting parameters
    [Header("Common Shooting Settings")]
    public bool isShooting, readyToShoot;
    private bool allowReset = true;
    public float shootingDelay = 2f;
    public int bulletsPerBurst = 3;
    private int burstBulletsLeft;
    public float spreadIntensity;
    public float bulletVelocity = 30f;
    public float bulletPrefabLifeTime = 3f;
    public enum ShootingMode { Single, Burst, Auto }
    public ShootingMode currentShootingMode;

    // Internal state
    private Camera activeCamera;
    private GameObject activePrefab;
    private Transform activeSpawn;
    private float turretPitch;
    private bool readyToMelee = true;

    void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        if (tankTurret != null)
            turretPitch = tankTurret.localEulerAngles.x;
        
        if (weaponClass == WeaponClass.Tank && tankCamera != null && tankBody != null)
            tankCamOffset = tankCamera.transform.position - tankBody.position;
        UpdateMode();
    }

    void Update()
    {
        UpdateMode();
        UpdateCrosshair(); 
        if (weaponClass == WeaponClass.Tank)
        {
            HandleTankMovement();
            HandleTurretRotation();
            OrbitTankCamera();    
        }

        // Ranged input (player or tank)
        if (currentShootingMode == ShootingMode.Auto)
            isShooting = Input.GetKey(KeyCode.Mouse0);
        else
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }

        // Melee input
        if (weaponClass == WeaponClass.Melee && Input.GetKeyDown(meleeKey) && readyToMelee)
            StartCoroutine(PerformMelee());
    }

    private void UpdateMode()
    {
        bool isTank = weaponClass == WeaponClass.Tank;
        activeCamera = isTank ? tankCamera : playerCamera;
        activePrefab = isTank ? tankShellPrefab : playerBulletPrefab;
        activeSpawn = isTank ? tankShellSpawn : playerBulletSpawn;

        // Switch cameras
        if (Camera.main != null) Camera.main.enabled = false;
        if (activeCamera != null) activeCamera.enabled = true;
    }

    private void HandleTankMovement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        // Move bottom along world axes; no rotation applied
        Vector3 move = new Vector3(h, 0f, v) * tankMoveSpeed * Time.deltaTime;
        tankBody.Translate(move, Space.World);
    }

    private void HandleTurretRotation()
    {
        if (tankTurret == null) return;
        // Yaw (horizontal)
        float mx = Input.GetAxis("Mouse X");
        tankTurret.Rotate(0f, mx * turretRotateSpeed * Time.deltaTime, 0f, Space.World);
        
        // Pitch (vertical), clamped
        float my = -Input.GetAxis("Mouse Y");
        turretPitch = Mathf.Clamp(turretPitch + my * turretRotateSpeed * Time.deltaTime, turretMinPitch, turretMaxPitch);
        Vector3 angles = tankTurret.localEulerAngles;
        angles.x = turretPitch;
        tankTurret.localEulerAngles = angles;
    }

    private void FireWeapon()
    {
        readyToShoot = false;
        Vector3 dir = CalculateDirectionAndSpread().normalized;
        
        var proj = Instantiate(activePrefab, activeSpawn.position, Quaternion.LookRotation(dir));
        proj.GetComponent<Rigidbody>().AddForce(dir * bulletVelocity, ForceMode.Impulse);
        StartCoroutine(DestroyAfter(proj, bulletPrefabLifeTime));

        if (allowReset)
        {
            Invoke(nameof(ResetShot), shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke(nameof(FireWeapon), shootingDelay);
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    private Vector3 CalculateDirectionAndSpread()
    {
        // If we’re in tank mode, base our shot on the turret’s forward vector:
        if (weaponClass == WeaponClass.Tank && tankTurret != null)
        {
            // Start with the exact barrel forward
            Vector3 dir = tankTurret.forward;

            // Cast out to see if we hit something
            Ray ray = new Ray(tankShellSpawn.position, dir);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
                dir = (hit.point - tankShellSpawn.position).normalized;

            // Add tank‐spread (optional)
            dir += new Vector3(
                Random.Range(-spreadIntensity, spreadIntensity),
                Random.Range(-spreadIntensity, spreadIntensity),
                Random.Range(-spreadIntensity, spreadIntensity)
            );
            return dir.normalized;
        }

        // Else (player mode), fallback to camera‐center ray
        var camRay = activeCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(camRay, out var camHit))
            camRay.direction = (camHit.point - activeSpawn.position);
        camRay.direction += new Vector3(
            Random.Range(-spreadIntensity, spreadIntensity),
            Random.Range(-spreadIntensity, spreadIntensity),
            0
        );
        return camRay.direction.normalized;
    }
    private void OrbitTankCamera()
    {
        if (tankCamera == null || tankBody == null || activeCamera != tankCamera) 
            return;

        // 1) rotate the offset around Y
        float mx = Input.GetAxis("Mouse X") * cameraOrbitSpeed * Time.deltaTime;
        tankCamOffset = Quaternion.AngleAxis(mx, Vector3.up) * tankCamOffset;

        // 2) re-position camera
        tankCamera.transform.position = tankBody.position + tankCamOffset;

        // 3) always look at the tank
        tankCamera.transform.LookAt(tankBody.position);
    }
    private void UpdateCrosshair()
    {
        if (weaponClass != WeaponClass.Tank || crosshairUI == null || tankTurret == null)
        {
            crosshairUI.gameObject?.SetActive(false);
            return;
        }

        crosshairUI.gameObject.SetActive(true);

        // Ray from turret forward
        Vector3 origin = tankShellSpawn.position;
        Vector3 direction = tankTurret.forward;
        Vector3 target = origin + direction * crosshairMaxDistance;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, crosshairMaxDistance))
            target = hit.point;

        // Project to screen
        Vector3 screenPos = activeCamera.WorldToScreenPoint(target);

        // Move the UI crosshair
        crosshairUI.position = screenPos;
    }


    private IEnumerator DestroyAfter(GameObject go, float t)
    {
        yield return new WaitForSeconds(t);
        Destroy(go);
    }

    private IEnumerator PerformMelee()
    {
        readyToMelee = false;
        if (animator != null) animator.SetTrigger("Melee");
        yield return new WaitForSeconds(meleeDelay * 0.5f);
        DoMeleeHit();
        yield return new WaitForSeconds(meleeDelay * 0.5f);
        readyToMelee = true;
    }

    private void DoMeleeHit()
    {
        Vector3 origin = playerCamera.transform.position;
        Vector3 forward = playerCamera.transform.forward;
        RaycastHit[] hits = Physics.SphereCastAll(origin, 0.5f, forward, meleeRange, meleeHitLayers);
        foreach (var h in hits)
            Debug.Log("Melee hit: " + h.collider.name);
    }

    void OnDrawGizmosSelected()
    {
        if (activeCamera == null) return;
        Gizmos.color = Color.red;
        Vector3 start = activeCamera.transform.position;
        Vector3 dir = activeCamera.transform.forward * meleeRange;
        Gizmos.DrawWireSphere(start + dir * 0.5f, 0.5f);
    }
}