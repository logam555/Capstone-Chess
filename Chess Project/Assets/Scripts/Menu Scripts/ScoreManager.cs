/* Written by Tommy Oh
 Edited by ___
 Last date edited: 11/11/2021
 VolumeMixer.cs - Manages the score and turn order.
 Version 1: The text changes accordingly to the score and turn.
 Version 1.5: made change turn and add score to function
 Version 2.0: The pieces spawn when they are captured
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public enum pieceType
    {
        sliverpawn,
        sliverbishop,
        sliverrook,
        sliverking,
        sliverqueen,
        sliverknight,

        goldpawn,
        goldbishop,
        goldrook,
        goldking,
        goldqueen,
        goldknight


    };
    public Text scoreText1;
    public Text scoreText2;
    public Text turnOrder;
    public static int scoreValue1;
    public static int scoreValue2;
    public static string turn;
    public GameManager gm;
    public GameObject winUI;

    public GameObject pawnGold;
    public GameObject pawnSliver;
    public GameObject rookGold;
    public GameObject rookSliver;
    public GameObject queenGold;
    public GameObject queenSliver;
    public GameObject kingGold;
    public GameObject kingSliver;
    public GameObject knightGold;
    public GameObject knightSliver;
    public GameObject bishopGold;
    public GameObject bishopSliver;
    public Transform uiCanvas;
    public Transform p1Container;
     public Transform p2Container;
    public RectTransform p1RectContainer;
    public RectTransform p2RectContainer;
    private Vector3 offSet1;
    private Vector3 offSet2;
    public float offSetAmount1 = 30.0f;
    public float offSetAmount2 = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        scoreText1.text = "P1 Score: " + 0;
        scoreText2.text = "P2 Score: " + 0;
        turnOrder.text = "Turn: " + "P1";
        p1RectContainer = p1Container.GetComponent<RectTransform>();
        p2RectContainer = p2Container.GetComponent<RectTransform>();
        //offSet1.x = p1RectContainer.rect.xMin;
        //offSet1.y = (p1RectContainer.rect.yMin);
        offSet1 = new Vector3(p1RectContainer.rect.xMin,p1RectContainer.rect.yMax) + p1RectContainer.transform.position;
        //offSet1 = p1Container.transform.position;
        offSet2 = new Vector3(p2RectContainer.rect.xMin ,p2RectContainer.rect.yMax) + p2RectContainer.transform.position;
        
    }
    void Update()
    {

        if(gm.IsGameOver)
        {
            winUI.SetActive(true);
        }
        //test the spawning
        if (Input.GetKeyDown(KeyCode.P)) 
        {
                   AddImgPieces(pieceType.sliverpawn);
        }
        if (Input.GetKeyDown(KeyCode.U)) 
        {
                   AddImgPieces(pieceType.goldpawn);
        }
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
    public void ChangeTurn(string x)
    {
        turn = x;
        turnOrder.text = "Turn: " + turn;
    }
    public void AddScore(int  s, int s2)
    {   
        scoreValue1 += s;
        scoreValue2 += s2;
        scoreText1.text = "P1 Score: " + scoreValue1;
        scoreText2.text = "P2 Score: " + scoreValue2;
    }
    //spawn the pieces when captured
    public void AddImgPieces(pieceType piece)
    {
        switch(piece)
        {
            case pieceType.sliverpawn:
                GameObject.Instantiate(pawnSliver, offSet1, Quaternion.identity, p1Container);
                offSet1.x += offSetAmount1;
                AddScore(0,1);
                break;
            case pieceType.goldpawn:
                GameObject.Instantiate(pawnGold, offSet2,  Quaternion.identity, p2Container);
                 offSet2.x += offSetAmount2;
                 AddScore(1,0);
                break;
            case pieceType.sliverrook:
                GameObject.Instantiate(rookSliver, offSet1,  Quaternion.identity, p1Container);
                offSet1.x += offSetAmount1;
                AddScore(0,5);
                break;
            case pieceType.goldrook:
                GameObject.Instantiate(rookGold, offSet2,  Quaternion.identity, p2Container);
                offSet2.x += offSetAmount2;
                AddScore(5,0);
                break;
            case pieceType.sliverbishop:
                GameObject.Instantiate(bishopSliver, offSet1,  Quaternion.identity, p1Container);
                offSet1.x += offSetAmount1;
                AddScore(0,3);
                break;
            case pieceType.goldbishop:
                GameObject.Instantiate(bishopGold, offSet2,  Quaternion.identity, p2Container);
                offSet2.x += offSetAmount2;
                AddScore(3,0);
                break;
            case pieceType.sliverqueen:
                GameObject.Instantiate(queenSliver, offSet1,  Quaternion.identity, p1Container);
                offSet1.x += offSetAmount1;
                AddScore(0,9);
                break;
            case pieceType.goldqueen:
                GameObject.Instantiate(queenGold, offSet2,  Quaternion.identity, p2Container);
                offSet2.x += offSetAmount2;
                AddScore(9,0);
                break;
            case pieceType.sliverking:
                GameObject.Instantiate(kingSliver, offSet1,  Quaternion.identity, p1Container);
                offSet1.x += offSetAmount1;
                break;
            case pieceType.goldking:
                GameObject.Instantiate(kingGold, offSet2,  Quaternion.identity, p2Container);
                offSet2.x += offSetAmount2;
                break;
            case pieceType.sliverknight:
                GameObject.Instantiate(knightSliver, offSet1,  Quaternion.identity, p1Container);
                offSet1.x += offSetAmount1;
                AddScore(0,3);
                break;
            case pieceType.goldknight:
                GameObject.Instantiate(knightGold, offSet2,  Quaternion.identity, p2Container);
                offSet2.x += offSetAmount2;
                AddScore(3,0);
                break;
        }  
}


}