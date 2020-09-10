using System.Collections;
using UnityEngine;

public class AttackPhase : GamePhase {
    private Spell spell = new Fireball();

    public AttackPhase(GameController controller) : base(controller) { }

    public override IEnumerator Run() {
        foreach (var unit in controller.Environment.units) {
            controller.PhaseText.text = "Phase d'attaque\nTour des ennemis";

            unit.StartOfTurn();

            if (unit == controller.Player) {
                controller.PhaseText.text = "Phase d'attaque\nVotre tour";
                var validTargets = spell.GetValidTargets(controller.Player);
                controller.TileDrawer.DrawTiles(validTargets);

                Vector2Int targetedTile;
                do {
                    targetedTile = controller.GetTargetedTile();
                    yield return null;
                } while (!Input.GetMouseButtonDown(0) || !validTargets.Contains(targetedTile));

                controller.TileDrawer.Clear();

                spell.Apply(controller.Player, targetedTile);
            }
        }

        foreach (var unit in controller.Environment.units)
            if (unit.health <= 0)
                Object.Destroy(unit);

        controller.Environment.units.RemoveAll(u => u.health <= 0);

        controller.Phase = new MovementPhase(controller);
    }
}
