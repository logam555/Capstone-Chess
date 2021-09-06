/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/06/2021
 Piece.cs - abstract class for chess pieces that sets the basis for moving and attacking for each piece

 Version 1.1: Edited code for optimization and compatibility with Unity editor. Removed PieceType enumerator
 from abstract class to be placed in game script. Removed Rook and Bishop directions to be placed in the inherited
 child classes. Added Move function and Attack function to be implemented by child classes. Added Fuzzy logic table
 for use in Attack function implementations and directions for all pieces that can move in any direction. Added GridPoint 
 properties to be accessed when moving and selecting pieces. Added Commander and Delegated properties for use of assigning
 commanders and if a piece has been delegated by the king.*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public bool IsWhite { get; set; }
    public Vector2Int GridPoint { get; set; }
    public Piece Commander { get; set; }
    public bool delegated = false;

    protected Vector2Int[] directions = {new Vector2Int(0,1), new Vector2Int(1,0),
                               new Vector2Int(0,-1), new Vector2Int(-1,0),
                               new Vector2Int(1,1), new Vector2Int(1,-1),
                               new Vector2Int(-1,-1), new Vector2Int(-1,1)};
    protected struct FuzzyLogic {
        public enum Pawn : int {
            Pawn = 4,
            Rook = 6,
            Bishop = 5,
            Knight = 6,
            Queen = 6,
            King = 6
        }

        public enum Rook : int {
            Pawn = 5,
            Rook = 5,
            Bishop = 5,
            Knight = 4,
            Queen = 4,
            King = 4
        }

        public enum Bishop : int {
            Pawn = 3,
            Rook = 5,
            Bishop = 4,
            Knight = 5,
            Queen = 5,
            King = 5
        }

        public enum Knight : int {
            Pawn = 2,
            Rook = 5,
            Bishop = 5,
            Knight = 5,
            Queen = 5,
            King = 5
        }

        public enum Queen : int {
            Pawn = 2,
            Rook = 5,
            Bishop = 4,
            Knight = 4,
            Queen = 4,
            King = 4
        }

        public enum King : int {
            Pawn = 1,
            Rook = 5,
            Bishop = 4,
            Knight = 4,
            Queen = 4,
            King = 4
        }
    }

    // Moves piece from current position to the new position
    public void Move(Vector2Int gridPoint, ref Piece[,] pieces, ref Board board) {
        // Move the piece in the shared array implementation of the board and update positions
        pieces[gridPoint.x, gridPoint.y] = this;
        pieces[this.GridPoint.x, this.GridPoint.y] = null;
        this.GridPoint = gridPoint;

        // Move the physical representation of the pieces
        board.MovePieceInGrid(this.gameObject, gridPoint);
    }

    // Will attempt to attack enemy piece with probabilities based on fuzzy-logic table
    // and will move to enemy piece's position if attack is successful 
    public abstract void Attack(Piece enemy, Vector2Int gridPoint);

    // Determines what positions are available to move to based on pieces movement restriction
    public abstract List<Vector2Int> LocationsAvailable(Vector2Int gridPoint);
}
