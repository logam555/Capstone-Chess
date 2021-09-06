using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum PieceType { King, Queen, Bishop, Knight, Rook, Pawn };
public abstract class Piece : MonoBehaviour
{
    public PieceType type;
    public bool isWhite;
    protected Vector2Int[] RookDirections = {new Vector2Int(0,1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};
    protected Vector2Int[] BishopDirections = {new Vector2Int(1,1), new Vector2Int(1, -1),
        new Vector2Int(-1, -1), new Vector2Int(-1, 1)};

    public abstract List<Vector2Int> LocationsAvailable(Vector2Int gridPoint);
}
