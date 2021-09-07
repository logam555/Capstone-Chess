using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    public BoardManager board; // Not instantiated, value is filled in the editor
    public Piece[,] Pieces { get; set; }
    public Piece SelectedPiece { get; set; }
    public Dice dice;
    
    public bool IsWhiteTurn { get; set; }
    public bool IsAttacking { get; set; }
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        IsWhiteTurn = true;
        Pieces = new Piece[8, 8];
        IsAttacking = false;
    }

    public void SelectPiece(Vector2Int position) {
        if (Pieces[position.x, position.y] == null)
            return;

        if (Pieces[position.x, position.y].IsWhite != IsWhiteTurn)
            return;

        SelectedPiece = Pieces[position.x, position.y];
        board.HighlightAllTiles(position, AvailableMoves(SelectedPiece),SelectedPiece);
    }

    public void MovePiece(Vector2Int avaliablePosition) {
        bool[,] availableMoves = AvailableMoves(SelectedPiece);
        
        if (availableMoves[avaliablePosition.x, avaliablePosition.y]) {
            Piece tempPiece = Pieces[avaliablePosition.x, avaliablePosition.y];
            if(tempPiece != null)
            {
                if (CanAttack(avaliablePosition))
                {
                    Attack(avaliablePosition);
                }
            }
            else
            {
                Move(avaliablePosition);
            }
        }
        SelectedPiece = null;
        board.RemoveHighlights();
    }
    public bool CanAttack(Vector2Int position)
    {
        if (IsThereEnemy(position))
        {
            if (IsEnemyBeside(position.x,position.y, SelectedPiece))
            {
                return true;
            }
        }
        return false;
    }
    public void Attack(Vector2Int position)
    {
        dice.isAttacking = true;

        bool isSuccessful = IsAttackSuccessful(dice.RollDice(),position);
        if (isSuccessful)
        {
            KillEnemy(Pieces[position.x, position.y]);
            // Move piece to new position
            Pieces[SelectedPiece.Position.x, SelectedPiece.Position.y] = null;
            Pieces[position.x, position.y] = SelectedPiece;
            SelectedPiece.Position = position;

            // Call function in board to move the piece game object
            board.MoveObject(SelectedPiece.gameObject, position);

            // Change turn order
            IsWhiteTurn = !IsWhiteTurn;
            dice.isAttacking = false;
        }   
    }
    public void Move(Vector2Int position)
    {
        Pieces[SelectedPiece.Position.x, SelectedPiece.Position.y] = null;
        Pieces[position.x, position.y] = SelectedPiece;
        SelectedPiece.Position = position;
        board.MoveObject(SelectedPiece.gameObject, position);
        IsWhiteTurn = !IsWhiteTurn;
    }
    public bool IsEnemyBeside(Vector2Int loc, Piece piece)
    {
        foreach (Vector2Int dir in piece.GetComponent<Piece>().Directions)
        {
            Vector2Int tempPosition = new Vector2Int(piece.Position.x + dir.x, piece.Position.y + dir.y);
            if (tempPosition == loc)
            {
                return true;
            }
        }
        return false;

    }
    public bool IsAttackSuccessful(int rollNumber,Vector2Int avaliablePosition)
    {
        Piece pieceToAttack = Pieces[avaliablePosition.x, avaliablePosition.y];
        return FindFuzzyLogic(rollNumber, pieceToAttack);
    }
    public bool FindFuzzyLogic(int rollNumber,Piece avaliablePosition)
    {
        Piece pieceToAttack = Pieces[avaliablePosition.Position.x, avaliablePosition.Position.y];

        if(pieceToAttack.type == Piece.PieceType.King)
        {
            int numberToMatch =FuzzyLogic.FindNumberKing(pieceToAttack);
            if(rollNumber == numberToMatch)
            {
                return true;
            }
        }
        else if (pieceToAttack.type == Piece.PieceType.Queen)
        {
            int numberToMatch = FuzzyLogic.FindNumberQueen(pieceToAttack);
            if (rollNumber == numberToMatch)
            {
                return true;
            }
        }
        else if (pieceToAttack.type == Piece.PieceType.Bishop)
        {
            int numberToMatch = FuzzyLogic.FindNumberBishop(pieceToAttack);
            if (rollNumber == numberToMatch)
            {
                return true;
            }
        }
        else if (pieceToAttack.type == Piece.PieceType.Pawn)
        {
            int numberToMatch = FuzzyLogic.FindNumberPawn(pieceToAttack);
            if (rollNumber == numberToMatch)
            {
                return true;
            }
        }
        else if (pieceToAttack.type == Piece.PieceType.Knight)
        {
            int numberToMatch = FuzzyLogic.FindNumberKnight(pieceToAttack);
            if (rollNumber == numberToMatch)
            {
                return true;
            }
        }
        else if (pieceToAttack.type == Piece.PieceType.Rook)
        {
            int numberToMatch = FuzzyLogic.FindNumberRook(pieceToAttack);
            if (rollNumber == numberToMatch)
            {
                return true;
            }
        }
        else
        {
            return false;
            Debug.Log("Piece does not have type on script!");
        }
        return false;
    }
    
    public bool IsEnemyBeside(int i,int j, Piece piece)
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
       if(IsWhiteTurn == !Pieces[positionOfPiece.x, positionOfPiece.y].IsWhite || !IsWhiteTurn == Pieces[positionOfPiece.x, positionOfPiece.y].IsWhite)
        {
            return true;
        }
        return false;
    }
    public bool IsThereEnemy(int i, int j)
    {
        if (IsWhiteTurn == !Pieces[i, j].IsWhite || !IsWhiteTurn == Pieces[i, j].IsWhite)
        {
            return true;
        }
        return false;
    }
    public void KillEnemy(Piece enemyPiece)
    {
        GameObject enemyObject = board.activePieces.Find(m => m.GetComponent<Piece>().Position.x == enemyPiece.Position.x && m.GetComponent<Piece>().Position.y == enemyPiece.Position.y);
        board.activePieces.Remove(enemyObject);
        Pieces[enemyPiece.Position.x, enemyPiece.Position.y] = null;
        Destroy(enemyObject);
    }
    public bool[,] AvailableMoves(Piece piece) {
        bool[,] allowedMoves = new bool[8, 8];
        List<Vector2Int> possibleMoves = piece.LocationsAvailable();

        foreach (Vector2Int position in possibleMoves) {
            allowedMoves[position.x, position.y] = true;
        }
        return allowedMoves;
    }

   
}
