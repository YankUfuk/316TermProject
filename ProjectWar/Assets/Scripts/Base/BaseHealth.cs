using UnityEngine;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;
    public bool isPlayerBase;

    [Header("UI Canvases")]
    public GameObject loseCanvas;   // assign your “You Lose” canvas here
    public GameObject winCanvas;    // assign your “You Win” canvas here

    private void Start()
    {
        // initialize health
        currentHealth = maxHealth;
        UpdateUI();

        // ensure both end‐game canvases are off at start
        if (loseCanvas != null) loseCanvas.SetActive(false);
        if (winCanvas  != null) winCanvas.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateUI();

        if (currentHealth <= 0)
        {
            // if this is the player base → show Lose
            if (isPlayerBase)
            {
                if (loseCanvas != null)
                    loseCanvas.SetActive(true);
            }
            // else it's the enemy base → show Win
            else
            {
                if (winCanvas != null)
                    winCanvas.SetActive(true);
            }

            // still call your GameOverManager if you need it
            FindObjectOfType<GameOverManager>().GameOver(isPlayerBase);
        }
    }

    public void IncreaseHealth(float amount)
    {
        currentHealth += Mathf.RoundToInt(amount);
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
    }
}