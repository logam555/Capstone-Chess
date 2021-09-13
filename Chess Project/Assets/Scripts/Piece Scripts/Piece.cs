/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 09/06/2021
 Piece.cs - abstract class for chess pieces that sets the basis for moving and attacking for each piece

 Version 1.1: Edited code for optimization and compatibility with Unity editor. Removed PieceType enumerator
 from abstract class to use type checking instead. Combined Rook and Bishop directions into a single list of cardinal directons.
 Added Attack function to be implemented by child classes. Added Fuzzy logic table for use in Attack function 
 implementations and directions for all pieces that can move in any direction. Added Postion 
 property to be accessed when moving and selecting pieces. Added Commander and Delegated properties for use of assigning
 commanders and if a piece has been delegated by the king. Added arguments to LocationsAvailable for number of moves
 each piece has for recursively finding all possible moves to account for nonlinear movement. Added Recursive locations function.*/

using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
// Fuzzy Logic object that contains enumerator types for each attacking piece and the minimum number required
// on a dice roll to capture a specified defending piece.

public abstract class Piece : MonoBehaviour
{
    [SerializeField]
    public enum PieceType { King, Queen, Bishop, Knight, Rook, Pawn };
    [SerializeField]
    protected bool isWhite;
    [SerializeField]
    public GameObject commander;
    [SerializeField]
    public List<GameObject> commandingList;
    [SerializeField]
    public int numberOfTurns = 3;
    [SerializeField]
    public int numberOfTurnsPawn = 1;
    [SerializeField]
    public System.Guid id;
    public int index;
    public bool isBishopDead { get; set; }
    protected FuzzyLogic fl = new FuzzyLogic();
    protected Vector2Int[] directions = {new Vector2Int(0,1), new Vector2Int(1,0),
                               new Vector2Int(0,-1), new Vector2Int(-1,0),
                               new Vector2Int(1,1), new Vector2Int(1,-1),
                               new Vector2Int(-1,-1), new Vector2Int(-1,1)};
    public bool IsWhite { get => isWhite; }
    public bool Delegated { get; set; }
    public Vector2Int[] Directions { get => directions; }
    public Vector2Int Position { get; set; }
   
    public PieceType type { get; set; }

    // Will attempt to attack enemy piece with probabilities based on fuzzy-logic table
    // and will move to enemy piece's position if attack is successful 
    public abstract void Attack(Piece enemy, Vector2Int position);

    // Determines what positions are available to move to based on pieces movement restriction
    public abstract List<Vector2Int> LocationsAvailable();

    // Recursive location function for pieces that move multiple tiles
    protected List<Vector2Int> RecursiveLocations(Vector2Int position, int moves) {
        List<Vector2Int> locations = new List<Vector2Int>();

        if (position.x < 0 || position.x > 7 || position.y < 0 || position.y > 7)
            return locations;

        if (FriendlyPieceAt(position))
            return locations;

        if (moves > 0) {
            foreach (Vector2Int dir in this.directions) {
                Vector2Int nextTile = new Vector2Int(position.x + dir.x, position.y + dir.y);
                locations.Add(nextTile);
                locations = locations.Union(RecursiveLocations(nextTile, moves - 1)).ToList();
            }
        }

        return locations;
    }

    // Function to remove spaces occupied by friendly pieces
    public bool FriendlyPieceAt(Vector2Int position) {
        Piece piece = GameManager.Instance.Pieces[position.x, position.y];

        if (piece == null) {
            return false;
        }

        if (piece.IsWhite != this.IsWhite) {
            return false;
        }

        return true;
    }
}
