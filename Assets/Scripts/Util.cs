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
}
