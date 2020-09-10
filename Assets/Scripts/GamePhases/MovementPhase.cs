using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovementPhase : GamePhase {
    public MovementPhase(GameController controller) : base(controller) { }

    public override IEnumerator Run() {
        controller.PhaseText.text = "Phase de déplacement\nVotre tour";

        var movementTiles = controller.Player.GetMovementTiles();

        controller.TileDrawer.DrawTiles(movementTiles.Keys.ToList());

        Vector2Int targetedTile;
        do {
            targetedTile = controller.GetTargetedTile();
            yield return null;
        } while (!Input.GetMouseButtonDown(0) || !movementTiles.Keys.Contains(targetedTile));

        controller.TileDrawer.Clear();

        yield return controller.Player.StartCoroutine(controller.Player.Move(movementTiles[targetedTile]));

        controller.PhaseText.text = "Phase de déplacement\nTour des ennemis";

        foreach (var unit in controller.Environment.units) {
            if (unit == controller.Player)
                continue;

            var tiles = unit.GetMovementTiles();
            var paths = new List<List<Vector2Int>>(tiles.Values);

            if (paths.Count <= 0)
                continue;

            var path = paths[Random.Range(0, paths.Count)];
            yield return unit.StartCoroutine(unit.Move(path));
        }

        controller.Phase = new AttackPhase(controller);
    }
}
