using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ChessBoard : MonoBehaviour {

    public static ChessBoard Instance;

    public ChessPiece[,] Board { get; set; }
    public ChessPiece[,] KingBoard { get; set; }
    public ChessPiece[,] LBishopBoard  { get; set; }
    public ChessPiece[,] RBishopBoard { get;set; }
    public ChessPiece SelectedPiece { get; set; }

    [SerializeField]
    private AudioSource sound;

    private bool isAttacking;
    private Vector2Int selectedPos;

    private void Awake() {
        Instance = this;
        isAttacking = false;
        Board = Instantiate();
        KingBoard = Instantiate();
        LBishopBoard = Instantiate();
        RBishopBoard = Instantiate();
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
    private ChessPiece[,] Instantiate() {
        ChessPiece[,] board = new ChessPiece[8, 8];
        SelectedPiece = null;

        // Create White Pieces
        // Create Commanders
        King king = new King(true, new Vector2Int(4, 0), "WK");
        Bishop lBishop = new Bishop(true, new Vector2Int(2, 0), "WB1", king);
        Bishop rBishop = new Bishop(true, new Vector2Int(5, 0), "WB2", king);

        // Add Commanders to board
        board[4, 0] = king;
        board[2, 0] = lBishop;
        board[5, 0] = rBishop;

        // Create Rooks
        board[0, 0] = new Rook(true, new Vector2Int(0, 0), king, "WR1");
        board[7, 0] = new Rook(true, new Vector2Int(7, 0), king, "WR2");

        // Create Knights
        board[1, 0] = new Knight(true, new Vector2Int(1, 0), lBishop, "WKn1");
        board[6, 0] = new Knight(true, new Vector2Int(6, 0), rBishop, "WKn2");

        // Create Queen
        board[3, 0] = new Queen(true, new Vector2Int(3, 0), king, "WQ");

        // Create Pawns
        for (int i = 0; i < 8; i++) {
            if (i < 3)
                board[i, 1] = new Pawn(true, new Vector2Int(i, 1), lBishop, "WP" + i.ToString());
            else if(i < 5)
                board[i, 1] = new Pawn(true, new Vector2Int(i, 1), king, "WP" + i.ToString());
            else
                board[i, 1] = new Pawn(true, new Vector2Int(i, 1), rBishop, "WP" + i.ToString());
        }


        // Create Black Pieces
        // Create Commanders
        king = new King(false, new Vector2Int(4, 7), "BK");
        lBishop = new Bishop(false, new Vector2Int(2, 7), "BB1", king);
        rBishop = new Bishop(false, new Vector2Int(5, 7), "BB2", king);

        // Add Commanders to board
        board[4, 7] = king;
        board[2, 7] = lBishop;
        board[5, 7] = rBishop;

        // Create Rooks
        board[0, 7] = new Rook(false, new Vector2Int(0, 7), king, "BR1");
        board[7, 7] = new Rook(false, new Vector2Int(7, 7), king, "BR2");

        // Create Knights
        board[1, 7] = new Knight(false, new Vector2Int(1, 7), lBishop, "BKn1");
        board[6, 7] = new Knight(false, new Vector2Int(6, 7), rBishop, "BKn2");

        // Create Queen
        board[3, 7] = new Queen(false, new Vector2Int(3, 7), king, "BQ");

        // Create Pawns
        for (int i = 0; i < 8; i++) {
            if (i < 3)
                board[i, 6] = new Pawn(false, new Vector2Int(i, 6), lBishop, "BP" + i.ToString());
            else if (i < 5)
                board[i, 6] = new Pawn(false, new Vector2Int(i, 6), king, "BP" + i.ToString());
            else
                board[i, 6] = new Pawn(false, new Vector2Int(i, 6), rBishop, "BP" + i.ToString());
        }

        // Call for attaching commanders when all pieces are spawned
        AttachSubordinatePieces(board);

        return board;
    }

    private void AttachSubordinatePieces(ChessPiece[,] board) {
        //Attach White side
        for(int i = 0; i < 8; i++) {
            for(int j = 0; j < 2; j++) {
                ChessPiece piece = board[i, j];
                if(piece is Subordinate) {
                    Subordinate subordinate = (Subordinate)piece;
                    subordinate.Commander.subordinates.Add(subordinate);
                }
            }
        }

        // Attach Black side
        for (int i = 0; i < 8; i++) {
            for (int j = 6; j < 8; j++) {
                ChessPiece piece = board[i, j];
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
    public ChessPiece PieceAt(Vector2Int position, ChessPiece[,] board) {
        if (ValidPosition(position))
            return board[position.x, position.y];
        return null;
    }

    // Function that returns true if space is occupied by an enemy piece
    public bool IsEnemyPieceAt(bool isWhite, Vector2Int position, ChessPiece[,] board) {
        ChessPiece piece = PieceAt(position, board);

        if (piece == null)
            return false;

        if (piece.IsWhite == isWhite)
            return false;

        return true;
    }

    // Function that returns true if space is occupied by a friendly piece
    public bool IsFriendlyPieceAt(bool isWhite, Vector2Int position, ChessPiece[,] board) {
        ChessPiece piece = PieceAt(position, board);

        if (piece == null)
            return false;

        if (piece.IsWhite != isWhite)
            return false;

        return true;
    }

    // Function that returns true if space is occupied
    public bool IsPieceAt(Vector2Int position, ChessPiece[,] board) {
        ChessPiece piece = PieceAt(position, board);

        if (piece == null)
            return false;

        return true;
    }

    // Function to filter tiles in movement range to valid positions
    public List<Vector2Int> FilterMoveRange(ChessPiece piece, ChessPiece[,] board) {
        List<Vector2Int> possibleMoves = piece.MoveRange(board);

        possibleMoves.RemoveAll(pos => !ValidPosition(pos));
        possibleMoves.RemoveAll(pos => IsPieceAt(pos, board));

        return possibleMoves;
    }

    // Function to filter tiles in attack range to valid positions
    public List<Vector2Int> FilterAttackRange(ChessPiece piece, ChessPiece[,] board) {
        List<Vector2Int> possibleMoves = piece.AttackRange(board);

        possibleMoves.RemoveAll(pos => !ValidPosition(pos));
        possibleMoves.RemoveAll(pos => !IsEnemyPieceAt(piece.IsWhite, pos, board));

        return possibleMoves;
    }
    #endregion

    #region INTERACTION
    // Function to select a valid piece and highlight all possible moves.
    public void SelectPiece(Vector2Int position) {
        if(IsPieceAt(position, Board) && PieceAt(position, Board).IsWhite == GameManager.Instance.CurrentPlayer.isWhite) {
            SelectedPiece = Board[position.x, position.y];
            ModelManager.Instance.HighlightAllTiles(position, FilterMoveRange(SelectedPiece, Board), FilterAttackRange(SelectedPiece, Board), SelectedPiece is Subordinate ? ((Subordinate)SelectedPiece).Commander : (Commander)SelectedPiece);
        }
        else {
            SelectedPiece = null;
            ModelManager.Instance.RemoveHighlights();
        }
    }

    // Function to move the selected piece in the array implementation of the board
    private void MovePiece(Vector2Int position, bool attacked) {

        // Check if selected piece is commander and is using free movement
        if (SelectedPiece is Commander) {
            Commander commander = (Commander)SelectedPiece;

            if(!commander.usedFreeMovement && (Mathf.Abs((position - SelectedPiece.Position).sqrMagnitude) <= 2)) {
                commander.usedFreeMovement = true;
                commander.commandActions += 1;
            }
        }

        if (SelectedPiece is Knight) {
            if(attacked)
                ((Knight)SelectedPiece).Moved = false;
            else
                ((Knight)SelectedPiece).Moved = true;
        }

        ChessPiece selectedPieceKing = PieceAt(SelectedPiece.Position, KingBoard);
        ChessPiece selectedPieceLBishop = PieceAt(SelectedPiece.Position, LBishopBoard);
        ChessPiece selectedPieceRBishop = PieceAt(SelectedPiece.Position, RBishopBoard);

        //store old location;
        Vector2Int oldPosition = SelectedPiece.Position;

        // Move piece to new position
        Board[SelectedPiece.Position.x, SelectedPiece.Position.y] = null;
        Board[position.x, position.y] = SelectedPiece;
        KingBoard[SelectedPiece.Position.x, SelectedPiece.Position.y] = null;
        KingBoard[position.x, position.y] = selectedPieceKing;
        LBishopBoard[SelectedPiece.Position.x, SelectedPiece.Position.y] = null;
        LBishopBoard[position.x, position.y] = selectedPieceLBishop;
        RBishopBoard[SelectedPiece.Position.x, SelectedPiece.Position.y] = null;
        RBishopBoard[position.x, position.y] = selectedPieceRBishop;
        SelectedPiece.Position = position;
        selectedPieceKing.Position = position;
        selectedPieceLBishop.Position = position;
        selectedPieceRBishop.Position = position;
        sound.Play();

        ModelManager.Instance.BoardTileLocationUpdate(oldPosition, position, SelectedPiece.IsWhite, SelectedPiece.GetType().Name, ModelManager.Instance.chessBoardGridCo);
        ModelManager.Instance.BoardTileLocationUpdate(oldPosition, position, SelectedPiece.IsWhite, SelectedPiece.GetType().Name, ModelManager.Instance.chessBoardCopyKing);
        ModelManager.Instance.BoardTileLocationUpdate(oldPosition, position, SelectedPiece.IsWhite, SelectedPiece.GetType().Name, ModelManager.Instance.chessBoardCopyBishop1);
        ModelManager.Instance.BoardTileLocationUpdate(oldPosition, position, SelectedPiece.IsWhite, SelectedPiece.GetType().Name, ModelManager.Instance.chessBoardCopyBishop2);

        // Call function in board to move the piece game object
        ModelManager.Instance.pieceObject = SelectedPiece;
        ModelManager.Instance.position = position;
        ModelManager.Instance.duration = 5;


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
        if (SelectedPiece is Knight && ((Knight)SelectedPiece).Moved)
            isMoving = true;

        // Change optional isMoving parameter if selected piece is a Knight attacking a non-adjacent piece
        if (isMoving) {
            attackSuccessful = FuzzyLogic.FindFuzzyNumber(SelectedPiece, Board[position.x, position.y]) <= DiceManager.Instance.diceNumber + 1;
        } else
            attackSuccessful = FuzzyLogic.FindFuzzyNumber(SelectedPiece, Board[position.x, position.y]) <= DiceManager.Instance.diceNumber;

        // Capture piece and remove model if attack is successful
        if (attackSuccessful) {
            // Remove the captured piece and add to capture pieces
            ChessPiece enemy = Board[position.x, position.y];
            ChessPiece enemyKing = KingBoard[position.x, position.y];
            ChessPiece enemyLBishop = LBishopBoard[position.x, position.y];
            ChessPiece enemyRBishop = RBishopBoard[position.x, position.y];

            GameManager.Instance.CapturePiece(enemy);
            ModelManager.Instance.RemoveObject(enemy);
            if (enemy is Subordinate) {
                ((Subordinate)enemy).Commander.subordinates.Remove((Subordinate)enemy);
                ((Subordinate)enemyKing).Commander.subordinates.Remove((Subordinate)enemyKing);
                ((Subordinate)enemyLBishop).Commander.subordinates.Remove((Subordinate)enemyLBishop);
                ((Subordinate)enemyRBishop).Commander.subordinates.Remove((Subordinate)enemyRBishop);
            }

            if (SelectedPiece is Knight)
                ((Knight)SelectedPiece).Moved = true;

            // Move the selected piece
            MovePiece(position, true);

        } else {
            // Reduce number of actions remaining
            Commander leader = SelectedPiece is Subordinate ? ((Subordinate)SelectedPiece).Commander : (Commander)SelectedPiece;
            leader.commandActions -= 1;

            if(SelectedPiece is Knight)
                ((Knight)SelectedPiece).Moved = false;

            // Deselect the piece
            SelectPiece(new Vector2Int(-1, -1));
        }
    }

    // Function to delegate a piece to new commander
    public void DelegatePiece() {
        if (SelectedPiece is Commander)
            return;

        Subordinate pieceToDelegate = (Subordinate)SelectedPiece;
        Subordinate pieceToDelegateKing = (Subordinate)PieceAt(SelectedPiece.Position, KingBoard);
        Subordinate pieceToDelegateLBishop = (Subordinate)PieceAt(SelectedPiece.Position, LBishopBoard);
        Subordinate pieceToDelegateRBishop = (Subordinate)PieceAt(SelectedPiece.Position, RBishopBoard);
        SelectPiece(new Vector2Int(-1, -1));
        SelectPiece(ModelManager.Instance.selection);

        if (SelectedPiece == (ChessPiece)pieceToDelegate) {
            if (pieceToDelegate.Delegated) {
                King king = (King)GameManager.Instance.CurrentPlayer.commanders.Find(commander => commander is King);
                King kingKing = (King)PieceAt(king.Position, KingBoard);
                King kingLBishop = (King)PieceAt(king.Position, LBishopBoard);
                King kingRBishop = (King)PieceAt(king.Position, RBishopBoard);
                king.RecallPiece(pieceToDelegate);
                kingKing.RecallPiece(pieceToDelegateKing);
                kingLBishop.RecallPiece(pieceToDelegateLBishop);
                kingRBishop.RecallPiece(pieceToDelegateRBishop);
            }

            SelectPiece(new Vector2Int(-1, -1));
        } else {
            if (SelectedPiece is Commander && !(SelectedPiece is King) && pieceToDelegate.Commander is King) {
                King king = (King)pieceToDelegate.Commander;
                King kingKing = (King)PieceAt(king.Position, KingBoard);
                King kingLBishop = (King)PieceAt(king.Position, LBishopBoard);
                King kingRBishop = (King)PieceAt(king.Position, RBishopBoard);
                king.DelegatePiece(pieceToDelegate, (Commander)SelectedPiece);
                kingKing.DelegatePiece(pieceToDelegateKing, (Commander)PieceAt(SelectedPiece.Position, KingBoard));
                kingLBishop.DelegatePiece(pieceToDelegateLBishop, (Commander)PieceAt(SelectedPiece.Position, LBishopBoard));
                kingRBishop.DelegatePiece(pieceToDelegateRBishop, (Commander)PieceAt(SelectedPiece.Position, RBishopBoard));
            }

            SelectPiece(new Vector2Int(-1, -1));
        }
    }

    public void DelegatePieceAI(Vector2Int position)
    {
        if (SelectedPiece is Commander)
            return;

        Subordinate pieceToDelegate = (Subordinate)SelectedPiece;
        Subordinate pieceToDelegateKing = (Subordinate)PieceAt(SelectedPiece.Position, KingBoard);
        Subordinate pieceToDelegateLBishop = (Subordinate)PieceAt(SelectedPiece.Position, LBishopBoard);
        Subordinate pieceToDelegateRBishop = (Subordinate)PieceAt(SelectedPiece.Position, RBishopBoard);
        SelectPiece(new Vector2Int(-1, -1));
        SelectPiece(position);

        if (SelectedPiece == (ChessPiece)pieceToDelegate)
        {
            if (pieceToDelegate.Delegated)
            {
                King king = (King)GameManager.Instance.CurrentPlayer.commanders.Find(commander => commander is King);
                King kingKing = (King)PieceAt(king.Position, KingBoard);
                King kingLBishop = (King)PieceAt(king.Position, LBishopBoard);
                King kingRBishop = (King)PieceAt(king.Position, RBishopBoard);
                king.RecallPiece(pieceToDelegate);
                kingKing.RecallPiece(pieceToDelegateKing);
                kingLBishop.RecallPiece(pieceToDelegateLBishop);
                kingRBishop.RecallPiece(pieceToDelegateRBishop);
            }

            SelectPiece(new Vector2Int(-1, -1));
        }
        else
        {
            if (SelectedPiece is Commander && !(SelectedPiece is King) && pieceToDelegate.Commander is King)
            {
                King king = (King)pieceToDelegate.Commander;
                King kingKing = (King)PieceAt(king.Position, KingBoard);
                King kingLBishop = (King)PieceAt(king.Position, LBishopBoard);
                King kingRBishop = (King)PieceAt(king.Position, RBishopBoard);
                king.DelegatePiece(pieceToDelegate, (Commander)SelectedPiece);
                kingKing.DelegatePiece(pieceToDelegateKing, (Commander)PieceAt(SelectedPiece.Position, KingBoard));
                kingLBishop.DelegatePiece(pieceToDelegateLBishop, (Commander)PieceAt(SelectedPiece.Position, LBishopBoard));
                kingRBishop.DelegatePiece(pieceToDelegateRBishop, (Commander)PieceAt(SelectedPiece.Position, RBishopBoard));
            }

            SelectPiece(new Vector2Int(-1, -1));
        }
    }

    // Function to check if the selected tile is a valid move for the selected piece and perform appropriate action.
    public bool PerformAction(Vector2Int position) {
        // Deselect the selected piece if position is not valid
        if (!ValidPosition(position) || SelectedPiece is null) {
            SelectPiece(position);
            return false;
        }


        // Get all available positions to move and list of enemy positions in range
        List<Vector2Int> availableMoves = FilterMoveRange(SelectedPiece, Board);
        List<Vector2Int> enemies = FilterAttackRange(SelectedPiece, Board);

        // Move piece if position is in available moves, attack enemy piece if position contains enemy, or change selection if position contains a friendly piece
        if (availableMoves.Contains(position)) {
            MovePiece(position, false);

        } else if (enemies.Contains(position)) {
            isAttacking = true;
            selectedPos = position;
            ModelManager.Instance.HighlightTile(2,position.x,position.y);
            ModelManager.Instance.HighlightSelected(SelectedPiece.Position);
            return true;
        } else if (IsFriendlyPieceAt(SelectedPiece.IsWhite, position, Board)) {
            ModelManager.Instance.RemoveHighlights();
            SelectPiece(position);

        } else
            // Deselect the piece
            SelectPiece(new Vector2Int(-1, -1));

        return false;
    }
    #endregion
}
