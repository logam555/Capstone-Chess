//Written by Hamza Khan
//last edited: 10/16/2021 - 4:30
//V2.0
using System;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player
{
	KingAI instance;
	BishopAI lInstance;
	//public Random rand = new Random();
	BishopAI rInstance;
	int[] kingMove;
	int[] lBishopMove;
	int[] rBishopMove;
	int[,] moves;
	bool first;

    private void Awake()
	{
		commanders = new List<Commander>();
		capturedPieces = new Dictionary<string, int>();
		capturedPieces.Add("King", 0);
		capturedPieces.Add("Queen", 0);
		capturedPieces.Add("Bishop", 0);
		capturedPieces.Add("Knight", 0);
		capturedPieces.Add("Rook", 0);
		capturedPieces.Add("Pawn", 0);

		instance = new GameObject().AddComponent<KingAI>();
		lInstance = new GameObject().AddComponent<BishopAI>();
		lInstance.getCommander(true);
		rInstance = new GameObject().AddComponent<BishopAI>();
		rInstance.getCommander(false);

		kingMove = new int[2];
		lBishopMove = new int[2];
		rBishopMove = new int[2];
		moves = new int[2, 3];
		first = true;

		ModelManager.Instance.BoardWideHeuristicCall();
	}

	public void Step()
    {
		kingMove = instance.Step();
		moveKing();
		lBishopMove = lInstance.Step();
		movelBishop();
		rBishopMove = rInstance.Step();
		moverBishop();
	}

	public void moveKing()
    {
		Vector2Int newPosition = new Vector2Int(0, 0);

		ChessBoard.Instance.SelectPiece(instance.bestPiece.Position);
		newPosition.x = kingMove[0];
		newPosition.y = kingMove[1];
		ChessBoard.Instance.PerformAction(newPosition);
	}

	public void movelBishop()
    {
		Vector2Int newPosition = new Vector2Int(0, 0);

		ChessBoard.Instance.SelectPiece(lInstance.bestPiece.Position);
		newPosition.x = lBishopMove[0];
		newPosition.y = lBishopMove[1];
		ChessBoard.Instance.PerformAction(newPosition);
	}

	public void moverBishop()
    {
		Vector2Int newPosition = new Vector2Int(0, 0);

		ChessBoard.Instance.SelectPiece(rInstance.bestPiece.Position);
		newPosition.x = rBishopMove[0];
		newPosition.y = rBishopMove[1];
		ChessBoard.Instance.PerformAction(newPosition);
	}
}
