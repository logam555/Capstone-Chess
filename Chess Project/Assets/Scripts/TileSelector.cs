

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public GameObject tileHighlightPrefab;
    private GameObject tileHighlight;
    public GameObject gameManager;
    public Camera activeCamera;

    void Start ()
    {
        Vector2Int gridPoint = Geometry.GridPoint(0, 0);
        Vector3 point = Geometry.PointFromGrid(gridPoint);
        tileHighlight = Instantiate(tileHighlightPrefab, point, Quaternion.identity, gameObject.transform);

        tileHighlight.SetActive(false);
        activeCamera = gameManager.GetComponent<GameManager>().activeCamera.GetComponent<Camera>();
    }

    void FixedUpdate ()
    {
        activeCamera = gameManager.GetComponent<GameManager>().activeCamera.GetComponent<Camera>();
        Ray ray = activeCamera.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (Physics.Raycast(ray, out hit))
        {
                Vector3 point = hit.point;
                if (point.x > -3.4 && point.z > -3.4)
                {
                    Vector2Int gridPoint = Geometry.GridFromPoint(point);
                    Vector3 pointGrid = Geometry.PointFromGrid(gridPoint);
                    tileHighlight.SetActive(true);
                    tileHighlight.transform.position = pointGrid;
                    if (Input.GetMouseButtonDown(0))
                    {
                        GameObject selectedPiece = GameManager.instance.PieceAtGrid(gridPoint);
                        if (GameManager.instance.DoesPieceBelongToCurrentPlayer(selectedPiece))
                        {
                            GameManager.instance.SelectPiece(selectedPiece);
                            ExitState(selectedPiece);
                        }
                    }
                        
                }
            
        }
        else
        {
            tileHighlight.SetActive(false);
        }

    }

    public void EnterState()
    {
        enabled = true;
    }

    private void ExitState(GameObject movingPiece)
    {
        this.enabled = false;
        tileHighlight.SetActive(false);
        MoveSelector move = GetComponent<MoveSelector>();
        move.EnterState(movingPiece);
    }
}
