/* Written by David Corredor
 Edited by Braden Stonehill, David Corredor
 Last date edited: 09/15/2021
 Piece.cs - abstract class for chess pieces that sets the basis for moving and attacking for each piece

 Version 1.4:
  - Created a commander abstract class that is a child of Piece specifically for use with King and Bishop to include commander utility.
 Edited all child classes to check if commander action has been used to limit available locations and enemies in range once the commander
 action has been used.

 - Altered the attack script so it no longer has to be implemented in the child classes and uses the same approach
 for all pieces using the Fuzzy Logic struct to determine the number needed for a successful attack.

  - Added EnemiesInRange function for detecting attackable enemies and removed FuzzyLogic object to be located in a separate
 script.

  - Edited code for optimization and compatibility with Unity editor. Removed PieceType enumerator
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

public abstract class Piece : MonoBehaviour
{
    #region PROTECTED PROPERTIES
    [SerializeField]
    protected bool isWhite;
    protected Vector2Int[] directions = {new Vector2Int(0,1), new Vector2Int(1,0),
                               new Vector2Int(0,-1), new Vector2Int(-1,0),
                               new Vector2Int(1,1), new Vector2Int(1,-1),
                               new Vector2Int(-1,-1), new Vector2Int(-1,1)};
    #endregion

    #region PUBLIC PROPERTIES
    public bool IsWhite { get => isWhite; }
    public bool Delegated { get; set; }
    public Vector2Int[] Directions { get => directions; }
    public Vector2Int Position { get; set; }
    public Commander Commander { get; set; }
    #endregion

    #region ABSTRACT METHODS
    // Determines what positions are available to move to based on pieces movement restriction
    public abstract List<Vector2Int> LocationsAvailable();

    // Function to determine if enemies are withing attacking range
    public abstract List<Vector2Int> EnemiesInRange();
    #endregion

    #region UNIVERSAL METHODS
    // Recursive location function for pieces that move multiple tiles
    public List<Vector2Int> RecursiveLocations(Vector2Int position, int moves, bool ignorePieces=false) {
        List<Vector2Int> locations = new List<Vector2Int>();

        if (!GameManager.ValidPosition(position))
            return locations;

        if (!ignorePieces && GameManager.Instance.IsPieceAt(position))
            return locations;

        if (moves > 0) {
            foreach (Vector2Int dir in this.directions) {
                Vector2Int nextTile = new Vector2Int(position.x + dir.x, position.y + dir.y);
                locations.Add(nextTile);
                locations = locations.Union(RecursiveLocations(nextTile, moves - 1, ignorePieces)).ToList();
            }
        }

        return locations;
    }

    // Will attempt to attack enemy piece with probabilities based on fuzzy-logic table
    // returning true if attack is successful, false otherwise. 
    public bool Attack(Piece enemy, bool isMoving = false) {
        // Simulate dice roll
        int roll = DiceManager.Instance.RollDice();

        // If piece is a Knight combining move and attack add one to the roll.
        if (isMoving)
            roll += 1;

        // Assign minimum attack number needed based off of fuzzy logic table
        int mininumValue = FuzzyLogic.FindFuzzyNumber(this, enemy);

        if (roll >= mininumValue)
            return true;

        return false;
    }
    #endregion
}

public abstract class Commander : Piece {
    public List<Piece> subordinates = new List<Piece>();
    public int commandActions = 1;
    public bool usedFreeMovement = false;
}
