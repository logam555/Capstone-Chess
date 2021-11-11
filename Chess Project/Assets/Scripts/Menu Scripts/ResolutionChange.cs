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
    Resolution[] resolutions;

    public Dropdown resolutionDropdown;

    void Start()
    {      
      if(!Screen.fullScreen)
      {
        buttonText.text = "OFF";
      }
      else if (Screen.fullScreen)
      {
        buttonText.text = "ON";
      }

       resolutions = Screen.resolutions;

       resolutionDropdown.ClearOptions();
      
       int currentRes = 0;
       List<string> resOptions = new List<string>();
        // adding the resolutions in the a dropdown options
        for (int i = 0; i < resolutions.Length; i++)
        {
          string options = resolutions[i].width + " x " + resolutions[i].height + " : " + resolutions[i].refreshRate + " Hz";
          resOptions.Add(options);
 
          //setting the current resolutions
          if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height && resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
          {
            currentRes = i;
          }
        }
          resolutionDropdown.AddOptions(resOptions);
          resolutionDropdown.value = currentRes;
          resolutionDropdown.RefreshShownValue();
    }

    //changes the screen resoultion from the dropdown
    public void SetResolution(int resolutionIndex)
    {
       Resolution resolution = resolutions[resolutionIndex];
       Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }
    //changes the screen resolution to 1080p
     public void FHD()
    {
      Screen.SetResolution(1920, 1080,Screen.fullScreen);
    }

    //changes the screen resolution to 720p
    public void HD()
    {
      Screen.SetResolution(1280, 720,Screen.fullScreen);

    }

    //changes the screen resolution to 480p
    public void SD()
    {
      Screen.SetResolution(640, 480,Screen.fullScreen);
    }

    //Toggle full screen on and off
    public void FSO()
    {
      Screen.fullScreen = !Screen.fullScreen;
      if(Screen.fullScreen)
      {
        buttonText.text = "OFF";
      }
      else if (!Screen.fullScreen)
      {
        buttonText.text = "ON";
      }
    }


   
}
