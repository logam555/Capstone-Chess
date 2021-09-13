using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public BoardManager board; // Not instantiated, value is filled in the editor
    public Piece[,] Pieces { get; set; }
    public Piece selectedPiece { get; set; }
    public Dice dice;
    
    public bool isWhiteTurn { get; set; }
    [SerializeField]
    public GameObject playerOne;
    [SerializeField]
    public GameObject playerTwo;
    [SerializeField]
    public GameObject ai;
    [SerializeField]
    public GameObject currentPlayer;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        isWhiteTurn = true;
        playerOne.GetComponent<Player>().isWhite = true;
        playerTwo.GetComponent<Player>().isWhite = false;
        Pieces = new Piece[8, 8];
    }

   
    
    
    
   
    public void MovePiece(Vector2Int avaliablePosition) {
        bool[,] availableMoves = AvailableMoves(selectedPiece);
        if (availableMoves[avaliablePosition.x, avaliablePosition.y]) {
            Piece tempPiece = Pieces[avaliablePosition.x, avaliablePosition.y];
            if (!board.CanTakeTurn())
            {
                SwitchPlayers();
            }
            if (tempPiece != null)
            {
                if (CanAttack(avaliablePosition))
                {
                   
                    if (CheckTurns(tempPiece))
                    {
                        Attack(avaliablePosition);
                        selectedPiece = null;
                    }
                    else
                    {
                        selectedPiece = null;
                    }
                    
                    
                }
            }
            else
            {
                if (CheckTurns(tempPiece))
                {      
                    Move(avaliablePosition);
                    selectedPiece = null;
                }
                else
                {
                    selectedPiece = null;
                }
            }
         
        }
        selectedPiece = null;
        board.RemoveHighlights();
    }




    
    #region SELECTING, HIGHLIGHTING, SUBTRACT FUNCTIONS
    // Functions for highlighting an object
    public void SelectPiece(Vector2Int position)
    {
        if (Pieces[position.x, position.y] == null)
            return;

        if (Pieces[position.x, position.y].IsWhite != isWhiteTurn)
            return;

        selectedPiece = Pieces[position.x, position.y];
        HighlightMoves(position, selectedPiece);
    }
    public Piece SelectPieceFromPosition(Vector2Int position)
    {
        if (Pieces[position.x, position.y] == null)
            return null;

        if (Pieces[position.x, position.y].IsWhite != isWhiteTurn)
            return null;
        Piece tempPiece = Pieces[position.x, position.y];
        return tempPiece;
    }
    public void HighlightMoves(Vector2Int position, Piece selectedPiece)
    {
        board.HighlightAllTiles(position, AvailableMoves(selectedPiece), selectedPiece);

    }
    public bool[,] AvailableMoves(Piece piece)
    {
        bool[,] allowedMoves = new bool[8, 8];
        List<Vector2Int> possibleMoves = piece.LocationsAvailable();

        foreach (Vector2Int position in possibleMoves)
        {
            allowedMoves[position.x, position.y] = true;
        }
        return allowedMoves;
    }
    public bool SubtractTurn(Piece activePiece, Piece commander)
    {
        Piece SuperCommander = isWhiteTurn ? board.activePieces[0].GetComponent<Piece>()
            : board.activePieces[16].GetComponent<Piece>();
        if (activePiece.type == Piece.PieceType.King && (activePiece.numberOfTurns > 0 && activePiece.numberOfTurnsPawn > 0))
        {
            SuperCommander.numberOfTurns--;
            return true;
        }
        else if (commander.type == Piece.PieceType.King && (SuperCommander.numberOfTurns > 0 && SuperCommander.numberOfTurnsPawn > 0))
        {
            SuperCommander.numberOfTurns--;
            SuperCommander.numberOfTurnsPawn--;
            return true;
        }
        else if (commander.type == Piece.PieceType.Bishop && (SuperCommander.numberOfTurns > 0 && commander.numberOfTurns > 0))
        {
            commander.numberOfTurns--;
            SuperCommander.numberOfTurns--;
            return true;
        }
        else if(SuperCommander.numberOfTurns < 0)
        {
            SwitchPlayers();
            return false;
        }
        return false;
    }
    public bool CheckTurns(Piece tempPiece)
    {
        Player cPlayer = currentPlayer.GetComponent<Player>();
        Piece activePiece = null;
        if (selectedPiece != null)
        {
            activePiece = board.activePieces.Find(m => m.GetComponent<Piece>().id == selectedPiece.id).GetComponent<Piece>();

        }
        Piece commander = activePiece.type == Piece.PieceType.King ? null : activePiece.commander.GetComponent<Piece>();

        if (cPlayer.isWhite)
        {
            return SubtractTurn(activePiece, commander);
        }
        else
        {
            return SubtractTurn(activePiece, commander);
        }
    }
    #endregion

    #region ATTACKING
    public bool CanAttack(Vector2Int position)
    {
        if (IsThereEnemy(position))
        {
            if (IsEnemyBeside(position.x, position.y, selectedPiece))
            {
                return true;
            }
        }
        return false;
    }
    
    public bool IsEnemyBeside(int i, int j, Piece piece)
    {
        foreach (Vector2Int dir in piece.GetComponent<Piece>().Directions)
        {
            Vector2Int tempPosition = new Vector2Int(piece.Position.x + dir.x, piece.Position.y + dir.y);
            if (tempPosition.x == i && tempPosition.y == j)
            {
                return true;
            }
        }
        return false;

    }
    public bool IsThereEnemy(Vector2Int positionOfPiece)
    {
        if (isWhiteTurn == !Pieces[positionOfPiece.x, positionOfPiece.y].IsWhite || !isWhiteTurn == Pieces[positionOfPiece.x, positionOfPiece.y].IsWhite)
        {
            return true;
        }
        return false;
    }
    public bool IsThereEnemy(int i, int j)
    {
        if (isWhiteTurn == !Pieces[i, j].IsWhite || !isWhiteTurn == Pieces[i, j].IsWhite)
        {
            return true;
        }
        return false;
    }
    public bool IsAttackSuccessful(int rollNumber, Vector2Int avaliablePosition)
    {
        Piece pieceToAttack = Pieces[avaliablePosition.x, avaliablePosition.y];
        return FuzzyLogic.FindFuzzyLogic(rollNumber, pieceToAttack,Pieces);
    }
    public void Attack(Vector2Int position)
    {
        dice.isAttacking = true;

        bool isSuccessful = IsAttackSuccessful(dice.RollDice(), position);
        if (isSuccessful)
        {
            KillEnemy(Pieces[position.x, position.y]);
            // Move piece to new position
            Pieces[selectedPiece.Position.x, selectedPiece.Position.y] = null;
            Pieces[position.x, position.y] = selectedPiece;
            selectedPiece.Position = position;

            // Call function in board to move the piece game object
            board.MoveObject(selectedPiece.gameObject, position);
            // Change turn order

            dice.isAttacking = false;
        }
    }
    public void KillEnemy(Piece enemyPiece)
    {
        GameObject enemyObject = board.activePieces.Find(m => m.GetComponent<Piece>().Position.x == enemyPiece.Position.x && m.GetComponent<Piece>().Position.y == enemyPiece.Position.y);
        board.activePieces.Remove(enemyObject);
        Pieces[enemyPiece.Position.x, enemyPiece.Position.y] = null;
        Destroy(enemyObject);
    }
    #endregion
    
    #region MOVE FUNCTIONS
    public void Move(Vector2Int position)
    {
        Pieces[selectedPiece.Position.x, selectedPiece.Position.y] = null;
        Pieces[position.x, position.y] = selectedPiece;
        selectedPiece.Position = position;
        board.MoveObject(selectedPiece.gameObject, position);

    }
    #endregion
   
    #region RESETANDSWITCHING FUNCTIONS
    public void ResetTurns(GameObject currentPlayer)
    {
        if (currentPlayer.GetComponent<Player>().isWhite)
        {
            List<GameObject> kingCommander = board.activePieces.FindAll(m => m.GetComponent<Piece>().type == Piece.PieceType.King && m.GetComponent<Piece>().IsWhite);
            List<GameObject> bishopCommander = board.activePieces.FindAll(m => m.GetComponent<Piece>().type == Piece.PieceType.Bishop && m.GetComponent<Piece>().IsWhite);
            foreach (GameObject i in kingCommander)
            {
                i.GetComponent<Piece>().numberOfTurns = 3;
                i.GetComponent<Piece>().numberOfTurnsPawn = 1;
            }
            foreach (GameObject i in bishopCommander)
            {
                i.GetComponent<Piece>().numberOfTurns = 1;
            }

        }
        else
        {
            List<GameObject> kingCommander = board.activePieces.FindAll(m => m.GetComponent<Piece>().type == Piece.PieceType.King && !m.GetComponent<Piece>().IsWhite);
            List<GameObject> bishopCommander = board.activePieces.FindAll(m => m.GetComponent<Piece>().type == Piece.PieceType.Bishop && !m.GetComponent<Piece>().IsWhite);
            foreach (GameObject i in kingCommander)
            {
                i.GetComponent<Piece>().numberOfTurns = 3;
                i.GetComponent<Piece>().numberOfTurnsPawn = 1;
            }
            foreach (GameObject i in bishopCommander)
            {
                i.GetComponent<Piece>().numberOfTurns = 1;
            }
        }
    }

    public void SwitchPlayers()
    {
        //Reset turns for the current player
        ResetTurns(currentPlayer);
        //Switch players
        if (!isWhiteTurn)
        {
            isWhiteTurn = !isWhiteTurn;

            currentPlayer = playerOne;

        }
        else
        {
            isWhiteTurn = !isWhiteTurn;

            currentPlayer = playerTwo;
        }

    }
    #endregion
}
