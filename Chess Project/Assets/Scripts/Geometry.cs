
using UnityEngine;
public class Geometry
{
    static public Vector3 PointFromGrid(Vector2Int gridPoint,GameObject pieceActive)
    {
        float x = -3.5f + 1.0f * gridPoint.x;
        float z = -3.5f + 1.0f * gridPoint.y;
        if(pieceActive.GetComponent<Piece>().type == PieceType.Pawn)
        {
            return new Vector3(x, 12.3f, z);
        }
        else
        {
            return new Vector3(x, 11.92f, z);

        }
    }
    static public Vector3 PointFromGrid(Vector2Int gridPoint)
    {
        float x = -3.5f + 1.0f * gridPoint.x;
        float z = -3.5f + 1.0f * gridPoint.y;

        return new Vector3(x, 12.2f, z);
    }
    static public Vector2Int GridPoint(int col, int row)
    {
        return new Vector2Int(col, row);
    }

    static public Vector2Int GridFromPoint(Vector3 point)
    {
        
            int col = Mathf.FloorToInt(3.6f + point.x);
            int row = Mathf.FloorToInt(3.6f + point.z);
            return new Vector2Int(col, row);
        
    }
}
