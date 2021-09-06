using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
 

    public override List<Vector2Int> LocationsAvailable(Vector2Int gridPoint)
    {
        List<Vector2Int> locations = new List<Vector2Int>();

        List<Vector2Int> directions = new List<Vector2Int>(BishopDirections);
        directions.AddRange(RookDirections);
        foreach(Vector2Int dir in directions)
        {
            Vector2Int nextGridPoint = new Vector2Int(gridPoint.x + dir.x, gridPoint.y + dir.y);
            locations.Add(nextGridPoint);
            if (!GameManager.instance.PieceAtGrid(nextGridPoint))
            {
                FindLocations(directions, nextGridPoint, ref locations);
            }
        }
        return locations;
    }
    public void FindLocations(List<Vector2Int> Directions,Vector2Int gridPoint,ref List<Vector2Int> locations)
    {
        foreach(Vector2Int dir in Directions)
        {
            for (int i = 1; i < 2; i++)
            {
                Vector2Int nextGridPoint = new Vector2Int(gridPoint.x + i * dir.x, gridPoint.y + i * dir.y);
                if (GameManager.instance.PieceAtGrid(nextGridPoint))
                {
                    break;
                }
                locations.Add(nextGridPoint);
                
            }
        }
    }
}
