using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

public abstract class Unit : SerializedMonoBehaviour {
    [HideInInspector] public Environment environment;

    private Vector2Int position;
    public Vector2Int Position => position;

    public Vector3 WorldPosition => ToWorldPosition(Position);

    private Vector3 targetPosition;

    private Queue<Vector2Int> pathToAnimate;

    [OdinSerialize] protected UnitStats baseStats;

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
        set => baseStats = value;
    }

    public void Spawn(Vector2Int spawnPosition) {
        position = spawnPosition;
        targetPosition = WorldPosition;
        baseStats.resistances = resistances;
        baseStats.affinities = affinities;
    }

    public void FixedUpdate() {
        transform.position = Vector3.Lerp(transform.position, targetPosition, 0.2f);
    }

    public void StartOfTurn() {
        tickerEffects.RemoveAll(effect => effect.Tick(this));
    }

    public void EndOfTurn() {
        statsEffects.RemoveAll(effect => effect.Tick(this));
    }

    public void FollowPath(List<Vector2Int> path) {
        foreach (var tile in path)
            position = tile;

        pathToAnimate = new Queue<Vector2Int>(path);
    }

    public IEnumerator AnimatePath() {
        if (pathToAnimate == null || pathToAnimate.Count == 0)
            yield break;

        foreach (var tile in pathToAnimate) {
            targetPosition = ToWorldPosition(tile);
            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
                yield return null;
        }

        pathToAnimate = null;
    }

    public Dictionary<Vector2Int, List<Vector2Int>> GetMovementTiles() {
        var tiles = new Dictionary<Vector2Int, List<Vector2Int>>();

        for (var y = position.y - Stats.movementPoints; y <= position.y + Stats.movementPoints; y++) {
            for (var x = position.x - Stats.movementPoints; x <= position.x + Stats.movementPoints; x++) {
                var tile = new Vector2Int(x, y);

                if (!environment.IsWalkable(tile))
                    continue;

                var unit = environment.GetUnit(tile);

                if (unit != null && unit != this)
                    continue;

                var path = environment.GetPath(position, tile);
                if (path != null && path.Count <= Stats.movementPoints)
                    tiles.Add(tile, path);
            }
        }

        return tiles;
    }

    public void Damage(int damage, Element element, Unit source, string spellName) {
        var multiplier = 1f;

        multiplier -= Stats.GetResistance(element) / 100f;

        multiplier += source.Stats.GetAffinity(element) / 100f;

        damage = Mathf.RoundToInt(damage * multiplier);

        baseStats.health -= damage;

        Debug.Log($"{source.name} inflige {damage} points de dégats de {element} à {name} avec {spellName}");

        if (baseStats.health <= 0)
            Destroy(gameObject);
    }

    public HashSet<Unit> KnockBack(Vector2Int from, int distance) {
        if (from == Position)
            return null;

        // TODO: collide with walls

        var dir = Position - from;
        var dest = distance * new Vector2(dir.x, dir.y).normalized;
        var targetTile = Position + new Vector2Int(Mathf.RoundToInt(dest.x), Mathf.RoundToInt(dest.y));
        var unit = environment.GetUnit(targetTile);

        if (unit == null) {
            position = targetTile;
            targetPosition = WorldPosition;
            return new HashSet<Unit> {this};
        }

        var units = unit.KnockBack(position, 1);
        units.Add(this);

        position = targetTile;
        targetPosition = WorldPosition;

        return units;
    }

    public void AddTickerEffect(TickerEffect effect) {
        tickerEffects.Add(effect);
    }

    public void AddStatsEffect(StatsEffect effect) {
        statsEffects.Add(effect);
    }

    public abstract IEnumerator PlayMovementPhase(GameController controller);
    public abstract IEnumerator PlayAttackPhase(GameController controller);

    protected static Vector3 ToWorldPosition(Vector2Int tile) => new Vector3(tile.x + 0.5f, tile.y + 0.2f);
}
