/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/07/2021
 DiceManager.cs - Manages the random number generation and movement of the dice object.

 Version 1.1: Function created to generate random number, calculate target rotation of the model based off
 of the number, and rotate the model over time for basic animation. Removed the raycasting input for moving the
 dice and simplified rotations.
 Version 2: Modify the code to no longer create a random number, but to apply a random force, a random torque, and a random rotation
 to make the dice roll more realistic and random.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceManager : MonoBehaviour
{
    public static DiceManager Instance { get; set; }
    public static Vector3 diceVelocity;
    public int diceNumber;
    public bool hasLanded;
    public bool thrown = false;
    public DiceCheckZoneScript diceZoneCollider;
    public Rigidbody diceRb;
    private GameObject diceObject;
    private Transform diceTransform;

    private void Awake() {
        Instance = this;
        hasLanded = false;
        diceObject = this.gameObject;
        diceRb = this.gameObject.GetComponent<Rigidbody>();
        diceTransform = this.gameObject.GetComponent<Transform>();
    }

    private void Update() {
        
    }

    public void RollDice() {
        thrown = true;
        diceVelocity = diceRb.velocity;
        //Create a random direction for the torque
        float dirTorqueX = Random.Range(0, 500);
        float dirTorqueY = Random.Range(0, 500);
        float dirTorqueZ = Random.Range(0, 500);
        float speed = 60;
        //Find the center position of the dice zone collider
        Vector3 centerPostion = diceZoneCollider.Instance.GetComponent<Renderer>().bounds.center;
        //Create a random force 
        Vector3 force = transform.forward;
        force = new Vector3(force.x, 1, force.z);
        //Rotate the dice 
        diceTransform.rotation = Random.rotation;
        //Spawn the dice in the center of the dice zone
        diceTransform.position = new Vector3(centerPostion.x, centerPostion.y + 2f, centerPostion.z);
        //Apply the random force and torque
        diceRb.AddForce(force * speed);
        diceRb.AddTorque(dirTorqueX, dirTorqueY, dirTorqueZ);

    }
   
}
