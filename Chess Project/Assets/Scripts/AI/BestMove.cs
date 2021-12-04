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
    Dictionary<string, BoardTile> chessBoardGridCo;
    List<Commander> commanders;
    Commander enemyCommander;

    public struct Moves {
        public ChessPiece piece;
        public Vector2Int targetPos;
        public int score;

        public Moves(ChessPiece piece, Vector2Int targetPos, int score) {
            this.piece = piece;
            this.targetPos = targetPos;
            this.score = score;
        }
    }


    public BestMove(ChessPiece[,] board)
    {
        commanders = new List<Commander>();
        this.board = board;
        commanders.Add((Commander)board[4, 0]);
        commanders.Add((Commander)board[2, 0]);
        commanders.Add((Commander)board[5, 0]);
    }

    public int[] getMove(ChessPiece piece, bool isLeft, bool free)
    {
        Commander leader;
        if(piece is Knight) {
            leader = ((Knight)piece).Commander;
        }
        else
            leader = (Commander)piece;

        if(leader is King) {
            chessBoardGridCo = ModelManager.Instance.chessBoardCopyKing;
            enemyCommander = commanders[0];
        }
        else if(leader is Bishop && isLeft) {
            chessBoardGridCo = ModelManager.Instance.chessBoardCopyBishop1;
            enemyCommander = commanders[1];
        }
        else {
            chessBoardGridCo = ModelManager.Instance.chessBoardCopyBishop2;
            enemyCommander = commanders[2];
        }

        if (piece is Knight)
            return bestLocalKnight(piece);

        return bestLocal(leader, free);
    }

    private int PieceValue(ChessPiece piece) {
        if (piece is Pawn)
            return 1;
        else if (piece is Rook)
            return 3;
        else if (piece is Knight)
            return 3;
        else if (piece is Bishop)
            return 5;
        else if (piece is Queen)
            return 2;
        else
            return 10;
    }

    //add Piece p into eval call
    public List<Moves> eval(Commander commander, ChessPiece[,] board) //sends board to heuristic to obtain a score for the move made
    {
        List<Moves> moves = new List<Moves>();
        List<ChessPiece> pieces = new List<ChessPiece>();
        pieces.Add(commander);
        pieces = pieces.Union(commander.subordinates).ToList();

        foreach(ChessPiece piece in pieces) {
            var heuristics = IndividualPieceScanner.Instance.singleScanner(commander.IsWhite, piece.Position, board);
            foreach (Vector2Int pos in possibleMoves(piece)) {
                float scan = heuristics[pos.x, pos.y].Heuristic;
                float boardH = ModelManager.Instance.BoardTileHeuristicValueReturn(pos.x, pos.y, commander.IsWhite).z;
                int score = -1; // board heuristics + scanner heruistics
                if (pos == commander.Position) {
                    score = (int)((scan + 1) * boardH);
                } else
                    score = (int)((scan + 1) * boardH);
                moves.Add(new Moves(piece, pos, score));
            }
        }

        return moves;
        
    }

    public List<Moves> evalFree(ChessPiece commander, ChessPiece[,] board) {
        List<Moves> moves = new List<Moves>();
        var heuristics = IndividualPieceScanner.Instance.singleScanner(commander.IsWhite, commander.Position, board);
        foreach (Vector2Int pos in possibleMoves(commander)) {
            float scan = heuristics[pos.x, pos.y].Heuristic;
            float boardH = ModelManager.Instance.BoardTileHeuristicValueReturn(pos.x, pos.y, commander.IsWhite).z;
            int score = -1; // board heuristics + scanner heruistics
            if (pos == commander.Position) {
                score = (int)((scan + 1) * boardH / 4.0f) - PieceValue(commander);
            } else
                score = (int)((scan + 1) * boardH) - PieceValue(commander);
            moves.Add(new Moves(commander, pos, score));
        }

        return moves;
    }

    public List<Vector2Int> possibleMoves(ChessPiece p) //Uses Bishop script to obtain possible moves for Bishop
    {
        List<Vector2Int> startPos = new List<Vector2Int>();
        startPos.Add(p.Position);
        return ChessBoard.Instance.FilterMoveRange(p, board).Union(ChessBoard.Instance.FilterAttackRange(p, board)).Union(startPos).ToList();
    }

    public int[] bestLocal(Commander piece, bool free) //obtains best possible move for Bishop
    {
        int[] move = new int[4];
        Moves bestMove = new Moves();
        int bestScore;
        int counter = 0;

        List<Moves> moves = !free ? eval(piece, board) : evalFree(piece, board);

        bestScore = int.MinValue;

        foreach(Moves possibleMove in moves) {
            int pieceX = possibleMove.piece.Position.x;
            int pieceY = possibleMove.piece.Position.y;

            ChessPiece temp = board[possibleMove.targetPos.x, possibleMove.targetPos.y];
            board[pieceX, pieceY] = null;
            board[possibleMove.targetPos.x, possibleMove.targetPos.y] = possibleMove.piece;
            possibleMove.piece.Position = possibleMove.targetPos;

            string pieceTypeStr = "";

            if (possibleMove.piece is King) {
                pieceTypeStr = "King";
            } else if (possibleMove.piece is Queen) {
                pieceTypeStr = "Queen";
            } else if (possibleMove.piece is Bishop) {
                pieceTypeStr = "Bishop";
            } else if (possibleMove.piece is Knight) {
                pieceTypeStr = "Knight";
            } else if (possibleMove.piece is Rook) {
                pieceTypeStr = "Rook";
            } else {
                pieceTypeStr = "Pawn";
            }

            //update board with temp move to check values in min/max
            ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(pieceX, pieceY), new Vector2Int(possibleMove.targetPos.x, possibleMove.targetPos.y), possibleMove.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
            //board wide huer tile only update
            ModelManager.Instance.BoardWideHeuristicTileCall(possibleMove.targetPos.x, possibleMove.targetPos.y, chessBoardGridCo);

            int j = possibleMove.score + minimax(1, piece, board, false, int.MinValue, int.MaxValue, free);
            if (j >= bestScore) {
                bestScore = j;
                bestMove = possibleMove;
            }

            board[possibleMove.targetPos.x, possibleMove.targetPos.y] = temp;
            board[pieceX, pieceY] = possibleMove.piece;
            possibleMove.piece.Position = new Vector2Int(pieceX, pieceY);

            ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(possibleMove.targetPos.x, possibleMove.targetPos.y), new Vector2Int(pieceX, pieceY), possibleMove.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
            ModelManager.Instance.BoardWideHeuristicTileCall(pieceX, pieceY, chessBoardGridCo);
        }

        move[0] = bestMove.piece.Position.x;
        move[1] = bestMove.piece.Position.y;
        move[2] = bestMove.targetPos.x;
        move[3] = bestMove.targetPos.y;
        Debug.Log(bestMove.score);

        return move;
    }

    public int[] bestLocalKnight(ChessPiece piece) {
        int[] move = new int[4];
        Moves bestMove = new Moves();
        int bestScore;
        int counter = 0;

        List<Moves> moves = evalFree(piece, board);

        bestScore = int.MinValue;

        foreach (Moves possibleMove in moves) {
            int pieceX = possibleMove.piece.Position.x;
            int pieceY = possibleMove.piece.Position.y;

            ChessPiece temp = board[possibleMove.targetPos.x, possibleMove.targetPos.y];
            board[pieceX, pieceY] = null;
            board[possibleMove.targetPos.x, possibleMove.targetPos.y] = possibleMove.piece;
            possibleMove.piece.Position = possibleMove.targetPos;

            string pieceTypeStr = "";

            if (possibleMove.piece is King) {
                pieceTypeStr = "King";
            } else if (possibleMove.piece is Queen) {
                pieceTypeStr = "Queen";
            } else if (possibleMove.piece is Bishop) {
                pieceTypeStr = "Bishop";
            } else if (possibleMove.piece is Knight) {
                pieceTypeStr = "Knight";
            } else if (possibleMove.piece is Rook) {
                pieceTypeStr = "Rook";
            } else {
                pieceTypeStr = "Pawn";
            }

            //update board with temp move to check values in min/max
            ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(pieceX, pieceY), new Vector2Int(possibleMove.targetPos.x, possibleMove.targetPos.y), possibleMove.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
            //board wide huer tile only update
            ModelManager.Instance.BoardWideHeuristicTileCall(possibleMove.targetPos.x, possibleMove.targetPos.y, chessBoardGridCo);

            int j = possibleMove.score + minimax(1, piece, board, false, int.MinValue, int.MaxValue, true);
            if (j >= bestScore) {
                bestScore = j;
                bestMove = possibleMove;
            }

            board[possibleMove.targetPos.x, possibleMove.targetPos.y] = temp;
            board[pieceX, pieceY] = possibleMove.piece;
            possibleMove.piece.Position = new Vector2Int(pieceX, pieceY);

            ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(possibleMove.targetPos.x, possibleMove.targetPos.y), new Vector2Int(pieceX, pieceY), possibleMove.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
            ModelManager.Instance.BoardWideHeuristicTileCall(pieceX, pieceY, chessBoardGridCo);
        }

        move[0] = bestMove.piece.Position.x;
        move[1] = bestMove.piece.Position.y;
        move[2] = bestMove.targetPos.x;
        move[3] = bestMove.targetPos.y;
        Debug.Log(bestMove.score);

        return move;
    }

    //public int minimax(int depth, int nodeIndex, bool isMax, int[] scores, int h) {
    //    if(depth == h)
    //        return scores[nodeIndex];
    //    if (isMax)
    //        return Math.Max(minimax(depth + 1, nodeIndex * 2, false, scores, h),
    //            minimax(depth + 1, nodeIndex * 2 + 1, false, scores, h));
    //    else
    //        return Math.Min(minimax(depth + 1, nodeIndex * 2, false, scores, h),
    //            minimax(depth + 1, nodeIndex * 2 + 1, false, scores, h));
    //}
    
    public int minimax(int depth, ChessPiece commander, ChessPiece[,] tempBoard, bool maximize, int alpha, int beta, bool free) //uses minimax algorithm to obtain the score
    {
        //Vector3Int evalsV3 = eval(piece); //uses heuristic to obtain score
        int score;
        int pieceY = -1;
        int pieceX = -1;
        int bestVal;

        if (depth == 4)
        {
            if (maximize) {
                List<Moves> evals = !free ? eval((Commander)commander, tempBoard) : evalFree(commander, tempBoard);
                int bestScore = int.MinValue;
                foreach (Moves move in evals) {
                    if (move.score > bestScore) {

                        bestScore = move.score;
                    }
                }

                return bestScore; //if specified depth is hit return score

            }
            else {
                List<Moves> evals = !free ? eval((Commander)commander, tempBoard) : evalFree(commander, tempBoard);
                int bestScore = int.MaxValue;
                foreach (Moves move in evals) {
                    if (move.score < bestScore) {

                        bestScore = move.score;
                    }
                }

                return bestScore; //if specified depth is hit return score
            }
        }

        if (maximize == true) //if maximizing (AIs turn)
        {
            List<Moves> moves = !free ? eval((Commander)commander, tempBoard) : evalFree(commander, tempBoard);
            bestVal = int.MinValue;

            foreach (Moves move in moves) {
                pieceX = move.piece.Position.x;
                pieceY = move.piece.Position.y;

                ChessPiece temp = tempBoard[move.targetPos.x, move.targetPos.y];
                tempBoard[pieceX, pieceY] = null;
                tempBoard[move.targetPos.x, move.targetPos.y] = move.piece;
                move.piece.Position = move.targetPos;

                string pieceTypeStr = "";

                if (move.piece is King) {
                    pieceTypeStr = "King";
                } else if (move.piece is Queen) {
                    pieceTypeStr = "Queen";
                } else if (move.piece is Bishop) {
                    pieceTypeStr = "Bishop";
                } else if (move.piece is Knight) {
                    pieceTypeStr = "Knight";
                } else if (move.piece is Rook) {
                    pieceTypeStr = "Rook";
                } else {
                    pieceTypeStr = "Pawn";
                }

                //update board with temp move to check values in min/max
                ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(pieceX, pieceY), new Vector2Int(move.targetPos.x, move.targetPos.y), move.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
                //board wide huer tile only update
                ModelManager.Instance.BoardWideHeuristicTileCall(move.targetPos.x, move.targetPos.y, chessBoardGridCo);

                score = move.score + minimax(depth + 1, commander, tempBoard, false, alpha, beta, free);
                if (score > bestVal)
                    bestVal = score;
                alpha = Math.Max(alpha, score);


                tempBoard[move.targetPos.x, move.targetPos.y] = temp;
                tempBoard[pieceX, pieceY] = move.piece;
                move.piece.Position = new Vector2Int(pieceX, pieceY);

                ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(move.targetPos.x, move.targetPos.y), new Vector2Int(pieceX, pieceY), move.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
                ModelManager.Instance.BoardWideHeuristicTileCall(pieceX, pieceY, chessBoardGridCo);

                if (beta <= alpha) {
                    break;
                }
            }
            return bestVal;
        } else {
            List<Moves> moves = eval(enemyCommander, tempBoard);
            bestVal = int.MaxValue;

            foreach (Moves move in moves) {
                pieceX = move.piece.Position.x;
                pieceY = move.piece.Position.y;

                ChessPiece temp = tempBoard[move.targetPos.x, move.targetPos.y];
                tempBoard[pieceX, pieceY] = null;
                tempBoard[move.targetPos.x, move.targetPos.y] = move.piece;
                move.piece.Position = move.targetPos;

                string pieceTypeStr = "";

                if (move.piece is King) {
                    pieceTypeStr = "King";
                } else if (move.piece is Queen) {
                    pieceTypeStr = "Queen";
                } else if (move.piece is Bishop) {
                    pieceTypeStr = "Bishop";
                } else if (move.piece is Knight) {
                    pieceTypeStr = "Knight";
                } else if (move.piece is Rook) {
                    pieceTypeStr = "Rook";
                } else {
                    pieceTypeStr = "Pawn";
                }

                //update board with temp move to check values in min/max
                ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(pieceX, pieceY), new Vector2Int(move.targetPos.x, move.targetPos.y), move.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
                //board wide huer tile only update
                ModelManager.Instance.BoardWideHeuristicTileCall(move.targetPos.x, move.targetPos.y, chessBoardGridCo);

                score = move.score + minimax(depth + 1, commander, tempBoard, true, alpha, beta, free);
                if (score < bestVal)
                    bestVal = score;
                beta = Math.Min(alpha, score);


                tempBoard[move.targetPos.x, move.targetPos.y] = temp;
                tempBoard[pieceX, pieceY] = move.piece;
                move.piece.Position = new Vector2Int(pieceX, pieceY);

                ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(move.targetPos.x, move.targetPos.y), new Vector2Int(pieceX, pieceY), move.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
                ModelManager.Instance.BoardWideHeuristicTileCall(pieceX, pieceY, chessBoardGridCo);

                if (beta <= alpha) {
                    break;
                }
            }
            return bestVal;
        }
    }
}


