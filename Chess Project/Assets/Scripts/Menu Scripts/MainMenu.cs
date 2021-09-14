/* Written by Tommy Oh
 Edited by ___
 Last date edited: 09/07/2021
 MainMenu.cs - Manages the main menu start and exit buttons.

 Version 1: Created functions for start and exit buttons.*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    //Starts the Game
    public void StartGame()
    {
        SceneManager.LoadScene("Braden-Test");
    }

    // Exits the Game
      public void ExitGame()
    {
        //Application.Quit();
        
        // Testing out if it works with the button
        UnityEditor.EditorApplication.isPlaying = false;
    }


}
