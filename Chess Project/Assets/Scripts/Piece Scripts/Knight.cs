/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/06/2021
 Knight.cs - child class of Piece.cs that implements move and attack using rules for the knight
 Version 1.1: Removed dependency on game manager for determining occupied spaces as it is already 
 handled in the game manager.*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Knight : Piece
{
    public override bool Attack(Piece enemy) {
        // Simulate dice roll
        int roll = DiceManager.Instance.RollDice();
        int mininumValue;

        // Assign minimum attack number needed based off of fuzzy logic table
        if (enemy is Pawn) {
            mininumValue = (int)FuzzyLogic.Knight.Pawn;
        } else if (enemy is Rook) {
            mininumValue = (int)FuzzyLogic.Knight.Rook;
        } else if (enemy is Knight) {
            mininumValue = (int)FuzzyLogic.Knight.Knight;
        } else if (enemy is Bishop) {
            mininumValue = (int)FuzzyLogic.Knight.Bishop;
        } else if (enemy is Queen) {
            mininumValue = (int)FuzzyLogic.Knight.Queen;
        } else {
            mininumValue = (int)FuzzyLogic.Knight.King;
        }

        if (roll >= mininumValue)
            return true;

        return false;
    }

    public override List<Vector2Int> LocationsAvailable() {
        List<Vector2Int> locations = new List<Vector2Int>();

        foreach (Vector2Int dir in this.directions) {
            Vector2Int nextTile = new Vector2Int(this.Position.x + dir.x, this.Position.y + dir.y);
            locations.Add(nextTile);
            locations = locations.Union(RecursiveLocations(nextTile, 3)).ToList();
        }

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
