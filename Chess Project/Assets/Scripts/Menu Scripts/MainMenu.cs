using System.Collections;
using System.Collections.Generic;
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
