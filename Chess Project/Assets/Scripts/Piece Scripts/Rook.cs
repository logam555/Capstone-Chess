/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/15/2021
 Rook.cs - child class of Piece.cs that implements move and attack using rules for the rook

 Version 1.3:
  - Removed the attack function as it no longer needs to be implemented by the child classes.

  - Implemented the Attack function using the fuzzy logic table and the EnemiesInRange function to 
 detect all attackable enemies up to two squares away (not including the enemy's square) nonlinearly.

  - Removed dependency on game manager for determining occupied spaces as it is already 
 handled in the game manager.*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rook : Piece
{

    public override List<Vector2Int> LocationsAvailable()
    {
        List<Vector2Int> locations = new List<Vector2Int>();

        if (this.Commander.commandActions <= 0)
            return locations;

        foreach (Vector2Int dir in this.directions) {
            Vector2Int nextTile = new Vector2Int(this.Position.x + dir.x, this.Position.y + dir.y);
            locations.Add(nextTile);
            locations = locations.Union(RecursiveLocations(nextTile, 1)).ToList();
        }

        return locations;
    }

    public override List<Vector2Int> EnemiesInRange() {
        List<Vector2Int> enemyPos = new List<Vector2Int>();
        List<Vector2Int> range = this.RecursiveLocations(this.Position, 3, true);

        if (this.Commander.commandActions <= 0)
            return enemyPos;

        foreach (Vector2Int position in range) {
            if (!GameManager.ValidPosition(position))
                continue;

            if (GameManager.Instance.IsEnemyPieceAt(this.IsWhite, position))
                enemyPos.Add(position);
        }

        return enemyPos;
    }
}