//int piece1X;
//int piece1Y;
//int piece2X;
//int piece2Y;
//int piece3X;
//int piece3Y;
//ChessPiece temp1;
//ChessPiece temp2 = null;
//ChessPiece temp3 = null;

//List<Moves> moves1 = eval(commanders[0], tempBoard);
//List<Moves> moves2 = eval(commanders[1], tempBoard);
//List<Moves> moves3 = eval(commanders[2], tempBoard);

//bestVal = int.MaxValue;

//foreach (Moves move1 in moves1) {
//    foreach (Moves move2 in moves2) {
//        foreach (Moves move3 in moves3) {
//            piece1X = move1.piece.Position.x;
//            piece1Y = move1.piece.Position.y;
//            piece2X = move2.piece.Position.x;
//            piece2Y = move2.piece.Position.y;
//            piece3X = move3.piece.Position.x;
//            piece3Y = move3.piece.Position.y;

//            temp1 = tempBoard[move1.targetPos.x, move1.targetPos.y];
//            tempBoard[piece1X, piece1Y] = null;
//            tempBoard[move1.targetPos.x, move1.targetPos.y] = move1.piece;
//            move1.piece.Position = move1.targetPos;

//            string pieceTypeStr = "";

//            if (move1.piece is King) {
//                pieceTypeStr = "King";
//            } else if (move1.piece is Queen) {
//                pieceTypeStr = "Queen";
//            } else if (move1.piece is Bishop) {
//                pieceTypeStr = "Bishop";
//            } else if (move1.piece is Knight) {
//                pieceTypeStr = "Knight";
//            } else if (move1.piece is Rook) {
//                pieceTypeStr = "Rook";
//            } else {
//                pieceTypeStr = "Pawn";
//            }

