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

// Fuzzy Logic object that contains enumerator types for each attacking piece and the minimum number required
// on a dice roll to capture a specified defending piece.
public struct FuzzyLogic {
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

public abstract class Piece : MonoBehaviour
{
    [SerializeField]
    protected bool isWhite;
    protected FuzzyLogic fl = new FuzzyLogic();
    protected Vector2Int[] directions = {new Vector2Int(0,1), new Vector2Int(1,0),
                               new Vector2Int(0,-1), new Vector2Int(-1,0),
                               new Vector2Int(1,1), new Vector2Int(1,-1),
                               new Vector2Int(-1,-1), new Vector2Int(-1,1)};


    public bool IsWhite { get => isWhite; }
    public bool Delegated { get; set; }
    public Vector2Int[] Directions { get => directions; }
    public Vector2Int Position { get; set; }
    public Piece Commander { get; set; }
    

    // Will attempt to attack enemy piece with probabilities based on fuzzy-logic table
    // and will move to enemy piece's position if attack is successful 
    public abstract void Attack(Piece enemy, Vector2Int position);

    // Determines what positions are available to move to based on pieces movement restriction
    public abstract List<Vector2Int> LocationsAvailable();

    // Function to determine if enemies are withing attacking range
    public abstract List<Vector2Int> EnemiesInRange();

    // Recursive location function for pieces that move multiple tiles
    public List<Vector2Int> RecursiveLocations(Vector2Int position, int moves, bool ignorePieces=false) {
        List<Vector2Int> locations = new List<Vector2Int>();

        if (position.x < 0 || position.x > 7 || position.y < 0 || position.y > 7)
            return locations;

        if (!ignorePieces && GameManager.Instance.PieceAt(position))
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

    
    
}
