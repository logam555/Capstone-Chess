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
	int[] knightMove;
	List<int[]> moves;
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
		knightMove = null;

		freeKing = new int[2];
		freelB = new int[2];
		freerB = new int[2];
		
		moves = new List<int[]>();
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
					instance.King.commandActions -= 1;
					moves.Add(kingMove);
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
					lInstance.bishop.commandActions -= 1;
					moves.Add(lBishopMove);
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
					rInstance.bishop.commandActions -= 1;
					moves.Add(rBishopMove);
                }
            }));

            threads[0].Start();
            threads[0].Join();
        }));

        await Task.WhenAll(tasks);

        tasks.Clear();
        tasks.Add(Task.Run(() => {
            List<Thread> threads = new List<Thread>();
            threads.Add(new Thread(() => {
                if (instance != null) {
                    freeKing = instance.useFreeMove();
                }
            }));

            threads[0].Start();
            threads[0].Join();

        }));

        tasks.Add(Task.Run(() => {
            List<Thread> threads = new List<Thread>();
            threads.Add(new Thread(() => {
                if (lInstance != null) {
                    freelB = lInstance.useFreeMove();
                }
            }));

            threads[0].Start();
            threads[0].Join();
        }));

        tasks.Add(Task.Run(() => {
            List<Thread> threads = new List<Thread>();
            threads.Add(new Thread(() => {
                if (rInstance != null) {
                    freerB = rInstance.useFreeMove();
                }
            }));

            threads[0].Start();
            threads[0].Join();
        }));

        await Task.WhenAll(tasks);

		StartCoroutine(TakeTurn());

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
		if(ChessBoard.Instance.KingBoard[kingMove[0], kingMove[1]] is Knight)
			knightMove = instance.getLocalKnight(ChessBoard.Instance.KingBoard[kingMove[2], kingMove[3]]);
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
		if (ChessBoard.Instance.LBishopBoard[lBishopMove[0], lBishopMove[1]] is Knight)
			knightMove = lBishopMove;
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
		if (ChessBoard.Instance.RBishopBoard[rBishopMove[0], rBishopMove[1]] is Knight)
			knightMove = rBishopMove;
		newPosition.x = rBishopMove[2];
		newPosition.y = rBishopMove[3];
		Debug.Log(ChessBoard.Instance.SelectedPiece);
		Debug.Log(newPosition);
		return ChessBoard.Instance.PerformAction(newPosition);
	}

	public bool moveKnight(int commander) {
		Knight knight;
		if (commander == -1)
			return false;
		try {
			if (commander == 0) {
				knight = (Knight)ChessBoard.Instance.KingBoard[kingMove[2], kingMove[3]];
				if (knight != null)
					knight.Moved = true;
				knightMove = instance.getLocalKnight(knight);
			} else if (commander == 1) {
				knight = (Knight)ChessBoard.Instance.LBishopBoard[lBishopMove[2], lBishopMove[3]];
				if (knight != null)
					knight.Moved = true;
				knightMove = instance.getLocalKnight(knight);
			} else {
				knight = (Knight)ChessBoard.Instance.RBishopBoard[rBishopMove[2], rBishopMove[3]];
				if (knight != null)
					knight.Moved = true;
				knightMove = instance.getLocalKnight(knight);
			}
		} catch (Exception e) { 
			return false;
		}
		Vector2Int newPosition = new Vector2Int(0, 0);

		ChessBoard.Instance.SelectPiece(new Vector2Int(knightMove[0], knightMove[1]));
		newPosition.x = knightMove[2];
		newPosition.y = knightMove[3];
		Debug.Log(ChessBoard.Instance.SelectedPiece);
		Debug.Log(newPosition);
		if(knight != null)
			knight.Moved = false;
		return ChessBoard.Instance.PerformAction(newPosition);
	}

	public bool makeMove(int[] move, out int commander) {
		if (move == kingMove) {
			commander = 0;
			if (instance != null)
				return moveKing();
		}
		if (move == lBishopMove) {
			commander = 1;
			if (lInstance != null)
				return movelBishop();
		}
		if (move == rBishopMove) {
			commander = 2;
			if (rInstance != null)
				return moverBishop();
		}

		commander = -1;
		return false;
    }

	public IEnumerator TakeTurn() {
		float wait = 1.0f;
		int com;

		moves.Sort(delegate (int[] x, int[] y) {
			if (x[4] == y[4]) {
				if (x[3] < y[3])
					return -1;
				if (x[3] == y[3])
					return 0;
				return 1;
			} else if (x[4] > y[4]) return -1;
			else
				return 1;
		});

		yield return new WaitForSeconds(0.25f);

		wait = makeMove(moves[0], out com) ? 4.0f : 1.0f;

		yield return new WaitForSeconds(wait);

		if(knightMove != null) {
			wait = moveKnight(com) ? 4.0f : 1.0f;
			knightMove = null;

			yield return new WaitForSeconds(wait);
		}

		

		if (moves.Count >= 2) {
			wait = makeMove(moves[1], out com) ? 4.0f : 1.0f;
		}

		yield return new WaitForSeconds(wait);

		if (knightMove != null) {
			wait = moveKnight(com) ? 4.0f : 1.0f;
			knightMove = null;

			yield return new WaitForSeconds(wait);
		}

		

		if (moves.Count >= 3) {
            wait = makeMove(moves[2], out com) ? 4.0f : 1.0f;
        }

		yield return new WaitForSeconds(wait);

		if (knightMove != null) {
			wait = moveKnight(com) ? 4.0f : 1.0f;
			knightMove = null;

			yield return new WaitForSeconds(wait);
		}

		moves.Clear();
		

		StartCoroutine(TakeFree());
		//GameManager.Instance.PassTurn();
	}

	public IEnumerator TakeFree()
	{
		float wait = 1.0f;

		if (instance != null)
		{
			wait = useFreeKing() ? 4.0f : 1.0f;
		}

		yield return new WaitForSeconds(wait);

		if (lInstance != null)
		{
			wait = useFreelBishop() ? 4.0f : 1.0f;
		}

		yield return new WaitForSeconds(wait);

		if (rInstance != null)
		{
			wait = useFreerBishop() ? 4.0f : 1.0f;
		}

		yield return new WaitForSeconds(wait);

		instance.King.commandActions = 1;
		lInstance.bishop.commandActions = 1;
		rInstance.bishop.commandActions = 1;

		GameManager.Instance.PassTurn();

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
		else
			return 20;
	}
}
