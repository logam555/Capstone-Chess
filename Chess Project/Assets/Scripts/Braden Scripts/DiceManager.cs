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
        diceObject = this.gameObject;
        isRotating = false;
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
        isRotating = true;
        targetRotation = CalculateRotation(number);
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
