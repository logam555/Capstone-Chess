using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    #region PROPERTIES
    public static ModelManager Instance { get; set; }

    public Vector2Int selection;

    //Dictionary for holding a key for searching Tile; value is custom tile class BoardTile
    public Dictionary<string, BoardTile> chessBoardGridCo;

    [SerializeField]
    private List<GameObject> piecePrefabs; // Not instantiated, list is populated in the editor
    [SerializeField]
    private List<GameObject> highlightPrefabs; // Not instantiated, list is populated in the editor
    [SerializeField]
    private Material commanderHighlightPrefab; // Not instantiated, populated in the editor
    [SerializeField]
    private Heuristics heuristics;

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private List<GameObject> activePieces;
    private List<GameObject> highlights;
    private MeshRenderer commanderModel;
    private Material commanderMaterial;
    private Dictionary<ChessPiece, GameObject> pieceLinks;
    #endregion

    private void Awake() {
        Instance = this;
        selection = new Vector2Int(-1, -1);
        activePieces = new List<GameObject>();
        highlights = new List<GameObject>();
        commanderModel = null;
        commanderMaterial = null;
        pieceLinks = new Dictionary<ChessPiece, GameObject>();

        //setup static board tile naming and default variables
        chessBoardGridCo = new Dictionary<string, BoardTile>();
        heuristics = GetComponent<Heuristics>();

        ChessboardTileSetup();
        SpawnAllPieces();
    }

    private void Start() {
        
    }

    private void Update() {
        UpdateSelection();
        //DrawChessboard();
    }

    #region INSTANTIATION
    //function to setup the tile position names, tile is occupied status, if occupied by which color/piece type and basic board wide heuirtics for each color.
    private void ChessboardTileSetup() {
        BoardTile board = new BoardTile();
        char letterBoard = '0';
        string showB = "";

        //setting up tiles and adding to dictionary default values
        for (int j = 1; j < 9; j++) {
            for (int i = 65; i < 73; i++) {
                letterBoard = '0';
                showB = "";

                letterBoard = Convert.ToChar(i);
                showB = letterBoard.ToString() + j.ToString(); //place letter before number
                board = new BoardTile();

                chessBoardGridCo.Add(showB, board);
                chessBoardGridCo[showB].isOccupied = false;
                chessBoardGridCo[showB].isWhite = false;
                chessBoardGridCo[showB].boardPosition.x = i - 65;
                chessBoardGridCo[showB].boardPosition.y = j - 1;
                chessBoardGridCo[showB].officalBoardPosition = showB;
                chessBoardGridCo[showB].occupiedPieceType = "";
                chessBoardGridCo[showB].whiteHeuristic = 0;
                chessBoardGridCo[showB].blackHeuristic = 0;
            }
        }

    }

    // Function to spawn a piece prefab on a tile and add the object to the list of active objects with correct color tag.
    // Edited By George; added string to function call; added in board tile variables.
    private void SpawnPiece(int index, Vector2Int position, string piece) {
        GameObject pieceObject = Instantiate(piecePrefabs[index], GetTileCenter(position.x, position.y), Quaternion.Euler(-90, 0, 0));
        pieceObject.transform.SetParent(transform);

        //adding tags for White and Black pieces. 0-5 index for white, 6-11 index for black. added by george to existing function
        if (index <= 5) {
            pieceObject.tag = "White Pieces";
            chessBoardGridCo[Convert.ToString(Convert.ToChar(position.x + 65) + Convert.ToString(position.y + 1))].isWhite = true;
        } else {
            pieceObject.tag = "Black Pieces";
            chessBoardGridCo[Convert.ToString(Convert.ToChar(position.x + 65) + Convert.ToString(position.y + 1))].isWhite = false;
        }

        chessBoardGridCo[Convert.ToString(Convert.ToChar(position.x + 65) + Convert.ToString(position.y + 1))].isOccupied = true;
        chessBoardGridCo[Convert.ToString(Convert.ToChar(position.x + 65) + Convert.ToString(position.y + 1))].occupiedPieceType = piece;

        activePieces.Add(pieceObject);
    }

    // Initilization function to spawn all chess pieces
    //Edited By George; added string of piece type to function calls.
    private void SpawnAllPieces() {
        // Spawn White Pieces
        // King
        SpawnPiece(0, new Vector2Int(4, 0), "King");

        // Queen
        SpawnPiece(1, new Vector2Int(3, 0), "Queen");

        // Bishops
        SpawnPiece(2, new Vector2Int(2, 0), "Bishop");
        SpawnPiece(2, new Vector2Int(5, 0), "Bishop");

        // Knights
        SpawnPiece(3, new Vector2Int(1, 0), "Knight");
        SpawnPiece(3, new Vector2Int(6, 0), "Knight");

        // Rooks
        SpawnPiece(4, new Vector2Int(0, 0), "Rook");
        SpawnPiece(4, new Vector2Int(7, 0), "Rook");

        // Pawns
        for (int i = 0; i < 8; i++)
            SpawnPiece(5, new Vector2Int(i, 1), "Pawn");

        // Spawn Black Pieces
        // King
        SpawnPiece(6, new Vector2Int(4, 7), "King");

        // Queen
        SpawnPiece(7, new Vector2Int(3, 7), "Queen");

        // Bishops
        SpawnPiece(8, new Vector2Int(2, 7), "Bishop");
        SpawnPiece(8, new Vector2Int(5, 7), "Bishop");

        // Knights
        SpawnPiece(9, new Vector2Int(1, 7), "Knight");
        SpawnPiece(9, new Vector2Int(6, 7), "Knight");

        // Rooks
        SpawnPiece(10, new Vector2Int(0, 7), "Rook");
        SpawnPiece(10, new Vector2Int(7, 7), "Rook");

        // Pawns
        for (int i = 0; i < 8; i++)
            SpawnPiece(11, new Vector2Int(i, 6), "Pawn");
    }

    // Function to link ChessPiece objects with the GameObject models
    public void LinkModels(ChessPiece[,] board) {
        // Link White Pieces
        // King
        pieceLinks.Add(board[4, 0], activePieces[0]);

        // Queen
        pieceLinks.Add(board[3, 0], activePieces[1]);

        // Bishops
        pieceLinks.Add(board[2, 0], activePieces[2]);
        pieceLinks.Add(board[5, 0], activePieces[3]);

        // Knights
        pieceLinks.Add(board[1, 0], activePieces[4]);
        pieceLinks.Add(board[6, 0], activePieces[5]);

        // Rooks
        pieceLinks.Add(board[0, 0], activePieces[6]);
        pieceLinks.Add(board[7, 0], activePieces[7]);

        // Pawns
        for (int i = 0; i < 8; i++)
            pieceLinks.Add(board[i, 1], activePieces[i + 8]);

        // Link Black Pieces
        // King
        pieceLinks.Add(board[4, 7], activePieces[16]);

        // Queen
        pieceLinks.Add(board[3, 7], activePieces[17]);

        // Bishops
        pieceLinks.Add(board[2, 7], activePieces[18]);
        pieceLinks.Add(board[5, 7], activePieces[19]);

        // Knights
        pieceLinks.Add(board[1, 7], activePieces[20]);
        pieceLinks.Add(board[6, 7], activePieces[21]);

        // Rooks
        pieceLinks.Add(board[0, 7], activePieces[22]);
        pieceLinks.Add(board[7, 7], activePieces[23]);

        // Pawns
        for (int i = 0; i < 8; i++)
            pieceLinks.Add(board[i, 6], activePieces[i + 24]);
    }
    #endregion

    #region INTERACTION
    // Function to determine what tile the mouse is hovering over, uses z component of raycast as the board is in the x-z plane
    private void UpdateSelection() {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane"))) {
            selection.x = (int)hit.point.x;
            selection.y = (int)hit.point.z;
        } else {
            selection.x = -1;
            selection.y = -1;
        }
    }

    public void MoveObject(ChessPiece piece, Vector2Int position) {
        pieceLinks[piece].transform.position = GetTileCenter(position.x, position.y);
    }

    public void RemoveObject(ChessPiece piece) {
        GameObject go = pieceLinks[piece];
        pieceLinks.Remove(piece);
        activePieces.Remove(go);
        Destroy(go);
    }
    #endregion

    #region RENDERING
    // Function to highlight all tiles associated with selected piece
    public void HighlightAllTiles(Vector2Int position, List<Vector2Int> availableMoves, List<Vector2Int> enemies, Commander commander) {
        HighlightSelected(position);
        HighlightAvailableMoves(availableMoves);
        HighlightEnemies(enemies);
        HighlightCommander(commander);
    }

    // Function to highlight the tile of the selected piece
    private void HighlightSelected(Vector2Int position) {
        HighlightTile(0, position.x, position.y);
    }

    // Function to highlight all available moves with available highlight prefab using a boolean map of the board
    private void HighlightAvailableMoves(List<Vector2Int> moves) {
        foreach (Vector2Int pos in moves) {
            HighlightTile(1, pos.x, pos.y);
        }
    }

    // Function to highlight all enemies within range for attack
    private void HighlightEnemies(List<Vector2Int> positions) {
        foreach (Vector2Int pos in positions) {
            HighlightTile(2, pos.x, pos.y);
        }
    }

    // Function to highlight commander
    private void HighlightCommander(Commander commander) {
        commanderModel = pieceLinks[(ChessPiece)commander].GetComponent<MeshRenderer>();
        commanderMaterial = commanderModel.material;
        commanderModel.material = commanderHighlightPrefab;
    }

    // Utility function to highlight any tile with the selected prefab at the x and y grid position
    private void HighlightTile(int index, int x, int y) {
        GameObject highlight = Instantiate(highlightPrefabs[index]);
        highlights.Add(highlight);
        highlight.transform.position = GetTileCenter(x, y) + Vector3.up * (index != 2 ? 0f : 0f);
    }

    // Utility function to destroy all highlight game objects
    public void RemoveHighlights() {
        foreach (GameObject highlight in highlights) {
            Destroy(highlight);
        }
        highlights.Clear();

        if (commanderMaterial != null && commanderModel != null) {
            commanderModel.material = commanderMaterial;
            commanderModel = null;
            commanderMaterial = null;
        }
    }
    #endregion

    #region UTILITY
    // Utility function to get the center position of a tile of the board game object
    private Vector3 GetTileCenter(int x, int y) {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;
        return origin;
    }

    // Utility Debug function for testing raycasting and selection
    private void DrawChessboard() {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;

        for (int i = 0; i < 9; i++) {
            Vector3 start = Vector3.forward * i;
            Debug.DrawLine(start, start + widthLine);
            for (int j = 0; j < 9; j++) {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
            }
        }

        if (selection.x >= 0 && selection.y >= 0) {
            Debug.DrawLine(
                Vector3.forward * selection.y + Vector3.right * selection.x,
                Vector3.forward * (selection.y + 1) + Vector3.right * (selection.x + 1));
            Debug.DrawLine(
                Vector3.forward * (selection.y + 1) + Vector3.right * selection.x,
                Vector3.forward * selection.y + Vector3.right * (selection.x + 1));
        }
    }
    #endregion

    #region HEURISTICS
    //take new positions and move piece and values to new tile; clear to null old tile.
    public void BoardTileLocationUpdate(Vector2Int oldPosition, Vector2Int newPosition, bool isWhiteP, string pieceTypeC) {
        chessBoardGridCo[Convert.ToString(Convert.ToChar(oldPosition.x + 65) + Convert.ToString(oldPosition.y + 1))].isOccupied = false;
        chessBoardGridCo[Convert.ToString(Convert.ToChar(oldPosition.x + 65) + Convert.ToString(oldPosition.y + 1))].isWhite = false;
        chessBoardGridCo[Convert.ToString(Convert.ToChar(oldPosition.x + 65) + Convert.ToString(oldPosition.y + 1))].occupiedPieceType = "";

        chessBoardGridCo[Convert.ToString(Convert.ToChar(newPosition.x + 65) + Convert.ToString(newPosition.y + 1))].isOccupied = true;
        chessBoardGridCo[Convert.ToString(Convert.ToChar(newPosition.x + 65) + Convert.ToString(newPosition.y + 1))].isWhite = isWhiteP;
        chessBoardGridCo[Convert.ToString(Convert.ToChar(newPosition.x + 65) + Convert.ToString(newPosition.y + 1))].occupiedPieceType = pieceTypeC;
    }

    public Vector3Int GetHighestValueFromBoard() {
        //entire heur board wide check

        Vector3Int posValue = new Vector3Int();

        posValue = heuristics.ReturnHighestValueBlack(chessBoardGridCo);
        //posValue = heuristics.ReturnHighestValueWhite(chessBoardGridCo);

        return posValue;
    }

    //pieces move range check
    public Vector3Int GetHighestValueFromTileMoveRange(ChessPiece p) {
        Vector2Int pLocV2 = new Vector2Int();

        //pLocV2 = p.Postion;

        Vector3Int posValue = new Vector3Int();

        //posValue = heuristics.ReturnHighestValueOnePieceRange(chessBoardGridCo, pLocV2);
        //posValue = heuristics.ReturnHighestValueWhite(chessBoardGridCo);

        return posValue;
    }

    public void BoardWideHeuristicCall() {
        heuristics.BoardWideHeuristic(ref chessBoardGridCo);
    }

    public void BoardWideHeuristicTileCall(int x, int y) {
        Vector2Int holderV2I = new Vector2Int();
        holderV2I.x = 0;
        holderV2I.y = 0;

        //used to adjust x,y values to Letter/Number combo for dictionary
        y = +1;
        x = +65;

        //y first to work with tile format, x is second for int after chessboard
        holderV2I = heuristics.BoardWideHeuristicTile(chessBoardGridCo, y, x);

        chessBoardGridCo[Convert.ToString(Convert.ToChar(x) + Convert.ToString(y))].whiteHeuristic = holderV2I.x;
        chessBoardGridCo[Convert.ToString(Convert.ToChar(x) + Convert.ToString(y))].blackHeuristic = holderV2I.y;
    }
    #endregion
}

public class BoardTile {
    public bool isOccupied;
    public bool isWhite;
    public Vector2Int boardPosition;//for vector 2 version of string position
    public string officalBoardPosition;
    public string occupiedPieceType;
    public int whiteHeuristic;
    public int blackHeuristic;

}
