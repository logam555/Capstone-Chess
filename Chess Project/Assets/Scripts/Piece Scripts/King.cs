/* Written by David Corredor
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
    public List<Piece> subordinates = new List<Piece>(6);
    public bool usedCommand = false;

    public override void Attack(Piece enemy, Vector2Int gridPoint) {
        throw new System.NotImplementedException();
    }

    public override List<Vector2Int> LocationsAvailable() {
        List<Vector2Int> locations = new List<Vector2Int>();

        foreach (Vector2Int dir in this.directions) {
            Vector2Int nextTile = new Vector2Int(this.Position.x + dir.x, this.Position.y + dir.y);
            locations.Add(nextTile);
            if(!this.usedCommand)
                locations = locations.Union(RecursiveLocations(nextTile, 2)).ToList();
        }

        locations.Remove(this.Position);
        return locations;
    }

}
