﻿using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [SerializeField, Min(0)] private int enemyCount = 2;

    [FormerlySerializedAs("text")] [SerializeField]
    private Text phaseText;

    public Text PhaseText => phaseText;

    [SerializeField] private Player playerPrefab;
    private Player player;
    public Player Player => player;

    [SerializeField] private Enemy enemyPrefab;

    [SerializeField] private UnitHealthBar healthBarPrefab;
    [SerializeField] private RectTransform healthBarParent;

    private Environment environment = new Environment();
    public Environment Environment => environment;

    private Coroutine phaseCoroutine;

    private GamePhase phase;

    public GamePhase Phase {
        get => phase;
        set {
            if (phaseCoroutine != null)
                StopCoroutine(phaseCoroutine);
            phase = value;
            phaseCoroutine = StartCoroutine(phase.Run());
        }
    }

    private TileDrawer tileDrawer;
    public TileDrawer TileDrawer => tileDrawer;

    private void Awake() {
        tileDrawer = GetComponent<TileDrawer>();

        GetComponent<RoomGenerator>().GenerateRoom(this);

        player = SpawnUnit(playerPrefab, new Vector2Int(environment.Width / 2, environment.Height / 2));

        Phase = new PreparationPhase(this);
    }

    public T SpawnUnit<T>(T prefab, Vector2Int position) where T : Unit {
        var unit = Instantiate(prefab);
        unit.Spawn(position);
        environment.AddUnit(unit);
        var healthBar = Instantiate(healthBarPrefab, healthBarParent);
        healthBar.unit = unit;
        return unit;
    }

    public Vector2Int GetTargetedTile() {
        var mousePositionInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        return new Vector2Int(
            Mathf.FloorToInt(mousePositionInWorld.x),
            Mathf.FloorToInt(mousePositionInWorld.y)
        );
    }

    private void OnDrawGizmosSelected() {
        if (environment == null)
            return;

        Gizmos.color = new Color(0, 1, 0, 0.5f);
        for (var x = 0; x < environment.Width; x++)
            for (var y = 0; y < environment.Height; y++)
                if (environment.IsWalkable(x, y))
                    Gizmos.DrawCube(new Vector3(x + 0.5f, y + 0.5f, 0), Vector3.one);
    }
}
