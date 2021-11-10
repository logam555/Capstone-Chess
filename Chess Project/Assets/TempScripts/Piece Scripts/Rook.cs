using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Rook : Subordinate {
    public Rook(bool isWhite, Vector2Int position, Commander commander, string name) : base(isWhite, position, commander, name) { }

    public override List<Vector2Int> MoveRange() {
        if (Commander.commandActions <= 0)
            return new List<Vector2Int>();
        return RecursiveMoveRange(Position, 2);
    }

    public override List<Vector2Int> AttackRange() {
        if (Commander.commandActions <= 0)
            return new List<Vector2Int>();
        return RecursiveMoveRange(Position, 3);
    }
}
