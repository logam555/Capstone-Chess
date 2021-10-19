/*Written by David Corredor 
 * Last Date edited:10/18/2021
 * DiceCheckZoneScrip.cs
 * 
 * Version 1: Allow the zone to detect when the dice
 * is not moving, and capture which side it landed on.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZoneScript : MonoBehaviour
{
    Vector3 diceVelocity;
    public DiceCheckZoneScript Instance;
    public Transform zoneTransform;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        zoneTransform = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        diceVelocity = DiceManager.diceVelocity;
    }
    private void OnTriggerStay(Collider other)
    {
        //If the dice has been thrown start detecting if the dice is no longer moving
        if (DiceManager.Instance.thrown) {
            //If the dice does not have a velocity it means it has landed, but could mean
            //that still has to bounce
            if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
            {
                //If the dice has stop, and has no velocity. Check if the dice is no longer moving
                //at all.
                if (DiceManager.Instance.diceRb.IsSleeping())
                {
                    switch (other.gameObject.name)
                    {
                        case "Side1":
                            DiceManager.Instance.diceNumber = 1;
                            DiceManager.Instance.hasLanded = true;
                            DiceManager.Instance.thrown = false;
                            break;
                        case "Side2":
                            DiceManager.Instance.diceNumber = 2;
                            DiceManager.Instance.hasLanded = true;
                            DiceManager.Instance.thrown = false;
                            break;
                        case "Side3":
                            DiceManager.Instance.diceNumber = 3;
                            DiceManager.Instance.hasLanded = true;
                            DiceManager.Instance.thrown = false;
                            break;
                        case "Side4":
                            DiceManager.Instance.diceNumber = 4;
                            DiceManager.Instance.hasLanded = true;
                            DiceManager.Instance.thrown = false;
                            break;
                        case "Side5":
                            DiceManager.Instance.diceNumber = 5;
                            DiceManager.Instance.hasLanded = true;
                            DiceManager.Instance.thrown = false;
                            break;
                        case "Side6":
                            DiceManager.Instance.diceNumber = 6;
                            DiceManager.Instance.hasLanded = true;
                            DiceManager.Instance.thrown = false;
                            break;
                    }
                }
            }
        }
    }
}
