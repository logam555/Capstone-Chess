/* Written by Tommy Oh
 Edited by ___
 Last date edited: 10/10/2021
 VolumeMixer.cs - Manages the score and turn order.
 Version 1: The text changes accordingly to the score and turn.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText1;
    public Text scoreText2;
    public Text turnOrder;
    public static int scoreValue1;
    public static int scoreValue2;
    public static string turn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText1.text = "P1 Score: " + scoreValue1;
        scoreText2.text = "P2 Score: " + scoreValue2;
        turnOrder.text = "Turn: " + turn;

    }
}
