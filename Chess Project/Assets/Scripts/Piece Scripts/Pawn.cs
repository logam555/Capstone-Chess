/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/07/2021
 Pawn.cs - child class of Piece.cs that implements move and attack using rules for the pawn

 Version 1.1: 
  - Implemented the attack function to utilize the fuzzy logic table and the EnemiesInRange function
 to detect enemies immediately in front of the pawn that can be attacked.

  - Removed dependency on a game manager instance for calculating forward direction. 
 Removed dependency on game manager for determining occupied spaces as it is already handled in 
 the game manager.*/

using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece
{

    public override bool Attack(Piece enemy) {
        // Simulate dice roll
        int roll = DiceManager.Instance.RollDice();

        // Assign minimum attack number needed based off of fuzzy logic table
        int mininumValue = FuzzyLogic.FindNumberPawn(enemy);

        if (roll >= mininumValue)
            return true;

        return false;
    }

    public override List<Vector2Int> LocationsAvailable()
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        int forwardDirection = this.IsWhite ? 1 : -1;

        
        Vector2Int forwardOne = new Vector2Int(this.Position.x, this.Position.y + forwardDirection);
        locations.Add(forwardOne);

        Vector2Int forwardRight = new Vector2Int(this.Position.x + 1, this.Position.y + forwardDirection);
        locations.Add(forwardRight);
        
        Vector2Int forwardLeft = new Vector2Int(this.Position.x - 1, this.Position.y + forwardDirection);
        locations.Add(forwardLeft);
        
        return locations;
    }

    public override List<Vector2Int> EnemiesInRange() {
        List<Vector2Int> enemyPos = new List<Vector2Int>();
        List<Vector2Int> availableMoves = this.LocationsAvailable();

        availableMoves.RemoveAll(pos => pos.x < 0 || pos.x > 7 || pos.y < 0 || pos.y > 7);

        foreach(Vector2Int pos in availableMoves) {
            if (GameManager.Instance.EnemyPieceAt(this.IsWhite, pos))
                enemyPos.Add(pos);
        }

        return enemyPos;
    }
}
