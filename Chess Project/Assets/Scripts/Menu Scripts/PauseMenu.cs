/* Written by Tommy Oh
Edited by ___
Last date edited: 09/14/2021
PauseMenu.cs - Creates the pause menu with buttons.

Version 1: Makes the pause menu
Verison 1.1: Fixed the bug with pause menu*/
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isGamePaused = false;
    public GameObject pauseMenuUI;

    [SerializeField]
    private GameObject root;

    public static GameObject Root { get; private set; }

    void Awake()
    {
        PauseMenu.Root = root;
    }


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
        SceneManager.LoadScene(0, LoadSceneMode.Additive);
        pauseMenuUI.SetActive(false);
        isGamePaused = false;
    }

    public void  Quit()
    {
        Application.Quit();
    }
}
