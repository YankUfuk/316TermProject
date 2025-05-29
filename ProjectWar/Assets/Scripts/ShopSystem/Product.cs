using UnityEngine;
using TMPro;

public class Product : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform spawnLocation;
    public int price;
    public MoneySystem moneySystem;

    public AudioClip clickSound;
    public AudioSource audioSource;

    void Start()
    {
        moneySystem = FindAnyObjectByType<MoneySystem>();

        audioSource = GetComponent<AudioSource>();
    }

    public void BuyProduct()
    {
        if (moneySystem.money >= price)
        {
            moneySystem.SpendMoney(price);
            Instantiate(objectToSpawn, spawnLocation.position, Quaternion.identity);

            if (clickSound == null)
            {
                Debug.LogWarning("🚫 clickSound NULL!");
            }
            if (audioSource == null)
            {
                Debug.LogWarning("🚫 audioSource NULL!");
            }

            if (clickSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(clickSound);
            }
        }
    }

}
