/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/07/2021
 Player.cs - Object to contain player information such as captured pieces, identifier for player, and color of pieces under control

 Version 1.2:
  - Removed the pieces property and changed capturedPieces property to a dictionary of values as game objects of pieces are deleted.
 Added functionality for three actions per turn.
*/

using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public Dictionary<string, int> capturedPieces;
    public string name;
    public bool isWhite;
    public int remainingActions;

    public Player(string name, bool isWhite)
    {
        this.name = name;
        this.isWhite = isWhite;
        this.remainingActions = 3;

        this.capturedPieces = new Dictionary<string, int>();
        this.capturedPieces.Add("King", 0);
        this.capturedPieces.Add("Queen", 0);
        this.capturedPieces.Add("Bishop", 0);
        this.capturedPieces.Add("Knight", 0);
        this.capturedPieces.Add("Rook", 0);
        this.capturedPieces.Add("Pawn", 0);

    }
}
