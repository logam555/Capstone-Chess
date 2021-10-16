//Written by Hamza Khan
//last edited: 10/16/2021 - 4:30
//V2.0

using System;
using System.Collections.Generic;
using UnityEngine;

public class BishopAI 
{
    private BoardManager bm;
    private bool isDead = false;
    public bool isWhite;
    private Piece bishop;
    private Piece Knight;
    private Piece[] pawn = new Piece[3];
    private Piece bestPiece { get; set; }
    private int[,] moves = new int[3,5];
    private Piece[,] board = new Piece[8,8];
    private Vector2Int[,] initialPositions = new Vector2Int(2, 5);

    public int[] Start(bool isWhite, bool left)
    {
        getBoard();

        this.isWhite = isWhite;

        getCommander(left);
        getKnight(left);
        getPawns(left);

        return bestGlobal();
    }

    public int[] Step()
    {
        getBoard();

        return bestGlobal();
    }

    public void getBoard() //Obtains board from BoardManager
    {
        board = bm.Pieces();
    }

    public int[] bestGlobal() //obtains the best move possible in corp and returns x and y coordinates and return as array
    {
        getLocal();
        subordinateMoves();

        int[] move = new int[2]
        int bestScore = moves[2][0];

        for (int j = 1; j < 5; j++)
        {
            if (bestScore < moves[2][j])
            {
                move[0] = moves[0][j];
                move[1] = moves[1][j];

                if(j == 0)
                {
                    bestPiece = bishop;
                }
                if (j == 1)
                {
                    bestPiece = Knight;
                }
                if (j == 2)
                {
                    bestPiece = pawn[0];
                }
                if (j == 3)
                {
                    bestPiece = pawn[1];
                }
                if (j == 4)
                {
                    bestPiece = pawn[2];
                }
            }
        }

        return move;
    }

    public void getLocal()
    {
        BestMove local = new BestMove();

        int[] bl = local.bestLocal(board, bishop);

        moves[0][0] = bl[1]; //x and y coordinates of best scoring move are recorded
        moves[1][0] = bl[2];
        moves[2][0] = bl[0]; //than score obtained is bestLocalScore
    }

    public void subordinateMoves() //obtains the moves and scores of its subordinate pieces
    {
        BestMove local = new BestMove();

        int[] bl = local.bestLocal(board, possibleMoves(), Knight);

        moves[0][1] = bl[1]; //x and y coordinates of best scoring move are recorded
        moves[1][1] = bl[2];
        moves[2][1] = bl[0]; //than score obtained is bestLocalScore

        bl = local.bestLocal(board, possibleMoves(), pawn[0]);

        moves[0][2] = bl[1]; //x and y coordinates of best scoring move are recorded
        moves[1][2] = bl[2];
        moves[2][2] = bl[0]; //than score obtained is bestLocalScore

        bl = local.bestLocal(board, possibleMoves(), pawn[1]);

        moves[0][3] = bl[1]; //x and y coordinates of best scoring move are recorded
        moves[1][3] = bl[2];
        moves[2][3] = bl[0]; //than score obtained is bestLocalScore

        bl = local.bestLocal(board, possibleMoves(), pawn[2]);

        moves[0][4] = bl[1]; //x and y coordinates of best scoring move are recorded
        moves[1][4] = bl[2];
        moves[2][4] = bl[0]; //than score obtained is bestLocalScore
    }

    public void getCommander(bool lBishop)
    {
        if (isWhite == true && lBishop == true)
        {
            initialPositions[0][0].x = 7;
            initialPositions[1][0].y = 2;

            bishop = bm.PieceAt(position);
        }
        if (isWhite == true && lBishop = false)
        {
            initialPositions[0][0].x = 7;
            initialPositions[1][0].y = 5;

            bishop = bm.PieceAt(position);
        }
        if (isWhite == false && lBishop = true)
        {
            initialPositions[0][0].x = 0;
            initialPositions[1][0].y = 2;

            bishop = bm.PieceAt(position);
        }
        if (isWhite == false && lBishop = false)
        {
            initialPositions[0][0].x = 0;
            initialPositions[1][0].y = 5;

            bishop = bm.PieceAt(position);
        }
    }

    public void getKnight(bool lKnight)
    {
        Vector2Int position = new Vector2Int(0, 0);
        if (isWhite == true && lKnight == true)
        {
            initialPositions[0][1].x = 7;
            initialPositions[1][1].y = 1;

            Knight = bm.PieceAt(position);
        }
        if (isWhite == true && lKnight = false)
        {
            initialPositions[0][1].x = 7;
            initialPositions[1][1].y = 6;

            Knight = bm.PieceAt(position);
        }
        if (isWhite == false && lKnight = true)
        {
            initialPositions[0][1].x = 0;
            initialPositions[1][1].y = 1;

            Knight = bm.PieceAt(position);
        }
        if (isWhite == false && lKnight = false)
        {
            initialPositions[0][1].x = 0;
            initialPositions[1][1].y = 6;

            Knight = bm.PieceAt(position);
        }
    }

    public void getPawns(bool lPawns)
    {
        Vector2Int position = new Vector2Int(0, 0);
        if (isWhite == true && lPawns == true)
        {
            position.x = 1;
            position.y = 0;

            pawn[0] = bm.PieceAt(position);

            position.y = 1;
            pawn[1] = bm.PieceAt(position);

            position.y = 2;
            pawn[2] = bm.PieceAt(position);
        }
        if (isWhite == true && lPawns = false)
        {
            position.x = 1;
            position.y = 5;

            pawn[0] = bm.PieceAt(position);

            position.y = 6;
            pawn[1] = bm.PieceAt(position);

            position.y = 7;
            pawn[2] = bm.PieceAt(position);
        }
        if (isWhite == false && lPawns = true)
        {
            position.x = 7;
            position.y = 0;

            pawn[0] = bm.PieceAt(position);

            position.y = 1;
            pawn[1] = bm.PieceAt(position);

            position.y = 2;
            pawn[2] = bm.PieceAt(position);
        }
        if (isWhite == false && lPawns = false)
        {
            position.x = 7;
            position.y = 5;

            pawn[0] = bm.PieceAt(position);

            position.y = 6;
            pawn[1] = bm.PieceAt(position);

            position.y = 7;
            pawn[2] = bm.PieceAt(position);
        }
    }
}