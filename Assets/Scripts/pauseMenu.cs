using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{
    // Reference to the menu GameObject
    public GameObject menu;

    // Reference to the crosshair RawImage
    public RawImage crosshair;

    // Flag indicating if the game is paused
    private bool isPaused;

    private void Start()
    {
        menu.SetActive(false);  
        
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !StoreInterface.inStore)
            {
                if (isPaused)
                {
                    resume();
                }
                else
                {
                    pause();                 
                }
            }
    }

    public void resume()
    {
        crosshair.enabled = true;
        PlayerController.active = true;
        Weapon.active = true;
        Cursor.visible = false;

        menu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void pause()
    {
        crosshair.enabled = false;
        PlayerController.active = false;
        Weapon.active = false;
        Cursor.visible = true;

        menu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void backMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void quit()
    {
        Application.Quit();
    }
}
