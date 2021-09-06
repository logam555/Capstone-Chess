using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    public GameObject DiceObject;
    public int number;
    public GameObject gameManager;
    public Camera activeCamera;
    public Vector3 previousState;
    // Start is called before the first frame update
    void Start()
    {
        activeCamera = gameManager.GetComponent<GameManager>().activeCamera.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        activeCamera = gameManager.GetComponent<GameManager>().activeCamera.GetComponent<Camera>();
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.tag == "Dice")
            {
                if (Input.GetMouseButtonDown(0))
                {
                    number = RollDice();
                    // 2 = 0 0 0
                    // 6 = 90 0 0
                    // 5 = 180 0 0
                    // 1 = 270 0 0
                    // 4 = 0 0 90
                    // 3 = 0 0 270
                    switch (number)
                    {
                        case 1:
                            previousState = DiceObject.GetComponent<Transform>().localEulerAngles;
                            previousState.x = previousState.x * -1;
                            previousState.y = previousState.y * -1;
                            previousState.z = previousState.z * -1;
                            DiceObject.GetComponent<Transform>().Rotate(previousState.x, previousState.y, previousState.z);
                            DiceObject.GetComponent<Transform>().Rotate(270.0f, 0.0f, 0.0f);
                            break;
                        case 2:
                            previousState = DiceObject.GetComponent<Transform>().localEulerAngles;
                            previousState.x = previousState.x * -1;
                            previousState.y = previousState.y * -1;
                            previousState.z = previousState.z * -1;
                            DiceObject.GetComponent<Transform>().Rotate(previousState.x, previousState.y, previousState.z);
                            DiceObject.GetComponent<Transform>().Rotate(0.0f, 0.0f, 0.0f);
                            break;
                        case 3:
                            previousState = DiceObject.GetComponent<Transform>().localEulerAngles;
                            previousState.x = previousState.x * -1;
                            previousState.y = previousState.y * -1;
                            previousState.z = previousState.z * -1;
                            DiceObject.GetComponent<Transform>().Rotate(previousState.x, previousState.y, previousState.z);
                            DiceObject.GetComponent<Transform>().Rotate(0.0f, 0.0f, 270.0f);
                            break;
                        case 4:
                            previousState = DiceObject.GetComponent<Transform>().localEulerAngles;
                            previousState.x = previousState.x * -1;
                            previousState.y = previousState.y * -1;
                            previousState.z = previousState.z * -1;
                            DiceObject.GetComponent<Transform>().Rotate(previousState.x, previousState.y, previousState.z);
                            DiceObject.GetComponent<Transform>().Rotate(0.0f, 0.0f, 90.0f);
                            break;
                        case 5:
                            previousState = DiceObject.GetComponent<Transform>().localEulerAngles;
                            previousState.x = previousState.x * -1;
                            previousState.y = previousState.y * -1;
                            previousState.z = previousState.z * -1;
                            DiceObject.GetComponent<Transform>().Rotate(previousState.x, previousState.y, previousState.z);
                            DiceObject.GetComponent<Transform>().Rotate(180.0f, 0.0f, 0.0f);
                            break;
                        case 6:
                            previousState = DiceObject.GetComponent<Transform>().localEulerAngles;
                            previousState.x = previousState.x * -1;
                            previousState.y = previousState.y * -1;
                            previousState.z = previousState.z * -1;
                            DiceObject.GetComponent<Transform>().Rotate(previousState.x, previousState.y, previousState.z);
                            DiceObject.GetComponent<Transform>().Rotate(90.0f, 0.0f, 0.0f);
                            break;

                    }
                }
            }
            

        }
            
    }
    private int RollDice()
    {
        return Random.Range(1, 7);
    }
}
