/*
Written By:
Edited By:
Last Date Edited:

Indiviual Piece Scanner scrpit is for each piece to scanner the area of the board they are in to 
inform their commander on hostile pieces.

Version 1:


*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ScanData
{
    public int FriendFoe;
    public bool IsVulnerable;
    public float CaptureChance;

    public void setScanData(int friendFoe, bool isVulnerable, float captureChance)
    {
        FriendFoe = friendFoe;
        IsVulnerable = isVulnerable;
        CaptureChance = captureChance;
    }

    public int friendFoe
    {
        get { return FriendFoe; }
        set { FriendFoe = value; }
    }


    public bool isVulnerable
    {
        get { return IsVulnerable; }
        set { IsVulnerable = value; }
    }

    public float captureChance
    {
        get { return CaptureChance; }
        set { CaptureChance = value; }
    }
}

public class IndiviualScanner : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


        public ScanData[,] RunScanThreats(bool isWhite, List<Vector2Int> LocationsAvailable, Piece scanPiece)
        {
            bool[,] enemyMoveMap = new bool[8, 8];
            int[,] AIScan = new int[8, 8];
            ScanData[,] ScanMap = new ScanData[8, 8];
            //LocationsAvailable = LocationsAvailable();

            bool[,] SelectedPieceAvailMoves = new bool[8, 8];

            SelectedPieceAvailMoves = BoardManager.Instance.AvailableMoves(scanPiece);

            int[,] locArray = new int[LocationsAvailable.Count, 1];
            locArray = BoardManager.Instance.Vector2Int2Array(LocationsAvailable); //O(n)

            for (int count = 0; count < LocationsAvailable.Count; count++)
            {
                if (BoardManager.Instance.IsFriendlyPieceAt(isWhite, LocationsAvailable[count]) == true)
                {
                    // AIScan[locArray[count, 0], locArray[count, 1]] = 2;
                    ScanMap[locArray[count, 0], locArray[count, 1]].friendFoe = 1;
                }
                else if (BoardManager.Instance.IsEnemyPieceAt(isWhite, LocationsAvailable[count]) == true)
                {
                    ScanMap[locArray[count, 0], locArray[count, 1]].friendFoe = 2;
                }
                else
                {
                    ScanMap[locArray[count, 0], locArray[count, 1]].friendFoe = 0;
                }
            }

            for (int k = 0; k < ScanMap.GetLength(0); k++)
            {
                for (int i = 0; i < ScanMap.GetLength(1); i++)
                {
                    if (ScanMap[k, i].friendFoe == 2)
                    {
                        enemyMoveMap = BoardManager.Instance.AvailableMoves(BoardManager.Piece[k, i]);
                        if (enemyMoveMap[k, i] == true && SelectedPieceAvailMoves[k, i] == true)
                        {
                            ScanMap[k, i].isVulnerable = true;
                            ScanMap[k, i].captureChance = FuzzyLogic.FindFuzzyNumber(BoardManager.Piece[k, i], scanPiece);


                        }

                    }
                }
            }

        return ScanMap;
        }
    

    //Run a Heiusitics Check from the local of piece based on pieces ability to move
    void RunBoardHuesiticsFromSinglePiece(int pieceType)
    {

        //Set movement and direction limit check based on range of chess piece

        //Set weight based on each piece attack movement/direction

        //make array of values based on chess board tile namng system

        //send update to commander piece

    }
}
