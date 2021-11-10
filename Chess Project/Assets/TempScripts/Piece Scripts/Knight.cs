﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Knight : Subordinate {
    public Knight(bool isWhite, Vector2Int position, Commander commander, string name) : base(isWhite, position, commander, name) { }

    public override List<Vector2Int> MoveRange() {
        if (Commander.commandActions <= 0)
            return new List<Vector2Int>();
        return RecursiveMoveRange(Position, 4);
    }

    public override List<Vector2Int> AttackRange() {
        return MoveRange();
    }
}