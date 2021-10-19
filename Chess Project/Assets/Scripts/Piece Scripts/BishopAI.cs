//Written by Hamza Khan
//last edited: 10/16/2021 - 4:30
//V2.0

using System;
using System.Collections.Generic;
using UnityEngine;

public class BishopAI 
{
    private BoardManager bm;
    //private bool isDead = false;
    public bool isWhite;
    private Commander bishop;
    public Piece bestPiece { get; set; }
    private int[,] moves = new int[3,5];
    private Piece[,] board = new Piece[8,8];

    public BishopAI(BoardManager bm) {
        this.bm = bm;
    }

    public int[] Start(bool isWhite, bool left)
    {
        getBoard();

        this.isWhite = isWhite;

        getCommander(left);

        return bestGlobal();
    }

    public int[] Step()
    {
        getBoard();

        return bestGlobal();
    }

    public void getBoard() //Obtains board from BoardManager
    {
        board = (Piece[,])bm.Pieces.Clone();
    }

    public int[] bestGlobal() //obtains the best move possible in corp and returns x and y coordinates and return as array
    {
        getLocal();
        subordinateMoves();

        int[] move = new int[2];
        int bestScore = moves[2,0];

        for (int j = 0; j < 5; j++)
        {
            if (bestScore < moves[2,j])
            {
                move[0] = moves[0,j];
                move[1] = moves[1,j];
                bestScore = moves[2, j];

                if (j == 0)
                {
                    bestPiece = bishop;
                }
                else
                {
                    bestPiece = bishop.subordinates[j - 1];
                }
            }
        }

        return move;
    }

    public void getLocal()
    {
        BestMove local = new BestMove();

        int[] bl = local.getMove(board, bishop,true, bm);

        moves[0,0] = bl[0]; //x and y coordinates of best scoring move are recorded
        moves[1,0] = bl[1];
        moves[2,0] = bl[2]; //than score obtained is bestLocalScore
    }

    public void subordinateMoves() //obtains the moves and scores of its subordinate pieces
    {
        BestMove local = new BestMove();

        for (int i = 0; i < bishop.subordinates.Count; i++)
        {
            int[] bl = local.getMove(board, bishop.subordinates[i], false, bm);

            moves[0, i + 1] = bl[0]; //x and y coordinates of best scoring move are recorded
            moves[1, i + 1] = bl[1];
            moves[2, i + 1] = bl[2]; //than score obtained is bestLocalScore 
        }
    }

    public void getCommander(bool lBishop)
    {
        Vector2Int position = new Vector2Int(0, 0);
        if (isWhite == false && lBishop == true)
        {
            position.x = 2;
            position.y = 7;

            bishop = (Commander) bm.PieceAt(position);
        }
        if (isWhite == false && lBishop == false)
        {
            position.x = 5;
            position.y = 7;

            bishop = (Commander) bm.PieceAt(position);
        }
        if (isWhite == true && lBishop == true)
        {
            position.x = 2;
            position.y = 0;

            bishop = (Commander) bm.PieceAt(position);
        }
        if (isWhite == true && lBishop == false)
        {
            position.x = 5;
            position.y = 0;

            bishop = (Commander) bm.PieceAt(position);
        }
    }
}