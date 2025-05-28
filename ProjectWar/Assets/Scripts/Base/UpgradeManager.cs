using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject turretPrefab;
    public Transform[] turretPoints;
    public int[] upgradeCosts = { 30, 50, 80 };

    public MoneySystem moneySystem;
    public TextMeshProUGUI priceText;
    public Button upgradeButton;

    public AudioClip upgradeSound;
    public AudioSource audioSource;

    private int upgradeLevel = 0;

    void Start()
    {
        moneySystem = FindObjectOfType<MoneySystem>();
        UpdatePriceUI();
    }

    public void BuyUpgrade()
    {


        if (upgradeLevel >= upgradeCosts.Length)
        {

            return;
        }

        int cost = upgradeCosts[upgradeLevel];


        if (moneySystem.money >= cost)
        {
            moneySystem.SpendMoney(cost);


            Instantiate(turretPrefab, turretPoints[upgradeLevel].position, turretPoints[upgradeLevel].rotation);
            upgradeLevel++;



            if (audioSource != null && upgradeSound != null)
            {

                audioSource.PlayOneShot(upgradeSound);
            }

            UpdatePriceUI();
        }

    }


    void UpdatePriceUI()
    {
        if (upgradeLevel < upgradeCosts.Length)
        {
            priceText.text = upgradeCosts[upgradeLevel].ToString();
        }
        else
        {
            priceText.text = "Max";
            upgradeButton.interactable = false;
        }
    }
}
