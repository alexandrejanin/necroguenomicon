using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode, System.Serializable, RequireComponent(typeof(SpriteRenderer))]
public class SpawnPoint : SerializedMonoBehaviour {
    [SerializeField] private Dictionary<Enemy, float> enemies;

    public Vector2Int Position =>
        new Vector2Int(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));

    private void Update() {
        transform.position = new Vector3(Position.x + 0.5f, Position.y + 0.5f);

        GetComponent<SpriteRenderer>().enabled = !Application.isPlaying;
        if (enemies != null && enemies.Count > 0)
            GetComponent<SpriteRenderer>().sprite =
                enemies.Keys.First().GetComponentInChildren<SpriteRenderer>().sprite;
    }

    public void SpawnEnemy(GameController gameController) {
        var rand = Random.Range(0f, 1f);
        var total = 0f;

        foreach (var kvp in enemies) {
            if (rand - total < kvp.Value) {
                gameController.SpawnUnit(kvp.Key, Position);
                break;
            }

            total += kvp.Value;
        }
    }
}
