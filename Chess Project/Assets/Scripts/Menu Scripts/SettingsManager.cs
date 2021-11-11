/* Written by Tommy Oh
Rewritten by Logan Keck
Last date edited: 11/4/2021
ResolutionChange.cs - Changes the resolution of the game.

Version 1: Makes the game change resolution with the buttons.
Version 1.1: Added a full screen toggle.
Version 1.2: Replaced the buttons for resolution with a dropdown.
Version 2.0: Complete overhaul of the file from name, comments, functions, and efficiency.*/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    // Reference to the volume silder object
    public Slider volumeSlider;

    // Keeps track of all resolution dimensions for different processes
    List<int> widthList = new List<int>();
    List<int> heightList = new List<int>();

    // Reference to the three dropdowns
    public Dropdown resolutionDropdown;
    public Dropdown graphicsDropdown;
    public Dropdown screenModeDropdown;

    void Start()
    {
        // Checks if there was a previous volume value used, else sets silder to default volume of 100%
        if (PlayerPrefs.HasKey("volume"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("volume");
        }
        else
            volumeSlider.value = AudioListener.volume;

        // Checks if there was a previous graphics value used, else gets current quality setting for dropdown
        if (PlayerPrefs.HasKey("Quality"))
            graphicsDropdown.value = PlayerPrefs.GetInt("Quality");
        else
            graphicsDropdown.value = QualitySettings.GetQualityLevel();
        graphicsDropdown.RefreshShownValue();

        // Checks if there was a previous fullscreen mode used, else defaults seting the dropdown to Windowed Fullscreen
        if (PlayerPrefs.HasKey("SelectedFullscreenMode"))
        {
            switch (PlayerPrefs.GetInt("SelectedFullscreenMode"))
            {
                case 0:
                    screenModeDropdown.value = 0;
                    break;
                case 1:
                    screenModeDropdown.value = 1;
                    break;
                case 2:
                    screenModeDropdown.value = 2;
                    break;
                case 3:
                    screenModeDropdown.value = 3;
                    break;
                default:
                    break;
            }
        }
        else
            screenModeDropdown.value = 1;
        screenModeDropdown.RefreshShownValue();

        // Gets all monitor approved resolutions
        Resolution[] resolutions;
        resolutions = Screen.resolutions;

        // Clear all resolution dropdown options
        resolutionDropdown.ClearOptions();

        // Variable to hold name of current resolution
        string currentRes = "";
        // Varible holding all different resolution options
        List<string> resOptions = new List<string>();
        // Creates all the options for the dropdown
        for (int i = 0; i < resolutions.Length; i++)
        {
            // Option format
            string options = resolutions[i].width + " x " + resolutions[i].height;
            
            // Adds only new and unique resolution options
            if (!resOptions.Contains(options))
            {
                resOptions.Add(options);
                widthList.Add(resolutions[i].width);
                heightList.Add(resolutions[i].height);
            }
            
            // Sets name of the current resolution for later display
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                currentRes = options;
        }

        // Adds all the options to the dropdown, sets displayed value to current resolution, and refreshs the value being displayed
        resolutionDropdown.AddOptions(resOptions);
        resolutionDropdown.value = resOptions.IndexOf(currentRes);
        resolutionDropdown.RefreshShownValue();
    }

    // Sets the game resolution to the new resolution
    public void SetResolution(int dropDownIndex)
    {
        Screen.SetResolution(widthList[dropDownIndex], heightList[dropDownIndex], Screen.fullScreenMode);
    }
    // Sets the screen mode to the new desired mode (fullscreen, windowed, etc) and saves the value to be the new default and trys to set refresh rate to 144 or max if entering exclusive fullscreen
    public void SetScreenModeGraphics(int dropDownIndex)
    {
        switch (dropDownIndex)
        {
            case 0:
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.ExclusiveFullScreen, 144);
                PlayerPrefs.SetInt("SelectedFullscreenMode", 0);
                return;
            case 1:
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.FullScreenWindow, 0);
                PlayerPrefs.SetInt("SelectedFullscreenMode", 1);
                return;
            case 2:
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.MaximizedWindow, 0);
                PlayerPrefs.SetInt("SelectedFullscreenMode", 2);
                return;
            case 3:
                Screen.SetResolution(Screen.width, Screen.height, FullScreenMode.Windowed, 0);
                PlayerPrefs.SetInt("SelectedFullscreenMode", 3);
                return;
            default:
                return;
        }
    }
    // Sets the graphics setting to the new setting and saves the value to be the new default
    public void SetScreenGraphics(int dropDownIndex)
    {
        QualitySettings.SetQualityLevel(dropDownIndex, true);
        PlayerPrefs.SetInt("Quality", dropDownIndex);
    }
    // Sets the volume setting to the new setting and saves the value to be the new default
    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("volume", volume);
    }
}
