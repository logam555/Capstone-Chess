using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FuzzyLogic
{
    public enum Pawn : int
    {
        Pawn = 4,
        Rook = 6,
        Bishop = 5,
        Knight = 6,
        Queen = 6,
        King = 6
    }

    public enum Rook : int
    {
        Pawn = 5,
        Rook = 5,
        Bishop = 5,
        Knight = 4,
        Queen = 4,
        King = 4
    }

    public enum Bishop : int
    {
        Pawn = 3,
        Rook = 5,
        Bishop = 4,
        Knight = 5,
        Queen = 5,
        King = 5
    }

    public enum Knight : int
    {
        Pawn = 2,
        Rook = 5,
        Bishop = 5,
        Knight = 5,
        Queen = 5,
        King = 5
    }

    public enum Queen : int
    {
        Pawn = 2,
        Rook = 5,
        Bishop = 4,
        Knight = 4,
        Queen = 4,
        King = 4
    }

    public enum King : int
    {
        Pawn = 1,
        Rook = 5,
        Bishop = 4,
        Knight = 4,
        Queen = 4,
        King = 4
    }
    public static int FindNumberKing(Piece pieceToAttack)
    {
        if (pieceToAttack.type == Piece.PieceType.King)
        {
            return (int)FuzzyLogic.King.King;
        }
        else if (pieceToAttack.type == Piece.PieceType.Queen)
        {
            return (int)FuzzyLogic.King.Queen;
        }
        else if (pieceToAttack.type == Piece.PieceType.Bishop)
        {
            return (int)FuzzyLogic.King.Bishop;
        }
        else if (pieceToAttack.type == Piece.PieceType.Pawn)
        {
            return (int)FuzzyLogic.King.Pawn;
        }
        else if (pieceToAttack.type == Piece.PieceType.Knight)
        {
            return (int)FuzzyLogic.King.Knight;
        }
        else if (pieceToAttack.type == Piece.PieceType.Rook)
        {
            return (int)FuzzyLogic.King.Rook;
        }
        return 0;
    }
    public static int FindNumberQueen(Piece pieceToAttack)
    {
        if (pieceToAttack.type == Piece.PieceType.King)
        {
            return (int)FuzzyLogic.Queen.King;
        }
        else if (pieceToAttack.type == Piece.PieceType.Queen)
        {
            return (int)FuzzyLogic.Queen.Queen;
        }
        else if (pieceToAttack.type == Piece.PieceType.Bishop)
        {
            return (int)FuzzyLogic.Queen.Bishop;
        }
        else if (pieceToAttack.type == Piece.PieceType.Pawn)
        {
            return (int)FuzzyLogic.Queen.Pawn;
        }
        else if (pieceToAttack.type == Piece.PieceType.Knight)
        {
            return (int)FuzzyLogic.Queen.Knight;
        }
        else if (pieceToAttack.type == Piece.PieceType.Rook)
        {
            return (int)FuzzyLogic.Queen.Rook;
        }
        return 0;


    }
    public static int FindNumberBishop(Piece pieceToAttack)
    {
        if (pieceToAttack.type == Piece.PieceType.King)
        {
            return (int)FuzzyLogic.Bishop.King;
        }
        else if (pieceToAttack.type == Piece.PieceType.Queen)
        {
            return (int)FuzzyLogic.Bishop.Queen;
        }
        else if (pieceToAttack.type == Piece.PieceType.Bishop)
        {
            return (int)FuzzyLogic.Bishop.Bishop;
        }
        else if (pieceToAttack.type == Piece.PieceType.Pawn)
        {
            return (int)FuzzyLogic.Bishop.Pawn;
        }
        else if (pieceToAttack.type == Piece.PieceType.Knight)
        {
            return (int)FuzzyLogic.Bishop.Knight;
        }
        else if (pieceToAttack.type == Piece.PieceType.Rook)
        {
            return (int)FuzzyLogic.Bishop.Rook;
        }
        return 0;

    }
    public static int FindNumberRook(Piece pieceToAttack)
    {
        if (pieceToAttack.type == Piece.PieceType.King)
        {
            return (int)FuzzyLogic.Rook.King;
        }
        else if (pieceToAttack.type == Piece.PieceType.Queen)
        {
            return (int)FuzzyLogic.Rook.Queen;
        }
        else if (pieceToAttack.type == Piece.PieceType.Bishop)
        {
            return (int)FuzzyLogic.Rook.Bishop;
        }
        else if (pieceToAttack.type == Piece.PieceType.Pawn)
        {
            return (int)FuzzyLogic.Rook.Pawn;
        }
        else if (pieceToAttack.type == Piece.PieceType.Knight)
        {
            return (int)FuzzyLogic.Rook.Knight;
        }
        else if (pieceToAttack.type == Piece.PieceType.Rook)
        {
            return (int)FuzzyLogic.Rook.Rook;
        }
        return 0;

    }
    public static int FindNumberKnight(Piece pieceToAttack)
    {
        if (pieceToAttack.type == Piece.PieceType.King)
        {
            return (int)FuzzyLogic.Knight.King;
        }
        else if (pieceToAttack.type == Piece.PieceType.Queen)
        {
            return (int)FuzzyLogic.Knight.Queen;
        }
        else if (pieceToAttack.type == Piece.PieceType.Bishop)
        {
            return (int)FuzzyLogic.Knight.Bishop;
        }
        else if (pieceToAttack.type == Piece.PieceType.Pawn)
        {
            return (int)FuzzyLogic.Knight.Pawn;
        }
        else if (pieceToAttack.type == Piece.PieceType.Knight)
        {
            return (int)FuzzyLogic.Knight.Knight;
        }
        else if (pieceToAttack.type == Piece.PieceType.Rook)
        {
            return (int)FuzzyLogic.Knight.Rook;
        }
        return 0;

    }
    public static int FindNumberPawn(Piece pieceToAttack)
    {
        if (pieceToAttack.type == Piece.PieceType.King)
        {
            return (int)FuzzyLogic.Pawn.King;
        }
        else if (pieceToAttack.type == Piece.PieceType.Queen)
        {
            return (int)FuzzyLogic.Pawn.Queen;
        }
        else if (pieceToAttack.type == Piece.PieceType.Bishop)
        {
            return (int)FuzzyLogic.Pawn.Bishop;
        }
        else if (pieceToAttack.type == Piece.PieceType.Pawn)
        {
            return (int)FuzzyLogic.Pawn.Pawn;
        }
        else if (pieceToAttack.type == Piece.PieceType.Knight)
        {
            return (int)FuzzyLogic.Pawn.Knight;
        }
        else if (pieceToAttack.type == Piece.PieceType.Rook)
        {
            return (int)FuzzyLogic.Pawn.Rook;
        }
        return 0;

    }
}