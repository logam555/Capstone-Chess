using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region PROPERTIES
    public static GameManager Instance { get; set; }

    public Player CurrentPlayer { get; set; }

    public bool IsGameOver { get; set; }

    [SerializeField]
    private Player user;
    [SerializeField]
    private Player ai;
    #endregion

    private void Awake() {
        Instance = this;
        IsGameOver = false;
        CurrentPlayer = user;
    }

    private void Start() {
        ScoreManager.turn = "P1";
    }

    private void Update() {
        if (!IsGameOver) {
            if (CurrentPlayer.name == "Human") {
                if (Input.GetKeyDown(KeyCode.Space)) {
                    PassTurn();
                }

                if (EndofTurn()) {
                    PassTurn();
                }
            } else {
                ((AI)CurrentPlayer).Step();
                PassTurn();
            }
        }
    }

    #region INSTANTIATION
    // Function to create the players
    public void LinkCommanders(ChessPiece[,] board) {
        // create collection of commanders
        Commander[] whiteCommanders = {
            (Commander)board[4,0],
            (Commander)board[2,0],
            (Commander)board[5,0],
        };

        Commander[] blackCommanders = {
            (Commander)board[4,7],
            (Commander)board[2,7],
            (Commander)board[5,7],
        };

        user.commanders = new List<Commander>(whiteCommanders);
        ai.commanders = new List<Commander>(blackCommanders);
    }
    #endregion

    #region TURN VALIDATION
    // Function to pass the turn to the next player
    private void PassTurn() {
        ChessBoard.Instance.SelectPiece(new Vector2Int(-1, -1));
        CurrentPlayer.ResetTurn();
        ScoreManager.turn = CurrentPlayer == user ? "P2" : "P1";
        CurrentPlayer = CurrentPlayer == user ? ai : user;
    }

    private bool EndofTurn() {
        if (CurrentPlayer.TotalActionsRemaining() <= 0 && CurrentPlayer.UsedAllFreeMovements())
            return true;
        return false;
    }
    #endregion

    #region INTERACTION
    // Function to add captured pieces to current player
    public void CapturePiece(ChessPiece captured) {
        if (CurrentPlayer == user) {
            ScoreManager.scoreValue1 += 1;
        } else {
            ScoreManager.scoreValue2 += 1;
        }
        if (captured is Pawn) {
            CurrentPlayer.capturedPieces["Pawn"] += 1;
        } else if (captured is Rook) {
            CurrentPlayer.capturedPieces["Rook"] += 1;
        } else if (captured is Knight) {
            CurrentPlayer.capturedPieces["Knight"] += 1;
        } else if (captured is Bishop) {
            CurrentPlayer.capturedPieces["Bishop"] += 1;
            Bishop bishop = (Bishop)captured;
            bishop.DelegatePieces();

            if (CurrentPlayer == user)
                ai.commanders.Remove(bishop);
            else
                user.commanders.Remove(bishop);

        } else if (captured is Queen) {
            CurrentPlayer.capturedPieces["Queen"] += 1;
        } else if (captured is King) {
            CurrentPlayer.capturedPieces["King"] += 1;
            IsGameOver = true;
        }
    }
    #endregion
}
