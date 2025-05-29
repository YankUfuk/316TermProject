using UnityEngine;
using System;

public class ControlSwitcher : MonoBehaviour
{
    [Header("RTS Mode")]
    public Camera rtsCamera;
    public MonoBehaviour[] rtsControllers;
    public GameObject[] rtsUIElements; // ✅ Now an array of UI elements
    public event Action OnDeath;

    [Header("Player Modes (0…N–1)")]
    [Tooltip("Set Size = number of different unit types you can possess.")]
    public ControlMode[] modes;

    int _activeIndex = -1;   

    [Serializable]
    public class ControlMode
    {
        public string modeName;
        public GameObject unitPrefab;
        public Transform spawnPoint;

        [HideInInspector] public GameObject instance;
        [HideInInspector] public UnitControlSetup setup;
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
        SetUIElementsActive(rtsUIElements, true); // ✅ Show RTS UI
        foreach (var c in rtsControllers)
            c.enabled = true;
    }

    void TearDownRTS()
    {
        rtsCamera.gameObject.SetActive(false);
        SetUIElementsActive(rtsUIElements, false); // ✅ Hide RTS UI
        foreach (var c in rtsControllers)
            c.enabled = false;
    }

    void BringUpMode(ControlMode m)
    {
        m.instance = Instantiate(m.unitPrefab, m.spawnPoint.position, m.spawnPoint.rotation);
        m.setup = m.instance.GetComponent<UnitControlSetup>();

        m.setup.unitCamera.gameObject.SetActive(true);
        foreach (var c in m.setup.controllers)
            c.enabled = true;

        if (m.setup.health != null)
            m.setup.health.OnDeath += OnUnitDeath;
    }

    void TearDownMode(ControlMode m)
    {
        if (m.setup.health != null)
            m.setup.health.OnDeath -= OnUnitDeath;

        m.setup.unitCamera.gameObject.SetActive(false);
        foreach (var c in m.setup.controllers)
            c.enabled = false;

        Destroy(m.instance);
        m.instance = null;
        m.setup = null;
    }

    void OnUnitDeath()
    {
        SwitchTo(-1);
    }

    public void Die()
    {
        OnDeath?.Invoke();
    }

    private void SetUIElementsActive(GameObject[] elements, bool isActive)
    {
        foreach (var go in elements)
        {
            if (go != null)
                go.SetActive(isActive);
        }
    }
}
