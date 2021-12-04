//Written by Hamza Khan
//last edited: 10/16/2021 - 4:30
//V2.0
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class AI : Player
{
	KingAI instance;
	BishopAI lInstance;
	//public Random rand = new Random();
	BishopAI rInstance;
	int[] kingMove;
	int[] freeKing;
	int[] freelB;
	int[] freerB;
	int[] lBishopMove;
	int[] rBishopMove;
	int[,] moves;
	int lBishopCount;
	int rBishopCount;
	int kingCount;
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

		freeKing = new int[2];
		freelB = new int[2];
		freerB = new int[2];
		
		moves = new int[2, 3];
		first = true;
		
		ModelManager.Instance.BoardWideHeuristicCall(ref ModelManager.Instance.chessBoardGridCo);
		ModelManager.Instance.MakeBoardWideHuerCopy();
	}

    private void Start() {
		instance = new GameObject().AddComponent<KingAI>();
		lInstance = new GameObject().AddComponent<BishopAI>();
		lInstance.leftBishop = true;
		rInstance = new GameObject().AddComponent<BishopAI>();
		rInstance.leftBishop = false;

		new GameObject().AddComponent<IndividualPieceScanner>();
	}

    public async void Step()
    {
		lBishopCount = lInstance is null ? int.MaxValue : lInstance.bishop.subordinates.Count;
		rBishopCount = rInstance is null ? int.MaxValue : rInstance.bishop.subordinates.Count;
		kingCount = instance.King.subordinates.Count;

		await Task.Run(() => kingDelegate());

		List<Task> tasks = new List<Task>();
        tasks.Add(Task.Run(() => {
            List<Thread> threads = new List<Thread>();
            threads.Add(new Thread(() => {
                if (instance != null) {
                    kingMove = instance.Step();
                }
            }));

            threads[0].Start();
            threads[0].Join();

        }));

        tasks.Add(Task.Run(() => {
            List<Thread> threads = new List<Thread>();
            threads.Add(new Thread(() => {
                if (lInstance != null) {
                    lBishopMove = lInstance.Step();
                }
            }));

            threads[0].Start();
            threads[0].Join();
        }));

        tasks.Add(Task.Run(() => {
            List<Thread> threads = new List<Thread>();
            threads.Add(new Thread(() => {
                if (rInstance != null) {
                    rBishopMove = rInstance.Step();
                }
            }));

            threads[0].Start();
            threads[0].Join();
        }));

        await Task.WhenAll(tasks);

        StartCoroutine(TakeTurn());

        //tasks.Clear();
        //tasks.Add(Task.Run(() => {
        //    List<Thread> threads = new List<Thread>();
        //    threads.Add(new Thread(() => {
        //        if (instance != null) {
        //            freeKing = instance.useFreeMove();
        //        }
        //    }));

        //    threads[0].Start();
        //    threads[0].Join();

        //}));

        //tasks.Add(Task.Run(() => {
        //    List<Thread> threads = new List<Thread>();
        //    threads.Add(new Thread(() => {
        //        if (lInstance != null) {
        //            freelB = lInstance.useFreeMove();
        //        }
        //    }));

        //    threads[0].Start();
        //    threads[0].Join();
        //}));

        //tasks.Add(Task.Run(() => {
        //    List<Thread> threads = new List<Thread>();
        //    threads.Add(new Thread(() => {
        //        if (rInstance != null) {
        //            freerB = rInstance.useFreeMove();
        //        }
        //    }));

        //    threads[0].Start();
        //    threads[0].Join();
        //}));

        //await Task.WhenAll(tasks);

        //StartCoroutine(TakeFree());
    }


	public void kingDelegate()
    {
		Debug.Log("Left Bishop Count: " + lBishopCount + " Right Bishop Count: " + rBishopCount + " King Count: " + kingCount);
		if (lBishopCount < (kingCount-1) && lBishopCount < rBishopCount && lInstance != null)
        {
			ChessBoard.Instance.SelectPiece(instance.King.subordinates[kingCount - 1].Position);
			ChessBoard.Instance.DelegatePieceAI(lInstance.bishop.Position);
			Debug.Log("lb delegated: " + lInstance.bishop.subordinates[lBishopCount].Name);
		}
		else if (rBishopCount < (kingCount-1) && rBishopCount <= lBishopCount && rInstance != null)
		{
			ChessBoard.Instance.SelectPiece(instance.King.subordinates[kingCount - 1].Position);
			ChessBoard.Instance.DelegatePieceAI(rInstance.bishop.Position);
			Debug.Log("rb delegated: " + rInstance.bishop.subordinates[rBishopCount].Name);
		}
		else if(kingCount < lBishopCount && lBishopCount > rBishopCount && lInstance != null)
        {
			for(int i = 0; i < lBishopCount; i++)
            {
				if(lInstance.bishop.subordinates[i].Delegated)
                {
					ChessBoard.Instance.SelectPiece(lInstance.bishop.subordinates[i].Position);
					ChessBoard.Instance.DelegatePieceAI(lInstance.bishop.subordinates[i].Position);
					break;
                }
            }
        }
		else if (kingCount < rBishopCount && lBishopCount < rBishopCount && rInstance != null)
		{
			for (int i = 0; i < rBishopCount; i++)
			{
				if (rInstance.bishop.subordinates[i].Delegated)
				{
					ChessBoard.Instance.SelectPiece(rInstance.bishop.subordinates[i].Position);
					ChessBoard.Instance.DelegatePieceAI(rInstance.bishop.subordinates[i].Position);
					break;
				}
			}
		}
		else
			Debug.Log("none delegated");
	}

	public bool useFreeKing()
    {
		if (freeKing[0] != 0 || freeKing[1] != 0)
        {
			ChessBoard.Instance.SelectPiece(instance.King.Position);
			Vector2Int newPosition = new Vector2Int(freeKing[2], freeKing[3]);
			Debug.Log(ChessBoard.Instance.SelectedPiece);
			Debug.Log(newPosition);
			return ChessBoard.Instance.PerformAction(newPosition);
        }

		return false;
	}

	public bool useFreelBishop()
    {
		if (freelB[0] != 0 || freelB[1] != 0)
		{
			ChessBoard.Instance.SelectPiece(lInstance.bishop.Position);
			Vector2Int newPosition = new Vector2Int(freelB[2], freelB[3]);
			Debug.Log(ChessBoard.Instance.SelectedPiece);
			Debug.Log(newPosition);
			return ChessBoard.Instance.PerformAction(newPosition);
		}

		return false;
	}

	public bool useFreerBishop()
    {
		if (freerB[0] != 0 || freerB[1] != 0)
		{
			ChessBoard.Instance.SelectPiece(rInstance.bishop.Position);
			Vector2Int newPosition = new Vector2Int(freerB[2], freerB[3]);
			Debug.Log(ChessBoard.Instance.SelectedPiece);
			Debug.Log(newPosition);
			return ChessBoard.Instance.PerformAction(newPosition);
		}

		return false;
	}

	public bool moveKing()
    {
		Vector2Int newPosition = new Vector2Int(0, 0);

		ChessBoard.Instance.SelectPiece(new Vector2Int(kingMove[0], kingMove[1]));
		newPosition.x = kingMove[2];
		newPosition.y = kingMove[3];
		Debug.Log(ChessBoard.Instance.SelectedPiece);
		Debug.Log(newPosition);
		return ChessBoard.Instance.PerformAction(newPosition);
	}

	public bool movelBishop()
    {
		Vector2Int newPosition = new Vector2Int(0, 0);

		ChessBoard.Instance.SelectPiece(new Vector2Int(lBishopMove[0], lBishopMove[1]));
		newPosition.x = lBishopMove[2];
		newPosition.y = lBishopMove[3];
		Debug.Log(ChessBoard.Instance.SelectedPiece);
		Debug.Log(newPosition);
		return ChessBoard.Instance.PerformAction(newPosition);
	}

	public bool moverBishop()
    {
		Vector2Int newPosition = new Vector2Int(0, 0);

		ChessBoard.Instance.SelectPiece(new Vector2Int(rBishopMove[0], rBishopMove[1]));
		newPosition.x = rBishopMove[2];
		newPosition.y = rBishopMove[3];
		Debug.Log(ChessBoard.Instance.SelectedPiece);
		Debug.Log(newPosition);
		return ChessBoard.Instance.PerformAction(newPosition);
	}

	public IEnumerator TakeTurn() {
		float wait = 1.0f;

		yield return new WaitForSeconds(0.25f);

		if (instance != null) {
			wait = moveKing() ? 5.0f : 1.5f;
		}

		yield return new WaitForSeconds(wait);

        if (lInstance != null) {
			wait = movelBishop() ? 5.0f : 1.5f;
		}

		yield return new WaitForSeconds(wait);

        if (rInstance != null) {
            wait = moverBishop() ? 5.0f : 1.5f;
        }

		yield return new WaitForSeconds(wait);

		GameManager.Instance.PassTurn();
    }

	public IEnumerator TakeFree()
	{
		float wait = 1.0f;

		if (instance != null)
		{
			wait = useFreeKing() ? 5.0f : 2.5f;
		}

		yield return new WaitForSeconds(wait);

		if (lInstance != null)
		{
			wait = useFreelBishop() ? 5.0f : 2.5f;
		}

		yield return new WaitForSeconds(wait);

		if (rInstance != null)
		{
			wait = useFreerBishop() ? 5.0f : 2.5f;
		}

		yield return new WaitForSeconds(wait);

		GameManager.Instance.PassTurn();

	}
}
