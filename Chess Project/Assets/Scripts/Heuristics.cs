/* Written by George Brunner
Edited by ___
 Last date edited: 09 / 15 / 2021
 Heuristics.cs - Manages the Heuristics of the board and pieces.
 Version 1: Created functions to collect Heuristics
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heuristics : MonoBehaviour
{
    [Serializable]
    private class chessPiece
    {
        public string pieceType;
        public int typeValue;
        public int moveRange;
        public int attackRange;
        public int distanceToClosetHostile;
        public int numOfHostilePieceInRange;
        public int distanceToClosetFriendly;
        public int numOfFriendlyPieceInRange;
        public bool friendlyInbetweenSelfHostile;
        public bool canMoveSpaces;
        public bool canAttack;
        public bool friendlyCommanderCloseby;
        public bool hostileCommanderCloseby;
    }


    [SerializeField]
    private List<chessPiece> chessTypes;//

    // Start is called before the first frame update
    //might need to remove start if runnign through voard script
    void Awake()
    {
        //list for holding chess piece types
        chessTypes = new List<chessPiece>();

        //fills list with the types of chess pieces
        HeuristicSetup();

        //Fills in peices value
        ChessPieceSetup();

        //uses the difficulty to send values used by Heuristics
        HeuristicDifficulty();
    }

    public void BoardWideHeuristic(ref Dictionary<string, BoardTile> board)
    {
        char letterBoard = '0';
        string showB = "";
        //Debug.Log("H BoardWideHeuristic being called");
        Vector2Int holderV2I = new Vector2Int();

        //setting up tiles and adding to dictionary default values
        for (int j = 1; j < 9; j++)//y value
        {
            for (int i = 65; i < 73; i++)//x value for letter
            {
                letterBoard = Convert.ToChar(i);
                showB = letterBoard.ToString() + j.ToString();

                holderV2I.x = 0;
                holderV2I.y = 0;

                holderV2I = BoardWideHeuristicTile(board, j, i);

                board[showB].whiteHeuristic = holderV2I.x;
                board[showB].blackHeuristic = holderV2I.y;
                //Debug.Log("H BoardWideHeuristic Huer at " + "x " + (i - 65) + " y " + (j - 1) + holderV2I);
            }
        }
    }

    public void HeuristicSetup()
    {
        chessPiece piece = new chessPiece();

        for (int i = 0; i < 6; i++)
        {
            piece = new chessPiece();

            //add in chess piece type for alter
            piece.pieceType = "";
            piece.typeValue = 0;
            piece.moveRange = 0;
            piece.attackRange = 0;
            piece.distanceToClosetHostile = 0;
            piece.numOfHostilePieceInRange = 0;
            piece.distanceToClosetFriendly = 0;
            piece.numOfFriendlyPieceInRange = 0;
            piece.friendlyInbetweenSelfHostile = false;
            piece.canMoveSpaces = false;
            piece.canAttack = false;
            piece.friendlyCommanderCloseby = false;
            piece.hostileCommanderCloseby = false;

            chessTypes.Add(piece);
        }
    }

    //Applies the set values of each chess piece to pieces array
    public void ChessPieceSetup()
    {
        //King
        chessTypes[0].pieceType = "King";
        chessTypes[0].typeValue = 5;
        chessTypes[0].moveRange = 5;
        chessTypes[0].attackRange = 5;

        //Queen
        chessTypes[1].pieceType = "Queen";
        chessTypes[1].typeValue = 5;
        chessTypes[1].moveRange = 5;
        chessTypes[1].attackRange = 5;

        //Bishop
        chessTypes[2].pieceType = "Bishop";
        chessTypes[2].typeValue = 4;
        chessTypes[2].moveRange = 5;
        chessTypes[2].attackRange = 5;

        //Knight
        chessTypes[3].pieceType = "Knight";
        chessTypes[3].typeValue = 3;
        chessTypes[3].moveRange = 5;
        chessTypes[3].attackRange = 5;

        //Rook
        chessTypes[4].pieceType = "Rook";
        chessTypes[4].typeValue = 2;
        chessTypes[4].moveRange = 3;
        chessTypes[4].attackRange = 3;

        //Pawn
        chessTypes[5].pieceType = "Pawn";
        chessTypes[5].typeValue = 1;
        chessTypes[5].moveRange = 1;
        chessTypes[5].attackRange = 1;
    }

    //alter the weights
    public void HeuristicDifficulty()
    {
        int diff = new int();

        diff = PlayerPrefs.GetInt("Difficulty");

        //base/Easy
        chessTypes[0].typeValue = 22;
        chessTypes[1].typeValue = 22;
        chessTypes[2].typeValue = 21;
        chessTypes[3].typeValue = 23;
        chessTypes[4].typeValue = 25;
        chessTypes[5].typeValue = 10;

        //normal
        if (diff == 2)
        {
            //scale
            chessTypes[0].typeValue *= 2;
            chessTypes[1].typeValue *= 2;
            chessTypes[2].typeValue *= 2;
            chessTypes[3].typeValue *= 2;
            chessTypes[4].typeValue *= 2;
            chessTypes[5].typeValue *= 2;
        }

        //hard
        if (diff == 1)
        {
            //scale
            chessTypes[0].typeValue *= 4;
            chessTypes[1].typeValue *= 4;
            chessTypes[2].typeValue *= 4;
            chessTypes[3].typeValue *= 4;
            chessTypes[4].typeValue *= 4;
            chessTypes[5].typeValue *= 4;
        }
    }


    //return vector 3: x and y for position; z for value for Highest White Huer
    public Vector3Int ReturnHighestValueWhite(Dictionary<string, BoardTile> boardPieceValue)
    {
        char letterBoard = '0';
        string showB = "";
        Vector2Int[] position = new Vector2Int[64];
        int index = new int();
        index = 0;
        int[] heurValueHolder = new int[64];

        for (int j = 1; j < 9; j++)
        {
            for (int i = 65; i < 73; i++)
            {
                letterBoard = Convert.ToChar(i);
                showB = letterBoard.ToString() + j.ToString();

                if (boardPieceValue[showB].isWhite == true)
                {
                    heurValueHolder[index] = boardPieceValue[showB].whiteHeuristic;
                    position[index] = boardPieceValue[showB].boardPosition;
                }
                else
                {
                    heurValueHolder[index] = -1;
                    position[index].x = -1;
                    position[index].y = -1;
                }

                index++;
            }
        }

        int highestValueIndex = new int();
        highestValueIndex = 0;
        int highestValue = new int();
        highestValue = 0;

        for (int k = 0; k < index; k++)
        {
            if (highestValue < heurValueHolder[k])
            {
                highestValue = heurValueHolder[k];
                highestValueIndex = k;
            }
        }

        Vector3Int posValue = new Vector3Int();

        posValue.x = position[highestValueIndex].x;
        posValue.y = position[highestValueIndex].y;
        posValue.z = highestValue;

        return posValue;
    }

    //return vector 3: x and y for position; z for value for Highest Black Huer
    public Vector3Int ReturnHighestValueBlack(Dictionary<string, BoardTile> boardPieceValue)
    {
        char letterBoard = '0';
        string showB = "";
        Vector2Int[] position = new Vector2Int[64];
        int index = new int();
        index = 0;
        int[] heurValueHolder = new int[64];

        for (int j = 1; j < 9; j++)
        {
            for (int i = 65; i < 73; i++)
            {
                letterBoard = Convert.ToChar(i);
                showB = letterBoard.ToString() + j.ToString();

                if (boardPieceValue[showB].isWhite == false)
                {
                    heurValueHolder[index] = boardPieceValue[showB].blackHeuristic;
                    position[index] = boardPieceValue[showB].boardPosition;
                }
                else
                {
                    heurValueHolder[index] = -1;
                    position[index].x = -1;
                    position[index].y = -1;
                }

                index++;
            }
        }

        int highestValueIndex = new int();
        highestValueIndex = 0;
        int highestValue = new int();
        highestValue = 0;

        for (int k = 0; k < index; k++)
        {
            if (highestValue < heurValueHolder[k])
            {
                highestValue = heurValueHolder[k];
                highestValueIndex = k;
            }
        }

        Vector3Int posValue = new Vector3Int();

        posValue.x = position[highestValueIndex].x;
        posValue.y = position[highestValueIndex].y;
        posValue.z = highestValue;

        return posValue;
    }

    //return vector 3: x and y for position; z for value for Lowest White Huer
    public Vector3Int ReturnLowestValueWhite(Dictionary<string, BoardTile> boardPieceValue)
    {
        char letterBoard = '0';
        string showB = "";
        Vector2Int[] position = new Vector2Int[64];
        int index = new int();
        index = 0;
        int[] heurValueHolder = new int[64];

        for (int j = 1; j < 9; j++)
        {
            for (int i = 65; i < 73; i++)
            {
                letterBoard = Convert.ToChar(i);
                showB = letterBoard.ToString() + j.ToString();

                if (boardPieceValue[showB].isWhite == true)
                {
                    heurValueHolder[index] = boardPieceValue[showB].blackHeuristic;
                    position[index] = boardPieceValue[showB].boardPosition;
                }
                else
                {
                    heurValueHolder[index] = -1;
                    position[index].x = -1;
                    position[index].y = -1;
                }

                index++;
            }
        }

        int lowestValueIndex = new int();
        lowestValueIndex = 0;
        int lowestValue = new int();
        lowestValue = 0;

        for (int k = 0; k < index; k++)
        {
            if (lowestValue > heurValueHolder[k])
            {
                if (lowestValue > 0)
                {
                    lowestValue = heurValueHolder[k];
                    lowestValueIndex = k;
                }
            }
        }

        Vector3Int posValue = new Vector3Int();

        posValue.x = position[lowestValueIndex].x;
        posValue.y = position[lowestValueIndex].y;
        posValue.z = lowestValue;

        return posValue;
    }

    //return vector 3: x and y for position; z for value for Lowest Black Huer
    public Vector3Int ReturnLowestValueBlack(Dictionary<string, BoardTile> boardPieceValue)
    {
        char letterBoard = '0';
        string showB = "";
        Vector2Int[] position = new Vector2Int[64];
        int index = new int();
        index = 0;
        int[] heurValueHolder = new int[64];

        for (int j = 1; j < 9; j++)
        {
            for (int i = 65; i < 73; i++)
            {
                letterBoard = Convert.ToChar(i);
                showB = letterBoard.ToString() + j.ToString();

                if (boardPieceValue[showB].isWhite == false)
                {
                    heurValueHolder[index] = boardPieceValue[showB].blackHeuristic;
                    position[index] = boardPieceValue[showB].boardPosition;
                }
                else
                {
                    heurValueHolder[index] = -1;
                    position[index].x = -1;
                    position[index].y = -1;
                }

                index++;
            }
        }

        int lowestValueIndex = new int();
        lowestValueIndex = 0;
        int lowestValue = new int();
        lowestValue = 0;

        for (int k = 0; k < index; k++)
        {
            if (lowestValue > heurValueHolder[k])
            {
                if (lowestValue > 0)
                {
                    lowestValue = heurValueHolder[k];
                    lowestValueIndex = k;
                }
            }
        }

        Vector3Int posValue = new Vector3Int();

        posValue.x = position[lowestValueIndex].x;
        posValue.y = position[lowestValueIndex].y;
        posValue.z = lowestValue;

        return posValue;
    }

    public Vector2Int BoardWideHeuristicTile(Dictionary<string, BoardTile> board, int j, int i)
    {
        Vector2Int holderV2I = new Vector2Int();
        holderV2I.x = 0;
        holderV2I.y = 0;

        //check space Primany Tile base i and j values
        holderV2I = holderV2I + TileCalc((i), (j), board);

        //check space Upper Left i - 1 and j + 1
        if (i - 1 >= 65 && i - 1 <= 72)
        {
            if (j + 1 >= 1 && j + 1 <= 8)
            {
                holderV2I = holderV2I + TileCalc((i - 1), (j + 1), board);
            }
        }

        //check space Upper i base and j + 1
        if (i >= 65 && i <= 72)
        {
            if (j + 1 >= 1 && j + 1 <= 8)
            {
                holderV2I = holderV2I + TileCalc((i), (j + 1), board);
            }
        }

        //check space Upper Right i + 1 and j + 1
        if (i + 1 >= 65 && i + 1 <= 72)
        {
            if (j + 1 >= 1 && j + 1 <= 8)
            {
                holderV2I = holderV2I + TileCalc((i + 1), (j + 1), board);
            }
        }

        //check space Left i - 1 and j base
        if (i - 1 >= 65 && i - 1 <= 72)
        {
            if (j >= 1 && j <= 8)
            {
                holderV2I = holderV2I + TileCalc((i - 1), (j), board);
            }
        }

        //check space Right i + 1  and j base
        if (i + 1 >= 65 && i + 1 <= 72)
        {
            if (j >= 1 && j <= 8)
            {
                holderV2I = holderV2I + TileCalc((i + 1), (j), board);
            }
        }

        //check space Lower Left i - 1 and j - 1
        if (i - 1 >= 65 && i - 1 <= 72)
        {
            if (j - 1 >= 1 && j - 1 <= 8)
            {
                holderV2I = holderV2I + TileCalc((i - 1), (j - 1), board);
            }
        }

        //check space Lower i base and j - 1
        if (i >= 65 && i <= 72)
        {
            if (j - 1 >= 1 && j - 1 <= 8)
            {
                holderV2I = holderV2I + TileCalc((i), (j - 1), board);
            }
        }

        //check space Lower Right i + 1 and j - 1
        if (i + 1 >= 65 && i + 1 <= 72)
        {
            if (j - 1 >= 1 && j - 1 <= 8)
            {
                holderV2I = holderV2I + TileCalc((i + 1), (j - 1), board);
            }
        }

        return holderV2I;
    }

    public Vector2Int TileCalc(int i, int j, Dictionary<string, BoardTile> board)
    {
        int whiteHeurTotal = new int();
        int blackHeurTotal = new int();
        whiteHeurTotal = 0;
        blackHeurTotal = 0;

        Vector2Int holderV2I = new Vector2Int();
        holderV2I.x = 0;
        holderV2I.y = 0;

        char letterBoard = '0';
        string showB = "";

        letterBoard = Convert.ToChar(i);
        showB = letterBoard.ToString() + j.ToString();

        if (board[showB].isOccupied == true)
        {
            if (board[showB].isWhite == true)
            {
                if (board[showB].occupiedPieceType == "King")
                {
                    blackHeurTotal += chessTypes[0].typeValue;
                }
                else if (board[showB].occupiedPieceType == "Queen")
                {
                    blackHeurTotal += chessTypes[1].typeValue;
                }
                else if (board[showB].occupiedPieceType == "Bishop")
                {
                    blackHeurTotal += chessTypes[2].typeValue;
                }
                else if (board[showB].occupiedPieceType == "Knight")
                {
                    blackHeurTotal += chessTypes[3].typeValue;
                }
                else if (board[showB].occupiedPieceType == "Rook")
                {
                    blackHeurTotal += chessTypes[4].typeValue;
                }
                else if (board[showB].occupiedPieceType == "Pawn")
                {
                    blackHeurTotal += chessTypes[5].typeValue;
                }
            }
            else
            {
                if (board[showB].occupiedPieceType == "King")
                {
                    whiteHeurTotal += chessTypes[0].typeValue;
                }
                else if (board[showB].occupiedPieceType == "Queen")
                {
                    whiteHeurTotal += chessTypes[1].typeValue;
                }
                else if (board[showB].occupiedPieceType == "Bishop")
                {
                    whiteHeurTotal += chessTypes[2].typeValue;
                }
                else if (board[showB].occupiedPieceType == "Knight")
                {
                    whiteHeurTotal += chessTypes[3].typeValue;
                }
                else if (board[showB].occupiedPieceType == "Rook")
                {
                    whiteHeurTotal += chessTypes[4].typeValue;
                }
                else if (board[showB].occupiedPieceType == "Pawn")
                {
                    whiteHeurTotal += chessTypes[5].typeValue;
                }
            }
        }

        holderV2I.x = whiteHeurTotal;
        holderV2I.y = blackHeurTotal;

        return holderV2I;
    }
}
