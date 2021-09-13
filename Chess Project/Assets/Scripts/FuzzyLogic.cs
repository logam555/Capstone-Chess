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
    public static void AttachCommandingPieces(GameObject piece,int index,List<GameObject> listPieces)
    {
        Piece p = piece.GetComponent<Piece>();
        if (p.type == Piece.PieceType.King && p.IsWhite)
        {
            p.commandingList.Add(listPieces[2]);
            p.commandingList.Add(listPieces[3]);
            p.commandingList.Add(listPieces[11]);
            p.commandingList.Add(listPieces[12]);

        }
        else if (p.type == Piece.PieceType.King && !p.IsWhite)
        {
            p.commandingList.Add(listPieces[18]);
            p.commandingList.Add(listPieces[19]);
            p.commandingList.Add(listPieces[27]);
            p.commandingList.Add(listPieces[28]);
        }
        else if (p.type == Piece.PieceType.Queen && p.IsWhite)
        {
            p.commander = listPieces[0];
            
        }
        else if (p.type == Piece.PieceType.Queen && !p.IsWhite)
        {
            p.commander = listPieces[16];
        }
        else if (p.type == Piece.PieceType.Bishop && p.IsWhite)
        {
            if (p.index == 2)
            {
                p.commandingList.Add(listPieces[8]);
                p.commandingList.Add(listPieces[9]);
                p.commandingList.Add(listPieces[10]);
            }
            else if (p.index == 5)
            {
                p.commandingList.Add(listPieces[13]);
                p.commandingList.Add(listPieces[14]);
                p.commandingList.Add(listPieces[15]);
            }

        }
        else if (p.type == Piece.PieceType.Bishop && !p.IsWhite)
        {
            if (p.index == 2)
            {
                p.commandingList.Add(listPieces[24]);
                p.commandingList.Add(listPieces[25]);
                p.commandingList.Add(listPieces[26]);
            }
            else if (p.index == 5)
            {
                p.commandingList.Add(listPieces[29]);
                p.commandingList.Add(listPieces[30]);
                p.commandingList.Add(listPieces[31]);
            }

        }
        else if (p.type == Piece.PieceType.Knight && p.IsWhite)
        {
            if(index == 1)
            {
                p.commander = listPieces[2];
            }
            else if(index == 6)
            {
                p.commander = listPieces[3];
            }
        }
        else if (p.type == Piece.PieceType.Knight && !p.IsWhite)
        {
            if(index == 1)
            {
                p.commander = listPieces[18];
            }
            else if(index == 6)
            {
                p.commander = listPieces[19];
            }
        }
        else if (p.type == Piece.PieceType.Pawn && p.IsWhite)
        {
            if(index >= 0 && index < 3)
            {
                p.commander = listPieces[2];
            }
            else if(index == 3 || index == 4)
            {
                p.commander = listPieces[0];
            }
            else if(index > 4 && index <8)
            {
                p.commander = listPieces[3];
            }
        }
        else if (p.type == Piece.PieceType.Pawn && !p.IsWhite)
        {
            if (index >= 0 && index < 3)
            {
                p.commander = listPieces[18];
            }
            else if (index == 3 || index == 4)
            {
                p.commander = listPieces[16];
            }
            else if (index > 4 && index < 8)
            {
                p.commander = listPieces[19];
            }
        }
        else if (p.type == Piece.PieceType.Rook && p.IsWhite)
        {
            if(index == 0)
            {
                p.commander = listPieces[2];
            }
            else if(index == 7)
            {
                p.commander = listPieces[3];
            }
        }
        else if (p.type == Piece.PieceType.Rook && !p.IsWhite)
        {
            if (index == 0)
            {
                p.commander = listPieces[18];
            }
            else if (index == 7)
            {
                p.commander = listPieces[19];
            }
        }
        else
        {
            Debug.Log("Cannot attach commanding pieces, there is no type for object!");
        }
    }
    public static void AttachCommandingPieces(Piece p, int index, List<GameObject> listPieces)
    {
        if (p.type == Piece.PieceType.King && p.IsWhite)
        {
            p.commandingList.Add(listPieces[2]);
            p.commandingList.Add(listPieces[3]);
            p.commandingList.Add(listPieces[11]);
            p.commandingList.Add(listPieces[12]);

        }
        else if (p.type == Piece.PieceType.King && !p.IsWhite)
        {
            p.commandingList.Add(listPieces[18]);
            p.commandingList.Add(listPieces[19]);
            p.commandingList.Add(listPieces[27]);
            p.commandingList.Add(listPieces[28]);
        }
        else if (p.type == Piece.PieceType.Queen && p.IsWhite)
        {
            p.commander = listPieces[0];
            
        }
        else if (p.type == Piece.PieceType.Queen && !p.IsWhite)
        {
            p.commander = listPieces[16];
        }
        else if (p.type == Piece.PieceType.Bishop && p.IsWhite)
        {
            if (p.index == 2)
            {
                p.commandingList.Add(listPieces[8]);
                p.commandingList.Add(listPieces[9]);
                p.commandingList.Add(listPieces[10]);
            }
            else if (p.index == 5)
            {
                p.commandingList.Add(listPieces[13]);
                p.commandingList.Add(listPieces[14]);
                p.commandingList.Add(listPieces[15]);
            }

        }
        else if (p.type == Piece.PieceType.Bishop && !p.IsWhite)
        {
            if (p.index == 2)
            {
                p.commandingList.Add(listPieces[24]);
                p.commandingList.Add(listPieces[25]);
                p.commandingList.Add(listPieces[26]);
            }
            else if (p.index == 5)
            {
                p.commandingList.Add(listPieces[29]);
                p.commandingList.Add(listPieces[30]);
                p.commandingList.Add(listPieces[31]);
            }

        }
        else if (p.type == Piece.PieceType.Knight && p.IsWhite)
        {
            if(index == 1)
            {
                p.commander = listPieces[2];
            }
            else if(index == 6)
            {
                p.commander = listPieces[3];
            }
        }
        else if (p.type == Piece.PieceType.Knight && !p.IsWhite)
        {
            if(index == 1)
            {
                p.commander = listPieces[18];
            }
            else if(index == 6)
            {
                p.commander = listPieces[19];
            }
        }
        else if (p.type == Piece.PieceType.Pawn && p.IsWhite)
        {
            if(index > 0 && index < 3)
            {
                p.commander = listPieces[2];
            }
            else if(index == 3 || index == 4)
            {
                p.commander = listPieces[0];
            }
            else if(index > 4 && index <8)
            {
                p.commander = listPieces[3];
            }
        }
        else if (p.type == Piece.PieceType.Pawn && !p.IsWhite)
        {
            if (index > 0 && index < 3)
            {
                p.commander = listPieces[18];
            }
            else if (index == 3 || index == 4)
            {
                p.commander = listPieces[16];
            }
            else if (index > 4 && index < 8)
            {
                p.commander = listPieces[19];
            }
        }
        else if (p.type == Piece.PieceType.Rook && p.IsWhite)
        {
            if(index == 0)
            {
                p.commander = listPieces[2];
            }
            else if(index == 7)
            {
                p.commander = listPieces[3];
            }
        }
        else if (p.type == Piece.PieceType.Rook && !p.IsWhite)
        {
            if (index == 0)
            {
                p.commander = listPieces[18];
            }
            else if (index == 7)
            {
                p.commander = listPieces[19];
            }
        }
        else
        {
            Debug.Log("Cannot attach commanding pieces, there is no type for object!");
        }
    }
    public static bool FindFuzzyLogic(int rollNumber, Piece avaliablePosition,Piece[,] Pieces)
    {
        Piece pieceToAttack = Pieces[avaliablePosition.Position.x, avaliablePosition.Position.y];

        if (pieceToAttack.type == Piece.PieceType.King)
        {
            int numberToMatch = FuzzyLogic.FindNumberKing(pieceToAttack);
            if (rollNumber == numberToMatch)
            {
                return true;
            }
        }
        else if (pieceToAttack.type == Piece.PieceType.Queen)
        {
            int numberToMatch = FuzzyLogic.FindNumberQueen(pieceToAttack);
            if (rollNumber == numberToMatch)
            {
                return true;
            }
        }
        else if (pieceToAttack.type == Piece.PieceType.Bishop)
        {
            int numberToMatch = FuzzyLogic.FindNumberBishop(pieceToAttack);
            if (rollNumber == numberToMatch)
            {
                return true;
            }
        }
        else if (pieceToAttack.type == Piece.PieceType.Pawn)
        {
            int numberToMatch = FuzzyLogic.FindNumberPawn(pieceToAttack);
            if (rollNumber == numberToMatch)
            {
                return true;
            }
        }
        else if (pieceToAttack.type == Piece.PieceType.Knight)
        {
            int numberToMatch = FuzzyLogic.FindNumberKnight(pieceToAttack);
            if (rollNumber == numberToMatch)
            {
                return true;
            }
        }
        else if (pieceToAttack.type == Piece.PieceType.Rook)
        {
            int numberToMatch = FuzzyLogic.FindNumberRook(pieceToAttack);
            if (rollNumber == numberToMatch)
            {
                return true;
            }
        }
        else
        {
            Debug.Log("Piece does not have type on script!");
            return false;
        }
        return false;
    }
}