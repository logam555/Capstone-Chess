//Written by Hamza Khan
//last edited: 10/16/2021 - 4:30
//V2.0
using System;
using System.Collections.Generic;
using UnityEngine;

public class KingAI : CommanderAI
{
    public Commander King { get; set; }
    //private bool isDead = false;
    public ChessPiece bestPiece { get; set; }
    public bool isWhite;
    private int[,] moves;
    private ChessPiece[,] board;

    private void Start() {
        moves = new int[3, 16];
        getBoard();
        isWhite = FindObjectOfType<AI>().isWhite;
        getCommander();
    }

    public override int[] Step()
    {
        getBoard();

        return bestGlobal();
    }

    public void getBoard() //Obtains board from GameManager
    {
        board = (ChessPiece[,])ChessBoard.Instance.Board.Clone();
    }

    public void getCommander()
    {
        if (isWhite == false)
        {
            King = (Commander)ChessBoard.Instance.PieceAt(new Vector2Int(4,7), board);
        }
        if (isWhite == true)
        {
            King = (Commander)ChessBoard.Instance.PieceAt(new Vector2Int(4, 0), board);
        }
    }

    public void subordinateMoves() //obtains the moves and scores of its subordinate pieces
    {
        BestMove local = new BestMove();

        for (int i = 0; i < King.subordinates.Count; i++)
        {
            int[] bl = local.getMove(board, King.subordinates[i], false);

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
        move[0] = moves[0, 0];
        move[1] = moves[1, 0];
        bestPiece = King;

        for (int j = 1; j < 6; j++)
        {
            if (bestScore < moves[2,j])
            {
                move[0] = moves[0,j];
                move[1] = moves[1,j];
                bestScore = moves[2, j];

                bestPiece = King.subordinates[j - 1];
            }
            
        }

        return move;
    }

    public void getLocal()
    {
        BestMove local = new BestMove();

        int[] bl = local.getMove(board, King, true);

        moves[0,0] = bl[0]; //x and y coordinates of best scoring move are recorded
        moves[1,0] = bl[1];
        moves[2,0] = bl[2]; //than score obtained is bestLocalScore
    }
}
