/* Written by Logan Keck
 Edited by Braden Stonehill
 Last date edited: 09/07/2021
 CameraManager.cs - Manages the orientation, position, and perspective of the camera throughout the game
 based on user inputs.

 Version 1.1: Edited the script to remove the reliance on three cameras and instead shift the main
 camera to anchor positions in the scene.*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Values are not instantiated, they are filled in the editor.
    public GameObject eagleEyeAnchor;
    public GameObject angledAnchor;
    public GameObject headOnAnchor;

    // Start is called before the first frame update
    void Start()
    {
        Camera.main.transform.position = angledAnchor.transform.position;
        Camera.main.transform.rotation = angledAnchor.transform.rotation;
        Camera.main.orthographic = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            Camera.main.transform.position = eagleEyeAnchor.transform.position;
            Camera.main.transform.rotation = eagleEyeAnchor.transform.rotation;
            Camera.main.orthographic = true;
        } else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Camera.main.transform.position = angledAnchor.transform.position;
            Camera.main.transform.rotation = angledAnchor.transform.rotation;
            Camera.main.orthographic = false;
        } else if(Input.GetKeyDown(KeyCode.Alpha3)) {
            Camera.main.transform.position = headOnAnchor.transform.position;
            Camera.main.transform.rotation = headOnAnchor.transform.rotation;
            Camera.main.orthographic = false;
        }
    }
}
