using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Product : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform spawnLocation;
    public int price;
    public TextMeshProUGUI priceText;
    public MoneySystem moneySystem;


    void Start()
    {
        moneySystem = FindAnyObjectByType<MoneySystem>();
        priceText.text = price.ToString();
    }
    public void BuyProduct()
    {
        if (moneySystem.money >= price)
        {

            moneySystem.SpendMoney(price);
            Instantiate(objectToSpawn, spawnLocation.position, Quaternion.identity);
        }
    }
}
