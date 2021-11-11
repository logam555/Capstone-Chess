//Written by Hamza Khan
//last edited: 10/16/2021 - 4:30
//V2.0
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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

		kingMove = new int[2];
		lBishopMove = new int[2];
		rBishopMove = new int[2];
		moves = new int[2, 3];
		first = true;

		ModelManager.Instance.BoardWideHeuristicCall();
	}

    private void Start() {
		instance = new GameObject().AddComponent<KingAI>();
		lInstance = new GameObject().AddComponent<BishopAI>();
		lInstance.leftBishop = true;
		rInstance = new GameObject().AddComponent<BishopAI>();
		rInstance.leftBishop = false;
	}

    public void Step()
    {
		List<Thread> threads = new List<Thread>();

		threads.Add(new Thread(() => {
			if(instance != null)
				kingMove = instance.Step();
		}));

		threads.Add(new Thread(() => {
			if(lInstance != null)
				lBishopMove = lInstance.Step();
		}));

		threads.Add(new Thread(() => {
			if(rInstance != null)
				rBishopMove = rInstance.Step();
		}));

		foreach(Thread thread in threads) {
			thread.Start();
        }

		foreach(Thread thread in threads) {
			thread.Join();
        }

		StartCoroutine(TakeTurn());
	}

	public bool moveKing()
    {
		Vector2Int newPosition = new Vector2Int(0, 0);

		ChessBoard.Instance.SelectPiece(instance.bestPiece.Position);
		newPosition.x = kingMove[0];
		newPosition.y = kingMove[1];
		return ChessBoard.Instance.PerformAction(newPosition);
	}

	public bool movelBishop()
    {
		Vector2Int newPosition = new Vector2Int(0, 0);

		ChessBoard.Instance.SelectPiece(lInstance.bestPiece.Position);
		newPosition.x = lBishopMove[0];
		newPosition.y = lBishopMove[1];
		return ChessBoard.Instance.PerformAction(newPosition);
	}

	public bool moverBishop()
    {
		Vector2Int newPosition = new Vector2Int(0, 0);

		ChessBoard.Instance.SelectPiece(rInstance.bestPiece.Position);
		newPosition.x = rBishopMove[0];
		newPosition.y = rBishopMove[1];
		return ChessBoard.Instance.PerformAction(newPosition);
	}

	public IEnumerator TakeTurn() {
		float wait = 1.0f;

		if (instance != null) {
			wait = moveKing() ? 2.5f : 0.75f;
		}

		yield return new WaitForSeconds(wait);

        if (lInstance != null) {
			wait = movelBishop() ? 2.5f : 0.75f;
		}

		yield return new WaitForSeconds(wait);

        if (rInstance != null) {
            moverBishop();
        }

		GameManager.Instance.PassTurn();
    }
}
