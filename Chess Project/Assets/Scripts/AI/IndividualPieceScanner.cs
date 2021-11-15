using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ScanData
{
    public int FriendFoe;
    public bool IsVulnerable;
    public float CaptureChance;
    public float ChanceToCapture;
    public float Heuristic;

    public int friendFoe
    {
        get { return FriendFoe; }
        set { FriendFoe = value; }
    }


    public bool isVulnerable
    {
        get { return IsVulnerable; }
        set { IsVulnerable = value; }
    }

    public float captureChance
    {
        get { return CaptureChance; }
        set { CaptureChance = value; }
    }

    public float chanceToCapture
    {
        get { return ChanceToCapture; }
        set { ChanceToCapture = value; }
    }

    public float heuristic
    {
        get { return Heuristic; }
        set { Heuristic = value; }
    }
}

public class IndividualPieceScanner : MonoBehaviour
{
    private ChessPiece[,] board;
    public static IndividualPieceScanner Instance { get; set; }
    //public ChessBoard chessBoard;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public ScanData[,] singleScanner(bool isWhite, Vector2Int position)
    {

        //replace Piece with ChessPiece  
        //ChessBoard.Instance can replace bm 
        //PieceAt Takes addittionally a board from AI 
        //Chessboard.Instance.FilterAttackRange(piece, board) 
        //Chessboard.Instance.FilterMoverange(piece, board)  
        //ChessPiece[,] board = new ChessPiece[8,8];

        board = (ChessPiece[,])ChessBoard.Instance.Board.Clone();
        //board = ChessBoard.Instance.Board.Clone();
        ChessPiece ScanPiece = ChessBoard.Instance.PieceAt(position, board);
        //openPositionMap = bm.availableMoves(ScanPiece); 

        List<Vector2Int> enemies = ChessBoard.Instance.FilterAttackRange(ScanPiece, board);
        List<Vector2Int> openTiles = ChessBoard.Instance.FilterMoveRange(ScanPiece, board);
        ScanData[,] IndividualScanMap = new ScanData[8, 8];


        for (int i = 0; i < enemies.Count; i++)
        {
            ChessPiece EnemyPiece = ChessBoard.Instance.PieceAt(enemies[i], board);
            IndividualScanMap[enemies[i].x, enemies[i].y].ChanceToCapture = 1 - (FuzzyLogic.FindFuzzyNumber(ScanPiece, EnemyPiece) / 6);
            IndividualScanMap[enemies[i].x, enemies[i].y].friendFoe = 2;
            //Begin Search on EnemiesMoves 
            List<Vector2Int> EnemyAttackList = ChessBoard.Instance.FilterAttackRange(EnemyPiece, board);

            for (int k = 0; k < EnemyAttackList.Count; k++)
            {

                IndividualScanMap[EnemyAttackList[k].x, EnemyAttackList[k].y].isVulnerable = true;
                IndividualScanMap[EnemyAttackList[k].x, EnemyAttackList[k].y].CaptureChance += 1 - (FuzzyLogic.FindFuzzyNumber(EnemyPiece, ScanPiece) / 6);
            }

        }
        for (int i = 0; i < openTiles.Count; i++)
        {
            IndividualScanMap[openTiles[i].x, openTiles[i].y].FriendFoe = 0;
        }

        for (int h = 0; h < 8; h++)
        {
            for (int f = 0; f < 8; f++)
            {
                IndividualScanMap[h, f].Heuristic = IndividualScanMap[h, f].ChanceToCapture * 5 - IndividualScanMap[h, f].captureChance;
            }
        }


        return IndividualScanMap;

    }
}