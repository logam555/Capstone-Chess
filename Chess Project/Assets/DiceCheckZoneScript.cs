using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceCheckZoneScript : MonoBehaviour
{
    Vector3 diceVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        diceVelocity = DiceManager.diceVelocity;
    }
    private void OnTriggerStay(Collider other)
    {
        if (DiceManager.Instance.thrown) {
            if (diceVelocity.x == 0f && diceVelocity.y == 0f && diceVelocity.z == 0f)
            {
                switch (other.gameObject.name)
                {
                    case "Side1":
                        DiceManager.Instance.diceNumber = 1;
                        DiceManager.Instance.hasLanded = true;
                        break;
                    case "Side2":
                        DiceManager.Instance.diceNumber = 2;
                        DiceManager.Instance.hasLanded = true;
                        break;
                    case "Side3":
                        DiceManager.Instance.diceNumber = 3;
                        DiceManager.Instance.hasLanded = true;
                        break;
                    case "Side4":
                        DiceManager.Instance.diceNumber = 4;
                        DiceManager.Instance.hasLanded = true;
                        break;
                    case "Side5":
                        DiceManager.Instance.diceNumber = 5;
                        DiceManager.Instance.hasLanded = true;
                        break;
                    case "Side6":
                        DiceManager.Instance.diceNumber = 6;
                        DiceManager.Instance.hasLanded = true;
                        break;
                }
            }
        }
    }
}
