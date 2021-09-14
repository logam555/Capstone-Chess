/* Written by Tommy Oh
Edited by ___
Last date edited: 09/07/2021
ResolutionChange.cs - Changes the resolution of the game.

Version 1: Makes the game change resolution with the buttons.*/
using UnityEngine;

using UnityEngine.UIElements;
public class ResolutionChange : MonoBehaviour
{

    //changes the screen resolution to 1080p
     public void FHD()
    {
      Screen.SetResolution(1920, 1080, true);
    }

    //changes the screen resolution to 720p
    public void HD()
    {
      Screen.SetResolution(1280, 720, true);

    }

    //changes the screen resolution to 480p
    public void SD()
    {
      Screen.SetResolution(640, 480, true);
    }
   
}
