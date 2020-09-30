using UnityEngine;
using UnityEngine.Tilemaps;

public class Building : MonoBehaviour {
    [SerializeField] private Vector2Int size, offset;
    [SerializeField] private Tilemap[] tilemapColliders;

    public Vector2Int Size => size;
    public Vector2Int Offset => offset;

    public int Width => size.x;
    public int Height => size.y;

    public bool IsWalkable(int x, int y) {
        foreach (var tilemapCollider in tilemapColliders)
            if (tilemapCollider.GetColliderType(new Vector3Int(x, y, 0)) == Tile.ColliderType.Grid)
                return false;

        return true;
    }

    private void OnDrawGizmosSelected() {
        var center = new Vector3(offset.x + size.x / 2, offset.y + size.y / 2);
        if (size.x % 2 != 0)
            center.x += 0.5f;
        if (size.y % 2 != 0)
            center.y += 0.5f;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + center, new Vector3(size.x, size.y));

        Gizmos.color = new Color(0, 1f, 0, 0.5f);
        for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                if (IsWalkable(x, y))
                    Gizmos.DrawCube(transform.position + new Vector3(x + 0.5f, y + 0.5f), Vector3.one);
    }
}
