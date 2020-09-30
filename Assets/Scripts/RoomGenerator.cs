using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour {
    [SerializeField] private Vector2Int minSize, maxSize;

    [SerializeField] private Tilemap tilemap;

    [SerializeField] private Tile ground;

    [SerializeField] private List<Building> buildingPrefabs;

    private GameController gameController;

    private void Awake() {
        gameController = GetComponent<GameController>();

        var width = Random.Range(minSize.x, maxSize.x);
        var height = Random.Range(minSize.y, maxSize.y);

        gameController.Environment.SetSize(width, height);

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                tilemap.SetTile(new Vector3Int(x, y, 0), ground);

        for (int i = 0; i < 10; i++) {
            var building = buildingPrefabs[Random.Range(0, buildingPrefabs.Count)];

            var pos = new Vector2Int(
                Random.Range(0, width - building.Width),
                Random.Range(0, height - building.Height)
            );
            if (gameController.Environment.PlaceBuilding(building, pos))
                Instantiate(building, new Vector3(pos.x, pos.y), Quaternion.identity);

        }
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