//            //update board with temp move to check values in min/max
//            ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(piece1X, piece1Y), new Vector2Int(move1.targetPos.x, move1.targetPos.y), move1.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
//            //board wide huer tile only update
//            ModelManager.Instance.BoardWideHeuristicTileCall(move1.targetPos.x, move1.targetPos.y, chessBoardGridCo);

//            if (move2.targetPos != move1.targetPos) {
//                temp2 = tempBoard[move2.targetPos.x, move2.targetPos.y];
//                tempBoard[piece2X, piece2Y] = null;
//                tempBoard[move2.targetPos.x, move2.targetPos.y] = move2.piece;
//                move2.piece.Position = move2.targetPos;

//                pieceTypeStr = "";

//                if (move2.piece is King) {
//                    pieceTypeStr = "King";
//                } else if (move2.piece is Queen) {
//                    pieceTypeStr = "Queen";
//                } else if (move2.piece is Bishop) {
//                    pieceTypeStr = "Bishop";
//                } else if (move2.piece is Knight) {
//                    pieceTypeStr = "Knight";
//                } else if (move2.piece is Rook) {
//                    pieceTypeStr = "Rook";
//                } else {
//                    pieceTypeStr = "Pawn";
//                }

//                //update board with temp move to check values in min/max
//                ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(piece2X, piece2Y), new Vector2Int(move2.targetPos.x, move2.targetPos.y), move2.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
//                //board wide huer tile only update
//                ModelManager.Instance.BoardWideHeuristicTileCall(move2.targetPos.x, move2.targetPos.y, chessBoardGridCo);
//            }

