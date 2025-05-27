using UnityEngine;

public class ControlSwitcher : MonoBehaviour
{
    [Header("Controllers")]
    [Tooltip("Your default player-controlled character script")]
    public MonoBehaviour    playerController;
    [Tooltip("The script that handles your mouse-look / mouse orbit")]
    public MonoBehaviour    mouseController;
    [Tooltip("Your RTS-style camera controller script")]
    public MonoBehaviour    rtsCameraController;

    [Header("Cameras")]
    [Tooltip("One shared camera you use for player mode")]
    public Camera playerCamera;
    [Tooltip("Your RTS-mode camera")]
    public Camera rtsCamera;

    [Header("Settings")]
    public KeyCode switchKey    = KeyCode.Tab;
    public string  troopTag     = "Troop";               // Tag all selectable units
    public Vector3 cameraOffset = new Vector3(0, 2, -4); // Tweak to taste

    // internal state
    private MonoBehaviour currentController;
    private Camera        currentCamera;
    private bool          usingPlayer = true;

    void Start()
    {
        // start out in player mode, controlling your default character
        currentController = playerController;
        currentCamera     = playerCamera;
        SetMode(!usingPlayer);
    }

    void Update()
    {
        if (!usingPlayer && Input.GetMouseButtonDown(0))
        {
            // 1) raycast from the exact center of the viewport
            var ray = rtsCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out var hit) && hit.collider.CompareTag(troopTag))
            {
                var newCtrl = hit.collider.GetComponent<PlayerMovement>();
                if (newCtrl != null)
                {
                    // disable the old
                    currentController.enabled = false;
                    currentCamera.enabled     = false;

                    // switch to the new
                    currentController = newCtrl;
                    currentCamera     = playerCamera;

                    // parent & offset the camera
                    playerCamera.transform.SetParent(newCtrl.transform);
                    playerCamera.transform.localPosition = cameraOffset;
                    playerCamera.transform.localRotation = Quaternion.identity;

                    usingPlayer = true;
                    SetMode(true);
                }
            }
        }

        // 2) Always allow Tab to flip between “player” and “RTS”
        if (Input.GetKeyDown(switchKey))
        {
            usingPlayer = !usingPlayer;
            SetMode(usingPlayer);
        }
    }

    private void SetMode(bool toPlayer)
    {
        // RTS components on when !toPlayer
        rtsCameraController.enabled = !toPlayer;
        rtsCamera.enabled           = !toPlayer;

        // Player components on when toPlayer
        playerController.enabled    = toPlayer;
        mouseController.enabled     = toPlayer;
        playerCamera.enabled        = toPlayer;

        if (toPlayer)
        {
            // make the player camera follow again
            playerCamera.transform.SetParent(currentController.transform);
            playerCamera.transform.localPosition = cameraOffset;
            playerCamera.transform.localRotation = Quaternion.identity;
        }
        else
        {
            // detach camera so it stops following the last player
            playerCamera.transform.SetParent(null);
        }
    }

}
