using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public BoardManager board; // Not instantiated, value is filled in the editor
    public Piece[,] Pieces { get; set; }
    public Piece SelectedPiece { get; set; }

    
    public bool IsWhiteTurn { get; set; }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        IsWhiteTurn = true;
        Pieces = new Piece[8, 8];
    }

    public void MovePiece(Vector2Int position) {
        if(SelectedPiece.LocationsAvailable(position).Count > 0) {
            
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
    }
}
