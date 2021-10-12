/* Written by David Corredor
 Edited by Braden Stonehill, David Corredor
 Last date edited: 10/06/2021
 Bishop.cs - child class of Piece.cs that implements move and attack using rules for the bishop

 Version 1.5: 
  - Utilized the new board property for accessing the virtual board

  - Added method to delegate all subordinates to king if captured

  - Altered class definition for Bishop to inherit commander class rather than piece class

  - Removed the attack function as it no longer needs to be implemented by the child classes.

  - Implemented the attack function using the fuzzy logic table. Added the EnemiesInRange
 function to scan the immediate area for enemy pieces that can be attacked.

  - Removed dependency on game manager for determining occupied spaces as it is already 
 handled in the game manager. Added subordinates and usedCommand property as bishop is a commander
 that controls other pieces and has a command authority. Changed locationsAvailable function to include
 if command authority has been used.*/

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bishop : Commander
{
    public Commander superCommander = null;

    public override List<Vector2Int> LocationsAvailable() {
        List<Vector2Int> locations = new List<Vector2Int>();

        if (this.commandActions <= 0 && this.usedFreeMovement)
            return locations;

        foreach (Vector2Int dir in this.directions) {
            Vector2Int nextTile = new Vector2Int(this.Position.x + dir.x, this.Position.y + dir.y);
            locations.Add(nextTile);
            if(this.commandActions > 0)
                locations = locations.Union(RecursiveLocations(nextTile, 1)).ToList();
        }

        return locations;
    }

    public override List<Vector2Int> EnemiesInRange() {
        List<Vector2Int> enemyPos = new List<Vector2Int>();

        if (this.commandActions <= 0)
            return enemyPos;

        foreach (Vector2Int dir in this.directions) {
            Vector2Int position = this.Position + dir;

            if (!board.ValidPosition(position))
                continue;

            if (board.IsEnemyPieceAt(this.IsWhite, position))
                enemyPos.Add(position);
        }

        return enemyPos;
    }

    public void DelegatePieces() {
        foreach (Piece piece in this.subordinates) {
            superCommander.subordinates.Add(piece);
            piece.Commander = superCommander;
        }

        this.subordinates.Clear();
    }

}
