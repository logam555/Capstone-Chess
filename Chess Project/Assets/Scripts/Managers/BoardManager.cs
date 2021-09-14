/* Written by Braden Stonehill
 Edited by ___
 Last date edited: 09/07/2021
 BoardManager.cs - Manages the instantiation, rendering, and interactions with the board.

 Version 1: Created methods to spawn all piece models, select game objects based on interaction with the board,
 and highlight tiles on the board.

 Version 1.1: Edited by George 09/09/2021: Adding Tags to chess pieces, adding base heuirtics, 
 adding in mouse over board hovering highlighting, add in board grid naming, ... .
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private Vector2Int selection = new Vector2Int(-1, -1);

    [SerializeField]
    private List<GameObject> piecePrefabs; // Not instantiated, list is filled in the editor
    [SerializeField]
    private List<GameObject> highlightPrefabs; // Not instantiated, list is filled in the editor

    private List<GameObject> activePieces;
    private List<GameObject> highlights;

    private Vector2Int positionCurrent = new Vector2Int(0, 0); //using for mouse over highlight

    private GameManager gm;

    private void Start() {
        gm = GameManager.Instance;
        highlights = new List<GameObject>();
        SpawnAllPieces();

        //setup static board naming
        ChessboardNaming();
    }

    private void Update() {
        UpdateSelection();
        DrawChessboard();

        if(Input.GetMouseButtonDown(0)) {
            if(selection.x >= 0 && selection.y >= 0) {
                if(gm.SelectedPiece == null) {
                    gm.SelectPiece(selection);
                } else {
                    gm.CheckMove(selection);
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        /// INTERACTION FUNCTIONS ///
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void MoveObject(GameObject pieceObject, Vector2Int position) {
        pieceObject.transform.position = GetTileCenter(position.x, position.y);
    }

    public void RemoveObject(GameObject pieceObject) {
        Destroy(pieceObject);
    }

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
        
        //mouse movement highlighting
        positionCurrent.x = (int)hit.point.x;
        positionCurrent.y = (int)hit.point.z;
        //Debug.Log("x"+ positionCurrent + "y");
 
        /*
        //testing mouse over using positionCurrent //redo positionCurrent 
        if ((positionCurrent.x != 0 && positionCurrent.y != 0))
            HighlightSelectedMouse(); //HighlightSelectedMouse(positionCurrent);
        else
            //highlights.Clear();
            Debug.Log("debug"+ positionCurrent);
        */
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        /// INSTANTIATION FUNCTIONS ///
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Function to spawn a piece prefab on a tile and add the object to the list of active objects with correct color tag.
    private void SpawnPiece(int index, Vector2Int position) {
        GameObject pieceObject = Instantiate(piecePrefabs[index], GetTileCenter(position.x, position.y), Quaternion.Euler(-90, 0, 0)) as GameObject;
        pieceObject.transform.SetParent(transform);
        gm.Pieces[position.x, position.y] = pieceObject.GetComponent<Piece>();
        gm.Pieces[position.x, position.y].Position = position;

        //adding tags for White and Black pieces. 0-5 index for white, 6-11 index for black.
        if (index <= 5)
            pieceObject.tag = "White Pieces";
        else
            pieceObject.tag = "Black Pieces";

        activePieces.Add(pieceObject);
    }

    // Initilization function to spawn all chess pieces: White index numbers 0-5, Black index numbers 6-11.
    private void SpawnAllPieces() {
        activePieces = new List<GameObject>();

        // Spawn White Pieces
        // King
        SpawnPiece(0, new Vector2Int(4, 0));

        // Queen
        SpawnPiece(1, new Vector2Int(3, 0));

        // Bishops
        SpawnPiece(2, new Vector2Int(2, 0));
        SpawnPiece(2, new Vector2Int(5, 0));

        // Knights
        SpawnPiece(3, new Vector2Int(1, 0));
        SpawnPiece(3, new Vector2Int(6, 0));

        // Rooks
        SpawnPiece(4, new Vector2Int(0, 0));
        SpawnPiece(4, new Vector2Int(7, 0));

        // Pawns
        for(int i = 0; i < 8; i++)
            SpawnPiece(5, new Vector2Int(i, 1));

        // Spawn Black Pieces
        // King
        SpawnPiece(6, new Vector2Int(4, 7));

        // Queen
        SpawnPiece(7, new Vector2Int(3, 7));

        // Bishops
        SpawnPiece(8, new Vector2Int(2, 7));
        SpawnPiece(8, new Vector2Int(5, 7));

        // Knights
        SpawnPiece(9, new Vector2Int(1, 7));
        SpawnPiece(9, new Vector2Int(6, 7));

        // Rooks
        SpawnPiece(10, new Vector2Int(0, 7));
        SpawnPiece(10, new Vector2Int(7, 7));

        // Pawns
        for (int i = 0; i < 8; i++)
            SpawnPiece(11, new Vector2Int(i, 6));
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        /// RENDERING FUNCTIONS ///
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

        if(selection.x >= 0 && selection.y >= 0) {
            Debug.DrawLine(
                Vector3.forward * selection.y + Vector3.right * selection.x,
                Vector3.forward * (selection.y + 1) + Vector3.right * (selection.x + 1));
            Debug.DrawLine(
                Vector3.forward * (selection.y + 1) + Vector3.right * selection.x,
                Vector3.forward * selection.y + Vector3.right * (selection.x + 1));
        }
    }

    // Function to highlight all tiles associated with selected piece
    public void HighlightAllTiles(Vector2Int position, bool[,] availableMoves, List<Vector2Int> enemies) {
        HighlightSelected(position);
        HighlightAvailableMoves(availableMoves);
        HighlightEnemies(enemies);
    }

    // Function to highlight the tile of the selected piece
    private void HighlightSelected(Vector2Int position) {
        HighlightTile(0, position.x, position.y);
    }

    // Function to highlight all available moves with available highlight prefab using a boolean map of the board
    private void HighlightAvailableMoves(bool[,] moves) {
        for(int i = 0; i < 8; i++) {
            for(int j = 0; j < 8; j++) {
                if(moves[i,j]) {
                    HighlightTile(1, i, j);
                }
            }
        }
    }

    // Function to highlight all enemies within range for attack
    private void HighlightEnemies(List<Vector2Int> positions) {
        foreach(Vector2Int pos in positions) {
            HighlightTile(2, pos.x, pos.y);
        }
    }

    // Utility function to highlight any tile with the selected prefab at the x and y grid position
    private void HighlightTile(int index, int x, int y) {
        GameObject highlight = Instantiate(highlightPrefabs[index]);
        highlights.Add(highlight);
        highlight.transform.position = GetTileCenter(x, y) + Vector3.up * (index != 2 ? -0.149f : -0.14f);
    }

    //function to spawn and remove mouse following highlighted tile.
    private void HighlightTileMouse(int index, int x, int y)
    {
        GameObject highlight = Instantiate(highlightPrefabs[index]);
        highlights.Add(highlight);
        highlight.transform.position = GetTileCenter(x, y) + Vector3.up * (index != 2 ? -0.149f : -0.14f);

        Destroy(highlight);
    }

    // Utility function to destroy all highlight game objects
    public void RemoveHighlights() {
        foreach(GameObject highlight in highlights) {
            Destroy(highlight);
        }
        highlights.Clear();
    }

    //testing
    private void HighlightSelectedMouse()
    {

    }

    //testing
    private void HighlightSelectedMouse(Vector2Int position)
    {

    }

    // Utility Debug function for testing raycasting and selection
    private void ChessboardNaming()
    {
        Vector3 widthLine = Vector3.right * 8;
        Vector3 heightLine = Vector3.forward * 8;
        Debug.Log(widthLine + "first" + heightLine);
        int test1 = 0;
        char letterBoard = '0';
        string showB = "";

        for (int i = 65; i < 73; i++)
        {
            Vector3 start = Vector3.forward * i;
            letterBoard = Convert.ToChar(i);
            showB = letterBoard.ToString();
            Debug.Log("letter" + showB);
            Debug.Log(start + "2nd" + widthLine + "i" + i);
            for (int j = 0; j < 9; j++)
            {
                start = Vector3.right * j;
                Debug.DrawLine(start, start + heightLine);
                Debug.Log(start + "3rd" + heightLine + "j" + j);
                Debug.Log("running count" + test1);
                test1++;
            }
        }

        /*
        if (selection.x >= 0 && selection.y >= 0)
        {
            Debug.DrawLine(
                Vector3.forward * selection.y + Vector3.right * selection.x,
                Vector3.forward * (selection.y + 1) + Vector3.right * (selection.x + 1));
            Debug.DrawLine(
                Vector3.forward * (selection.y + 1) + Vector3.right * selection.x,
                Vector3.forward * selection.y + Vector3.right * (selection.x + 1));
        }
        */
    }
}
