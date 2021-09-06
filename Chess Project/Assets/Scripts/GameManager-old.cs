
//using System.Collections.Generic;
//using UnityEngine;

//public class GameManager : MonoBehaviour
//{
//    public static GameManager instance;

//    public Board board;
//    public GameObject activeCamera;
//    public GameObject whiteKing;
//    public GameObject whiteQueen;
//    public GameObject whiteBishop;
//    public GameObject whiteKnight;
//    public GameObject whiteRook;
//    public GameObject whitePawn;

//    public GameObject blackKing;
//    public GameObject blackQueen;
//    public GameObject blackBishop;
//    public GameObject blackKnight;
//    public GameObject blackRook;
//    public GameObject blackPawn;

//    private Piece[,] pieces;
//    private List<GameObject> movedPawns;

//    private Player white;
//    private Player black;
//    public Player currentPlayer;
//    public Player otherPlayer;

//    void Awake()
//    {
//        instance = this;
//    }

//    void Start ()
//    {
//        pieces = new Piece[8, 8];
//        movedPawns = new List<GameObject>();

//        white = new Player("Human", true);
//        black = new Player("AI", false);

//        currentPlayer = white;
//        otherPlayer = black;

//        InitialSetup();
//    }

//    private void InitialSetup()
//    {
//        AddPieceToBoard(whiteRook, white, 0, 0);
//        AddPieceToBoard(whiteKnight, white, 1, 0);
//        AddPieceToBoard(whiteBishop, white, 2, 0);
//        AddPieceToBoard(whiteQueen, white, 3, 0);
//        AddPieceToBoard(whiteKing, white, 4, 0);
//        AddPieceToBoard(whiteBishop, white, 5, 0);
//        AddPieceToBoard(whiteKnight, white, 6, 0);
//        AddPieceToBoard(whiteRook, white, 7, 0);

//        for (int i = 0; i < 8; i++)
//        {
//            AddPieceToBoard(whitePawn, white, i, 1);
//        }

//        AddPieceToBoard(blackRook, black, 0, 7);
//        AddPieceToBoard(blackKnight, black, 1, 7);
//        AddPieceToBoard(blackBishop, black, 2, 7);
//        AddPieceToBoard(blackQueen, black, 3, 7);
//        AddPieceToBoard(blackKing, black, 4, 7);
//        AddPieceToBoard(blackBishop, black, 5, 7);
//        AddPieceToBoard(blackKnight, black, 6, 7);
//        AddPieceToBoard(blackRook, black, 7, 7);

//        for (int i = 0; i < 8; i++)
//        {
//            AddPieceToBoard(blackPawn, black, i, 6);
//        }
//    }

//    public void AddPieceToBoard(GameObject prefab, Player player, int col, int row)
//    {
//        GameObject pieceObject = board.AddPieceToGrid(prefab, col, row);
//        player.pieces.Add(pieceObject);
//        pieces[col, row] = pieceObject.GetComponent<Piece>();
//    }

//    public List<Vector2Int> MovesForPiece(GameObject pieceObject)
//    {
//        Piece piece = pieceObject.GetComponent<Piece>();
//        Vector2Int gridPoint = GridForPiece(pieceObject);
//        List<Vector2Int> locations = piece.LocationsAvailable(gridPoint);

//        locations.RemoveAll(gp => gp.x < 0 || gp.x > 7 || gp.y < 0 || gp.y > 7);
//        locations.RemoveAll(gp => FriendlyPieceAt(gp));
//        if(piece is Rook)
//        {
//           locations.RemoveAll(gp=>RemoveLocationsWithoutEnemy(gp, pieceObject));
//        }
//        //Remove the ability for the rook to delete stuff
//        if(piece is Rook)
//        {
//            locations.RemoveAll(gp => RemoveEnemyPieceAt(gp));
//        }
//        return locations;
//    }
//    public bool RemoveLocationsWithoutEnemy(Vector2Int loc,GameObject piece)
//    {
//        if(GameManager.instance.PieceAtGrid(loc))
//        {
//            if(!IsEnemyBeside(loc, piece))
//            {
//               return true;
//            }
//        }
//        return false;
//    }
//    public bool IsEnemyBeside(Vector2Int loc,GameObject piece)
//    {
//        Vector2Int pieceLocation = GameManager.instance.GridForPiece(piece);
//        foreach (Vector2Int dir in piece.GetComponent<Piece>().Directions)
//        {
//            Vector2Int tempPosition = new Vector2Int(pieceLocation.x + dir.x, pieceLocation.y + dir.y);
//            if(tempPosition == loc)
//            {
//                return true;
//            }
//        }
//        return false;
//    }
//    public bool RemoveEnemyPieceAt(Vector2Int gridPoint)
//    {
//        GameObject piece = PieceAtGrid(gridPoint);
//        if (piece == null)
//        {
//            return false;
//        }
//        if (otherPlayer.pieces.Contains(piece))
//        {
//            return true;
//        }
//        return false;
//    }
    
//    public bool FriendlyPieceAt(Vector2Int gridPoint)
//    {
//        GameObject piece = PieceAtGrid(gridPoint);

//        if (piece == null)
//        {
//            return false;
//        }

//        if (otherPlayer.pieces.Contains(piece))
//        {
//            return false;
//        }

//        return true;
//    }
//    public bool HasPawnMoved(GameObject pawn)
//    {
//        return movedPawns.Contains(pawn);
//    }
//    public void PawnMoved(GameObject pawn)
//    {
//        movedPawns.Add(pawn);
//    }
//    public void CapturePieceAt(Vector2Int gridPoint)
//    {
//        GameObject pieceToCapture = PieceAtGrid(gridPoint);
//        if (pieceToCapture.GetComponent<Piece>() is King)
//        {
//            Destroy(board.GetComponent<TileSelector>());
//            Destroy(board.GetComponent<MoveSelector>());
//        }
//        currentPlayer.capturedPieces.Add(pieceToCapture);
//        pieces[gridPoint.x, gridPoint.y] = null;
//        Destroy(pieceToCapture);
//    }

//    public void SelectPiece(GameObject piece)
//    {
//        board.SelectPieceInGrid(piece);
//    }

//    public void DeselectPiece(GameObject piece)
//    {
//        board.DeselectPiece(piece);
//    }

//    public bool DoesPieceBelongToCurrentPlayer(GameObject piece)
//    {
//        return currentPlayer.pieces.Contains(piece);
//    }

//    public GameObject PieceAtGrid(Vector2Int gridPoint)
//    {
//        if (gridPoint.x > 7 || gridPoint.y > 7 || gridPoint.x < 0 || gridPoint.y < 0)
//        {
//            return null;
//        }
//        return pieces[gridPoint.x, gridPoint.y].gameObject;
//    }

//    public Vector2Int GridForPiece(GameObject piece)
//    {
//        for (int i = 0; i < 8; i++) 
//        {
//            for (int j = 0; j < 8; j++)
//            {
//                if (pieces[i, j] == piece)
//                {
//                    return new Vector2Int(i, j);
//                }
//            }
//        }

//        return new Vector2Int(-1, -1);
//    }
//    public void Move(GameObject piece, Vector2Int gridPoint)
//    {
//        Piece pieceComponent = piece.GetComponent<Piece>();
//        pieceComponent.Move(gridPoint, ref pieces, ref board);
//    }
    
    
//    public void NextPlayer()
//    {
//        Player tempPlayer = currentPlayer;
//        currentPlayer = otherPlayer;
//        otherPlayer = tempPlayer;
//    }
//}
