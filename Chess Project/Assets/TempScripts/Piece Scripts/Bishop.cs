using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Bishop : Commander {
    public Commander King { get; set; }

    public Bishop(bool isWhite, Vector2Int position, string name, Commander king) : base(isWhite, position, name) {
        King = king;
    }

    public override List<Vector2Int> MoveRange() {
        if (commandActions <= 0)
            return new List<Vector2Int>();
        return RecursiveMoveRange(Position, 2);
    }

    public override List<Vector2Int> AttackRange() {
        if (commandActions <= 0)
            return new List<Vector2Int>();
        return RecursiveMoveRange(Position, 1);
    }

    public void DelegatePieces() {
        King.subordinates = King.subordinates.Union(subordinates).ToList();
        subordinates.Clear();
    }

    public override void Reset() {
        commandActions = 1;
        usedFreeMovement = false;
    }
}
