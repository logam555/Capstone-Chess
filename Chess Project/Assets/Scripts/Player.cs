/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 10/04/2021
 Player.cs - Object to contain player information such as captured pieces, identifier for player, and color of pieces under control

 Version 1.3:
  - Changed remaining actions to list of commanders and added functions to reset commanders move when resetting a players turn
 and function to calculate the number of actions remaining based on the number of actions available for each commander

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
    public List<Commander> commanders;

    public Player(string name, bool isWhite, List<Commander> commanders)
    {
        this.name = name;
        this.isWhite = isWhite;
        this.commanders = commanders;

        capturedPieces = new Dictionary<string, int>();
        capturedPieces.Add("King", 0);
        capturedPieces.Add("Queen", 0);
        capturedPieces.Add("Bishop", 0);
        capturedPieces.Add("Knight", 0);
        capturedPieces.Add("Rook", 0);
        capturedPieces.Add("Pawn", 0);
    }

    public int TotalActionsRemaining() {
        int actions = 0;

        foreach(Commander commander in commanders) {
            actions += commander.commandActions;
        }

        return actions;
    }

    public void ResetTurn() {
        foreach (Commander commander in commanders) {
            commander.commandActions = 1;
            commander.usedFreeMovement = false;

            if(commander is King) {
                King king = (King)commander;
                king.usedDelegation = false;
            }
        }
    }

    public bool UsedAllFreeMovements() {
        foreach (Commander commander in commanders) {
            if (!commander.usedFreeMovement)
                return false;
        }

        return true;
    }
}
