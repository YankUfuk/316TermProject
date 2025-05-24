using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    public void Start()
    {
        defeatPanel.SetActive(false);
        victoryPanel.SetActive(false);
    }

    public void GameOver(bool playerBase)
    {
        Time.timeScale = 0f;

        if (playerBase)
        {
            defeatPanel.SetActive(true);
        }
        else
        {
            victoryPanel.SetActive(true);
        }
    }
}
