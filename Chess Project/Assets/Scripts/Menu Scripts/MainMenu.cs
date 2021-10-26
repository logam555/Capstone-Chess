/* Written by Tommy Oh
 Edited by ___
 Last date edited: 09/07/2021
 MainMenu.cs - Manages the main menu start and exit buttons.

 Version 1: Created functions for start and exit buttons.*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    //Starts the Game On easy
    public void StartGameEasy()
    {
        PlayerPrefs.SetInt("Difficulty", 1);
        SceneManager.LoadScene(1);
    }

    //Starts the Game on Normal
    public void StartGameNorm()
    {
        PlayerPrefs.SetInt("Difficulty", 2);
        SceneManager.LoadScene(1);
    }

    //Starts the Game on Hard
    public void StartGameHard()
    {
        PlayerPrefs.SetInt("Difficulty", 3);
        SceneManager.LoadScene(1);
    }

    // Exits the Game
    public void ExitGame()
    {
        Application.Quit();
        
        // Testing out if it works with the button
        //UnityEditor.EditorApplication.isPlaying = false;
    }


}
