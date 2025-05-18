using UnityEngine;

public class ControlSwitcher : MonoBehaviour
{
    [Header("Controllers")]
    public MonoBehaviour characterController;     
    public MonoBehaviour rtsCameraController;     

    [Header("Cameras")]
    public Camera characterCamera;                
    public Camera rtsCamera;                      

    [Header("Settings")]
    public KeyCode switchKey = KeyCode.Tab;       

    private bool usingCharacter = true;

    void Start()
    {
        SetMode(usingCharacter);
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            usingCharacter = !usingCharacter;
            SetMode(usingCharacter);
        }
    }

    private void SetMode(bool characterMode)
    {
        characterController.enabled    = characterMode;
        rtsCameraController.enabled    = !characterMode;

        characterCamera.enabled        = characterMode;
        rtsCamera.enabled              = !characterMode;
    }
}