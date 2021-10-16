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
    public static Vector3 diceVelocity;
    public int diceNumber;
    public bool hasLanded;
    public bool thrown = false;
    public GameObject diceZoneCollider;
    private GameObject diceObject;
    public Rigidbody diceRb;
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

        float dirX = Random.Range(0, 500);
        float dirY = Random.Range(0, 500);
        float dirZ = Random.Range(0, 500);
        diceTransform.position = new Vector3(diceZoneCollider.GetComponent<Transform>().position.x, diceZoneCollider.GetComponent<Transform>().position.y + 5f, diceZoneCollider.GetComponent<Transform>().position.z);
        diceRb.rotation = Random.rotation;
        diceRb.AddTorque(dirX, dirY, dirZ);

    }
   
}
