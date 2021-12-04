using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Knight : Subordinate {
    public bool Moved { get; set; }

    public Knight(bool isWhite, Vector2Int position, Commander commander, string name) : base(isWhite, position, commander, name) { 
        Moved = false;
    }

    public override List<Vector2Int> MoveRange(ChessPiece[,] board) {
        if (Commander.commandActions <= 0)
            return new List<Vector2Int>();
        return RecursiveMoveRange(Position, 4, board);
    }

    public override List<Vector2Int> AttackRange(ChessPiece[,] board) {
        if (Commander.commandActions > 0)
            return RecursiveMoveRange(Position, 1, board);
        if(Commander.commandActions <= 0 && Moved)
            return RecursiveMoveRange(Position, 1, board);
        return new List<Vector2Int>();
    }
}