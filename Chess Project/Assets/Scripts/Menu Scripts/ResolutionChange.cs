/* Written by Tommy Oh
Edited by ___
Last date edited: 10/16/2021
ResolutionChange.cs - Changes the resolution of the game.

Version 1: Makes the game change resolution with the buttons.
Version 1.1: Added a full screen toggle.
Version 1.2: Replaced the buttons for resolution with a dropdown.*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionChange : MonoBehaviour
{
    public Text buttonText;
    
    List<int> widthList = new List<int>();
    List<int> heightList = new List<int>();
    List<int> rrList = new List<int>();

    public Dropdown resolutionDropdown;
    public Dropdown refreshRateDropdown;

    void Start()
    {
        if (!Screen.fullScreen)
        {
            buttonText.text = "OFF";
        }
        else if (Screen.fullScreen)
        {
            buttonText.text = "ON";
        }

        Resolution[] resolutions;

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        string currentRes = "";
        int currentRR = 0;
        List<string> resOptions = new List<string>();
        List<string> rrOptions = new List<string>();
        // adding the resolutions in the a dropdown options
        for (int i = 0; i < resolutions.Length; i++)
        {
            string options = resolutions[i].width + " x " + resolutions[i].height;
            if (!resOptions.Contains(options))
            {
                resOptions.Add(options);
                widthList.Add(resolutions[i].width);
                heightList.Add(resolutions[i].height);
            }
            if (!rrList.Contains(resolutions[i].refreshRate))
            {
                rrList.Add(resolutions[i].refreshRate);
            }
            
            //setting the current resolutions
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentRes = options;
            }
            if (resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentRR = resolutions[i].refreshRate;
            }
        }
        resolutionDropdown.AddOptions(resOptions);
        resolutionDropdown.value = resOptions.IndexOf(currentRes);
        resolutionDropdown.RefreshShownValue();

        rrList.Sort();
        foreach (int test in rrList)
        {
            rrOptions.Add(test + " Hz");
        }

        refreshRateDropdown.AddOptions(rrOptions);
        refreshRateDropdown.value = rrList.IndexOf(currentRR);
        refreshRateDropdown.RefreshShownValue();
    }

    //changes the screen resoultion from the dropdown
    public void SetResolution(int dropDownIndex)
    {
        //Resolution resolution = resolutions[dropDownIndex];
        Screen.SetResolution(widthList[dropDownIndex], heightList[dropDownIndex], Screen.fullScreenMode);
    }
    //changes the screen refresh rate from the dropdown
    public void SetRefreshRate(int dropDownIndex)
    {
        //Resolution resolution = resolutions[dropDownIndex];
        Screen.SetResolution(Screen.width, Screen.height, Screen.fullScreenMode, rrList[dropDownIndex]);
    }
    //changes the screen resolution to 1080p
    public void FHD()
    {
        Screen.SetResolution(1920, 1080, Screen.fullScreen);
    }

    //changes the screen resolution to 720p
    public void HD()
    {
        Screen.SetResolution(1280, 720, Screen.fullScreen);

    }

    //changes the screen resolution to 480p
    public void SD()
    {
        Screen.SetResolution(640, 480, Screen.fullScreen);
    }

    //Toggle full screen on and off
    public void FSO()
    {
        Screen.fullScreen = !Screen.fullScreen;
        if (Screen.fullScreen)
        {
            buttonText.text = "OFF";
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else if (!Screen.fullScreen)
        {
            buttonText.text = "ON";
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
    }



}
