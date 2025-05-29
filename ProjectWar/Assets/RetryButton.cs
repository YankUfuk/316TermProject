using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    public void RetryScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("DemoScene");
    }
}