/* Written by Braden Stonehill
 Edited by David Corredor, Braden Stonehill
 Last date edited: 09/15/2021
 FuzzyLogic.cs - struct object that contains int enumerators for each attacking piece with the minimum roll needed
 to capture the defending piece.

 Version 1.3: 
  - Changed all functions to private and added a function that determines the type of the attacker and defender
 to determine what function to use for calculating the minimum roll needed.

  - Added functions for fetching the values for each attacking piece based on the type of the defending piece. */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FuzzyLogic {
    #region ENUMERATOR PROPERTIES
    public enum AttackPawn : int {
        Pawn = 4,
        Rook = 6,
        Bishop = 5,
        Knight = 6,
        Queen = 6,
        King = 6
    }

    public enum AttackRook : int {
        Pawn = 5,
        Rook = 5,
        Bishop = 5,
        Knight = 4,
        Queen = 4,
        King = 4
    }

    public enum AttackBishop : int {
        Pawn = 3,
        Rook = 5,
        Bishop = 4,
        Knight = 5,
        Queen = 5,
        King = 5
    }

    public enum AttackKnight : int {
        Pawn = 2,
        Rook = 5,
        Bishop = 5,
        Knight = 5,
        Queen = 5,
        King = 5
    }

    public enum AttackQueen : int {
        Pawn = 2,
        Rook = 5,
        Bishop = 4,
        Knight = 4,
        Queen = 4,
        King = 4
    }

    public enum AttackKing : int {
        Pawn = 1,
        Rook = 5,
        Bishop = 4,
        Knight = 4,
        Queen = 4,
        King = 4
    }
    #endregion

    #region STATIC METHODS
    private static int FindNumberKing(ChessPiece pieceToAttack) {
        if (pieceToAttack is King) {
            return (int)AttackKing.King;
        } else if (pieceToAttack is Queen) {
            return (int)FuzzyLogic.AttackKing.Queen;
        } else if (pieceToAttack is Bishop) {
            return (int)FuzzyLogic.AttackKing.Bishop;
        } else if (pieceToAttack is Pawn ) {
            return (int)FuzzyLogic.AttackKing.Pawn;
        } else if (pieceToAttack is Knight ) {
            return (int)FuzzyLogic.AttackKing.Knight;
        } else if (pieceToAttack is Rook ) {
            return (int)FuzzyLogic.AttackKing.Rook;
        }
        return 0;
    }
    private static int FindNumberQueen(ChessPiece pieceToAttack) {
        if (pieceToAttack is King ) {
            return (int)FuzzyLogic.AttackQueen.King;
        } else if (pieceToAttack is Queen ) {
            return (int)FuzzyLogic.AttackQueen.Queen;
        } else if (pieceToAttack is Bishop ) {
            return (int)FuzzyLogic.AttackQueen.Bishop;
        } else if (pieceToAttack is Pawn ) {
            return (int)FuzzyLogic.AttackQueen.Pawn;
        } else if (pieceToAttack is Knight ) {
            return (int)FuzzyLogic.AttackQueen.Knight;
        } else if (pieceToAttack is Rook ) {
            return (int)FuzzyLogic.AttackQueen.Rook;
        }
        return 0;


    }
    private static int FindNumberBishop(ChessPiece pieceToAttack) {
        if (pieceToAttack is King ) {
            return (int)FuzzyLogic.AttackBishop.King;
        } else if (pieceToAttack is Queen ) {
            return (int)FuzzyLogic.AttackBishop.Queen;
        } else if (pieceToAttack is Bishop ) {
            return (int)FuzzyLogic.AttackBishop.Bishop;
        } else if (pieceToAttack is Pawn ) {
            return (int)FuzzyLogic.AttackBishop.Pawn;
        } else if (pieceToAttack is Knight ) {
            return (int)FuzzyLogic.AttackBishop.Knight;
        } else if (pieceToAttack is Rook ) {
            return (int)FuzzyLogic.AttackBishop.Rook;
        }
        return 0;

    }
    private static int FindNumberRook(ChessPiece pieceToAttack) {
        if (pieceToAttack is King) {
            return (int)FuzzyLogic.AttackRook.King;
        } else if (pieceToAttack is Queen ) {
            return (int)FuzzyLogic.AttackRook.Queen;
        } else if (pieceToAttack is Bishop) {
            return (int)FuzzyLogic.AttackRook.Bishop;
        } else if (pieceToAttack is Pawn ) {
            return (int)FuzzyLogic.AttackRook.Pawn;
        } else if (pieceToAttack is Knight) {
            return (int)FuzzyLogic.AttackRook.Knight;
        } else if (pieceToAttack is Rook ) {
            return (int)FuzzyLogic.AttackRook.Rook;
        }
        return 0;

    }
    private static int FindNumberKnight(ChessPiece pieceToAttack) {
        if (pieceToAttack is King) {
            return (int)FuzzyLogic.AttackKnight.King;
        } else if (pieceToAttack is Queen ) {
            return (int)FuzzyLogic.AttackKnight.Queen;
        } else if (pieceToAttack is Bishop) {
            return (int)FuzzyLogic.AttackKnight.Bishop;
        } else if (pieceToAttack is Pawn ) {
            return (int)FuzzyLogic.AttackKnight.Pawn;
        } else if (pieceToAttack is Knight) {
            return (int)FuzzyLogic.AttackKnight.Knight;
        } else if (pieceToAttack is Rook ) {
            return (int)FuzzyLogic.AttackKnight.Rook;
        }
        return 0;

    }
    private static int FindNumberPawn(ChessPiece pieceToAttack) {
        if (pieceToAttack is King) {
            return (int)FuzzyLogic.AttackPawn.King;
        } else if (pieceToAttack is Queen ) {
            return (int)FuzzyLogic.AttackPawn.Queen;
        } else if (pieceToAttack is Bishop) {
            return (int)FuzzyLogic.AttackPawn.Bishop;
        } else if (pieceToAttack is Pawn ) {
            return (int)FuzzyLogic.AttackPawn.Pawn;
        } else if (pieceToAttack is Knight) {
            return (int)FuzzyLogic.AttackPawn.Knight;
        } else if (pieceToAttack is Rook ) {
            return (int)FuzzyLogic.AttackPawn.Rook;
        }
        return 0;

    }

    public static int FindFuzzyNumber(ChessPiece attacker, ChessPiece defender) {
        if (attacker is King)
            return FindNumberKing(defender);
        else if (attacker is Queen)
            return FindNumberQueen(defender);
        else if (attacker is Bishop)
            return FindNumberBishop(defender);
        else if (attacker is Knight)
            return FindNumberKnight(defender);
        else if (attacker is Rook)
            return FindNumberRook(defender);
        else
            return FindNumberPawn(defender);
    }
    #endregion
}

