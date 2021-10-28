/* Written by Tommy Oh
 Edited by ___
 Last date edited: 09/07/2021
 MainMenu.cs - Manages the main menu start and exit buttons.

 Version 1: Created functions for start and exit buttons.*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private GameObject root;

    //Starts the Game On easy
    public void StartGameEasy()
    {
        PlayerPrefs.SetInt("Difficulty", 1);
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    //Starts the Game on Normal
    public void StartGameNorm()
    {
        PlayerPrefs.SetInt("Difficulty", 2);
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    //Starts the Game on Hard
    public void StartGameHard()
    {
        PlayerPrefs.SetInt("Difficulty", 3);
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }

    // Exits the Game
    public void ExitGame()
    {
        Application.Quit();
        
        // Testing out if it works with the button
        //UnityEditor.EditorApplication.isPlaying = false;
    }

    // Returns to game scene if it exists
    public void ResumeGame()
    {
        if (PauseMenu.Root != null)
        {
            SceneManager.UnloadSceneAsync(0);
            root = PauseMenu.Root;
            root.SetActive(true);
            Time.timeScale = 1f;
        }
    }
}
