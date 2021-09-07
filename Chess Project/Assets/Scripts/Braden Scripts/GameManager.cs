using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public BoardManager board; // Not instantiated, value is filled in the editor
    public Piece[,] Pieces { get; set; }
    public Piece SelectedPiece { get; set; }

    
    public bool IsWhiteTurn { get; set; }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        IsWhiteTurn = true;
        Pieces = new Piece[8, 8];
    }

    public void SelectPiece(Vector2Int position) {
        if (Pieces[position.x, position.y] == null)
            return;

        if (Pieces[position.x, position.y].IsWhite != IsWhiteTurn)
            return;

        SelectedPiece = Pieces[position.x, position.y];
        board.HighlightAllTiles(position, AvailableMoves(SelectedPiece), SelectedPiece.EnemiesInRange());
    }

    public void MovePiece(Vector2Int position) {
        bool[,] availableMoves = AvailableMoves(SelectedPiece);
        
        if (availableMoves[position.x, position.y]) {
            // Move piece to new position
            Pieces[SelectedPiece.Position.x, SelectedPiece.Position.y] = null;
            Pieces[position.x, position.y] = SelectedPiece;
            SelectedPiece.Position = position;

            // Call function in board to move the piece game object
            board.MoveObject(SelectedPiece.gameObject, position);

            // Change turn order
            IsWhiteTurn = !IsWhiteTurn;
        }
        

        SelectedPiece = null;
        board.RemoveHighlights();
    }

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

}
