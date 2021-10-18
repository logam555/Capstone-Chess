//Written by Hamza Khan
//last edited: 10/16/2021 - 4:30
//V2.0
using System;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player
{
	bool isTurn;
	KingAI instance;
	BishopAI lInstance;
	private BoardManager bm;
	//public Random rand = new Random();
	BishopAI rInstance;
	int[] kingMove;
	int[] lBishopMove;
	int[] rBishopMove;
	int[,] moves;
	bool first;

public AI (string name, bool isWhite, List<Commander> commanders, BoardManager bm) : base(name, isWhite, commanders)
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

		this.bm = bm;
		instance = new KingAI(bm);
		lInstance = new BishopAI(bm);
		rInstance = new BishopAI(bm);
		kingMove = new int[2];
		lBishopMove = new int[2];
		rBishopMove = new int[2];
		moves = new int[2, 3];
		first = true;
	}


public void Start()
    {
		if (first == true)
		{
			kingMove = instance.Start(this.isWhite);
			moveKing();
			lBishopMove = lInstance.Start(this.isWhite, true);
			movelBishop();
			rBishopMove = rInstance.Start(this.isWhite, false);
			moverBishop();
			first = false;
		}
		else
			Step();
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
