//Written by Hamza Khan
//last edited: 10/16/2021 - 4:30
//V2.0
using System;
using System.Collections.Generic;
using UnityEngine;

public class KingAI
{
    private Commander King;
    private BoardManager bm;
    //private bool isDead = false;
    public Piece bestPiece { get; set; }
    public bool isWhite;
    private int[,] moves;
    private Piece[,] board;

    public KingAI(BoardManager bm) {
        this.bm = bm;
        moves = new int[3, 6];
        board = new Piece[8, 8];
    }


    public int[] Start(bool isWhite)
    {
        getBoard();

        this.isWhite = isWhite;

        getCommander();
        
        return bestGlobal();
    }

    public int[] Step()
    {
        getBoard();

        return bestGlobal();
    }

    public void getBoard() //Obtains board from GameManager
    {
        board = (Piece[,])bm.Pieces.Clone();
    }

    public void getCommander()
    {
        Vector2Int position = new Vector2Int(0, 0);
        if (isWhite == false)
        {
            position.x = 4;
            position.y = 7;

            King = (Commander)bm.PieceAt(position);
        }
        if (isWhite == true)
        {
            position.x = 4;
            position.y = 0;

            King = (Commander)bm.PieceAt(position);
        }
    }

    public void subordinateMoves() //obtains the moves and scores of its subordinate pieces
    {
        BestMove local = new BestMove();

        for (int i = 0; i < King.subordinates.Count; i++)
        {
            int[] bl = local.getMove(board, King.subordinates[i], false, bm);

            moves[0, i+1] = bl[0]; //x and y coordinates of best scoring move are recorded
            moves[1, i+1] = bl[1];
            moves[2, i+1] = bl[2]; //than score obtained is bestLocalScore 
        }
    }

    public int[] bestGlobal() //obtains the best move possible in corp and returns x and y coordinates and return as array
    {
        getLocal();
        subordinateMoves();

        int[] move = new int[2];
        int bestScore = moves[2,0];

        for (int j = 0; j < 6; j++)
        {
            if (bestScore < moves[2,j])
            {
                move[0] = moves[0,j];
                move[1] = moves[1,j];
                bestScore = moves[2, j];

                if (j == 0) {
                    bestPiece = King;
                } else {
                    bestPiece = King.subordinates[j - 1];
                }
            }
            
        }

        return move;
    }

    public void getLocal()
    {
        BestMove local = new BestMove();

        int[] bl = local.getMove(board, King, true, bm);

        moves[0,0] = bl[0]; //x and y coordinates of best scoring move are recorded
        moves[1,0] = bl[1];
        moves[2,0] = bl[2]; //than score obtained is bestLocalScore
    }
}
