using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("DemoScene");
    }

    public void OpenOptions()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
