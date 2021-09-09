/* Written by Tommy Oh
 Edited by ___
 Last date edited: 09/07/2021
 PauseMenu.cs - Manages the pause menu.

 Version 1: Makes the game pause with escape key and navigation for the pause menu.*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isGamePaused = false;
    public GameObject pauseMenuUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void  Quit()
    {
        Application.Quit();
    }
}
