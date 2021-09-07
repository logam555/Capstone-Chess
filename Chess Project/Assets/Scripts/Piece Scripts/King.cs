﻿/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/06/2021
 King.cs - child class of Piece.cs that implements move and attack using rules for the King
 Version 1.1: Removed dependency on game manager for determining occupied spaces as it is already 
handled in the game manager. Added subordinates and usedCommand property as king is a commander
 that controls other pieces and has a command authority. Changed locationsAvailable function to include
 if command authority has been used.*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class King : Piece
{
    public List<Piece> subordinates = new List<Piece>();
    public bool usedCommand = false;

    public override bool Attack(Piece enemy) {
        // Simulate dice roll
        int roll = DiceManager.Instance.RollDice();
        int mininumValue;

        // Assign minimum attack number needed based off of fuzzy logic table
        if (enemy is Pawn) {
            mininumValue = (int)FuzzyLogic.King.Pawn;
        } else if (enemy is Rook) {
            mininumValue = (int)FuzzyLogic.King.Rook;
        } else if (enemy is Knight) {
            mininumValue = (int)FuzzyLogic.King.Knight;
        } else if (enemy is Bishop) {
            mininumValue = (int)FuzzyLogic.King.Bishop;
        } else if (enemy is Queen) {
            mininumValue = (int)FuzzyLogic.King.Queen;
        } else {
            mininumValue = (int)FuzzyLogic.King.King;
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
            if(!this.usedCommand)
                locations = locations.Union(RecursiveLocations(nextTile, 2)).ToList();
        }

        return locations;
    }

    public override List<Vector2Int> EnemiesInRange() {
        List<Vector2Int> enemyPos = new List<Vector2Int>();

        foreach (Vector2Int dir in this.directions) {
            Vector2Int position = this.Position + dir;

            if (position.x < 0 || position.x > 7 || position.y < 0 || position.y > 7)
                continue;

            if (GameManager.Instance.EnemyPieceAt(this.IsWhite, position))
                enemyPos.Add(position);
        }

        return enemyPos;
    }

}
