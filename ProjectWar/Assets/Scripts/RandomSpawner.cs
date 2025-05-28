using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] unitPrefabs;
    public Transform spawnPoint;
    public float spawnInterval = 5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRandomUnit();
            timer = 0f;
        }
    }

    void SpawnRandomUnit()
    {
        if (unitPrefabs.Length == 0) return;

        int index = Random.Range(0, unitPrefabs.Length);
        GameObject selectedUnit = unitPrefabs[index];

        Instantiate(selectedUnit, spawnPoint.position, spawnPoint.rotation);
    }
}
