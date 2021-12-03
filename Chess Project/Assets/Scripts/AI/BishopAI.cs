//Written by Hamza Khan
//last edited: 10/16/2021 - 4:30
//V2.0

using System;
using System.Collections.Generic;
using UnityEngine;

public class BishopAI : CommanderAI
{
    //private bool isDead = false;
    public bool isWhite;
    public Commander bishop { get; set; }
    public ChessPiece bestPiece { get; set; }
    public bool freeMove;
    private int[,] moves;
    private ChessPiece[,] board;
    private BestMove local;

    private bool initialized;
    public bool leftBishop;

    private void Awake() {
        initialized = false;
    }

    private void Start() {
        moves = new int[3, 16];
        getBoard();
        isWhite = FindObjectOfType<AI>().isWhite;
        getCommander(leftBishop);
        
    }

    private void Update() {
        
        if(initialized && bishop.isDead) {
            Destroy(this.gameObject);
        }
    }

    public override int[] Step()
    {
        if (!initialized) {
            initialized = true;
            local = new BestMove(board);
        }

        freeMove = false;

        return getLocal();
    }

    public int[] useFreeMove()
    {
        int[] move = new int[5];
        if (freeMove == false)
        {

            move = local.getMove(bishop, true, true);

            freeMove = true;

            return move;
        }

        return move;
    }

    public void getBoard() //Obtains board from BoardManager
    {
        board = leftBishop ? ChessBoard.Instance.LBishopBoard : ChessBoard.Instance.RBishopBoard;
    }

    public int[] bestGlobal() //obtains the best move possible in corp and returns x and y coordinates and return as array
    {
        getLocal();
        //subordinateMoves();

        int[] move = new int[2];
        int bestScore = moves[2,0];
        move[0] = moves[0, 0];
        move[1] = moves[1, 0];
        bestPiece = bishop;

        for (int j = 1; j < 5; j++)
        {
            if (bestScore < moves[2,j])
            {
                move[0] = moves[0,j];
                move[1] = moves[1,j];
                bestScore = moves[2, j];
                bestPiece = bishop.subordinates[j - 1];
            }
        }

        return move;
    }

    public int[] getLocal()
    {

        int[] bl = local.getMove(bishop, leftBishop, false);
        bestPiece = ChessBoard.Instance.PieceAt(new Vector2Int(bl[0], bl[1]), board);

        return bl;
    }

    //public void subordinateMoves() //obtains the moves and scores of its subordinate pieces
    //{
    //    BestMove local = new BestMove();

    //    for (int i = 0; i < bishop.subordinates.Count; i++)
    //    {
    //        int[] bl = local.getMove(board, bishop.subordinates[i], false);

    //        moves[0, i + 1] = bl[0]; //x and y coordinates of best scoring move are recorded
    //        moves[1, i + 1] = bl[1];
    //        moves[2, i + 1] = bl[2]; //than score obtained is bestLocalScore 
    //    }
    //}

    public void getCommander(bool lBishop)
    {
        Vector2Int position = new Vector2Int(0, 0);
        if (isWhite == false && lBishop == true)
        {
            position.x = 2;
            position.y = 7;

            bishop = (Commander) ChessBoard.Instance.PieceAt(position, board);
        }
        if (isWhite == false && lBishop == false)
        {
            position.x = 5;
            position.y = 7;

            bishop = (Commander) ChessBoard.Instance.PieceAt(position, board);
        }
        if (isWhite == true && lBishop == true)
        {
            position.x = 2;
            position.y = 0;

            bishop = (Commander) ChessBoard.Instance.PieceAt(position, board);
        }
        if (isWhite == true && lBishop == false)
        {
            position.x = 5;
            position.y = 0;

            bishop = (Commander) ChessBoard.Instance.PieceAt(position, board);
        }
    }
}