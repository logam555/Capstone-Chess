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
    //Heuristics h = new Heuristics();
    Piece piece;

    private ModelManager boardModel;
    private GameObject tempGO;


    public BestMove()
    {
        //GameObject tempGO = new GameObject();
        tempGO = GameObject.Find("Chess Board");

        boardModel = new ModelManager();
        boardModel = tempGO.GetComponent<ModelManager>();
    }

    public int[] getMove(Piece[,] board, Piece piece, bool isCommander, BoardManager bm)
    {
        this.board = board;
        this.piece = piece;
        this.isCommander = isCommander;

        this.bm = bm;

        return bestLocal();
    }

    public int eval() //sends board to heuristic to obtain a score for the move made
    {
        Vector3Int posValue = new Vector3Int();
        posValue = boardModel.GetHighestValueFromBoard();
        
        int highestValue = new int();
        highestValue = posValue.z;

        return highestValue;
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
                            pieceX = k;
                            pieceY = d;
                        }
                    }
                }
                if (possibleMoves(piece)[i,j] == true)
                {
                    
                    Piece temp = board[i,j];
                    board[i,j] = piece;
                    board[pieceX,pieceY] = null;
                    int dice = UnityEngine.Random.Range(1, 6);
                    int score = minimax(0,board, true, dice);
                    board[i,j] = temp;
                    board[pieceX, pieceY] = piece;
                    
                    if (score > move[2]) //if score obtained is better than bestLocalScore
                    {
                        move[2] = score; //than score obtained is bestLocalScore
                        move[0] = i; //x and y coordinates of best scoring move are recorded
                        move[1] = j;
                    }
                }
                
            }
        }

        //if (piece.EnemiesInRange().Count > 0) {
        //    foreach(Vector2Int pos in piece.EnemiesInRange()) {
        //        Piece temp = board[pos.x, pos.y];
        //        board[pos.x, pos.y] = piece;
        //        board[pieceX, pieceY] = null;
        //        int dice = UnityEngine.Random.Range(1, 6);
        //        int score = minimax(0, board, true, dice);
        //        board[pos.x, pos.y] = temp;
        //        board[pieceX, pieceY] = piece;

        //        if (score > move[2]) //if score obtained is better than bestLocalScore
        //        {
        //            move[2] = score; //than score obtained is bestLocalScore
        //            move[0] = pos.x; //x and y coordinates of best scoring move are recorded
        //            move[1] = pos.y;
        //        }
        //    }
           
        //}

        return move;
    }

    public int minimax(int depth, Piece[,] tempBoard, bool maximize, int dice) //uses minimax algorithm to obtain the score
    {
        int score = eval(); //uses heuristic to obtain score
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
                                pieceX = k;
                                pieceY = d;
                            }
                        }
                    }
                    if (possibleMoves(piece)[i,j] == true) //If space is available
                    {
                        tempBoard[i,j] = piece; //move piece
                        tempBoard[pieceX,pieceY] = null; //move empty space to  pieces previous position
                        dice = UnityEngine.Random.Range(1, 6);
                        score = Math.Max(score, minimax(depth, tempBoard, false, dice));
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
                            if (bm.IsEnemyPieceAt(piece.IsWhite,new Vector2Int(k,d)) == true)
                            {
                                tempPiece = tempBoard[k,d];
                                pieceX = k;
                                pieceY = d;

                                if (possibleMoves(tempPiece)[i, j] == true) {
                                    tempBoard[i, j] = tempPiece;
                                    tempBoard[pieceX, pieceY] = null;
                                    dice = UnityEngine.Random.Range(1, 6);
                                    score = Math.Min(score, minimax(depth + 1, tempBoard, true, dice));
                                    tempBoard[i, j] = null;
                                    tempBoard[pieceX, pieceY] = tempPiece;
                                }
                            }
                        }
                    }
                    
                }
            }
            return score; 
        }

        return score;
    }
}
