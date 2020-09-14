using UnityEngine;
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

        player = SpawnUnit(playerPrefab, new Vector2Int(0, 0));
        player.name = "Joueur";
        for (var i = 0; i < enemyCount; i++) {
            var unit = SpawnUnit(enemyPrefab, new Vector2Int(Random.Range(-5, 5), Random.Range(-5, 5)));
            unit.name = $"Ivrogne Cogneur {i}";
        }

        Phase = new PreparationPhase(this);
    }

    private T SpawnUnit<T>(T prefab, Vector2Int position) where T : Unit {
        var unit = Instantiate(prefab);
        unit.Spawn(position);
        environment.AddUnit(unit);
        var canvas = FindObjectOfType<Canvas>();
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
