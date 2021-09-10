﻿
using System.Collections.Generic;
using UnityEngine;

public class Player:MonoBehaviour
{
    public List<GameObject> pieces;
    public List<GameObject> capturedPieces;

    public string name;
    public int forward;
    public int numberOfTurns;

    public Player(string name, bool positiveZMovement)
    {
        this.name = name;
        pieces = new List<GameObject>();
        capturedPieces = new List<GameObject>();

        if (positiveZMovement == true)
        {
            this.forward = 1;
        }
        else
        {
            this.forward = -1;
        }
    }
}