//            if (move3.targetPos != move1.targetPos && move3.targetPos != move2.targetPos) {
//                temp3 = tempBoard[move3.targetPos.x, move3.targetPos.y];
//                tempBoard[piece3X, piece3Y] = null;
//                tempBoard[move3.targetPos.x, move3.targetPos.y] = move3.piece;
//                move3.piece.Position = move3.targetPos;

//                pieceTypeStr = "";

//                if (move3.piece is King) {
//                    pieceTypeStr = "King";
//                } else if (move3.piece is Queen) {
//                    pieceTypeStr = "Queen";
//                } else if (move3.piece is Bishop) {
//                    pieceTypeStr = "Bishop";
//                } else if (move3.piece is Knight) {
//                    pieceTypeStr = "Knight";
//                } else if (move3.piece is Rook) {
//                    pieceTypeStr = "Rook";
//                } else {
//                    pieceTypeStr = "Pawn";
//                }

//                //update board with temp move to check values in min/max
//                ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(piece3X, piece3Y), new Vector2Int(move3.targetPos.x, move3.targetPos.y), move3.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
//                //board wide huer tile only update
//                ModelManager.Instance.BoardWideHeuristicTileCall(move3.targetPos.x, move3.targetPos.y, chessBoardGridCo);
//            }

//            score = minimax(depth + 1, commander, tempBoard, true, alpha, beta, free);
//            if (score < bestVal)
//                bestVal = score;
//            beta = Math.Max(beta, score);

