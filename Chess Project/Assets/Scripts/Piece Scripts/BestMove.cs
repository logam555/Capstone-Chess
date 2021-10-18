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
    BoardManager bm;
    bool isCommander;
    Heuristics h = new Heuristics();
    Piece piece;

    public int[] getMove(Piece[,] board, Piece piece, bool isCommander)
    {
        this.board = board;
        this.piece = piece;
        this.isCommander = isCommander;

        return bestLocal();
    }

    public int eval(Vector2Int position) //sends board to heuristic to obtain a score for the move made
    {
        String p = Convert.ToString(Convert.ToChar(position.x + 65) + Convert.ToString(position.y + 1));



        return 0;
    }

    public bool[,] possibleMoves(Piece p) //Uses Bishop script to obtain possible moves for Bishop
    {
        return bm.AvailableMoves(p);
    }

    public int[] bestLocal() //obtains best possible move for Bishop
    {
        int[] move = new int[3];
        int pieceY = 0;
        int pieceX = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                for (int k = 0; k < 8; k++)
                {
                    for (int d = 0; d < 8; d++)
                    {
                        if (board[k,d] == piece)
                        {
                            k = pieceX;
                            d = pieceY;
                        }
                    }
                }
                if (board[i,j] == null && possibleMoves(piece)[i,j] == true)
                {
                    
                    Piece temp = board[i,j];
                    board[i,j] = piece;
                    board[pieceX,pieceY] = null;
                    int dice = UnityEngine.Random.Range(1, 6);
                    int score = minimax(0,board, false, dice);
                    board[i,j] = temp;
                    
                    if (score > move[0]) //if score obtained is better than bestLocalScore
                    {
                        move[2] = score; //than score obtained is bestLocalScore
                        move[0] = i; //x and y coordinates of best scoring move are recorded
                        move[1] = j;
                    }
                }
            }
        }

        return move;
    }

    public int minimax(int depth, Piece[,] tempBoard, bool maximize, int dice) //uses minimax algorithm to obtain the score
    {
        int score = eval(piece.Position); //uses heuristic to obtain score
        int pieceY = 0;
        int pieceX = 0;

        if(depth == 1)
        {
            return score; //if specified depth is hit return score
        }

        if (maximize == true) //if maximizing (AIs turn)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++) //Iteratate through board
                {
                    for (int k = 0; k < 8; k++)
                    {
                        for (int d = 0; d < 8; d++) //Iterate through board
                        {
                            if (tempBoard[k,d] == piece) //finds position of board on piece
                            {
                                k = pieceX;
                                d = pieceY;
                            }
                        }
                    }
                    if (possibleMoves(piece)[i,j] == true) //If space is available
                    {
                        tempBoard[i,j] = piece; //move piece
                        tempBoard[pieceX,pieceY] = null; //move empty space to  pieces previous position
                        dice = UnityEngine.Random.Range(1, 6);
                        score = Math.Max(score, minimax(depth+1, tempBoard, false, dice));
                        tempBoard[i,j] = null;
                        tempBoard[pieceX,pieceY] = piece;
                    }
                }
            }
            return score;
        }

        if (maximize == false)
        {
            Piece tempPiece = tempBoard[0,0];
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {
                        for (int d = 0; d < 8; d++)
                        {
                            if (bm.IsEnemyPieceAt(false,tempBoard[k,d].Position) == true)
                            {
                                tempPiece = tempBoard[k,d];
                                k = pieceX;
                                d = pieceY;
                            }
                        }
                    }
                    if (possibleMoves(tempPiece)[i, j] == true)
                    {
                        tempBoard[i, j] = tempPiece;
                        tempBoard[pieceX, pieceY] = null;
                        dice = UnityEngine.Random.Range(1, 6);
                        score = Math.Min(score, minimax(depth + 1, tempBoard, true, dice));
                        tempBoard[i, j] = null;
                        tempBoard[pieceX, pieceY] = tempPiece;
                    }
                }
            }
            return score; 
        }

        return score;
    }
}
