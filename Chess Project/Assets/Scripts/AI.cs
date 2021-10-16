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
	public Random rand = new Random();
	BishopAI rInstance = new BishopAI();
	int[,] moves = new moves(2,3)


    public void Start()
    {
		int[] kingMove = instance.Start(true);
		int[] lBishopMove = lInstance.Start(true, true, true, true);
		int[] rBishopMove = rInstance.Start(true, true, true, true);

		makeMove();
	}

	public void Step()
    {
		int[] kingMove = instance.Step(true);
		int[] lBishopMove = lInstance.Step(true, true, true, true);
		int[] rBishopMove = rInstance.Step(true, true, true, true);

		makeMove();
	}

	public void makeMove()
    {
		Vector2Int newPosition = new Vector2Int(0, 0);

		bm.SelectPiece(instance.bestPiece.Position);
		newPosition.x = kingMove[0];
		newPosition.y = kingMove[1];
		bm.MovePiece(newPosition);

		bm.SelectPiece(lInstance.bestPiece.Position);
		newPosition.x = lBishopMove[0];
		newPosition.y = lBishopMove[1];
		bm.MovePiece(newPosition);

		bm.SelectPiece(rInstance.bestPiece.Position);
		newPosition.x = rBishopMove[0];
		newPosition.y = rBishopMove[1];
		bm.MovePiece(newPosition);
	}
}
