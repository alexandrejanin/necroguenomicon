using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class DisplayMovementRange : MonoBehaviour {
    [SerializeField]
    private GameObject tilePrefab;

    private Unit unit;

    private List<GameObject> tiles = new List<GameObject>();

    private void Start() {
        unit = GetComponent<Unit>();
        foreach (var tile in unit.GetMovementTiles().Keys) {
            tiles.Add(Instantiate(tilePrefab, GetWorldPosition(tile), Quaternion.identity, transform));
        }
    }

    private void Update() {
        if (!(FindObjectOfType<GameController>().Phase is MovementPhase)) {
            foreach (var tile in tiles) {
                tile.SetActive(false);
            }

            return;
        }

        foreach (var tile in tiles) {
            tile.SetActive(true);
        }

        var movementTiles = unit.GetMovementTiles(); 
        if (movementTiles.Keys.Count == tiles.Count) {
            var i = 0;
            foreach (var tile in movementTiles.Keys) {
                tiles[i].transform.position = GetWorldPosition(tile);
                i++;
            }
        } else {
            foreach (var tile in tiles) {
                Destroy(tile);
            }
            tiles.Clear();
            foreach (var tile in movementTiles.Keys) {
                tiles.Add(Instantiate(tilePrefab, GetWorldPosition(tile), Quaternion.identity, transform));
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
