using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    public Environment environment;

    [SerializeField] private Vector2Int position;
    public Vector2Int Position => position;

    public int maxHealth;
    public int health;
    public int maxMovementPoints;
    public int movementPoints;

    public Dictionary<Element, int> resistances = new Dictionary<Element, int>();
    public Dictionary<Element, int> affinities = new Dictionary<Element, int>();

    [SerializeField] private List<Effect> effects = new List<Effect>();
    public List<Effect> Effects => effects;

    public void Spawn(Vector2Int position) {
        this.position = position;
    }

    public void Update() {
        transform.position = new Vector3(Position.x + 0.5f, Position.y + 0.5f);
    }

    public void StartOfTurn() {
        Effects.RemoveAll((effect) => effect.Apply(this));
    }

    public void Move(List<Vector2Int> path) {
        if (path == null || path.Count == 0)
            return;
        this.position = path[path.Count - 1];
    }

    public Dictionary<Vector2Int, List<Vector2Int>> GetMovementTiles() {
        var tiles = new Dictionary<Vector2Int, List<Vector2Int>>();
        for (var y = position.y - movementPoints; y <= position.y + movementPoints; y++) {
            for (var x = position.x - movementPoints; x <= position.x + movementPoints; x++) {
                var tile = new Vector2Int(x, y);

                var unit = environment.GetUnit(tile);

                if (unit != null && unit != this)
                    continue;

                var path = GetPath(tile);
                if (path.Count <= movementPoints)
                    tiles.Add(tile, path);
            }
        }

        return tiles;
    }

    public List<Vector2Int> GetPath(Vector2Int goal) {
        if (goal == Position)
            return new List<Vector2Int>();

        var path = new List<Vector2Int>();

        var tile = Position;

        do {
            if (tile.x < goal.x)
                tile.x++;
            else if (tile.x > goal.x)
                tile.x--;
            else if (tile.y < goal.y)
                tile.y++;
            else if (tile.y > goal.y)
                tile.y--;

            path.Add(tile);
        } while (tile != goal);

        return path;
    }

    public void Damage(Element element, int damage) {
        var mult = 100;

        if (resistances != null && resistances.ContainsKey(element))
            mult -= resistances[element];

        damage = Mathf.RoundToInt(damage * mult / 100f);

        health -= damage;

        if (health <= 0)
            Destroy(gameObject);
    }
}
