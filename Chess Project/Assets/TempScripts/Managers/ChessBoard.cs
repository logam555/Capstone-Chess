using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ChessBoard : MonoBehaviour {

    public static ChessBoard Instance;

    public ChessPiece[,] Board { get; set; }
    public ChessPiece SelectedPiece { get; set; }

    [SerializeField]
    private AudioSource audio;

    private bool isAttacking;
    private Vector2Int selectedPos;

    private void Awake() {
        Instance = this;
        isAttacking = false;
        Instantiate();
    }

    private void Start() {
        ModelManager.Instance.LinkModels(Board);
        GameManager.Instance.LinkCommanders(Board);
    }

    private void Update() {
        if (isAttacking && !DiceManager.Instance.thrown) {
            DiceManager.Instance.RollDice();
            isAttacking = false;
        }
        if (DiceManager.Instance.hasLanded && DiceManager.Instance.GetComponent<Rigidbody>().IsSleeping()) {
            DiceManager.Instance.hasLanded = false;
            AttackPiece(selectedPos);    
        }

        if (GameManager.Instance.CurrentPlayer.name == "Human" && !isAttacking) {

            if (Input.GetMouseButtonDown(1) && SelectedPiece != null) {
                DelegatePiece();
            }
            if (Input.GetMouseButtonDown(0)) {
                if (SelectedPiece == null) {
                    SelectPiece(ModelManager.Instance.selection);

                } else {
                    PerformAction(ModelManager.Instance.selection);
                }
            }
        }
    }


    #region INSTANTIATION
    private void Instantiate() {
        Board = new ChessPiece[8, 8];
        SelectedPiece = null;

        // Create White Pieces
        // Create Commanders
        King king = new King(true, new Vector2Int(4, 0), "WK");
        Bishop lBishop = new Bishop(true, new Vector2Int(2, 0), "WB1", king);
        Bishop rBishop = new Bishop(true, new Vector2Int(5, 0), "WB2", king);

        // Add Commanders to board
        Board[4, 0] = king;
        Board[2, 0] = lBishop;
        Board[5, 0] = rBishop;

        // Create Rooks
        Board[0, 0] = new Rook(true, new Vector2Int(0, 0), king, "WR1");
        Board[7, 0] = new Rook(true, new Vector2Int(7, 0), king, "WR2");

        // Create Knights
        Board[1, 0] = new Knight(true, new Vector2Int(1, 0), lBishop, "WKn1");
        Board[6, 0] = new Knight(true, new Vector2Int(6, 0), rBishop, "WKn2");

        // Create Queen
        Board[3, 0] = new Queen(true, new Vector2Int(3, 0), king, "WQ");

        // Create Pawns
        for (int i = 0; i < 8; i++) {
            if (i < 3)
                Board[i, 1] = new Pawn(true, new Vector2Int(i, 1), lBishop, "WP" + i.ToString());
            else if(i < 5)
                Board[i, 1] = new Pawn(true, new Vector2Int(i, 1), king, "WP" + i.ToString());
            else
                Board[i, 1] = new Pawn(true, new Vector2Int(i, 1), rBishop, "WP" + i.ToString());
        }


        // Create Black Pieces
        // Create Commanders
        king = new King(false, new Vector2Int(4, 7), "BK");
        lBishop = new Bishop(false, new Vector2Int(2, 7), "BB1", king);
        rBishop = new Bishop(false, new Vector2Int(5, 7), "BB2", king);

        // Add Commanders to board
        Board[4, 7] = king;
        Board[2, 7] = lBishop;
        Board[5, 7] = rBishop;

        // Create Rooks
        Board[0, 7] = new Rook(false, new Vector2Int(0, 7), king, "BR1");
        Board[7, 7] = new Rook(false, new Vector2Int(7, 7), king, "BR2");

        // Create Knights
        Board[1, 7] = new Knight(false, new Vector2Int(1, 7), lBishop, "BKn1");
        Board[6, 7] = new Knight(false, new Vector2Int(6, 7), rBishop, "BKn2");

        // Create Queen
        Board[3, 7] = new Queen(false, new Vector2Int(3, 7), king, "BQ");

        // Create Pawns
        for (int i = 0; i < 8; i++) {
            if (i < 3)
                Board[i, 6] = new Pawn(false, new Vector2Int(i, 6), lBishop, "BP" + i.ToString());
            else if (i < 5)
                Board[i, 6] = new Pawn(false, new Vector2Int(i, 6), king, "BP" + i.ToString());
            else
                Board[i, 6] = new Pawn(false, new Vector2Int(i, 6), rBishop, "BP" + i.ToString());
        }

        // Call for attaching commanders when all pieces are spawned
        AttachSubordinatePieces();
    }

    private void AttachSubordinatePieces() {
        //Attach White side
        for(int i = 0; i < 8; i++) {
            for(int j = 0; j < 2; j++) {
                ChessPiece piece = Board[i, j];
                if(piece is Subordinate) {
                    Subordinate subordinate = (Subordinate)piece;
                    subordinate.Commander.subordinates.Add(subordinate);
                }
            }
        }

        // Attach Black side
        for (int i = 0; i < 8; i++) {
            for (int j = 6; j < 8; j++) {
                ChessPiece piece = Board[i, j];
                if (piece is Subordinate) {
                    Subordinate subordinate = (Subordinate)piece;
                    subordinate.Commander.subordinates.Add(subordinate);
                }
            }
        }
    }
    #endregion

    #region EVALUATION
    // Function that returns true if position is within boundaries of the board.
    public bool ValidPosition(Vector2Int position) {
        if (position.x < 0 || position.x > 7 || position.y < 0 || position.y > 7)
            return false;

        return true;
    }

    // Function that returns the object at board position
    public ChessPiece PieceAt(Vector2Int position) {
        if (ValidPosition(position))
            return Board[position.x, position.y];
        return null;
    }

    // Function that returns true if space is occupied by an enemy piece
    public bool IsEnemyPieceAt(bool isWhite, Vector2Int position) {
        ChessPiece piece = PieceAt(position);

        if (piece == null)
            return false;

        if (piece.IsWhite == isWhite)
            return false;

        return true;
    }

    // Function that returns true if space is occupied by a friendly piece
    public bool IsFriendlyPieceAt(bool isWhite, Vector2Int position) {
        ChessPiece piece = PieceAt(position);

        if (piece == null)
            return false;

        if (piece.IsWhite != isWhite)
            return false;

        return true;
    }

    // Function that returns true if space is occupied
    public bool IsPieceAt(Vector2Int position) {
        ChessPiece piece = PieceAt(position);

        if (piece == null)
            return false;

        return true;
    }

    // Function to filter tiles in movement range to valid positions
    public List<Vector2Int> FilterMoveRange(ChessPiece piece) {
        List<Vector2Int> possibleMoves = piece.MoveRange();

        possibleMoves.RemoveAll(pos => !ValidPosition(pos));
        possibleMoves.RemoveAll(pos => IsPieceAt(pos));

        return possibleMoves;
    }

    // Function to filter tiles in attack range to valid positions
    public List<Vector2Int> FilterAttackRange(ChessPiece piece) {
        List<Vector2Int> possibleMoves = piece.AttackRange();

        possibleMoves.RemoveAll(pos => !ValidPosition(pos));
        possibleMoves.RemoveAll(pos => !IsEnemyPieceAt(piece.IsWhite, pos));

        return possibleMoves;
    }
    #endregion

    #region INTERACTION
    // Function to select a valid piece and highlight all possible moves.
    public void SelectPiece(Vector2Int position) {
        if(IsPieceAt(position) && PieceAt(position).IsWhite == GameManager.Instance.CurrentPlayer.isWhite) {
            SelectedPiece = Board[position.x, position.y];
            ModelManager.Instance.HighlightAllTiles(position, FilterMoveRange(SelectedPiece), FilterAttackRange(SelectedPiece), SelectedPiece is Subordinate ? ((Subordinate)SelectedPiece).Commander : (Commander)SelectedPiece);
        }
        else {
            SelectedPiece = null;
            ModelManager.Instance.RemoveHighlights();
        }
    }

    // Function to move the selected piece in the array implementation of the board
    private void MovePiece(Vector2Int position) {

        // Check if selected piece is commander and is using free movement
        if (SelectedPiece is Commander) {
            Commander commander = (Commander)SelectedPiece;

            if(!commander.usedFreeMovement && (Mathf.Abs((position - SelectedPiece.Position).sqrMagnitude) <= 2)) {
                commander.usedFreeMovement = true;
                commander.commandActions += 1;
            }
        }

        //store old location;
        Vector2Int oldPosition = SelectedPiece.Position;

        // Move piece to new position
        Board[SelectedPiece.Position.x, SelectedPiece.Position.y] = null;
        Board[position.x, position.y] = SelectedPiece;
        SelectedPiece.Position = position;
        audio.Play();

        ModelManager.Instance.BoardTileLocationUpdate(oldPosition, position, SelectedPiece.IsWhite, SelectedPiece.GetType().Name); ;

        // Call function in board to move the piece game object
        ModelManager.Instance.MoveObject(SelectedPiece, position);


        // Reduce number of actions remaining
        Commander leader = SelectedPiece is Subordinate ? ((Subordinate)SelectedPiece).Commander : (Commander)SelectedPiece;
        leader.commandActions -= 1;

        // Deselect the piece
        SelectPiece(new Vector2Int(-1, -1));
    }

    // Function to attack a piece at the position
    public void AttackPiece(Vector2Int position) {
        bool isMoving = false;
        bool attackSuccessful;

       
        // Check if selected piece is a knight and is attacking a non-adjacent piece
        if (SelectedPiece is Knight && (Mathf.Abs((position - SelectedPiece.Position).sqrMagnitude) > 2))
            isMoving = true;

        // Change optional isMoving parameter if selected piece is a Knight attacking a non-adjacent piece
        if (isMoving)
            attackSuccessful = FuzzyLogic.FindFuzzyNumber(SelectedPiece, Board[position.x, position.y]) <= DiceManager.Instance.diceNumber + 1;
        else
            attackSuccessful = FuzzyLogic.FindFuzzyNumber(SelectedPiece, Board[position.x, position.y]) <= DiceManager.Instance.diceNumber;

        // Capture piece and remove model if attack is successful
        if (attackSuccessful) {
            // Remove the captured piece and add to capture pieces
            ChessPiece enemy = Board[position.x, position.y];
            GameManager.Instance.CapturePiece(enemy);
            ModelManager.Instance.RemoveObject(enemy);

            // Move the selected piece
            MovePiece(position);

        } else {
            // Move knight next to defending piece if attacking a non-adjacent piece
            if (SelectedPiece is Knight && isMoving) {
                List<Vector2Int> locations = FilterMoveRange(SelectedPiece);

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
                Commander leader = SelectedPiece is Subordinate ? ((Subordinate)SelectedPiece).Commander : (Commander)SelectedPiece;
                leader.commandActions -= 1;

                // Deselect the piece
                SelectPiece(new Vector2Int(-1, -1));
            }
        }
    }

    // Function to delegate a piece to new commander
    public void DelegatePiece() {
        if (SelectedPiece is Commander)
            return;

        Subordinate pieceToDelegate = (Subordinate)SelectedPiece;
        SelectPiece(new Vector2Int(-1, -1));
        SelectPiece(ModelManager.Instance.selection);

        if (SelectedPiece == (ChessPiece)pieceToDelegate) {
            if (pieceToDelegate.Delegated) {
                King king = (King)GameManager.Instance.CurrentPlayer.commanders.Find(commander => commander is King);
                king.RecallPiece(pieceToDelegate);
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

    // Function to check if the selected tile is a valid move for the selected piece and perform appropriate action.
    public void PerformAction(Vector2Int position) {
        // Deselect the selected piece if position is not valid
        if (!ValidPosition(position)) {
            SelectPiece(position);
            return;
        }


        // Get all available positions to move and list of enemy positions in range
        List<Vector2Int> availableMoves = FilterMoveRange(SelectedPiece);
        List<Vector2Int> enemies = FilterAttackRange(SelectedPiece);

        // Move piece if position is in available moves, attack enemy piece if position contains enemy, or change selection if position contains a friendly piece
        if (availableMoves.Contains(position)) {
            MovePiece(position);

        } else if (enemies.Contains(position)) {
            isAttacking = true;
            selectedPos = position;
        } else if (IsFriendlyPieceAt(SelectedPiece.IsWhite, position)) {
            ModelManager.Instance.RemoveHighlights();
            SelectPiece(position);

        } else
            // Deselect the piece
            SelectPiece(new Vector2Int(-1, -1));
    }
    #endregion
}
