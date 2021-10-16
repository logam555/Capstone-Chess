//Written by Hamza Khan
//last edited: 10/16/2021 - 4:30
//V2.0
using System;
using System.Collections.Generic;
using UnityEngine;

public class BestMove
{
    Piece[,] board = new Piece[8,8];
    bool[,] pm = new bool[8, 8];
    bool isCommander;
    public Random rand = new Random();
    heuristics h = new heuristics();
    Piece piece;

    public int[] getMove(Piece[,] board, Piece piece, bool isCommander)
    {
        this.board = board;
        this.piece = piece;
        this.isCommander = isCommander;

        return bestLocal();
    }

    public int eval(Piece[,] board, int dice) //sends board to heuristic to obtain a score for the move made
    {
        if (isCommander == true)
        {
            return h.CommanderHeuristic(board, dice);
        }
        else
            return h.IndividualHeuristic(board, dice);
    }

    public void possibleMoves(Piece p) //Uses Bishop script to obtain possible moves for Bishop
    {
        BoardManager bm;

        pm = bm.AvailableMoves(p);
    }

    public int[] bestMove(Piece[,] board)
    {
        int[] move = new int[3];
        BestMove local = new BestMove();

        int[] bl = local.bestLocal(board, possibleMoves());

        move[0][0] = bl[1]; //x and y coordinates of best scoring move are recorded
        move[1][0] = bl[2];
        move[2][0] = bl[0]; //than score obtained is bestLocalScore
    }

    public int[] bestLocal() //obtains best possible move for Bishop
    {
        int[] move = new int[3];
        int pieceY;
        int pieceX;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    for (int d = 0; d < 8; d++)
                    {
                        if (board[k][d] == piece)
                        {
                            k = pieceX;
                            d = pieceY;
                        }
                    }
                }
                if (board[i][j] == null && possibleMoves[i][j] == true)
                {
                    
                    Piece temp = board[i][j];
                    board[i][j] = piece;
                    board[pieceX][pieceY] = null;
                    int dice = rand.Next(1, 7);
                    int score = minimax(board, false, dice);
                    board[i][j] = temp;
                    
                    if (score > move[0]) //if score obtained is better than bestLocalScore
                    {
                        move[2] = score; //than score obtained is bestLocalScore
                        move[0] = i; //x and y coordinates of best scoring move are recorded
                        move[1] = j;
                    }
                }
            }
        }
    }

    public int minimax(Piece[,] tempBoard, bool maximize, int dice) //uses minimax algorithm to obtain the score
    {
        int score = eval(board, dice); //uses heuristic to obtain score
        int pieceY;
        int pieceX;

        if (maximize == true) //if maximizing
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        for (int d = 0; d < 8; d++)
                        {
                            if (tempBoard[k][d] == piece)
                            {
                                k = pieceX;
                                d = pieceY;
                            }
                        }
                    }
                    if (board[i][j] == null && possibleMoves()[i][j] == true)
                    {
                        Piece temp = tempBoard[i][j];
                        tempBoard[i][j] = piece;
                        tempBoard[pieceX][pieceY] = temp;
                        dice = rand.Next(1, 7);
                        score = Math.Max(score, minimax(tempBoard, false, dice));
                        tempBoard[i][j] = temp;
                        tempBoard[pieceX][pieceY] = piece;
                    }
                }
            }
            return score;
        }

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    for (int d = 0; d < 8; d++)
                    {
                        if (tempBoard[k][d] == piece)
                        {
                            k = pieceX;
                            d = pieceY;
                        }
                    }
                }
                if (board[i][j] == null && possibleMoves()[i][j] == true)
                {
                    Piece temp = tempBoard[i][j];
                    tempBoard[i][j] = piece;
                    tempBoard[pieceX][pieceY] = temp;
                    dice = rand.Next(1, 7);
                    score = Math.Min(score, minimax(tempBoard, false, dice));
                    tempBoard[i][j] = temp;
                    tempBoard[pieceX][pieceY] = piece;
                }
            }
        }
        return score;
    }
}
