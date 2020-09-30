using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour {
    [SerializeField] private Vector2Int minSize, maxSize;

    [SerializeField] private Tilemap tilemap;

    [SerializeField] private Tile ground;

    [SerializeField] private List<Building> buildingPrefabs;

    private bool[,] tiles;

    public int Width => tiles.GetLength(0);
    public int Height => tiles.GetLength(1);

    private bool IsTileFree(int x, int y) => x >= 0 && x < Width && y >= 0 && y < Height && !tiles[x, y];

    private void Awake() {
        var width = Random.Range(minSize.x, maxSize.x);
        var height = Random.Range(minSize.y, maxSize.y);

        tiles = new bool[width, height];

        for (int x = 0; x < width; x++)
        for (int y = 0; y < height; y++)
            tilemap.SetTile(new Vector3Int(x, y, 0), ground);

        for (int i = 0; i < 10; i++) {
            var building = buildingPrefabs[Random.Range(0, buildingPrefabs.Count)];

            var pos = new Vector2Int(Random.Range(0, width - building.Width),
                Random.Range(0, height - building.Height));
            if (CanPlaceBuilding(building, pos)) {
                Instantiate(building, new Vector3(pos.x, pos.y), Quaternion.identity);
                for (int x = 0; x < building.Width; x++)
                for (int y = 0; y < building.Height; y++)
                    tiles[pos.x + x, pos.y + y] = true;
                continue;
            }
        }
    }

    private bool CanPlaceBuilding(Building building, Vector2Int position) {
        for (int x = 0; x < building.Width; x++)
        for (int y = 0; y < building.Height; y++)
            if (!IsTileFree(position.x + x, position.y + y))
                return false;

        return true;
    }

    private void OnDrawGizmos() {
        var minCenter = new Vector3(minSize.x / 2, minSize.y / 2);
        if (minSize.x % 2 != 0)
            minCenter.x += 0.5f;
        if (minSize.y % 2 != 0)
            minCenter.y += 0.5f;
        Gizmos.DrawWireCube(minCenter, new Vector3(minSize.x, minSize.y));

        var maxCenter = new Vector3(maxSize.x / 2, maxSize.y / 2);
        if (maxSize.x % 2 != 0)
            maxCenter.x += 0.5f;
        if (maxSize.y % 2 != 0)
            maxCenter.y += 0.5f;
        Gizmos.DrawWireCube(maxCenter, new Vector3(maxSize.x, maxSize.y));
    }
}
