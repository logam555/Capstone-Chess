/* Written by George Brunner
 Edited by ___
 Last date edited: 09/15/2021
 Heuristics.cs - Manages the Heuristics of the board and pieces.

 Version 1: Created functions to collect Heuristics
*/

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

        //uses the difficulty to send values used by Heuristics
        HeuristicDifficulty();

        //fills list with the types of chess pieces
        HeuristicSetup();
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

    public void CommanderHeuristic()
    {
        //need commander game logic
    }

    public void BoardWideHeuristic()
    {
        Debug.Log("board call");
    }

    public void HeuristicSetup()
    {
        chessPiece piece = new chessPiece();
        chessTypes = new List<chessPiece>();

        for (int i = 0; i <5; i++)
        {
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

        Debug.Log("setup call");
    }

    //alter the weights
    public void HeuristicDifficulty()
    {
        int diff = new int();

        Debug.Log("difficulty call");

        diff = PlayerPrefs.GetInt("Difficulty"); ;


        //easy
        if(diff == 1)
        {
            //base
        }

        //normal
        if (diff == 2)
        {
            //scale
        }

        //hard
        if (diff == 3)
        {
            //scale
        }
    }


}
