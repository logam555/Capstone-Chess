/* Written by Tommy Oh
 Edited by ___
 Last date edited: 10/05/2021
 VolumeMixer.cs - Manages the volume mixer.
 Version 1: Able to change the volume of the game.
*/
using UnityEngine;
using UnityEngine.UI;


public class VolumeMixer : MonoBehaviour
{   
    public Slider volSlider;

    public  float soundVolume = 0.5f;
    
     void Start()
    {
        LoadValue();
    }
    // Allows the slider to change the volume of the audio
    public void ChangeVolume()
    {
        soundVolume = volSlider.value;
        PlayerPrefs.SetFloat("volume",soundVolume);
        LoadValue();
        
    }

    public void LoadValue()
    {
        soundVolume = PlayerPrefs.GetFloat("volume");
        volSlider.value = soundVolume;
        AudioListener.volume = soundVolume;
    }



}