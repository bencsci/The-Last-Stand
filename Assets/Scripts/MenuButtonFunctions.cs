using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_Button_Functions : MonoBehaviour
{
    public GameObject howToPlayPanel;
    public void PlayGame()
    {
        SceneManager.LoadScene("World");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
