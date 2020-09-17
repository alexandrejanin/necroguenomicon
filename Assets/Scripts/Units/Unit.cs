using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public abstract class Unit : SerializedMonoBehaviour {
    [HideInInspector]
    public Environment environment;

    private Vector2Int position;
    public Vector2Int Position => position;

    public Vector3 WorldPosition => new Vector3(Position.x + 0.5f, Position.y + 0.5f);

    [OdinSerialize]
    protected UnitStats baseStats;

    protected Dictionary<Element, int> resistances, affinities;

    private List<TickerEffect> tickerEffects = new List<TickerEffect>();
    private List<StatsEffect> statsEffects = new List<StatsEffect>();

    [OdinSerialize, ReadOnly]
    public UnitStats Stats {
        get {
            var modifiedStats = new UnitStats(baseStats);

            foreach (var statsEffect in statsEffects)
                statsEffect.apply(modifiedStats);

            return modifiedStats;
        }
        set {
        }
    }

    public void Spawn(Vector2Int position) {
        this.position = position;
        this.baseStats.resistances = resistances;
        this.baseStats.affinities = affinities;
    }

    public void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, WorldPosition, 0.2f);
    }

    public void StartOfTurn() {
        tickerEffects.RemoveAll(effect => effect.Tick(this));
    }

    public void EndOfTurn() {
        statsEffects.RemoveAll(effect => effect.Tick(this));
    }

    public IEnumerator Move(List<Vector2Int> path) {
        if (path == null || path.Count == 0)
            yield break;

        foreach (var tile in path) {
            position = tile;
            while (Vector3.Distance(transform.position, WorldPosition) > 0.01f) {
                yield return null;
            }
        }
    }

    public Dictionary<Vector2Int, List<Vector2Int>> GetMovementTiles() {
        var tiles = new Dictionary<Vector2Int, List<Vector2Int>>();
        for (var y = position.y - Stats.movementPoints; y <= position.y + Stats.movementPoints; y++) {
            for (var x = position.x - Stats.movementPoints; x <= position.x + Stats.movementPoints; x++) {
                var tile = new Vector2Int(x, y);

                var unit = environment.GetUnit(tile);

                if (unit != null && unit != this)
                    continue;

                var path = GetPath(tile);
                if (path.Count <= Stats.movementPoints)
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

    public void Damage(int damage, Element element, Unit source, string name) {
        var multiplier = 1f;

        multiplier -= Stats.GetResistance(element) / 100f;

        multiplier += source.Stats.GetAffinity(element) / 100f;

        damage = Mathf.RoundToInt(damage * multiplier);

        baseStats.health -= damage;

        Debug.Log($"{source.name} inflige {damage} points de dégats de {element} à {this.name} avec {name}");

        if (baseStats.health <= 0)
            Destroy(gameObject);
    }

    public void KnockBack(Vector2Int from, int distance) {
        var dir = this.Position - from;
        var dest = distance * new Vector2(dir.x, dir.y).normalized;
        position = new Vector2Int(Mathf.RoundToInt(dest.x), Mathf.RoundToInt(dest.y));
    }

    public void AddTickerEffect(TickerEffect effect) {
        tickerEffects.Add(effect);
    }

    public void AddStatsEffect(StatsEffect effect) {
        statsEffects.Add(effect);
    }

    public abstract IEnumerator PlayMovementPhase(GameController controller);
    public abstract IEnumerator PlayAttackPhase(GameController controller);
}
