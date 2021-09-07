﻿/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/06/2021
 Queen.cs - child class of Piece.cs that implements move and attack using rules for the queen
 Version 1.1: Removed dependency on game manager for determining occupied spaces as it is already 
 handled in the game manager.*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Queen : Piece
{
    public override void Attack(Piece enemy, Vector2Int gridPoint) {
        throw new System.NotImplementedException();
    }

    public override List<Vector2Int> LocationsAvailable() {
        List<Vector2Int> locations = new List<Vector2Int>();

        foreach (Vector2Int dir in this.directions) {
            Vector2Int nextTile = new Vector2Int(this.Position.x + dir.x, this.Position.y + dir.y);
            locations.Add(nextTile);
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
