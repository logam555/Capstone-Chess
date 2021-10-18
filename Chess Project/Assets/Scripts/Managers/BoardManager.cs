/* Written by David Corredor
 Edited by Braden Stonehill
 Last date edited: 10/14/2021
 BoardManager.cs - tracking and moving pieces, and checking the state of the pieces on
 the board and the players.

 Version 1.0:
  - Moved Functions that dealt with interaction with the virtual board to this class from the Game Manager
  - Added the place piece sound (tommy oh) */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoardManager : MonoBehaviour
{
    public Piece[,] Pieces { get; set; }
    public Piece SelectedPiece { get; set; }
    public static BoardManager Instance { get; set; }
    private GameManager gm;

    public ModelManager boardModel; 

    private void Awake() {
        Pieces = new Piece[8, 8];
    }

    private void Start() {
        gm = GameManager.Instance;

        GameObject tempGO = new GameObject();
        tempGO = GameObject.Find("Chess Board");

        boardModel = tempGO.GetComponent<ModelManager>();

        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("In BoardMangaer; test  Occupied " + boardModel.chessBoardGridCo.Count);
        }
    }

    #region PIECE INTERACTION FUNCTIONS - Functions that interact with the object representation of the pieces.
    // Function to select a valid piece and highlight all possible moves.
    public void SelectPiece(Vector2Int position) {
        if (IsPieceAt(position) && PieceAt(position).IsWhite == gm.CurrentPlayer.isWhite) {
            SelectedPiece = Pieces[position.x, position.y];
            gm.boardModel.HighlightAllTiles(position, AvailableMoves(SelectedPiece), SelectedPiece.EnemiesInRange(), SelectedPiece.Commander);
        } else {
            SelectedPiece = null;
            gm.boardModel.RemoveHighlights();
        }
    }

    // Function to move the selected piece in the array implementation of the board
    private void MovePiece(Vector2Int position) {
        // Check if selected piece is commander and is using free movement
        if (SelectedPiece is Commander && !SelectedPiece.Commander.usedFreeMovement && (Mathf.Abs((position - SelectedPiece.Position).sqrMagnitude) <= 2)) {
            SelectedPiece.Commander.usedFreeMovement = true;
            SelectedPiece.Commander.commandActions += 1;
        }

        //store old location
        Vector2Int oldPosition = new Vector2Int();
        oldPosition = SelectedPiece.Position;

        // Move piece to new position
        Pieces[SelectedPiece.Position.x, SelectedPiece.Position.y] = null;
        Pieces[position.x, position.y] = SelectedPiece;
        SelectedPiece.Position = position;
        GetComponent<AudioSource>().Play();
        
        //update Board with location new and old
        boardModel.BoardTileLocationUpdate(oldPosition, position);

        // Call function in board to move the piece game object
        gm.boardModel.MoveObject(SelectedPiece.gameObject, position);

        // Reduce number of actions remaining
        SelectedPiece.Commander.commandActions -= 1;

        // Deselect the piece
        SelectPiece(new Vector2Int(-1, -1));
    }

    // Function to delegate a piece to new commander
    public void DelegatePiece() {
        Piece pieceToDelegate = SelectedPiece;
        SelectPiece(new Vector2Int(-1, -1));
        SelectPiece(gm.boardModel.selection);

        if (SelectedPiece == pieceToDelegate) {
            if (pieceToDelegate.Delegated) {
                King king = (King)gm.CurrentPlayer.commanders.Find(commander => commander is King);
                king.RecallPiece(pieceToDelegate, pieceToDelegate.Commander);
            }

            SelectPiece(new Vector2Int(-1, -1));
        } else {
            if (SelectedPiece is Commander && !(SelectedPiece is King) && pieceToDelegate.Commander is King) {
                King king = (King)pieceToDelegate.Commander;
                king.DelegatePiece(pieceToDelegate, (Commander)SelectedPiece);
            }

            SelectPiece(new Vector2Int(-1, -1));
        }
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
            if (SelectedPiece is Knight && (Mathf.Abs((position - SelectedPiece.Position).sqrMagnitude) > 2))
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
                gm.CapturePiece(enemy);
                gm.boardModel.RemoveObject(enemy.gameObject);

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
                    SelectedPiece.Commander.commandActions -= 1;

                    // Deselect the piece
                    SelectPiece(new Vector2Int(-1, -1));
                }
            }

        } else if (IsFriendlyPieceAt(gm.CurrentPlayer.isWhite, position)) {
            gm.boardModel.RemoveHighlights();
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

    //converts List of Vector2Int into an array where each piece location is accessable as an integer
    public int[,] Vector2Int2Array(List<Vector2Int> v) {   //(5, 3)
        int[,] arr = new int[v.Count, 2];
        for (int index = 0; index < v.Count - 1; index++) {
            string tempString = v[index].ToString();
            arr[index, 0] = Convert.ToInt32(tempString.Substring(1, 1));
            arr[index, 1] = Convert.ToInt32(tempString.Substring(4, 1));
        }
        return arr;
    }

    //paramaters Bool isWhite and List<Vector2Int> of locationsAvailable 
    //Example Call: AIScanning(False, Pawn.Instance.LocationsAvailable); 
    //Returns an int[,] array of a given pieces local area scan. 
    //1=Empty square, 2=Friendly piece, 3=Enemy Piece, 0=Unknown/Square beyond local scan
    public int[,] AIScanning(bool isWhite, List<Vector2Int> LocationsAvailable) {
        int[,] AIScan = new int[8, 8];

        //LocationsAvailable = LocationsAvailable();

        int[,] locArray = new int[LocationsAvailable.Count, 1];
        locArray = Vector2Int2Array(LocationsAvailable);

        for (int count = 0; count < LocationsAvailable.Count; count++) {
            Debug.Log("Count: " + count);
            Debug.Log("PossibleLocations: " + LocationsAvailable[count]);
            if (IsFriendlyPieceAt(isWhite, LocationsAvailable[count]) == true) {
                AIScan[locArray[count, 0], locArray[count, 1]] = 2;
            } else if (IsEnemyPieceAt(isWhite, LocationsAvailable[count]) == true) {
                AIScan[locArray[count, 0], locArray[count, 1]] = 3;
            } else {
                AIScan[locArray[count, 0], locArray[count, 1]] = 1;
            }
        }

        return AIScan;
    }

    #endregion

    #region INSTATIATION FUNCTIONS - Functions that instantiate objects
    // Function to add pieces as subordinates to their appropriate commanders
    public void AttachCommandingPieces() {
        // Attach White Pieces

        // Attach white pieces to left white bishop
        Bishop leftBishop = (Bishop)PieceAt(new Vector2Int(2, 0));
        leftBishop.Commander = leftBishop;

        // Left Knight
        leftBishop.subordinates.Add(PieceAt(new Vector2Int(1, 0)));

        // Left three pawns
        leftBishop.subordinates.Add(PieceAt(new Vector2Int(0, 1)));
        leftBishop.subordinates.Add(PieceAt(new Vector2Int(1, 1)));
        leftBishop.subordinates.Add(PieceAt(new Vector2Int(2, 1)));

        // Set the commander for each piece
        foreach (Piece piece in leftBishop.subordinates) {
            piece.Commander = leftBishop;
        }

        // Attach white pieces to right white bishop
        Bishop rightBishop = (Bishop)PieceAt(new Vector2Int(5, 0));
        rightBishop.Commander = rightBishop;

        // Right Knight
        rightBishop.subordinates.Add(PieceAt(new Vector2Int(6, 0)));

        // Right three pawns
        rightBishop.subordinates.Add(PieceAt(new Vector2Int(7, 1)));
        rightBishop.subordinates.Add(PieceAt(new Vector2Int(6, 1)));
        rightBishop.subordinates.Add(PieceAt(new Vector2Int(5, 1)));

        // Set the commander for each piece
        foreach (Piece piece in rightBishop.subordinates) {
            piece.Commander = rightBishop;
        }

        // Attach white pieces to white king
        King king = (King)PieceAt(new Vector2Int(4, 0));
        king.Commander = king;

        // Center two pawns
        king.subordinates.Add(PieceAt(new Vector2Int(3, 1)));
        king.subordinates.Add(PieceAt(new Vector2Int(4, 1)));

        // Both Rooks
        king.subordinates.Add(PieceAt(new Vector2Int(0, 0)));
        king.subordinates.Add(PieceAt(new Vector2Int(7, 0)));

        // Queen
        king.subordinates.Add(PieceAt(new Vector2Int(3, 0)));

        // Set the commander for each piece
        foreach (Piece piece in king.subordinates) {
            piece.Commander = king;
        }

        // Attach commanding bishops to king
        leftBishop.superCommander = king;
        rightBishop.superCommander = king;

        king.leftBishop = leftBishop;
        king.rightBishop = rightBishop;


        // Attach black Pieces

        // Attach black pieces to left black bishop
        leftBishop = (Bishop)PieceAt(new Vector2Int(2, 7));
        leftBishop.Commander = leftBishop;

        // Left Knight
        leftBishop.subordinates.Add(PieceAt(new Vector2Int(1, 7)));

        // Left three pawns
        leftBishop.subordinates.Add(PieceAt(new Vector2Int(0, 6)));
        leftBishop.subordinates.Add(PieceAt(new Vector2Int(1, 6)));
        leftBishop.subordinates.Add(PieceAt(new Vector2Int(2, 6)));

        // Set the commander for each piece
        foreach (Piece piece in leftBishop.subordinates) {
            piece.Commander = leftBishop;
        }

        // Attach black pieces to right black bishop
        rightBishop = (Bishop)PieceAt(new Vector2Int(5, 7));
        rightBishop.Commander = rightBishop;

        // Right Knight
        rightBishop.subordinates.Add(PieceAt(new Vector2Int(6, 7)));

        // Right three pawns
        rightBishop.subordinates.Add(PieceAt(new Vector2Int(7, 6)));
        rightBishop.subordinates.Add(PieceAt(new Vector2Int(6, 6)));
        rightBishop.subordinates.Add(PieceAt(new Vector2Int(5, 6)));

        // Set the commander for each piece
        foreach (Piece piece in rightBishop.subordinates) {
            piece.Commander = rightBishop;
        }

        // Attach black pieces to black king
        king = (King)PieceAt(new Vector2Int(4, 7));
        king.Commander = king;

        // Center two pawns
        king.subordinates.Add(PieceAt(new Vector2Int(3, 6)));
        king.subordinates.Add(PieceAt(new Vector2Int(4, 6)));

        // Both Rooks
        king.subordinates.Add(PieceAt(new Vector2Int(0, 7)));
        king.subordinates.Add(PieceAt(new Vector2Int(7, 7)));

        // Queen
        king.subordinates.Add(PieceAt(new Vector2Int(3, 7)));

        // Set the commander for each piece
        foreach (Piece piece in king.subordinates) {
            piece.Commander = king;
        }

        // Attach commanding bishops to king
        leftBishop.superCommander = king;
        rightBishop.superCommander = king;

        king.leftBishop = leftBishop;
        king.rightBishop = rightBishop;
    }

    // Function to attach piece object with game model script
    public void LinkPiece(Vector2Int position, Piece piece) {
        Pieces[position.x, position.y] = piece;
        Pieces[position.x, position.y].Position = position;
        Pieces[position.x, position.y].Delegated = false;
    }
    #endregion
}
