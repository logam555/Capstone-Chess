/* Written by George Brunner
 Edited by ___
 Last date edited: 09/15/2021
 Heuristics.cs - Manages the Heuristics of the board and pieces.

 Version 1: Created functions to collect Heuristics
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heuristics : MonoBehaviour
{
    private class chessPiece
    {
        public string pieceType;
        //public bool isWhite;
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
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        //just a temp for testing
        /*
        if(Input.GetKeyDown("space"))
        {
            //Debug.Log("space down");
            BoardWideHeuristic();
        }
        */
    }

    public void IndividualHeuristic()
    {
        //may not be needed here, or a merge with individual scanning script when completed
    }

    public void IndividualHeuristicScan(int range)
    {

        //small scan; maybe linked or separate from IndividualHeuristic
    }

    public void CommanderHeuristic()
    {
        //need commander game logic
    }

    public void BoardWideHeuristic(ref Dictionary<string, ModelManager.BoardTile> chessBoardGridCo)
    {
        //Debug.Log("BoardWideHeuristic call");

        char letterBoard = '0';
        string showB = "";

        char letterBoardSC = '0';
        string showBSC = "";

        int whiteHeurTotal = new int();
        int blackHeurTotal = new int();
        whiteHeurTotal = 0;
        blackHeurTotal = 0;

        //setting up tiles and adding to dictionary default values
        for (int j = 1; j < 9; j++)
        {
            for (int i = 65; i < 73; i++)
            {
                whiteHeurTotal = 0;
                blackHeurTotal = 0;

                letterBoard = Convert.ToChar(i);
                showB = letterBoard.ToString() + j.ToString();

                //attack/type/move/piece

                //Debug.Log("In Main Loop BWH call; First IF is occup " + chessBoardGridCo[showB].isOccupied);
                //check space
                if (chessBoardGridCo[showB].isOccupied == true)
                {
                    //
                    //Debug.Log("In Main Loop BWH call; 2nd IF is white " + chessBoardGridCo[showB].isWhite);
                    if (chessBoardGridCo[showB].isWhite == true)
                    {
                        //Debug.Log("In Main Loop BWH call; 3rd IF is Type " + chessBoardGridCo[showB].occupiedPieceType);
                        if (chessBoardGridCo[showB].occupiedPieceType == "King")
                        {
                            blackHeurTotal += chessTypes[0].typeValue;
                        }
                        else if (chessBoardGridCo[showB].occupiedPieceType == "Queen")
                        {
                            blackHeurTotal += chessTypes[1].typeValue;
                        }
                        else if (chessBoardGridCo[showB].occupiedPieceType == "Bishop")
                        {
                            blackHeurTotal += chessTypes[2].typeValue;
                        }
                        else if (chessBoardGridCo[showB].occupiedPieceType == "Knight")
                        {
                            blackHeurTotal += chessTypes[3].typeValue;
                        }
                        else if (chessBoardGridCo[showB].occupiedPieceType == "Rook")
                        {
                            blackHeurTotal += chessTypes[4].typeValue;
                        }
                        else if (chessBoardGridCo[showB].occupiedPieceType == "Pawn")
                        {
                            blackHeurTotal += chessTypes[5].typeValue;
                        }
                    }
                    else
                    {
                        //Debug.Log("In Main Loop BWH call; 3rd IF is Type " + chessBoardGridCo[showB].occupiedPieceType);
                        if (chessBoardGridCo[showB].occupiedPieceType == "King")
                        {
                            whiteHeurTotal += chessTypes[0].typeValue;
                        }
                        else if (chessBoardGridCo[showB].occupiedPieceType == "Queen")
                        {
                            whiteHeurTotal += chessTypes[1].typeValue;
                        }
                        else if (chessBoardGridCo[showB].occupiedPieceType == "Bishop")
                        {
                            whiteHeurTotal += chessTypes[2].typeValue;
                        }
                        else if (chessBoardGridCo[showB].occupiedPieceType == "Knight")
                        {
                            whiteHeurTotal += chessTypes[3].typeValue;
                        }
                        else if (chessBoardGridCo[showB].occupiedPieceType == "Rook")
                        {
                            whiteHeurTotal += chessTypes[4].typeValue;
                        }
                        else if (chessBoardGridCo[showB].occupiedPieceType == "Pawn")
                        {
                            whiteHeurTotal += chessTypes[5].typeValue;
                        }
                    }


                }

                //Debug.Log("In Main Loop BWH call; test temp Huer White " + whiteHeurTotal);
                //Debug.Log("In Main Loop BWH call; test temp Huer Black " + blackHeurTotal);

                chessBoardGridCo[showB].whiteHeuristic = whiteHeurTotal;
                chessBoardGridCo[showB].blackHeuristic = blackHeurTotal;

                //Debug.Log("In Main Loop BWH call; test Huer White " + chessBoardGridCo[showB].whiteHeuristic);
                //Debug.Log("In Main Loop BWH call; test Huer Black " + chessBoardGridCo[showB].blackHeuristic);
            }
        }
    }

    public void HeuristicSetup()
    {
        chessPiece piece = new chessPiece();
        //chessTypes = new List<chessPiece>();

        for (int i = 0; i <6; i++)
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

        //Debug.Log("setup call");
    }

    //Applies the set values of each chess piece to pieces array
    public void ChessPieceSetup()
    {
        //Debug.Log("ChessPieceSetup call");
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

        //Debug.Log("HeuristicDifficulty call");

        diff = PlayerPrefs.GetInt("Difficulty"); ;

        //base/Easy
        chessTypes[0].typeValue = 5;
        chessTypes[1].typeValue = 5;
        chessTypes[2].typeValue = 4;
        chessTypes[3].typeValue = 3;
        chessTypes[4].typeValue = 2;
        chessTypes[5].typeValue = 1;

        //normal
        if (diff == 2)
        {
            //scale
            chessTypes[0].typeValue = 10;
            chessTypes[1].typeValue = 10;
            chessTypes[2].typeValue = 8;
            chessTypes[3].typeValue = 6;
            chessTypes[4].typeValue = 4;
            chessTypes[5].typeValue = 1;
        }

        //hard
        if (diff == 3)
        {
            //scale
            chessTypes[0].typeValue = 15;
            chessTypes[1].typeValue = 15;
            chessTypes[2].typeValue = 12;
            chessTypes[3].typeValue = 9;
            chessTypes[4].typeValue = 6;
            chessTypes[5].typeValue = 2;
        }
    }


    //return vector 3? x and y for position; z for value
    public Vector3Int ReturnHighestValueWhite(Dictionary<string, ModelManager.BoardTile> boardPieceValue)
    {
        Debug.Log("Call Check ReturnHighestValueWhite in Heur");
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

                heurValueHolder[index] = boardPieceValue[showB].whiteHeuristic;
                position[index] = boardPieceValue[showB].boardPosition;

                /*
                Debug.Log("showB is " + showB);
                Debug.Log("In Main ModelMan; test  White " + chessBoardGridCo[showB].isWhite);
                Debug.Log("In Main ModelMan; test  type " + chessBoardGridCo[showB].occupiedPieceType);
                Debug.Log("In Main ModelMan; test  Occupied " + chessBoardGridCo[showB].isOccupied);
                Debug.Log("In Main ModelMan; test Huer White " + chessBoardGridCo[showB].whiteHeuristic);
                Debug.Log("In Main ModelMan; test Huer Black " + chessBoardGridCo[showB].blackHeuristic);
                Debug.Log("In Main ModelMan; test v2 position " + chessBoardGridCo[showB].boardPosition);
                */
                Debug.Log("white A is " + boardPieceValue[showB].whiteHeuristic);
                Debug.Log("white B is " + heurValueHolder[index]);
                Debug.Log(boardPieceValue[showB].officalBoardPosition);
                index ++;
                
            }
        }

        int highestValueIndex = new int();
        highestValueIndex = 0;
        int highestValue = new int();
        highestValue = 0;

        //Debug.Log("heurValueHolder count " + heurValueHolder.Length);
        for (int k = 0; k < index; k++)
        {
            Debug.Log("current high value" + heurValueHolder[k]);
            if (highestValue > heurValueHolder[k])
            {
                highestValue = heurValueHolder[k];
                highestValueIndex = k;
                Debug.Log("new high value of " + highestValue);
            }
        }



        Vector3Int posValue = new Vector3Int();

        posValue.x = position[highestValueIndex].x;
        posValue.y = position[highestValueIndex].y;
        posValue.z = highestValue;

        Debug.Log("end of get highest test value in Heur call " + posValue);

        return posValue;
    }

    public void ReturnHighestValueBlack(Dictionary<string, ModelManager.BoardTile> boardPieceValue)
    {

    }


}
