using System.Collections.Generic;
using UnityEngine;

public class TileDrawer : MonoBehaviour {
    [SerializeField] private GameObject tilePrefab;

    private List<GameObject> tileObjects = new List<GameObject>();

    public void Clear() {
        foreach (var tile in tileObjects)
            Destroy(tile);
        tileObjects.Clear();
    }

    public void DrawTiles(ICollection<Vector2Int> tiles) {
        if (tiles == null || tiles.Count == 0) {
            Clear();
            return;
        }

        if (tiles.Count == tileObjects.Count) {
            var i = 0;
            foreach (var tile in tiles) {
                tileObjects[i].transform.position = GetWorldPosition(tile);
                i++;
            }
        } else {
            Clear();

            foreach (var tile in tiles)
                tileObjects.Add(Instantiate(
                    tilePrefab,
                    GetWorldPosition(tile),
                    Quaternion.identity,
                    transform
                ));
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
