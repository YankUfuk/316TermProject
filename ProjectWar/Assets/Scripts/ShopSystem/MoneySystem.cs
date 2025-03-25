using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MoneySystem : MonoBehaviour
{
    public int money;
    public TextMeshProUGUI moneyText;
    public int moneyPerSecond;
    public float timer;



    void Update()
    {

        timer += Time.deltaTime;
        if (timer >= 1f)
        {
            money += moneyPerSecond;
            timer = 0f;
            updateMoneyText();
        }

    }

    public void updateMoneyText()
    {

        moneyText.text = "Money: " + money.ToString() + " $";

    }

    public void SpendMoney(int cost)
    {
        if (money >= cost)
        {
            money -= cost;
            updateMoneyText();

        }

    }
}