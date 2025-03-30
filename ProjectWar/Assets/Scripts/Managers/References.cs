using System;
using UnityEngine;

public class References : MonoBehaviour
{
    public static References Instance { get; set; }
    public GameObject bulletImpactEffectPrefab;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}
    