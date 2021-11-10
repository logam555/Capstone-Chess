using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class ChessPiece {

    protected List<Vector2Int> directions;

    public bool IsWhite { get; set; }
    public Vector2Int Position { get; set; }

    public string Name { get; set; }

    public ChessPiece(bool isWhite, Vector2Int pos, string name) {
        IsWhite = isWhite;
        Position = pos;
        Name = name;
        directions = new List<Vector2Int>();
        
        for(int i = -1; i <= 1; i++) {
            for(int j = -1; j <= 1; j++) {
                if (i == 0 && j == 0)
                    continue;
                directions.Add(new Vector2Int(i, j));
            }
        }
    }

    // Determines what positions are available to move to based on pieces movement restriction
    public abstract List<Vector2Int> MoveRange();

    // Function to determine if enemies are withing attacking range
    public abstract List<Vector2Int> AttackRange();

    // Recursive function for nonlinear movement range
    public List<Vector2Int> RecursiveMoveRange(Vector2Int currentPos, int range) {
        List<Vector2Int> positions = new List<Vector2Int>();

        if (range > 0) {
            foreach(Vector2Int dir in directions) {
                Vector2Int nextPos = currentPos + dir;

                if (!ChessBoard.Instance.ValidPosition(nextPos))
                    continue;

                positions.Add(nextPos);

                if (ChessBoard.Instance.IsPieceAt(nextPos))
                    continue;
                positions = positions.Union(RecursiveMoveRange(nextPos, range - 1)).ToList();
            }
        }

        return positions;
    }
}

public abstract class Commander : ChessPiece {
    public List<Subordinate> subordinates;
    public int commandActions;
    public bool usedFreeMovement;

    public Commander(bool isWhite, Vector2Int position, string name) : base(isWhite, position, name) {
        subordinates = new List<Subordinate>();
        commandActions = 1;
        usedFreeMovement = false;
    }

    public abstract void Reset();
}

public abstract class Subordinate : ChessPiece {
    public bool Delegated { get; set; }
    public Commander Commander { get; set; }

    public Subordinate(bool isWhite, Vector2Int position, Commander commander, string name) : base(isWhite, position, name) {
        Delegated = false;
        Commander = commander;
    }
}
