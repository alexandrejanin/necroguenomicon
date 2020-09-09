using System.Collections.Generic;
using UnityEngine;

public class Environment {
    public List<Unit> units = new List<Unit>();

    public Unit AddUnit(Unit unit) {
        units.Add(unit);
        unit.environment = this;
        return unit;
    }

    public Unit GetUnit(Vector2Int position) {
        foreach (var unit in units) {
            if (unit.Position == position)
                return unit;
        }

        return null;
    }

    public List<Vector2Int> ManhattanRange(Vector2Int center, int range) {
        var tiles = new List<Vector2Int>();
        for (var y = center.y - range; y <= center.y + range; y++) {
            for (var x = center.x - range; x <= center.x + range; x++) {
                var tile = new Vector2Int(x, y);
                if (Util.ManhattanDistance(center, tile) <= range)
                    tiles.Add(tile);
            }
        }

        return tiles;
    }
}
