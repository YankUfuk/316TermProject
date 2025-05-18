using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    public GameObject turretPrefab;
    public Transform[] turretPoints; // 3 farklı spawn noktası
    public int[] upgradeCosts = { 30, 50, 80 };

    public MoneySystem moneySystem;
    public TextMeshProUGUI priceText;
    public Button upgradeButton;

    private int upgradeLevel = 0;

    void Start()
    {
        moneySystem = FindObjectOfType<MoneySystem>();
        UpdatePriceUI();
    }

    public void BuyUpgrade()
    {
        if (upgradeLevel >= upgradeCosts.Length) return;

        int cost = upgradeCosts[upgradeLevel];

        if (moneySystem.money >= cost)
        {
            moneySystem.SpendMoney(cost);

            // Yeni turret ekle
            Instantiate(turretPrefab, turretPoints[upgradeLevel].position, turretPoints[upgradeLevel].rotation);
            upgradeLevel++;

            // Fiyat UI’sini güncelle
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
            upgradeButton.interactable = false; // Butonu pasifleştir
        }
    }
}
