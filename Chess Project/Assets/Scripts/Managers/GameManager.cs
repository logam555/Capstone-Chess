using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region PROPERTIES
    public static GameManager Instance { get; set; }

    public Player CurrentPlayer { get; set; }

    public bool IsGameOver { get; set; }
    
    public bool IsPlayerWin;

    [SerializeField]
    public Player user;
    [SerializeField]
    public Player ai;

    public ScoreManager score;

    private bool aiRunning;
    #endregion

    private void Awake() {
        Instance = this;
        IsGameOver = false;
        CurrentPlayer = user;
        aiRunning = false;
        IsPlayerWin = false;
    }

    private void Start() {
        score.ChangeTurn("P1");
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
                if (!aiRunning) {
                    aiRunning = true;
                    ((AI)CurrentPlayer).Step();
                }
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
    public void PassTurn() {
        ChessBoard.Instance.SelectPiece(new Vector2Int(-1, -1));
        CurrentPlayer.ResetTurn();
        score.ChangeTurn(CurrentPlayer == user ? "P2" : "P1");
        score.LoadingActive(CurrentPlayer== user ? true : false);
        CurrentPlayer = CurrentPlayer == user ? ai : user;
        aiRunning = false;
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

        if (captured is Pawn) {
            CurrentPlayer.capturedPieces["Pawn"] += 1;
            if (CurrentPlayer == user)
                score.AddImgPieces(ScoreManager.pieceType.goldpawn);
            else
                score.AddImgPieces(ScoreManager.pieceType.sliverpawn);
        } else if (captured is Rook) {
            CurrentPlayer.capturedPieces["Rook"] += 1;
            if (CurrentPlayer == user)
                score.AddImgPieces(ScoreManager.pieceType.goldrook);
            else
                score.AddImgPieces(ScoreManager.pieceType.sliverrook);
        } else if (captured is Knight) {
            CurrentPlayer.capturedPieces["Knight"] += 1;
            if (CurrentPlayer == user)
                score.AddImgPieces(ScoreManager.pieceType.goldknight);
            else
                score.AddImgPieces(ScoreManager.pieceType.sliverknight);
        } else if (captured is Bishop) {
            CurrentPlayer.capturedPieces["Bishop"] += 1;
            Bishop bishop = (Bishop)captured;
            bishop.DelegatePieces();
            ((Commander)ChessBoard.Instance.PieceAt(bishop.Position, ChessBoard.Instance.KingBoard)).isDead = true;
            ((Commander)ChessBoard.Instance.PieceAt(bishop.Position, ChessBoard.Instance.LBishopBoard)).isDead = true;
            ((Commander)ChessBoard.Instance.PieceAt(bishop.Position, ChessBoard.Instance.RBishopBoard)).isDead = true;
            if (CurrentPlayer == user)
                score.AddImgPieces(ScoreManager.pieceType.goldbishop);
            else
                score.AddImgPieces(ScoreManager.pieceType.sliverbishop);


            if (CurrentPlayer == user) {
                ai.commanders.Remove(bishop);
            } else {
                user.commanders.Remove(bishop);
            }

        } else if (captured is Queen) {
            CurrentPlayer.capturedPieces["Queen"] += 1;
            if (CurrentPlayer == user)
                score.AddImgPieces(ScoreManager.pieceType.goldqueen);
            else
                score.AddImgPieces(ScoreManager.pieceType.sliverqueen);
        } else if (captured is King) {
            CurrentPlayer.capturedPieces["King"] += 1;
            if (CurrentPlayer == user)
            {
                score.AddImgPieces(ScoreManager.pieceType.goldking);
                IsPlayerWin = true;
            }
            else
            {
                score.AddImgPieces(ScoreManager.pieceType.sliverking);
                IsPlayerWin = false;
            }
            IsGameOver = true;
        }
    }

    public IEnumerator StartAI() {
        aiRunning = true;
        ((AI)CurrentPlayer).Step();
        yield return new WaitForSeconds(10);
        PassTurn();
    }

    public void ButtonPassTurn() {
        if(CurrentPlayer == user) {
            PassTurn();
        }
    }
    #endregion
}
