/* Written by Tommy Oh
Edited by ___
Last date edited: 10/05/2021
ResolutionChange.cs - Changes the resolution of the game.

Version 1: Makes the game change resolution with the buttons.
Version 1.1: Added a full screen toggle*/
using UnityEngine;

using UnityEngine.UI;
public class ResolutionChange : MonoBehaviour
{
    public Text buttonText;

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
