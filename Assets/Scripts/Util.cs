using System.Collections.Generic;
using UnityEngine;

public static class Util {
    public static int ManhattanDistance(Vector2Int a, Vector2Int b) {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    public static Vector2Int DirectionFromTo(this Vector2Int from, Vector2Int to) =>
        new Vector2Int(
            from.x < to.x ? 1 : from.x > to.x ? -1 : 0,
            from.y < to.y ? 1 : from.y > to.y ? -1 : 0
        );

    public static List<Vector2Int> Line(Vector2Int from, Vector2Int to) {
        var dx = Mathf.Abs(to.x - from.x);
        var sx = from.x < to.x ? 1 : from.x > to.x ? -1 : 0;
        var dy = -Mathf.Abs(to.y - from.y);
        var sy = from.y < to.y ? 1 : from.y > to.y ? -1 : 0;
        var err = dx + dy;
        var e2 = 0;

        var line = new List<Vector2Int>();
        while (true) {
            line.Add(from);
            if (from.x == to.x && from.y == to.y) {
                line.RemoveAt(0);
                return line;
            }
            e2 = 2 * err;
            if (e2 >= dy) {
                err += dy;
                from.x += sx;
            }
            if (e2 <= dx) {
                err += dx;
                from.y += sy;
            }
        }
    }
}
