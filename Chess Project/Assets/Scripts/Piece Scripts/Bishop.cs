/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/07/2021
 Bishop.cs - child class of Piece.cs that implements move and attack using rules for the bishop

 Version 1.2: 
  - Implemented the attack function using the fuzzy logic table. Added the EnemiesInRange
 function to scan the immediate area for enemy pieces that can be attacked.

  - Removed dependency on game manager for determining occupied spaces as it is already 
 handled in the game manager. Added subordinates and usedCommand property as bishop is a commander
 that controls other pieces and has a command authority. Changed locationsAvailable function to include
 if command authority has been used.*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bishop : Piece
{
    public List<Piece> subordinates = new List<Piece>(6);
    public bool usedCommand = false;

    public override bool Attack(Piece enemy, bool isMoving = false) {
        // Simulate dice roll
        int roll = DiceManager.Instance.RollDice();

        // Assign minimum attack number needed based off of fuzzy logic table
        int mininumValue = FuzzyLogic.FindNumberBishop(enemy);

        if (roll >= mininumValue)
            return true;

        return false;
    }

    public override List<Vector2Int> LocationsAvailable() {
        List<Vector2Int> locations = new List<Vector2Int>();

        foreach (Vector2Int dir in this.directions) {
            Vector2Int nextTile = new Vector2Int(this.Position.x + dir.x, this.Position.y + dir.y);
            locations.Add(nextTile);
            if(!this.usedCommand)
                locations = locations.Union(RecursiveLocations(nextTile, 1)).ToList();
        }

        return locations;
    }

    public override List<Vector2Int> EnemiesInRange() {
        List<Vector2Int> enemyPos = new List<Vector2Int>();

        foreach(Vector2Int dir in this.directions) {
            Vector2Int position = this.Position + dir;

            if (position.x < 0 || position.x > 7 || position.y < 0 || position.y > 7)
                continue;

            if (GameManager.Instance.IsEnemyPieceAt(this.IsWhite, position))
                enemyPos.Add(position);
        }

        return enemyPos;
    }

}
