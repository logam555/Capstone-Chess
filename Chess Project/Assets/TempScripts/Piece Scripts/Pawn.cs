using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Pawn : Subordinate {

    public Pawn(bool isWhite, Vector2Int position, Commander commander, string name) : base(isWhite, position, commander, name) { }

    public override List<Vector2Int> MoveRange() {
        List<Vector2Int> positions = new List<Vector2Int>();
        int forwardDirection = IsWhite ? 1 : -1;

        if (Commander.commandActions <= 0)
            return positions;

        Vector2Int diagLeft = new Vector2Int(Position.x - 1, Position.y + forwardDirection);
        Vector2Int diagRight = new Vector2Int(Position.x + 1, Position.y + forwardDirection);
        Vector2Int straight = new Vector2Int(Position.x, Position.y + forwardDirection);

        positions.Add(diagLeft);
        positions.Add(diagRight);
        positions.Add(straight);

        return positions;
    }

    public override List<Vector2Int> AttackRange() {
        return MoveRange();
    }

}
