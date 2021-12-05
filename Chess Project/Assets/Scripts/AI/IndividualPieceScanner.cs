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

    public ScanData[,] singleScanner(bool isWhite, Vector2Int position, ChessPiece[,] board)
    {

        //replace Piece with ChessPiece  
        //ChessBoard.Instance can replace bm 
        //PieceAt Takes addittionally a board from AI 
        //Chessboard.Instance.FilterAttackRange(piece, board) 
        //Chessboard.Instance.FilterMoverange(piece, board)  
        //ChessPiece[,] board = new ChessPiece[8,8];

        //board = ChessBoard.Instance.Board.Clone();
        ChessPiece ScanPiece = ChessBoard.Instance.PieceAt(position, board);
        //openPositionMap = bm.availableMoves(ScanPiece); 
        ScanData[,] IndividualScanMap = new ScanData[8, 8];

        if (ScanPiece == null)
            return IndividualScanMap;

        List<Vector2Int> enemies = ChessBoard.Instance.FilterAttackRange(ScanPiece, board);
        List<Vector2Int> openTiles = ChessBoard.Instance.FilterMoveRange(ScanPiece, board);
        


        for (int i = 0; i < enemies.Count; i++)
        {
            ChessPiece EnemyPiece = ChessBoard.Instance.PieceAt(enemies[i], board);
            IndividualScanMap[enemies[i].x, enemies[i].y].ChanceToCapture = (((7 - FuzzyLogic.FindFuzzyNumber(ScanPiece, EnemyPiece)) / 6.0f) + 1) * PieceValue(EnemyPiece);
            IndividualScanMap[enemies[i].x, enemies[i].y].friendFoe = 2;
            //Begin Search on EnemiesMoves 
            List<Vector2Int> EnemyAttackList = ChessBoard.Instance.FilterAttackRange(EnemyPiece, board);

            for (int k = 0; k < EnemyAttackList.Count; k++)
            {

                IndividualScanMap[EnemyAttackList[k].x, EnemyAttackList[k].y].isVulnerable = true;
                IndividualScanMap[EnemyAttackList[k].x, EnemyAttackList[k].y].CaptureChance += (((7 - FuzzyLogic.FindFuzzyNumber(EnemyPiece, ScanPiece)) / 6.0f) + 1) * PieceValue(ScanPiece);
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
                IndividualScanMap[h, f].Heuristic = IndividualScanMap[h, f].ChanceToCapture - IndividualScanMap[h, f].captureChance;
            }
        }


        return IndividualScanMap;

    }

    public int PieceValue(ChessPiece piece) {
        if (piece is Pawn)
            return 1;
        else if (piece is Rook)
            return 5;
        else if (piece is Knight)
            return 5;
        else if (piece is Bishop)
            return 10;
        else if (piece is Queen)
            return 5;
        else if (piece is King)
            return 20;
        else
            return 0; ;
    }
}