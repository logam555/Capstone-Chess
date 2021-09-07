/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/07/2021
 GameManager.cs - Manages the rules, turn order, tracking and moving pieces, and checking the state of the pieces on
 the board and the players.

 Version 1.2: 
  - Added functionality for passing turn, tracking players and captured pieces, and support for three actions per turn.

  - Removed functions that dealt with board and object manipulation as that is handled in the BoardManager.
 Moved attack functions to indiviudal pieces. Edited several functions to accomodate for new board system and altered
 piece scripts. Combined and simplified scripts that checked positions of all, friendly, and enemy pieces and capturing
 pieces. Removed two square movement from initial movements of pawns.*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public BoardManager board; // Not instantiated, value is filled in the editor
    public Piece[,] Pieces { get; set; }
    public Piece SelectedPiece { get; set; }


    private Player user;
    private Player ai;

    [SerializeField]
    public Player currentPlayer;

    public bool isGameOver;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        user = new Player("Human", true);
        ai = new Player("AI", false);
        currentPlayer = user;
        Pieces = new Piece[8, 8];

        isGameOver = false;
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space) || currentPlayer.remainingActions <= 0) {
            PassTurn();
        }
    }

    // Function that grabs a reference to the Piece object at the selected tile and
    // cals the board function to highlight all available squares for the piece.
    public void SelectPiece(Vector2Int position) {
        if (Pieces[position.x, position.y] == null)
            return;

        if (Pieces[position.x, position.y].IsWhite != currentPlayer.isWhite)
            return;

        SelectedPiece = Pieces[position.x, position.y];
        board.HighlightAllTiles(position, AvailableMoves(SelectedPiece), SelectedPiece.EnemiesInRange());
    }

    // Function to check if the selected tile is a valid move for the selected piece.
    public void CheckMove(Vector2Int position) {
        bool[,] availableMoves = AvailableMoves(SelectedPiece);
        List<Vector2Int> enemies = SelectedPiece.EnemiesInRange();
        
        if (availableMoves[position.x, position.y]) {
            MovePiece(position);
        } else if (enemies.Contains(position)) {
            bool attackSuccessful = SelectedPiece is Knight && (Mathf.Abs(position.sqrMagnitude - SelectedPiece.Position.sqrMagnitude) <= 2) ? SelectedPiece.Attack(Pieces[position.x, position.y]) : SelectedPiece.Attack(Pieces[position.x, position.y], true);
            if (attackSuccessful) {
                // Remove the captured piece and add to capture pieces
                Piece enemy = Pieces[position.x, position.y];
                CapturePiece(enemy);
                board.RemoveObject(enemy.gameObject);

                // Move the selected piece
                MovePiece(position);

            } else {
                // Reduce number of actions remaining
                currentPlayer.remainingActions -= 1;
            }
        }
        

        SelectedPiece = null;
        board.RemoveHighlights();
    }

    // Function to move the selected piece in the array implementation of the board
    public void MovePiece(Vector2Int position) {
        // Move piece to new position
        Pieces[SelectedPiece.Position.x, SelectedPiece.Position.y] = null;
        Pieces[position.x, position.y] = SelectedPiece;
        SelectedPiece.Position = position;

        // Call function in board to move the piece game object
        board.MoveObject(SelectedPiece.gameObject, position);

        // Reduce number of actions remaining
        currentPlayer.remainingActions -= 1;
    }

    // Function that returns a boolean map of the board with all positions that can be moved to by
    // the selected piece set to true
    public bool[,] AvailableMoves(Piece piece) {
        bool[,] allowedMoves = new bool[8, 8];
        List<Vector2Int> possibleMoves = piece.LocationsAvailable();

        possibleMoves.RemoveAll(pos => pos.x < 0 || pos.x > 7 || pos.y < 0 || pos.y > 7);
        possibleMoves.RemoveAll(pos => PieceAt(pos));

        foreach (Vector2Int position in possibleMoves) {
            allowedMoves[position.x, position.y] = true;
        }
        return allowedMoves;
    }


    // Function that returns true if space is occupied
    public bool PieceAt(Vector2Int position) {
        Piece piece = Pieces[position.x, position.y];

        if (piece == null) {
            return false;
        }

        return true;
    }

    // Function that returns true if space is occupied by a friendly piece
    public bool FriendlyPieceAt(bool isWhite, Vector2Int position) {
        Piece piece = Pieces[position.x, position.y];

        if (piece == null) {
            return false;
        }

        if (piece.IsWhite != isWhite) {
            return false;
        }

        return true;
    }

    // Function that returns true if space is occupied by an enemy piece
    public bool EnemyPieceAt(bool isWhite, Vector2Int position) {
        Piece piece = Pieces[position.x, position.y];

        if (piece == null) {
            return false;
        }

        if (piece.IsWhite == isWhite) {
            return false;
        }

        return true;
    }

    // Function to pass the turn to the next player
    private void PassTurn() {
        currentPlayer.remainingActions = 3;
        currentPlayer = currentPlayer == user ? ai : user;
    }

    // Function to add captured pieces to current player
    private void CapturePiece(Piece captured) { 
        if(captured is Pawn) {
            currentPlayer.capturedPieces["Pawn"] += 1;
        } else if (captured is Rook) {
            currentPlayer.capturedPieces["Rook"] += 1;
        } else if (captured is Knight) {
            currentPlayer.capturedPieces["Knight"] += 1;
        } else if (captured is Bishop) {
            currentPlayer.capturedPieces["Bishop"] += 1;
        } else if (captured is Queen) {
            currentPlayer.capturedPieces["Queen"] += 1;
        } else if (captured is King) {
            currentPlayer.capturedPieces["King"] += 1;
            isGameOver = true;
        }
    }

}
