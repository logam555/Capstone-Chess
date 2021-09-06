/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/06/2021
 Rook.cs - child class of Piece.cs that implements move and attack using rules for the rook
 Version 1.1: Removed dependency on game manager for determining occupied spaces as it is already 
 handled in the game manager.*/

using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{

    public override void Attack(Piece enemy, Vector2Int gridPoint) {
        throw new System.NotImplementedException();
    }

    public override List<Vector2Int> LocationsAvailable(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();

        foreach (Vector2Int dir in this.directions)
        {
            for (int i = 1; i <= 2; i++)
            {
                Vector2Int nextGridPoint = new Vector2Int(gridPoint.x + i * dir.x, gridPoint.y + i * dir.y);
                locations.Add(nextGridPoint);
            }
        }

        return locations;
    }
}
