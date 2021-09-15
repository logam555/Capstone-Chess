/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/14/2021
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
    #region PRIVATE PROPERTIES
    private Player user;
    private Player ai;
    #endregion

    #region PUBLIC PROPERTIES
    public static GameManager Instance { get; set; }
    public BoardManager board; // Not instantiated, value is populated in the editor
    public Piece[,] Pieces { get; set; }
    public Piece SelectedPiece { get; set; }
    public Player CurrentPlayer { get; set; }
    public bool IsGameOver { get; set; }
    #endregion

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        user = new Player("Human", true);
        ai = new Player("AI", false);
        CurrentPlayer = user;
        Pieces = new Piece[8, 8];
        IsGameOver = false;
    }

    private void Update() {
        if (!IsGameOver) {
            if (Input.GetMouseButtonDown(0)) {
                if (SelectedPiece == null) {
                    SelectPiece(board.selection);
                } else {
                    CheckMove(board.selection);
                }
            }


            if (Input.GetKeyDown(KeyCode.Space) || CurrentPlayer.remainingActions <= 0) {
                PassTurn();
            }
        }
    }

    #region PIECE INTERACTION FUNCTIONS - Functions that interact with the object representation of the pieces.
    // Function to select a valid piece and highlight all possible moves.
    public void SelectPiece(Vector2Int position) {
        if (IsPieceAt(position) && PieceAt(position).IsWhite == CurrentPlayer.isWhite) {
            SelectedPiece = Pieces[position.x, position.y];
            board.HighlightAllTiles(position, AvailableMoves(SelectedPiece), SelectedPiece.EnemiesInRange());
        } else {
            SelectedPiece = null;
            board.RemoveHighlights();
        }
    }

    // Function to move the selected piece in the array implementation of the board
    private void MovePiece(Vector2Int position) {
        // Move piece to new position
        Pieces[SelectedPiece.Position.x, SelectedPiece.Position.y] = null;
        Pieces[position.x, position.y] = SelectedPiece;
        SelectedPiece.Position = position;

        // Call function in board to move the piece game object
        board.MoveObject(SelectedPiece.gameObject, position);

        // Reduce number of actions remaining
        CurrentPlayer.remainingActions -= 1;

        // Deselect the piece
        SelectPiece(new Vector2Int(-1, -1));
    }

    // Function to add captured pieces to current player
    private void CapturePiece(Piece captured) {
        if (captured is Pawn) {
            CurrentPlayer.capturedPieces["Pawn"] += 1;
        } else if (captured is Rook) {
            CurrentPlayer.capturedPieces["Rook"] += 1;
        } else if (captured is Knight) {
            CurrentPlayer.capturedPieces["Knight"] += 1;
        } else if (captured is Bishop) {
            CurrentPlayer.capturedPieces["Bishop"] += 1;
        } else if (captured is Queen) {
            CurrentPlayer.capturedPieces["Queen"] += 1;
        } else if (captured is King) {
            CurrentPlayer.capturedPieces["King"] += 1;
            IsGameOver = true;
        }
    }
    #endregion

    #region TURN VALIDATION FUNCTIONS - Functions that handle condition checking for turn orders and number of actions in turn.
    // Function to pass the turn to the next player
    private void PassTurn() {
        CurrentPlayer.remainingActions = 3;
        CurrentPlayer = CurrentPlayer == user ? ai : user;
    }
    #endregion

    #region BOARD EVALUTATION FUNCTIONS - Functions that scan the board array for pieces and available positions.
    // Function to check if the selected tile is a valid move for the selected piece and perform appropriate action.
    public void CheckMove(Vector2Int position) {
        // Deselect the selected piece if position is not valid
        if (!ValidPosition(position)) {
            SelectPiece(position);
            return;
        }
            

        // Get all available positions to move and list of enemy positions in range
        bool[,] availableMoves = AvailableMoves(SelectedPiece);
        List<Vector2Int> enemies = SelectedPiece.EnemiesInRange();

        // Move piece if position is in available moves, attack enemy piece if position contains enemy, or change selection if position contains a friendly piece
        if (availableMoves[position.x, position.y]) {
            MovePiece(position);

        } else if (enemies.Contains(position)) {
            bool isMoving = false;
            bool attackSuccessful;

            // Check if selected piece is a knight and is attacking a non-adjacent piece
            if (SelectedPiece is Knight && (Mathf.Abs(position.sqrMagnitude - SelectedPiece.Position.sqrMagnitude) > 2))
                isMoving = true;

            // Change optional isMoving parameter if selected piece is a Knight attacking a non-adjacent piece
            if (isMoving)
                attackSuccessful = SelectedPiece.Attack(Pieces[position.x, position.y], true);
            else
                attackSuccessful = SelectedPiece.Attack(Pieces[position.x, position.y]);

            // Capture piece and remove model if attack is successful
            if (attackSuccessful) {
                // Remove the captured piece and add to capture pieces
                Piece enemy = Pieces[position.x, position.y];
                CapturePiece(enemy);
                board.RemoveObject(enemy.gameObject);

                // Move the selected piece
                MovePiece(position);

            } else {
                // Move knight next to defending piece if attacking a non-adjacent piece
                if (SelectedPiece is Knight && isMoving) {
                    List<Vector2Int> locations = SelectedPiece.LocationsAvailable();
                    locations.RemoveAll(pos => !ValidPosition(pos));
                    locations.RemoveAll(pos => IsPieceAt(pos));

                    foreach (Vector2Int pos in locations) {
                        Vector2Int diff = Vector2Int.zero;
                        diff.x = Mathf.Abs(position.x - pos.x);
                        diff.y = Mathf.Abs(position.y - pos.y);
                        if (diff.sqrMagnitude <= 2 && diff.sqrMagnitude > 0) {
                            MovePiece(pos);
                            break;
                        }
                    }
                } else {
                    // Reduce number of actions remaining
                    CurrentPlayer.remainingActions -= 1;

                    // Deselect the piece
                    SelectPiece(new Vector2Int(-1, -1));
                }  
            }

        } else if (IsFriendlyPieceAt(CurrentPlayer.isWhite, position)) {
            board.RemoveHighlights();
            SelectPiece(position);

        } else
            // Deselect the piece
            SelectPiece(new Vector2Int(-1, -1));
    }

    // Function that returns a boolean map of the board with all positions that are available to selected piece.
    public bool[,] AvailableMoves(Piece piece) {
        bool[,] allowedMoves = new bool[8, 8];
        List<Vector2Int> possibleMoves = piece.LocationsAvailable();

        possibleMoves.RemoveAll(pos => !ValidPosition(pos));
        possibleMoves.RemoveAll(pos => IsPieceAt(pos));

        foreach (Vector2Int position in possibleMoves) {
            allowedMoves[position.x, position.y] = true;
        }
        return allowedMoves;
    }

    // Function that returns the object at board position
    public Piece PieceAt(Vector2Int position) {
        if (ValidPosition(position))
            return Pieces[position.x, position.y];
        return null;
    }

    // Function that returns true if space is occupied
    public bool IsPieceAt(Vector2Int position) {
        if (!ValidPosition(position))
            return false;

        Piece piece = Pieces[position.x, position.y];

        if (piece == null) {
            return false;
        }

        return true;
    }

    // Function that returns true if space is occupied by a friendly piece
    public bool IsFriendlyPieceAt(bool isWhite, Vector2Int position) {
        if (!ValidPosition(position))
            return false;

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
    public bool IsEnemyPieceAt(bool isWhite, Vector2Int position) {
        if (!ValidPosition(position))
            return false;

        Piece piece = Pieces[position.x, position.y];

        if (piece == null) {
            return false;
        }

        if (piece.IsWhite == isWhite) {
            return false;
        }

        return true;
    }

    // Function that returns true if position is within boundaries of the board.
    public bool ValidPosition(Vector2Int position) {
        if (position.x < 0 || position.x > 7 || position.y < 0 || position.y > 7)
            return false;

        return true;
    }
    #endregion
}
