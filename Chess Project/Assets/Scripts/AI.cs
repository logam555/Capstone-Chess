//Written by Hamza Khan
//last edited: 10/16/2021 - 4:30
//V2.0
using System;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player
{
	bool isTurn;
	KingAI instance = new KingAI();
	BishopAI lInstance = new BishopAI();
	private BoardManager bm;
	//public Random rand = new Random();
	BishopAI rInstance = new BishopAI();
	int[] kingMove = new int[2];
	int[] lBishopMove = new int[2];
	int[] rBishopMove = new int[2];
	int[,] moves = new int[2, 3];

public AI (string name, bool isWhite, List<Commander> commanders) : base(name, isWhite, commanders)
	{
        this.name = name;
        this.isWhite = isWhite;
        this.commanders = commanders;

        capturedPieces = new Dictionary<string, int>();
        capturedPieces.Add("King", 0);
        capturedPieces.Add("Queen", 0);
        capturedPieces.Add("Bishop", 0);
        capturedPieces.Add("Knight", 0);
        capturedPieces.Add("Rook", 0);
        capturedPieces.Add("Pawn", 0);
    }


public void Start()
    {
		kingMove = instance.Start(true);
		moveKing();
		lBishopMove = lInstance.Start(true, true);
		movelBishop();
		rBishopMove = rInstance.Start(true, true);
		moverBishop();
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

		bm.SelectPiece(instance.bestPiece.Position);
		newPosition.x = kingMove[0];
		newPosition.y = kingMove[1];
		bm.CheckMove(newPosition);
	}

	public void movelBishop()
    {
		Vector2Int newPosition = new Vector2Int(0, 0);

		bm.SelectPiece(lInstance.bestPiece.Position);
		newPosition.x = lBishopMove[0];
		newPosition.y = lBishopMove[1];
		bm.CheckMove(newPosition);
	}

	public void moverBishop()
    {
		Vector2Int newPosition = new Vector2Int(0, 0);

		bm.SelectPiece(rInstance.bestPiece.Position);
		newPosition.x = rBishopMove[0];
		newPosition.y = rBishopMove[1];
		bm.CheckMove(newPosition);
	}
}
