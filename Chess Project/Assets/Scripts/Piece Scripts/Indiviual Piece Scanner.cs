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

    //Run Scan Function checks the local area for immedate threats
    void RunScanThreats()
    {

        //Checks for Hostile piece in area

        //Flags hostile piece location

        //temp output to console for debugging

        //send update to commander
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
