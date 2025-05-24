using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;
    public bool isPlayerBase;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateUI();

        if (currentHealth <= 0)
        {
            FindObjectOfType<GameOverManager>().GameOver(isPlayerBase);
        }
    }

    void UpdateUI()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
    }
}
