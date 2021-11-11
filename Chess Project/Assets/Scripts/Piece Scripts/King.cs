using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class King : Commander {

    public bool usedDelegation;

    public King(bool isWhite, Vector2Int position, string name) : base(isWhite, position, name) {
        usedDelegation = false;
    }

    public override List<Vector2Int> MoveRange(ChessPiece[,] board) {
        if (commandActions <= 0)
            return new List<Vector2Int>();
        return RecursiveMoveRange(Position, 3, board);
    }

    public override List<Vector2Int> AttackRange(ChessPiece[,] board) {
        if (commandActions <= 0)
            return new List<Vector2Int>();
        return RecursiveMoveRange(Position, 1, board);
    }

    public void DelegatePiece(Subordinate subordinate, Commander commander) {
        if (commander.subordinates.Count() >= 6 || usedDelegation)
            return;

        commander.subordinates.Add(subordinate);
        subordinate.Commander = commander;

        subordinates.Remove(subordinate);
        subordinate.Delegated = true;
        usedDelegation = true;
    }

    public void RecallPiece(Subordinate subordinate) {
        if (!subordinate.Delegated || usedDelegation)
            return;

        subordinates.Add(subordinate);
        subordinate.Commander.subordinates.Remove(subordinate);

        subordinate.Delegated = false;
        subordinate.Commander = this;
        usedDelegation = true;
    }

    public override void Reset() {
        commandActions = 1;
        usedFreeMovement = false;
        usedDelegation = false;
    }
}
