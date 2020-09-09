using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    [SerializeField, Min(0)] private int enemyCount = 2;

    [SerializeField] private Text text;

    [SerializeField] private Unit playerPrefab;
    private Unit player;
    public Unit Player => player;

    [SerializeField] private Unit enemyPrefab;

    [SerializeField] private UnitHealthBar healthBarPrefab;

    private Environment environment = new Environment();
    public Environment Environment => environment;

    private GamePhase phase;
    public GamePhase Phase => phase;

    private TileDrawer tileDrawer;
    public TileDrawer TileDrawer => tileDrawer;

    private void Awake() {
        tileDrawer = GetComponent<TileDrawer>();

        phase = new MovementPhase();
        player = SpawnUnit(playerPrefab, new Vector2Int(0, 0));
        for (var i = 0; i < enemyCount; i++) {
            SpawnUnit(enemyPrefab, new Vector2Int(Random.Range(-5, 5), Random.Range(-5, 5)));
        }
    }

    private void Update() {
        var newPhase = phase.Update(this);
        if (newPhase != phase) {
            phase = newPhase;
            newPhase.Start(this);
        }

        text.text = phase?.Text(this);
    }

    private Unit SpawnUnit(Unit prefab, Vector2Int position) {
        var unit = Instantiate(prefab);
        unit.Spawn(position);
        environment.AddUnit(unit);
        var canvas = GameObject.FindObjectOfType<Canvas>();
        var healthBar = Instantiate(healthBarPrefab, canvas.transform);
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
}
