/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/06/2021
 King.cs - child class of Piece.cs that implements move and attack using rules for the King
 Version 1.1: Removed dependency on game manager for determining occupied spaces as it is already 
handled in the game manager. Added subordinates and usedCommand property as king is a commander
 that controls other pieces and has a command authority. Changed locationsAvailable function to include
 if command authority has been used.*/

using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public List<Piece> subordinates = new List<Piece>(6);
    public bool usedCommand = false;

    public override void Attack(Piece enemy, Vector2Int gridPoint) {
        throw new System.NotImplementedException();
    }

    public override List<Vector2Int> LocationsAvailable(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();
        int movement = this.usedCommand ? 1 : 3;

        foreach (Vector2Int dir in this.directions)
        {
            for (int i = 1; i <= movement; i++)
            {
                Vector2Int nextGridPoint = new Vector2Int(gridPoint.x + i * dir.x, gridPoint.y + i * dir.y);
                locations.Add(nextGridPoint);
            }
        }

        return locations;
    }
}
