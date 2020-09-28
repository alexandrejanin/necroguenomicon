using UnityEngine;

public class Building : MonoBehaviour {
    [SerializeField]
    private Vector2Int size, offset;

    public Vector2Int Size => size;
    public Vector2Int Offset => offset;

    public int Width => size.x;
    public int Height => size.y;

    private void OnDrawGizmos() {
        var center = new Vector3(offset.x + size.x / 2, offset.y + size.y / 2);
        if (size.x % 2 != 0)
            center.x += 0.5f;
        if (size.y % 2 != 0)
            center.y += 0.5f;
        Gizmos.DrawWireCube(transform.position + center, new Vector3(size.x, size.y));
    }
}
