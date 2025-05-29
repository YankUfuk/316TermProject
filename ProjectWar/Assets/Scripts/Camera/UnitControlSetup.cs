using UnityEngine;

public class UnitControlSetup : MonoBehaviour
{
    [Tooltip("Drag your unit’s child Camera here (from the prefab).")]
    public Camera           unitCamera;

    [Tooltip("All of the movement / look / input scripts on this prefab.")]
    public MonoBehaviour[]  controllers;

    [Tooltip("If this prefab has a Health component with OnDeath, drag it here.")]
    public PlayerHealth           health;
}