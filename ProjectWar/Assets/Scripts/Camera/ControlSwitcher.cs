using UnityEngine;
using System;

public class ControlSwitcher : MonoBehaviour
{
    [Header("RTS Mode")]
    public Camera         rtsCamera;
    public MonoBehaviour[] rtsControllers;
    public event Action OnDeath;
    [Header("Player Modes (0…N–1)")]
    [Tooltip("Set Size = number of different unit types you can possess.")]
    public ControlMode[]  modes;

    int _activeIndex = -1;   

    [Serializable]
    public class ControlMode
    {
        public string     modeName;
        public GameObject unitPrefab;
        public Transform  spawnPoint;

        [HideInInspector] public GameObject         instance;
        [HideInInspector] public UnitControlSetup   setup;
    }

    void Start()
    {
        EnterRTS();
    }
    
    public void SwitchTo(int modeIndex)
    {
        if (modeIndex == _activeIndex) return;

        if (_activeIndex >= 0)
            TearDownMode(modes[_activeIndex]);
        else
            TearDownRTS();

        _activeIndex = modeIndex;

        if (_activeIndex >= 0)
            BringUpMode(modes[_activeIndex]);
        else
            EnterRTS();
    }

    void EnterRTS()
    {
        rtsCamera.gameObject.SetActive(true);
        foreach (var c in rtsControllers) c.enabled = true;
    }

    void TearDownRTS()
    {
        rtsCamera.gameObject.SetActive(false);
        foreach (var c in rtsControllers) c.enabled = false;
    }

    void BringUpMode(ControlMode m)
    {
        // spawn prefab
        m.instance = Instantiate(
            m.unitPrefab,
            m.spawnPoint.position,
            m.spawnPoint.rotation);

        // grab its UnitControlSetup
        m.setup = m.instance.GetComponent<UnitControlSetup>();

        // enable that unit’s camera + controllers
        m.setup.unitCamera.gameObject.SetActive(true);
        foreach (var c in m.setup.controllers)
            c.enabled = true;

        // listen for death
        if (m.setup.health != null)
            m.setup.health.OnDeath += OnUnitDeath;
    }

    void TearDownMode(ControlMode m)
    {
        // unsubscribe
        if (m.setup.health != null)
            m.setup.health.OnDeath -= OnUnitDeath;

        // disable camera & controllers (in case)
        m.setup.unitCamera.gameObject.SetActive(false);
        foreach (var c in m.setup.controllers)
            c.enabled = false;

        // destroy the spawned GameObject
        Destroy(m.instance);
        m.instance = null;
        m.setup    = null;
    }

    void OnUnitDeath()
    {
        SwitchTo(-1);
    }public void Die()
    {
        OnDeath?.Invoke();
        // Additional death logic...
    }
}
