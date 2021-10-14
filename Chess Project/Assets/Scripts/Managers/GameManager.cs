/* Written by David Corredor
 Edited by Braden Stonehill, David Corredor
 Last date edited: 10/10/2021
 GameManager.cs - Manages the rules, turn order, tracking and moving pieces, and checking the state of the pieces on
 the board and the players.

 Version 2.0:
  - Moved functions to a separate BoardManager class specifically for handling the virtual board.
 Changed the name of the original boardmanager to ModelManager to better represent its functionality.

  - Added functions to assign pieces to their commanders. Added functions for instantiation of players.

  - Added functionality for passing turn, tracking players and captured pieces, and support for three actions per turn.

  - Removed functions that dealt with board and object manipulation as that is handled in the BoardManager.
 Moved attack functions to indiviudal pieces. Edited several functions to accomodate for new board system and altered
 piece scripts. Combined and simplified scripts that checked positions of all, friendly, and enemy pieces and capturing
 pieces. Removed two square movement from initial movements of pawns.
 
  - Added score and turn order to show on the UI (Tommy Oh)*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    #region PRIVATE PROPERTIES
    private Player user;
    private Player ai;
    #endregion

    #region PUBLIC PROPERTIES
    public static GameManager Instance { get; set; }
    public ModelManager boardModel; // Not instantiated, value is populated in the editor
    public BoardManager board; // Not instantiated, value is populated in the editor
    
    public Player CurrentPlayer { get; set; }
    public bool IsGameOver { get; set; }
    #endregion

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        IsGameOver = false;
        ScoreManager.turn = "P1";
    }

    private void Update() {
        if (!IsGameOver) {
            if (Input.GetMouseButtonDown(0)) {
                if (board.SelectedPiece == null) {
                    board.SelectPiece(boardModel.selection);

                } else {
                    board.CheckMove(boardModel.selection);
                    GetComponent<AudioSource>().Play();
  
                }
            }


            if (Input.GetKeyDown(KeyCode.Space)) {
                PassTurn();
            }

            if (Input.GetMouseButtonDown(1) && board.SelectedPiece != null) {
                board.DelegatePiece();
            }

            if(EndofTurn()) {
                PassTurn();
            }

        }
    }

    #region TURN VALIDATION FUNCTIONS - Functions that handle condition checking for turn orders and number of actions in turn.
    // Function to pass the turn to the next player
    // changed to it public so it work with a button
    public void PassTurn() {
        board.SelectPiece(new Vector2Int(-1, -1));
        CurrentPlayer.ResetTurn();
        ScoreManager.turn = CurrentPlayer == user ? "P2" : "P1";
        CurrentPlayer = CurrentPlayer == user ? ai : user;

    }

    private bool EndofTurn() {
        if (CurrentPlayer.TotalActionsRemaining() <= 0 && CurrentPlayer.UsedAllFreeMovements())
            return true;
        return false;
    }

    // Function to add captured pieces to current player
    public void CapturePiece(Piece captured) {
        if (captured is Pawn) {
            CurrentPlayer.capturedPieces["Pawn"] += 1;
            if(CurrentPlayer == user)
            {
              ScoreManager.scoreValue1 += CurrentPlayer.capturedPieces["Pawn"];
            }
            else
            {
              ScoreManager.scoreValue2 += CurrentPlayer.capturedPieces["Pawn"];
            }
        } else if (captured is Rook) {
            CurrentPlayer.capturedPieces["Rook"] += 1;
            if(CurrentPlayer == user)
            {
              ScoreManager.scoreValue1 += CurrentPlayer.capturedPieces["Rook"];
            }
            else
            {
              ScoreManager.scoreValue2 += CurrentPlayer.capturedPieces["Rook"];
            }
        } else if (captured is Knight) {
            CurrentPlayer.capturedPieces["Knight"] += 1;
            if(CurrentPlayer == user)
            {
              ScoreManager.scoreValue1 += CurrentPlayer.capturedPieces["Knight"];
            }
            else
            {
              ScoreManager.scoreValue2 += CurrentPlayer.capturedPieces["Knight"];
            }
        } else if (captured is Bishop) {
            CurrentPlayer.capturedPieces["Bishop"] += 1;
            if(CurrentPlayer == user)
            {
              ScoreManager.scoreValue1 += CurrentPlayer.capturedPieces["Bishop"];
            }
            else
            {
              ScoreManager.scoreValue2 += CurrentPlayer.capturedPieces["Bishop"];
            }
            Bishop bishop = (Bishop)captured;
            bishop.DelegatePieces();

            if (CurrentPlayer == user)
                ai.commanders.Remove(bishop);
            else
                user.commanders.Remove(bishop);

        } else if (captured is Queen) {
            CurrentPlayer.capturedPieces["Queen"] += 1;
            if(CurrentPlayer == user)
            {
              ScoreManager.scoreValue1 += CurrentPlayer.capturedPieces["Queen"];
            }
            else
            {
              ScoreManager.scoreValue2 += CurrentPlayer.capturedPieces["Queen"];
            }
        } else if (captured is King) {
            CurrentPlayer.capturedPieces["King"] += 1;
            if(CurrentPlayer == user)
            {
              ScoreManager.scoreValue1 += CurrentPlayer.capturedPieces["King"];
            }
            else
            {
              ScoreManager.scoreValue2 += CurrentPlayer.capturedPieces["King"];
            }
            IsGameOver = true;
        }
    }
    #endregion

    #region INSTANTIATION FUNCTIONS - Functions that instatiate objects and run at beginning of game.
    // Function to create the players
    public void CreatePlayers() {
        // create collection of commanders
        Commander[] whiteCommanders = {
            (Commander)board.PieceAt(new Vector2Int(4,0)),
            (Commander)board.PieceAt(new Vector2Int(2,0)),
            (Commander)board.PieceAt(new Vector2Int(5,0)),
        };

        Commander[] blackCommanders = {
            (Commander)board.PieceAt(new Vector2Int(4,7)),
            (Commander)board.PieceAt(new Vector2Int(2,7)),
            (Commander)board.PieceAt(new Vector2Int(5,7)),
        };

        user = new Player("Human", true, new List<Commander>(whiteCommanders));
        ai = new Player("AI", false, new List<Commander>(blackCommanders));
        CurrentPlayer = user;
    }
    #endregion

}