//            tempBoard[move1.targetPos.x, move1.targetPos.y] = temp1;
//            tempBoard[piece1X, piece1Y] = move1.piece;
//            move1.piece.Position = new Vector2Int(piece1X, piece1Y);

//            ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(move1.targetPos.x, move1.targetPos.y), new Vector2Int(piece1X, piece1Y), move1.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
//            ModelManager.Instance.BoardWideHeuristicTileCall(piece1X, piece1Y, chessBoardGridCo);

//            if (move2.targetPos != move1.targetPos) {
//                tempBoard[move2.targetPos.x, move2.targetPos.y] = temp2;
//                tempBoard[piece2X, piece2Y] = move2.piece;
//                move2.piece.Position = new Vector2Int(piece2X, piece2Y);

//                ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(move2.targetPos.x, move2.targetPos.y), new Vector2Int(piece2X, piece2Y), move2.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
//                ModelManager.Instance.BoardWideHeuristicTileCall(piece2X, piece2Y, chessBoardGridCo);
//            }

//            if (move3.targetPos != move1.targetPos && move3.targetPos != move2.targetPos) {
//                tempBoard[move3.targetPos.x, move3.targetPos.y] = temp3;
//                tempBoard[piece3X, piece3Y] = move3.piece;
//                move3.piece.Position = new Vector2Int(piece3X, piece3Y);

//                ModelManager.Instance.BoardTileLocationUpdate(new Vector2Int(move3.targetPos.x, move3.targetPos.y), new Vector2Int(piece3X, piece3Y), move3.piece.IsWhite, pieceTypeStr, chessBoardGridCo);
//                ModelManager.Instance.BoardWideHeuristicTileCall(piece3X, piece3Y, chessBoardGridCo);
//            }

//            if (beta <= alpha) {
//                break;
//            }
//        }
//    }
//}

//return bestVal;
//        }