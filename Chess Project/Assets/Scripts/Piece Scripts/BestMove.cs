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
        this.board = bm.Pieces;
        this.piece = piece;
        this.isCommander = isCommander;

        this.bm = bm;

        return bestLocal();
    }

    //add Piece p into eval call
    public Vector3Int eval() //sends board to heuristic to obtain a score for the move made
    {
        Vector3Int posValue = boardModel.GetHighestValueFromBoard();

        //change to be made below
        //Vector3Int posValue = GetHighestValueFromTileMoveRange(p);

        //int highestValue = posValue.z;

        return posValue;
    }

    public bool[,] possibleMoves(Piece p) //Uses Bishop script to obtain possible moves for Bishop
    {
        return bm.AvailableMoves(p);
    }

    public bool possibleAttack(int x, int y)
    {
        return bm.CheckMove(new Vector2Int(x, y));
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
                if (possibleMoves(piece)[i,j] == true || possibleAttack(i,j) == true)
                {
                    
                    Piece temp = board[i,j];
                    board[i,j] = piece;
                    board[pieceX,pieceY] = null;
                    int dice = UnityEngine.Random.Range(1, 6);
                    int score = minimax(0,board, true, dice, int.MinValue, int.MaxValue);
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
        return move;
    }

    public int minimax(int depth, Piece[,] tempBoard, bool maximize, int dice, int alpha, int beta) //uses minimax algorithm to obtain the score
    {
        Vector3Int evalsV3 = eval(); //uses heuristic to obtain score
        //Vector3Int evalsV3 = eval(piece); //uses heuristic to obtain score
        int score = evalsV3.z;
        int pieceY = 0;
        int pieceX = 0;
        int bestVal = score;

        if(depth == 1)
        {
            return score; //if specified depth is hit return score
        }

        if (maximize == true) //if maximizing (AIs turn)
        {
            bestVal = int.MinValue;
            for (int i = 0; i < 8; i++)//x value of board
            {
                for (int j = 0; j < 8; j++) //Iteratate through board, y value of board
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

                        //board tile location update values to send to board update
                        bool pieceWhite = new bool();

                        if (piece.IsWhite)
                        {
                            pieceWhite = true;
                        }
                        else
                        {
                            pieceWhite = false;
                        }

                        string pieceTypeStr = "";

                        if (piece is King)
                        {
                            pieceTypeStr = "King";
                        }
                        else if (piece is Queen)
                        {
                            pieceTypeStr = "Queen";
                        }
                        else if (piece is Bishop)
                        {
                            pieceTypeStr = "Bishop";
                        }
                        else if (piece is Knight)
                        {
                            pieceTypeStr = "Knight";
                        }
                        else if (piece is Rook)
                        {
                            pieceTypeStr = "Rook";
                        }
                        else
                        {
                            pieceTypeStr = "Pawn";
                        }

                        //update board with temp move to check values in min/max
                        boardModel.BoardTileLocationUpdate(new Vector2Int(pieceX,pieceY), new Vector2Int(i,j), pieceWhite, pieceTypeStr);

                        //board wide huer tile only update
                        boardModel.BoardWideHeuristicTileCall(i,j);

                        dice = UnityEngine.Random.Range(1, 6);
                        score = minimax(depth, tempBoard, false, dice,alpha,beta);
                        bestVal = Math.Max(bestVal, score);
                        alpha = Math.Max(alpha, bestVal);
                        tempBoard[i,j] = null;
                        tempBoard[pieceX,pieceY] = piece;
                        
                        boardModel.BoardTileLocationUpdate(new Vector2Int(i, j), new Vector2Int(pieceX, pieceY), pieceWhite, pieceTypeStr);
                        boardModel.BoardWideHeuristicTileCall(pieceX, pieceY);

                    }
                    if (beta <= alpha)
                    {
                        return bestVal;
                    }
                }
            }
            return bestVal;
        }

        if (maximize == false)
        {
            bestVal = int.MaxValue;
            Piece tempPiece = tempBoard[0,0];
            for (int i = 0; i < 8; i++)//x value of board
            {
                for (int j = 0; j < 8; j++)//y value of board
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

                                bool pieceWhite = new bool();

                                if (tempPiece.IsWhite)
                                {
                                    pieceWhite = true;
                                }
                                else
                                {
                                    pieceWhite = false;
                                }

                                string pieceTypeStr = "";

                                if (tempPiece is King)
                                {
                                    pieceTypeStr = "King";
                                }
                                else if (tempPiece is Queen)
                                {
                                    pieceTypeStr = "Queen";
                                }
                                else if (tempPiece is Bishop)
                                {
                                    pieceTypeStr = "Bishop";
                                }
                                else if (tempPiece is Knight)
                                {
                                    pieceTypeStr = "Knight";
                                }
                                else if (tempPiece is Rook)
                                {
                                    pieceTypeStr = "Rook";
                                }
                                else
                                {
                                    pieceTypeStr = "Pawn";
                                }

                                //update board with temp move to check values in min/max
                                boardModel.BoardTileLocationUpdate(new Vector2Int(pieceX, pieceY), new Vector2Int(i, j), pieceWhite, pieceTypeStr);

                                //board wide huer tile only update
                                boardModel.BoardWideHeuristicTileCall(i, j);

                                if (possibleMoves(tempPiece)[i, j] == true) {
                                    tempBoard[i, j] = tempPiece;
                                    tempBoard[pieceX, pieceY] = null;
                                    dice = UnityEngine.Random.Range(1, 6);
                                    score = minimax(depth + 1, tempBoard, true, dice, alpha, beta);
                                    bestVal = Math.Min(bestVal, score);
                                    beta = Math.Min(beta, bestVal);
                                    tempBoard[i, j] = null;
                                    tempBoard[pieceX, pieceY] = tempPiece;
                                    boardModel.BoardTileLocationUpdate(new Vector2Int(i, j), new Vector2Int(pieceX, pieceY), pieceWhite, pieceTypeStr);
                                    boardModel.BoardWideHeuristicTileCall(pieceX, pieceY);
                                }
                            }
                        }
                        if(beta <= alpha)
                        {
                            return bestVal;
                        }
                    }
                    
                }
            }
            return bestVal; 
        }

        return bestVal;
    }
}
