using UnityEngine;

public class Board : MonoBehaviour
{
    public Material defaultMaterialWhite;
    public Material defaultMaterialBlack;
    public Material selectedMaterial;

    public GameObject AddPieceToGrid(GameObject piece, int col, int row)
    {
        Vector2Int gridPoint = Geometry.GridPoint(col, row);
        GameObject newPiece = Instantiate(piece, Geometry.PointFromGrid(gridPoint,piece), Quaternion.identity, gameObject.transform);
        newPiece.transform.Rotate(-90.0f, 0.0f, 0.0f, Space.World);
        return newPiece;
    }

    public void MovePieceInGrid(GameObject piece, Vector2Int gridPoint)
    {
        piece.transform.position = Geometry.PointFromGrid(gridPoint);
    }

    public void SelectPieceInGrid(GameObject piece)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
        renderers.material = selectedMaterial;
    }

    public void DeselectPiece(GameObject piece)
    {
        MeshRenderer renderers = piece.GetComponentInChildren<MeshRenderer>();
        if (piece.GetComponent<Piece>().isWhite)
        {
            renderers.material = defaultMaterialWhite;
        }
        else
        {
            renderers.material = defaultMaterialBlack;
        }
        
    }
}
