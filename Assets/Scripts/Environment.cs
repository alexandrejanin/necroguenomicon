using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Environment {
    public List<Unit> units = new List<Unit>();
    
    private bool[,] isWalkable;

    public int Width => isWalkable.GetLength(0);
    public int Height => isWalkable.GetLength(1);

    public bool IsWalkable(Vector2Int pos) => IsWalkable(pos.x, pos.y);
    public bool IsWalkable(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height && isWalkable[x, y];

    public void SetSize(int width, int height) {
        isWalkable = new bool[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                isWalkable[x, y] = true;
    }

    public bool PlaceBuilding(Building building, Vector2Int pos) {
        if (!CanPlaceBuilding(building, pos))
            return false;

        for (int x = 0; x < building.Width; x++)
            for (int y = 0; y < building.Height; y++)
                isWalkable[pos.x + x, pos.y + y] = building.IsWalkable(x, y);

        return true;
    }

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

    public HashSet<Vector2Int> ManhattanRange(Vector2Int center, int range) {
        var tiles = new HashSet<Vector2Int>();
        for (var y = center.y - range; y <= center.y + range; y++) {
            for (var x = center.x - range; x <= center.x + range; x++) {
                var tile = new Vector2Int(x, y);
                if (Util.ManhattanDistance(center, tile) <= range)
                    tiles.Add(tile);
            }
        }

        return tiles;
    }

    public bool CanPlaceBuilding(Building building, Vector2Int position) {
        for (int x = 0; x < building.Width; x++)
            for (int y = 0; y < building.Height; y++)
                if (!IsWalkable(position.x + x, position.y + y))
                    return false;

        return true;
    }

    public List<Vector2Int> GetPath(Vector2Int start, Vector2Int goal) {
        var goalUnit = GetUnit(goal);
        var open = new HashSet<Vector2Int>{start};

        System.Func<Vector2Int, int> h = t => Mathf.Abs((goal - t).x) + Mathf.Abs((goal - t).y);
        var f = new Dictionary<Vector2Int, int>{{start, h(start)}};
        var g = new Dictionary<Vector2Int, int>{{start, 0}};
        var parent = new Dictionary<Vector2Int, Vector2Int>();

        while (open.Count > 0) {
            Vector2Int current = open.First();
            foreach (var node in open)
                if (current == null || f.ContainsKey(node) && f[node] < f[current])
                    current = node;

            if (current == goal) {
                var path = new List<Vector2Int>{current};
                while (parent.ContainsKey(current) && parent[current] != start) {
                    current = parent[current];
                    path.Insert(0, current);
                }
                if (goalUnit != null)
                    path.RemoveAt(path.Count - 1);
                return path;
            }

            open.Remove(current);
            foreach (var neighbor in new[]{current + Vector2Int.left, current + Vector2Int.up, current + Vector2Int.right, current + Vector2Int.down}) {
                if (!IsWalkable(neighbor) || GetUnit(neighbor) != null && GetUnit(neighbor) != goalUnit)
                    continue;
                var newG = g[current] + 1;
                if (!g.ContainsKey(neighbor) || newG < g[neighbor]) {
                    parent[neighbor] = current;
                    g[neighbor] = newG;
                    f[neighbor] = newG + h(neighbor);
                    open.Add(neighbor);
                }
            }
        }

        return null;
    }

}
