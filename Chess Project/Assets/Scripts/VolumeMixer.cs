/* Written by Tommy Oh
 Edited by ___
 Last date edited: 09/07/2021
 PauseMenu.cs - Manages the volume mixer.
Version 1: Able to change the volume of the game.*/
using UnityEngine;
using UnityEngine.UIElements;

public class VolumeMixer : MonoBehaviour
{
    public Slider volumeSlider; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
    }
}
