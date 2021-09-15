/* Written by David Corredor
 Edited by Braden Stonehill, David Corredor
 Last date edited: 09/15/2021
 King.cs - child class of Piece.cs that implements move and attack using rules for the King

 Version 1.4: 
  - Edited class definition to have king inherit commander class rather than piece class.

  - Removed the attack function as it no longer needs to be implemented by the child classes.

  - Implemented the attack function using the Fuzzy Logic table and implemented the EnemiesInRange function
 to find enemies in the immediate area that can be attacked.

  - Removed dependency on game manager for determining occupied spaces as it is already 
 handled in the game manager. Added subordinates and usedCommand property as king is a commander
 that controls other pieces and has a command authority. Changed locationsAvailable function to include
 if command authority has been used.*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class King : Commander
{
    public Commander leftBishop = null;
    public Commander rightBishop = null;

    public override List<Vector2Int> LocationsAvailable() {
        List<Vector2Int> locations = new List<Vector2Int>();

        if (this.commandActions <= 0 && this.usedFreeMovement)
            return locations;

        foreach (Vector2Int dir in this.directions) {
            Vector2Int nextTile = new Vector2Int(this.Position.x + dir.x, this.Position.y + dir.y);
            locations.Add(nextTile);
            if(this.commandActions > 0)
                locations = locations.Union(RecursiveLocations(nextTile, 2)).ToList();
        }

        return locations;
    }

    public override List<Vector2Int> EnemiesInRange() {
        List<Vector2Int> enemyPos = new List<Vector2Int>();

        if (this.commandActions <= 0)
            return enemyPos;

        foreach (Vector2Int dir in this.directions) {
            Vector2Int position = this.Position + dir;

            if (!GameManager.ValidPosition(position))
                continue;

            if (GameManager.Instance.IsEnemyPieceAt(this.IsWhite, position))
                enemyPos.Add(position);
        }

        return enemyPos;
    }

}
