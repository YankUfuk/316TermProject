using UnityEngine;

public class VehicleSwitcher : MonoBehaviour
{
    [Tooltip("Reference to your Weapon component")]
    public Weapon playerWeapon;

    [Tooltip("Key to toggle between player and tank")]
    public KeyCode switchKey = KeyCode.E;

    private bool inTank = false;

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            inTank = !inTank;
            playerWeapon.SetUsingTank(inTank);

            if (playerWeapon.playerCamera != null && playerWeapon.tankCamera != null)
            {
                playerWeapon.playerCamera.gameObject.SetActive(!inTank);
                playerWeapon.tankCamera.gameObject.SetActive( inTank);
            }
        }
    }
}