/* Written by Braden Stonehill
 Edited by ___
 Last date edited: 09/06/2021
 Board.cs - Manages the instantiation, rendering, and interactions with the board.
 The Pieces property is a two-dimensional representation of the board to be used with other scripts.

 Version 1: Created methods to spawn all piece models, select game objects based on interaction with the board,
 and highlight tiles on the board.*/

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
    private List<GameObject> activePieces;

    private GameManager gm;

    private void Start() {
        gm = GameManager.instance;
        SpawnAllPieces();
    }

    private void Update() {
        UpdateSelection();
        DrawChessboard();

        if(Input.GetMouseButtonDown(0)) {
            if(selection.x >= 0 && selection.y >= 0) {
                if(gm.SelectedPiece == null) {
                    SelectPiece(selection);
                } else {
                    gm.MovePiece(selection);
                }
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        /// INTERACTION FUNCTIONS ///
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Function to select a piece object for movement
    private void SelectPiece(Vector2Int position) {
        if (gm.Pieces[position.x, position.y] == null)
            return;

        if (gm.Pieces[position.x, position.y].IsWhite != GameManager.instance.IsWhiteTurn)
            return;

        gm.SelectedPiece = gm.Pieces[position.x, position.y];
    }

    public void MoveObject(GameObject pieceObject, Vector2Int position) {
        pieceObject.transform.position = GetTileCenter(position.x, position.y);
    }

    // Function to determine what tile the mouse is hovering over, uses z component of raycast as the board is in the x-z plane
    private void UpdateSelection() {
        if (!Camera.main)
            return;

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 25.0f, LayerMask.GetMask("ChessPlane"))) {
            selection.x = (int)hit.point.x;
            selection.y = (int)hit.point.z;
        } else {
            selection.x = -1;
            selection.y = -1;
        }
    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                        /// INSTANTIATION FUNCTIONS ///
    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Function to spawn a piece prefab on a tile and add the object to the list of active objects
    private void SpawnPiece(int index, Vector2Int position) {
        GameObject pieceObject = Instantiate(piecePrefabs[index], GetTileCenter(position.x, position.y), Quaternion.Euler(-90, 0, 0)) as GameObject;
        pieceObject.transform.SetParent(transform);
        gm.Pieces[position.x, position.y] = pieceObject.GetComponent<Piece>();
        gm.Pieces[position.x, position.y].Position = position;
        activePieces.Add(pieceObject);
    }

    // Initilization function to spawn all chess pieces
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

        for(int i = 0; i < 9; i++) {
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
}
