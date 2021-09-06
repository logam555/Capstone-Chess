
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Board board;
    public GameObject activeCamera;
    public GameObject whiteKing;
    public GameObject whiteQueen;
    public GameObject whiteBishop;
    public GameObject whiteKnight;
    public GameObject whiteRook;
    public GameObject whitePawn;

    public GameObject blackKing;
    public GameObject blackQueen;
    public GameObject blackBishop;
    public GameObject blackKnight;
    public GameObject blackRook;
    public GameObject blackPawn;

    private GameObject[,] pieces;
    private List<GameObject> movedPawns;

    private Player white;
    private Player black;
    public Player currentPlayer;
    public Player otherPlayer;

    void Awake()
    {
        instance = this;
    }

    void Start ()
    {
        pieces = new GameObject[8, 8];
        movedPawns = new List<GameObject>();

        white = new Player("Human", true);
        black = new Player("AI", false);

        currentPlayer = white;
        otherPlayer = black;

        InitialSetup();
    }

    private void InitialSetup()
    {
        AddPieceToBoard(whiteRook, white, 0, 0);
        AddPieceToBoard(whiteKnight, white, 1, 0);
        AddPieceToBoard(whiteBishop, white, 2, 0);
        AddPieceToBoard(whiteQueen, white, 3, 0);
        AddPieceToBoard(whiteKing, white, 4, 0);
        AddPieceToBoard(whiteBishop, white, 5, 0);
        AddPieceToBoard(whiteKnight, white, 6, 0);
        AddPieceToBoard(whiteRook, white, 7, 0);

        for (int i = 0; i < 8; i++)
        {
            AddPieceToBoard(whitePawn, white, i, 1);
        }

        AddPieceToBoard(blackRook, black, 0, 7);
        AddPieceToBoard(blackKnight, black, 1, 7);
        AddPieceToBoard(blackBishop, black, 2, 7);
        AddPieceToBoard(blackQueen, black, 3, 7);
        AddPieceToBoard(blackKing, black, 4, 7);
        AddPieceToBoard(blackBishop, black, 5, 7);
        AddPieceToBoard(blackKnight, black, 6, 7);
        AddPieceToBoard(blackRook, black, 7, 7);

        for (int i = 0; i < 8; i++)
        {
            AddPieceToBoard(blackPawn, black, i, 6);
        }
    }

    public void AddPieceToBoard(GameObject prefab, Player player, int col, int row)
    {
        GameObject pieceObject = board.AddPieceToGrid(prefab, col, row);
        player.pieces.Add(pieceObject);
        pieces[col, row] = pieceObject;
    }

    public List<Vector2Int> MovesForPiece(GameObject pieceObject)
    {
        Piece piece = pieceObject.GetComponent<Piece>();
        Vector2Int gridPoint = GridForPiece(pieceObject);
        List<Vector2Int> locations = piece.LocationsAvailable(gridPoint);

        locations.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);
        locations.RemoveAll(gp => FriendlyPieceAt(gp));

        //Remove the ability for the rook to delete stuff
        if(pieceObject.GetComponent<Piece>().type == PieceType.Rook)
        {
            locations.RemoveAll(gp => RemoveEnemyPieceAt(gp));
        }
        return locations;
    }

    
    public bool RemoveEnemyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);
        if (piece == null)
        {
            return false;
        }
        if (otherPlayer.pieces.Contains(piece))
        {
            return true;
        }
        return false;
    }
    
    public bool FriendlyPieceAt(Vector2Int gridPoint)
    {
        GameObject piece = PieceAtGrid(gridPoint);

        if (piece == null)
        {
            return false;
        }

        if (otherPlayer.pieces.Contains(piece))
        {
            return false;
        }

        return true;
    }
    public bool HasPawnMoved(GameObject pawn)
    {
        return movedPawns.Contains(pawn);
    }
    public void PawnMoved(GameObject pawn)
    {
        movedPawns.Add(pawn);
    }
    public void CapturePieceAt(Vector2Int gridPoint)
    {
        GameObject pieceToCapture = PieceAtGrid(gridPoint);
        if (pieceToCapture.GetComponent<Piece>().type == PieceType.King)
        {
            Destroy(board.GetComponent<TileSelector>());
            Destroy(board.GetComponent<MoveSelector>());
        }
        currentPlayer.capturedPieces.Add(pieceToCapture);
        pieces[gridPoint.x, gridPoint.y] = null;
        Destroy(pieceToCapture);
    }

    public void SelectPiece(GameObject piece)
    {
        board.SelectPieceInGrid(piece);
    }

    public void DeselectPiece(GameObject piece)
    {
        board.DeselectPiece(piece);
    }

    public bool DoesPieceBelongToCurrentPlayer(GameObject piece)
    {
        return currentPlayer.pieces.Contains(piece);
    }

    public GameObject PieceAtGrid(Vector2Int gridPoint)
    {
        if (gridPoint.x > 7 || gridPoint.y > 7 || gridPoint.x < 0 || gridPoint.y < 0)
        {
            return null;
        }
        return pieces[gridPoint.x, gridPoint.y];
    }

    public Vector2Int GridForPiece(GameObject piece)
    {
        for (int i = 0; i < 8; i++) 
        {
            for (int j = 0; j < 8; j++)
            {
                if (pieces[i, j] == piece)
                {
                    return new Vector2Int(i, j);
                }
            }
        }

        return new Vector2Int(-1, -1);
    }
    public void Move(GameObject piece, Vector2Int gridPoint)
    {
        Piece pieceComponent = piece.GetComponent<Piece>();
        if (pieceComponent.type == PieceType.Pawn && !HasPawnMoved(piece))
        {
            movedPawns.Add(piece);
        }

        Vector2Int startGridPoint = GridForPiece(piece);
        pieces[startGridPoint.x, startGridPoint.y] = null;
        pieces[gridPoint.x, gridPoint.y] = piece;
        board.MovePieceInGrid(piece, gridPoint);
    }
    
    
    public void NextPlayer()
    {
        Player tempPlayer = currentPlayer;
        currentPlayer = otherPlayer;
        otherPlayer = tempPlayer;
    }
}
