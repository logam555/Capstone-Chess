
using System.Collections.Generic;
using UnityEngine;

public class Player:MonoBehaviour
{
    public List<GameObject> pieces;
    public List<GameObject> capturedPieces;
    public int forward;
    public bool isWhite { get; set; }
    public bool isPlaying;
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
