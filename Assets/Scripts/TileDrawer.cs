using System.Collections.Generic;
using UnityEngine;

public class TileDrawer : MonoBehaviour {
    [SerializeField] private SpriteRenderer tilePrefab;

    private Dictionary<Color, List<SpriteRenderer>> tileObjects = new Dictionary<Color, List<SpriteRenderer>>();

    public void Clear(Color color) {
        if (!tileObjects.ContainsKey(color))
            return;
        foreach (var tile in tileObjects[color])
            Destroy(tile);
        tileObjects[color].Clear();
    }

    public void DrawTiles(Color color, ICollection<Vector2Int> tiles) {
        if (tiles == null || tiles.Count == 0) {
            Clear(color);
            return;
        }

        if (tileObjects.ContainsKey(color) && tiles.Count == tileObjects[color].Count) {
            var i = 0;
            foreach (var tile in tiles) {
                tileObjects[color][i].transform.position = GetWorldPosition(tile);
                i++;
            }
        } else {
            Clear(color);

            if (!tileObjects.ContainsKey(color))
                tileObjects[color] = new List<SpriteRenderer>();

            foreach (var tile in tiles) {
                var spriteRenderer = Instantiate(
                    tilePrefab,
                    GetWorldPosition(tile),
                    Quaternion.identity,
                    transform
                );
                spriteRenderer.color = new Color(color.r, color.g, color.b, 0.6f);
                tileObjects[color].Add(spriteRenderer);
            }
        }
    }

    public Vector3 GetWorldPosition(Vector2Int position) {
        return new Vector3(
            position.x + 0.5f,
            position.y + 0.5f,
            10
        );
    }
}
