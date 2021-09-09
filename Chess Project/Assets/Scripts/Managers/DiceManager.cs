/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/07/2021
 DiceManager.cs - Manages the random number generation and movement of the dice object.

 Version 1.1: Function created to generate random number, calculate target rotation of the model based off
 of the number, and rotate the model over time for basic animation. Removed the raycasting input for moving the
 dice and simplified rotations.*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance { get; set; }
    public bool isRotating;

    private GameObject diceObject;
    private float rotationSpeed;
    private Quaternion targetRotation;

    private void Awake() {
        Instance = this;
        isRotating = false;

        diceObject = this.gameObject;
        rotationSpeed = 12.0f;
        targetRotation = Quaternion.identity;
    }

    private void Update() {
        if(isRotating) {
            RotateDice(targetRotation);
        }
    }

    public int RollDice() {
        int number =  Random.Range(1, 7);
        targetRotation = CalculateRotation(number);

        isRotating = true;
        return number;
    }

    private void RotateDice(Quaternion targetRotation) {
        
        if (Vector3.Distance(diceObject.transform.rotation.eulerAngles, targetRotation.eulerAngles) > 0.01f) {
            diceObject.transform.rotation = Quaternion.Lerp(diceObject.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        } else {
            diceObject.transform.rotation = targetRotation;
            isRotating = false;
        }
    }

    private Quaternion CalculateRotation(int number) {
        /* Rotation values for dice faces:
         * 1 = 270 0 0,
         * 2 = 0 0 0,
         * 3 = 0 0 270.
         * 4 = 0 0 90,
         * 5 = 180 0 0,
         * 6 = 90 0 0 */

        Quaternion targetRotation = Quaternion.identity;

        switch (number) {
            case 1:
                targetRotation = Quaternion.Euler(270, 0, 0);
                break;
            case 2:
                targetRotation = Quaternion.Euler(0, 0, 0);
                break;
            case 3:
                targetRotation = Quaternion.Euler(0, 0, 270);
                break;
            case 4:
                targetRotation = Quaternion.Euler(0, 0, 90);
                break;
            case 5:
                targetRotation = Quaternion.Euler(180, 0, 0);
                break;
            case 6:
                targetRotation = Quaternion.Euler(90, 0, 0);
                break;
        }

        return targetRotation;
    }
}
