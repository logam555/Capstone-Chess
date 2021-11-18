//Written by Hamza Khan
//last edited: 10/16/2021 - 4:30
//V2.0
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BestMove
{
    ChessPiece[,] board;
    bool isCommander;
    //Heuristics h = new Heuristics();
    ChessPiece piece;


    public BestMove()
    {

    }

    public int[] getMove(ChessPiece[,] board, ChessPiece piece, bool isCommander)
    {
        this.board = board;
        this.piece = piece;
        this.isCommander = isCommander;

        return bestLocal();
    }

    //add Piece p into eval call
    public Vector3Int eval() //sends board to heuristic to obtain a score for the move made
    {
        Vector3Int posValue = ModelManager.Instance.GetHighestValueFromBoardBlack();

        if (piece is King)
        {
            posValue.z /= 5;
        }
        else if (piece is Bishop)
        {
            posValue.z /= 3;
        }

        return posValue;
    }

    public List<Vector2Int> possibleMoves(ChessPiece p) //Uses Bishop script to obtain possible moves for Bishop
    {
        return ChessBoard.Instance.FilterMoveRange(p, board).Union(ChessBoard.Instance.FilterAttackRange(p, board)).ToList();
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
                pieceX = piece.Position.x;
                pieceY = piece.Position.y;
                if (possibleMoves(piece).Contains(new Vector2Int(i, j)))
                {

                    ChessPiece temp = board[i, j];
                    board[i, j] = piece;
                    board[pieceX, pieceY] = null;
                    int score = minimax(0, board, true, int.MinValue, int.MaxValue);
                    board[i, j] = temp;
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

    public int minimax(int depth, ChessPiece[,] tempBoard, bool maximize, int alpha, int beta) //uses minimax algorithm to obtain the score
    {
        Vector3Int evalsV3 = eval(); //uses heuristic to obtain score
        //Vector3Int evalsV3 = eval(piece); //uses heuristic to obtain score
        int score = evalsV3.z;
        int pieceY = 0;
        int pieceX = 0;
        int bestVal = score;

        if (depth == 1)
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
                    pieceX = piece.Position.x;
                    pieceY = piece.Position.y;
                    if (possibleMoves(piece).Contains(new Vector2Int(i, j))) //If space is available
                    {
                        ChessPiece temp = tempBoard[i, j];
                        tempBoard[i, j] = piece; //move piece
                        tempBoard[pieceX, pieceY] = null; //move empty space to  pieces previous position

                        //board tile location update values to send to board update
                        bool pieceWhite = new bool();

                        if(j < 4 && i > 2 && i < 5 || i > 0 || j > 0)
                        {
                            score *= 2;
                        }
                        if(j < 4 && ChessBoard.Instance.PieceAt(new Vector2Int(i, j), tempBoard) is King)
                        {
                            score *= 15;
                        }
                        if (j < 4 && ChessBoard.Instance.PieceAt(new Vector2Int(i, j), tempBoard) is Queen)
                        {
                            score *= 10;
                        }
                        if (j < 4 && ChessBoard.Instance.PieceAt(new Vector2Int(i, j), tempBoard) is Bishop)
                        {
                            score *= 5;
                        }

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
                        ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(pieceX, pieceY), new Vector2Int(i, j), pieceWhite, pieceTypeStr);

                        //board wide huer tile only update
                        ModelManager.Instance.BoardWideHeuristicTileCall(i, j);

                        score = minimax(depth, tempBoard, false, alpha, beta);
                        bestVal = Math.Max(bestVal, score);
                        alpha = Math.Max(alpha, bestVal);
                        tempBoard[i, j] = temp;
                        tempBoard[pieceX, pieceY] = piece;

                        ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(i, j), new Vector2Int(pieceX, pieceY), pieceWhite, pieceTypeStr);
                        ModelManager.Instance.BoardWideHeuristicTileCall(pieceX, pieceY);

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
            ChessPiece tempPiece = tempBoard[0, 0];
            for (int i = 0; i < 8; i++)//x value of board
            {
                for (int j = 0; j < 8; j++)//y value of board
                {
                    if (ChessBoard.Instance.IsEnemyPieceAt(piece.IsWhite, new Vector2Int(i, j), board))
                    {
                        tempPiece = tempBoard[i, j];
                        pieceX = i;
                        pieceY = j;

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

                        foreach (Vector2Int pos in possibleMoves(tempPiece))
                        {
                            ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(pieceX, pieceY), pos, pieceWhite, pieceTypeStr);

                            //board wide huer tile only update

                            ModelManager.Instance.BoardWideHeuristicTileCall(pos.x, pos.y);

                            ChessPiece temp = tempBoard[pos.x, pos.y];
                            tempBoard[pos.x, pos.y] = tempPiece;
                            tempBoard[pieceX, pieceY] = null;
                            score = minimax(depth + 1, tempBoard, true, alpha, beta);
                            bestVal = Math.Min(bestVal, score);
                            beta = Math.Min(beta, bestVal);
                            tempBoard[pos.x, pos.y] = temp;
                            tempBoard[pieceX, pieceY] = tempPiece;

                            ModelManager.Instance.BoardTileLocationUpdate(pos, new Vector2Int(pieceX, pieceY), pieceWhite, pieceTypeStr);
                            ModelManager.Instance.BoardWideHeuristicTileCall(pieceX, pieceY);
                        }

                        //update board with temp move to check values in min/max
                    }
                }
                if (beta <= alpha)
                {
                    return bestVal;
                }
            }
            return bestVal;
        }

        return bestVal;
    }
}
