/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 10/06/2021
 Knight.cs - child class of Piece.cs that implements move and attack using rules for the knight

 Version 1.3:
  - Utilized the new board property for accessing the virtual board

  - Removed the attack function as it no longer needs to be implemented by the child classes.

  - Implemented the attack function using the fuzzy logic table and the EnemiesInRange function to find all enemies within
 movement range.
 
  - Removed dependency on game manager for determining occupied spaces as it is already 
 handled in the game manager.*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Knight : Piece
{
  
    public override List<Vector2Int> LocationsAvailable() {
        List<Vector2Int> locations = new List<Vector2Int>();

        if (this.Commander.commandActions <= 0)
            return locations;

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

        if (this.Commander.commandActions <= 0)
            return enemyPos;

        availableMoves.RemoveAll(pos => !board.ValidPosition(pos));

        foreach(Vector2Int pos in availableMoves) {
            if (board.IsEnemyPieceAt(this.IsWhite, pos))
                enemyPos.Add(pos);
        }

        return enemyPos;
    }
}
