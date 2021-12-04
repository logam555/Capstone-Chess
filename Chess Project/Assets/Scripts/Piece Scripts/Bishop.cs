using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bishop : Commander {
    public Commander King { get; set; }

    public Bishop(bool isWhite, Vector2Int position, string name, Commander king) : base(isWhite, position, name) {
        King = king;
    }

    public override List<Vector2Int> MoveRange(ChessPiece[,] board) {
        if (commandActions <= 0 && usedFreeMovement)
            return new List<Vector2Int>();
        if (commandActions <= 0 && !usedFreeMovement)
            return RecursiveMoveRange(Position, 1, board);
        if (usedFreeMovement)
            return RecursiveMoveRange(Position, 1, board);
        return RecursiveMoveRange(Position, 2, board);
    }

    public override List<Vector2Int> AttackRange(ChessPiece[,] board) {
        if (commandActions <= 0)
            return new List<Vector2Int>();
        return RecursiveMoveRange(Position, 1, board);
    }

    public void DelegatePieces() {
        Commander kingKing = (Commander)ChessBoard.Instance.PieceAt(King.Position, ChessBoard.Instance.KingBoard);
        Commander lbishopKing = (Commander)ChessBoard.Instance.PieceAt(King.Position, ChessBoard.Instance.LBishopBoard);
        Commander rbishopKing = (Commander)ChessBoard.Instance.PieceAt(King.Position, ChessBoard.Instance.RBishopBoard);

        Commander kingBishop = (Commander)ChessBoard.Instance.PieceAt(Position, ChessBoard.Instance.KingBoard);
        Commander lbishopBishop = (Commander)ChessBoard.Instance.PieceAt(Position, ChessBoard.Instance.LBishopBoard);
        Commander rbishopBishop = (Commander)ChessBoard.Instance.PieceAt(Position, ChessBoard.Instance.RBishopBoard);

        King.subordinates = King.subordinates.Union(subordinates).ToList();
        kingKing.subordinates = kingKing.subordinates.Union(kingBishop.subordinates).ToList();
        lbishopKing.subordinates = lbishopKing.subordinates.Union(lbishopBishop.subordinates).ToList();
        rbishopKing.subordinates = rbishopKing.subordinates.Union(rbishopBishop.subordinates).ToList();

        for(int i = 0; i < subordinates.Count; i++) {
            subordinates[i].Commander = King;
            kingBishop.subordinates[i].Commander = kingKing;
            lbishopBishop.subordinates[i].Commander = lbishopKing;
            rbishopBishop.subordinates[i].Commander = rbishopKing;
        }

        subordinates.Clear();
        kingBishop.subordinates.Clear();
        lbishopBishop.subordinates.Clear();
        rbishopBishop.subordinates.Clear();

        isDead = true;
    }

    public override void Reset() {
        commandActions = 1;
        usedFreeMovement = false;
    }
}
