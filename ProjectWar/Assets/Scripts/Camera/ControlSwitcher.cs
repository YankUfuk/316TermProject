using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControlSwitcher : MonoBehaviour
{
    [Header("Controllers")]
    [Tooltip("Script on your default character (e.g. camera-orbit)")]
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
    public string  troopTag     = "Switch";               // Now used dynamically!
    public Vector3 cameraOffset = new Vector3(0, 2, -4);  // Tweak to taste

    // internal
    private Enemy           _aiComp;       // your Enemy or MeleeEnemy script
    private PlayerMovement  _pcComp;       // your player-control script
    private GameObject      _troopRoot;
    private bool            _controllingTroop = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // 1) Ray from the center of your RTS camera
            Vector3 center = new Vector3(rtsCamera.pixelWidth / 2f,
                                         rtsCamera.pixelHeight / 2f,
                                         0f);
            Ray ray = rtsCamera.ScreenPointToRay(center);

            if (Physics.Raycast(ray, out var hit) && hit.collider.CompareTag(troopTag))
            {
                // 2) Climb to the topmost ancestor that has your troopTag
                Transform root = hit.collider.transform;
                while (root != null && !root.CompareTag(troopTag))
                    root = root.parent;
                if (root == null) return;

                // 3) BFS‐style search: stop as soon as we find both targets
                var names = new[] { "MeleeAI", "MeleeEnemy" };
                var found = new Dictionary<string, Transform>();
                var queue = new Queue<Transform>();
                queue.Enqueue(root);

                while (queue.Count > 0 && found.Count < names.Length)
                {
                    var cur = queue.Dequeue();

                    foreach (var name in names)
                    {
                        if (!found.ContainsKey(name) && cur.name.Trim() == name)
                        {
                            found[name] = cur;
                            break;  // don't enqueue this one's children just yet
                        }
                    }

                    // only keep searching this branch if we still need matches
                    if (found.Count < names.Length)
                        foreach (Transform child in cur)
                            queue.Enqueue(child);
                }

                // 4) pull out the two transforms
                if (!found.TryGetValue("MeleeAI", out var aiT) ||
                    !found.TryGetValue("MeleeEnemy", out var ctrlT))
                {
                    Debug.LogWarning($"Couldn't find both 'MeleeAI' & 'MeleeEnemy' under '{root.name}'");
                    return;
                }

                // 5) toggle them
                aiT.gameObject. SetActive(false);
                ctrlT.gameObject.SetActive(true);
            }
        }
    }
    

    private void StartControl(GameObject troopRoot)
    {
        // cache for later
        _troopRoot = troopRoot;

        // find the AI component
        _aiComp = troopRoot.GetComponentInChildren<Enemy>();
        // find the player-control component
        _pcComp = troopRoot.GetComponentInChildren<PlayerMovement>();

        if (_aiComp == null || _pcComp == null)
        {
            Debug.LogWarning($"Troop needs both an Enemy (AI) and PlayerMovement component.\n" +
                             $"Found Enemy? {_aiComp!=null}, PlayerMovement? {_pcComp!=null}");
            _troopRoot = null;
            return;
        }

        // disable AI, enable player-control
        _aiComp.enabled = false;
        _pcComp.enabled = true;

        // swap cameras
        SetRTSMode(false);

        // parent the player camera to the troop
        playerCamera.transform.SetParent(_pcComp.transform);
        playerCamera.transform.localPosition = cameraOffset;
        playerCamera.transform.localRotation = Quaternion.identity;
        playerCamera.enabled = true;

        _controllingTroop = true;
    }

    private void StopControl()
    {
        // re-enable AI, disable player-control
        if (_aiComp != null) _aiComp.enabled = true;
        if (_pcComp != null) _pcComp.enabled = false;

        // restore cameras
        playerCamera.transform.SetParent(null);
        playerCamera.enabled = false;
        SetRTSMode(true);

        _troopRoot = null;
        _aiComp = null;
        _pcComp = null;
        _controllingTroop = false;
    }

    private void SetRTSMode(bool on)
    {
        rtsCamera.gameObject.SetActive(on);
        rtsCameraController.enabled = on;
        mouseController.enabled     = !on; // if you want mouse-look off in RTS
    }
}
